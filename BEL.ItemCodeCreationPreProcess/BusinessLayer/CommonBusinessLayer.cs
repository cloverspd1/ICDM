namespace BEL.ItemCodeCreationPreProcess.BusinessLayer
{
    using BEL.CommonDataContract;
    using BEL.DataAccessLayer;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.UserProfiles;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Common Business Layer
    /// </summary>
    public sealed class CommonBusinessLayer
    {
        /// <summary>
        ///  Lazy Instance
        /// </summary>
        private static readonly Lazy<CommonBusinessLayer> lazy = new Lazy<CommonBusinessLayer>(() => new CommonBusinessLayer());

        /// <summary>
        /// The context
        /// </summary>
        private ClientContext context = null;

        /// <summary>
        /// The web
        /// </summary>
        private Web web = null;

        /// <summary>
        /// Prevents a default instance of the <see cref="CommonBusinessLayer"/> class from being created.
        /// </summary>
        private CommonBusinessLayer()
        {
            BELDataAccessLayer helper = new BELDataAccessLayer();
            string siteURL = helper.GetSiteURL(SiteURLs.ITEMCODECREATIONSITEURL);
            if (!string.IsNullOrEmpty(siteURL))
            {
                if (this.context == null)
                {
                    this.context = BELDataAccessLayer.Instance.CreateClientContext(siteURL);
                }
                if (this.web == null)
                {
                    this.web = BELDataAccessLayer.Instance.CreateWeb(this.context);
                }
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CommonBusinessLayer Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="applicationName">application Name</param>
        /// <returns>Byte Array</returns>
        public byte[] DownloadFile(string url, string applicationName)
        {
            BELDataAccessLayer helper = new BELDataAccessLayer();
            ////string siteURL = helper.GetSiteURL(applicationName);
            ////context = helper.CreateClientContext(siteURL);     
            ClientContext contexts = null;
            string siteURL = helper.GetSiteURL(SiteURLs.ITEMCODECREATIONSITEURL);
            if (!string.IsNullOrEmpty(siteURL))
            {
                contexts = BELDataAccessLayer.Instance.CreateClientContextforattachment(siteURL);
            }

            return helper.GetFileBytesByUrl(contexts, url);
        }

        /// <summary>
        /// Validates the users.
        /// </summary>
        /// <param name="emailList">The email list.</param>
        /// <returns>list of invalid users</returns>
        public List<string> ValidateUsers(List<string> emailList)
        {
            BELDataAccessLayer helper = new BELDataAccessLayer();
            return helper.GetInvalidUsers(emailList);
        }

        /// <summary>
        /// Removes the cache keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void RemoveCacheKeys(List<string> keys)
        {
            if (keys != null && keys.Count != 0)
            {
                foreach (string key in keys)
                {
                    GlobalCachingProvider.Instance.RemoveItem(key);
                }
            }
        }

        /// <summary>
        /// Gets the cache keys.
        /// </summary>
        /// <returns>list of string</returns>
        public List<string> GetCacheKeys()
        {
            return GlobalCachingProvider.Instance.GetAllKeys();
        }

        /// <summary>
        /// Gets the file name list.
        /// </summary>
        /// <param name="sectionDetails">The section details.</param>
        /// <param name="type">The type.</param>
        /// <returns>ISection Detail</returns>
        public ISection GetFileNameList(ISection sectionDetails, Type type)
        {
            if (sectionDetails == null)
            {
                return null;
            }
            dynamic dysectionDetails = Convert.ChangeType(sectionDetails, type);
            dysectionDetails.FileNameList = string.Empty;
            if (dysectionDetails.Files != null && dysectionDetails.Files.Count > 0)
            {
                dysectionDetails.FileNameList = JsonConvert.SerializeObject(dysectionDetails.Files);
            }
            return dysectionDetails;
        }

        /// <summary>
        /// Gets the file name list from current approver.
        /// </summary>
        /// <param name="sectionDetails">The section details.</param>
        /// <param name="type">The type.</param>
        /// <returns>I Section</returns>
        public ISection GetFileNameListFromCurrentApprover(ISection sectionDetails, Type type)
        {
            if (sectionDetails == null)
            {
                return null;
            }
            dynamic dysectionDetails = Convert.ChangeType(sectionDetails, type);
            dysectionDetails.FileNameList = string.Empty;
            if (dysectionDetails.CurrentApprover != null && dysectionDetails.CurrentApprover.Files != null && dysectionDetails.CurrentApprover.Files.Count > 0)
            {
                dysectionDetails.FileNameList = JsonConvert.SerializeObject(dysectionDetails.CurrentApprover.Files);
            }
            return dysectionDetails;
        }

        /// <summary>
        /// Gets the login user detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>User Details</returns>
        public UserDetails GetLoginUserDetail(string id)
        {
            MasterDataHelper masterHelper = new MasterDataHelper();
            List<UserDetails> userInfoList = masterHelper.GetAllEmployee(this.context, this.web);
            UserDetails detail = userInfoList.FirstOrDefault(p => p.UserId == id);
            return detail;
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns>returns User</returns>
        public User GetCurrentUser(string userid)
        {
            return BELDataAccessLayer.EnsureUser(this.context, this.web, userid);
        }

        /// <summary>
        /// Gets the user detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserDetails GetUserDetail(int id)
        {
            UserDetails detail = new UserDetails();
            var peopleManager = new PeopleManager(this.context);
            User usr = this.context.Web.GetUserById(id);
            PersonProperties personProperties = peopleManager.GetMyProperties();
            this.context.Load(usr, p => p.Id, p => p.LoginName, p => p.Email);
            this.context.Load(personProperties);
            this.context.ExecuteQuery();
            detail.UserId = usr.Id.ToString();
            detail.LoginName = usr.LoginName;
            detail.Department = personProperties.UserProfileProperties["Department"];
            detail.FullName = personProperties.DisplayName;
            detail.UserEmail = usr.Email; //personProperties.Email;
            return detail;
        }
    }
}