namespace BEL.ItemCodeCreationPreProcess.BusinessLayer
{
    using BEL.CommonDataContract;
    using BEL.DataAccessLayer;
    using Microsoft.SharePoint.Client;
    using System;

    /// <summary>
    /// Business Layer Base
    /// </summary>
    public class BusinessLayerBase
    {
        /// <summary>
        /// The context
        /// </summary>
        public static ClientContext context = null;

        /// <summary>
        /// The web
        /// </summary>
        public static Web web = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLayerBase"/> class.
        /// </summary>
        protected BusinessLayerBase()
        {
            try
            {
                BELDataAccessLayer helper = new BELDataAccessLayer();
                string siteURL = helper.GetSiteURL(SiteURLs.ITEMCODECREATIONSITEURL);
                if (!string.IsNullOrEmpty(siteURL))
                {
                    context = helper.CreateClientContext(siteURL);
                    web = helper.CreateWeb(context);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}