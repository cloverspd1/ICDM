namespace BEL.ItemCodeCreationPreProcess.BusinessLayer
{
    using BEL.CommonDataContract;
    using BEL.DataAccessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Role;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.UserProfiles;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Role Activity BusinessLayer
    /// </summary>
    public class RoleActivityBusinessLayer
    {
        /// <summary>
        /// The lazy
        /// </summary>
        private static readonly Lazy<RoleActivityBusinessLayer> lazy = new Lazy<RoleActivityBusinessLayer>(() => new RoleActivityBusinessLayer());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static RoleActivityBusinessLayer Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        /// <summary>
        /// The padlock
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="RoleActivityBusinessLayer"/> class from being created.
        /// </summary>
        private RoleActivityBusinessLayer()
        {
            string siteURL = BELDataAccessLayer.Instance.GetSiteURL(SiteURLs.ITEMCODECREATIONSITEURL);
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
        /// The context
        /// </summary>
        private ClientContext context = null;

        /// <summary>
        /// The web
        /// </summary>
        private Web web = null;

        /// <summary>
        /// Gets the u ser detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserDetails GetUSerDetail(int id)
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

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetAllRoles()
        {
            List<NameValueData> roleList = new List<NameValueData>();
            try
            {
                List spList = this.web.Lists.GetByTitle(Masters.ROLEMASTER);
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = @"<View>
                                    <Query>
                                       <OrderBy>
                                          <FieldRef Name='Role' Ascending='True' />
                                       </OrderBy>
                                    </Query>
                                   </View>";
                ListItemCollection items = spList.GetItems(camlQuery);
                this.context.Load(items);
                this.context.ExecuteQuery();

                if (items != null && items.Count > 0)
                {
                    foreach (ListItem item in items)
                    {
                        NameValueData role = new NameValueData();
                        role.Name = Convert.ToString(item["ID"]);
                        role.Value = Convert.ToString(item["Role"]);
                        roleList.Add(role);
                    }
                }
                return roleList;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while get all role : Message = " + ex.Message + " ,Stack Trace = " + ex.StackTrace);
                return roleList;
            }
        }

        /// <summary>
        /// Gets all screens.
        /// </summary>
        /// <returns></returns>
        public List<MenuDetails> GetAllScreens()
        {
            List<MenuDetails> menuList = new List<MenuDetails>();
            try
            {
                //get all nav nodes
                List<MenuDetails> allNavs = this.GetAllNavigationNodes();
                //Add all nav nodes in screen Master
                if (allNavs != null && allNavs.Count > 0)
                {
                    foreach (var nav in allNavs)
                    {
                        MenuDetails menu = new MenuDetails();
                        menu.ParentMenu = !string.IsNullOrWhiteSpace(nav.ParentMenu) ? nav.ParentMenu : string.Empty;
                        menu.ChildMenu = !string.IsNullOrWhiteSpace(nav.ChildMenu) ? nav.ChildMenu : string.Empty;
                        menuList.Add(menu);
                    }
                }
                return menuList;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while get updated screen from nav. Message = " + ex.Message + "Stack Trace = " + ex.StackTrace);
                return menuList;
            }
        }

        /// <summary>
        /// Deletes all items of list.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        public void DeleteAllItemsOfList(string listName)
        {
            try
            {
                List list = this.web.Lists.GetByTitle(listName);
                if (list != null)
                {
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"<View></View>";
                    ListItemCollection items = list.GetItems(camlQuery);
                    this.context.Load(items);
                    this.context.ExecuteQuery();

                    if (items != null && items.Count > 0)
                    {
                        foreach (ListItem item in items.ToList())
                        {
                            item.DeleteObject();
                        }
                        this.context.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while delete items in listname " + listName + ": Message = " + ex.Message + ": StackTrace = " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets all navigation nodes.
        /// </summary>
        /// <returns></returns>
        public List<MenuDetails> GetAllNavigationNodes()
        {
            List<MenuDetails> list = new List<MenuDetails>();
            try
            {
                List<NavigationNode> removeNavNode = new List<NavigationNode>();
                NavigationNodeCollection navColl = this.web.Navigation.QuickLaunch;

                this.context.Load(navColl);
                this.context.ExecuteQuery();
                if (navColl != null && navColl.Count > 0)
                {
                    string[] title = new string[] { "Recent", "PageNotFoundError", "Employee Master", "default", "PermissionDenied", "ManageRoleActivity", "Home", "CacheManagement", "Page not found", "EmployeeMaster", "PowerBIReport", "ICDMReport", "ChangeApproverActivity" };
                    foreach (NavigationNode navNode in navColl)
                    {
                        if (title.Contains(navNode.Title))
                        {
                            removeNavNode.Add(navNode);
                        }
                        else
                        {
                            this.context.Load(navNode.Children);
                            this.context.ExecuteQuery();
                            if (navNode.Children != null && navNode.Children.Count > 0)
                            {
                                foreach (var child in navNode.Children)
                                {
                                    MenuDetails menudetail = new MenuDetails();
                                    menudetail.ParentMenu = !string.IsNullOrWhiteSpace(navNode.Title) ? navNode.Title.Trim() : string.Empty;
                                    menudetail.ChildMenu = !string.IsNullOrWhiteSpace(child.Title) ? child.Title.Trim() : string.Empty;
                                    list.Add(menudetail);
                                }
                            }
                            else
                            {
                                MenuDetails menudetails = new MenuDetails();
                                menudetails.ParentMenu = !string.IsNullOrWhiteSpace(navNode.Title) ? navNode.Title.Trim() : string.Empty;
                                if (list.FindAll(m => m.ParentMenu == navNode.Title).Count == 0)
                                {
                                    list.Add(menudetails);
                                }
                            }
                        }
                    }
                    if (removeNavNode != null && removeNavNode.Count > 0)
                    {
                        foreach (NavigationNode navNode in removeNavNode)
                        {
                            navNode.DeleteObject();
                            this.context.ExecuteQuery();
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while get navigation nodes : Message = " + ex.Message + " StackTrace = " + ex.StackTrace);
                return list;
            }
        }

        /// <summary>
        /// Gets the role activity details by role identifier.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public List<RoleScreenMappingDetails> GetRoleActivityDetailsByRoleId(string roleName)
        {
            List<RoleScreenMappingDetails> roleMenuList = new List<RoleScreenMappingDetails>();
            List<RoleScreenMappingDetails> assignedMenuList = new List<RoleScreenMappingDetails>();
            List<RoleScreenMappingDetails> finalList = new List<RoleScreenMappingDetails>();
            try
            {
                //get all nav nodes
                List<MenuDetails> menuList = this.GetAllScreens();
                List spList = this.web.Lists.GetByTitle(ICCPListNames.ROLESCREENMAPPING);
                var q = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='RoleID' /><Value Type='Lookup'>" + roleName + "</Value></Eq></Where></Query></View>" };
                var roleMapItems = spList.GetItems(q);
                this.context.Load(roleMapItems);
                this.context.ExecuteQuery();

                if (roleMapItems == null || roleMapItems.Count == 0)
                {
                    foreach (MenuDetails menu in menuList)
                    {
                        RoleScreenMappingDetails roleMenu = new RoleScreenMappingDetails();
                        roleMenu.ID = 0;
                        roleMenu.RoleName = roleName;
                        roleMenu.ParentMenuName = menu.ParentMenu;
                        roleMenu.ChildMenuName = menu.ChildMenu;
                        roleMenu.IsAuthorized = false;
                        roleMenuList.Add(roleMenu);
                    }
                }
                else if (roleMapItems != null && roleMapItems.Count > 0)
                {
                    foreach (ListItem roleMapItem in roleMapItems)
                    {
                        RoleScreenMappingDetails roleMenu = new RoleScreenMappingDetails();
                        roleMenu.ID = roleMapItem.Id;
                        roleMenu.ParentMenuName = Convert.ToString(roleMapItem["ParentMenuName"]);
                        roleMenu.ChildMenuName = Convert.ToString(roleMapItem["ChildMenuName"]);
                        roleMenu.IsAuthorized = Convert.ToBoolean(roleMapItem["IsAuthorised"]);
                        roleMenu.RoleName = (roleMapItem["RoleID"] as FieldLookupValue).LookupValue;
                        roleMenuList.Add(roleMenu);
                    }
                }

                if (roleMenuList != null)
                {
                    int i = 0;
                    foreach (MenuDetails menu in menuList)
                    {
                        if (!roleMenuList.FindAll(m => menu.ParentMenu.ToUpper().Trim() == m.ParentMenuName.ToUpper().Trim()).Any())
                        {
                            RoleScreenMappingDetails roleMenu = new RoleScreenMappingDetails();
                            roleMenu.RoleName = roleName;
                            roleMenu.ParentMenuName = menu.ParentMenu;
                            roleMenu.ChildMenuName = menu.ChildMenu;
                            roleMenu.IsAuthorized = false;
                            roleMenuList.Insert(i, roleMenu);
                        }

                        if (!roleMenuList.FindAll(m => menu.ChildMenu.ToUpper().Trim() == m.ChildMenuName.ToUpper().Trim()).Any())
                        {
                            RoleScreenMappingDetails roleMenu = new RoleScreenMappingDetails();
                            roleMenu.RoleName = roleName;
                            roleMenu.ParentMenuName = menu.ParentMenu;
                            roleMenu.ChildMenuName = menu.ChildMenu;
                            roleMenu.IsAuthorized = false;
                            roleMenuList.Insert(i, roleMenu);
                        }
                        i++;
                    }

                    foreach (RoleScreenMappingDetails roleMenu in roleMenuList.ToList())
                    {
                        if (roleMenuList.FindAll(m => m.ParentMenuName == roleMenu.ParentMenuName).Any())
                        {
                            assignedMenuList = roleMenuList.FindAll(m => m.ParentMenuName == roleMenu.ParentMenuName).ToList();
                            finalList.AddRange(assignedMenuList.OrderBy(m => m.ChildMenuName).ToList());
                            roleMenuList.RemoveAll(m => m.ParentMenuName == roleMenu.ParentMenuName);
                        }
                    }
                }
                return finalList;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while GetRoleActivityDetailsByRoleId: Message = " + ex.Message + " ,StackTrace = " + ex.StackTrace);
                return roleMenuList;
            }
        }

        /// <summary>
        /// Updates the role wise activity.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public ActionStatus UpdateRoleWiseActivity(List<RoleScreenMappingDetails> list)
        {
            lock (Padlock)
            {
                ActionStatus status = new ActionStatus();
                try
                {
                    List spList = this.web.Lists.GetByTitle(ICCPListNames.ROLESCREENMAPPING);
                    if (list != null && list.Count > 0 && spList != null)
                    {
                        foreach (var i in list)
                        {
                            if (i.ID == 0 || i.ID == null)
                            {
                                ListItemCreationInformation info = new ListItemCreationInformation();
                                ListItem newItem = spList.AddItem(info);
                                FieldLookupValue flv = new FieldLookupValue();
                                flv.LookupId = Convert.ToInt32(i.RoleId);
                                newItem["RoleID"] = flv;
                                newItem["ParentMenuName"] = i.ParentMenuName;
                                newItem["ChildMenuName"] = i.ChildMenuName;
                                newItem["IsAuthorised"] = i.IsAuthorized;
                                newItem.Update();
                                this.context.ExecuteQuery();
                            }
                            else
                            {
                                int itemId = Convert.ToInt32(i.ID);
                                ListItem editItem = spList.GetItemById(itemId);
                                this.context.Load(editItem);
                                this.context.ExecuteQuery();
                                editItem["ParentMenuName"] = i.ParentMenuName;
                                editItem["ChildMenuName"] = i.ChildMenuName;
                                editItem["IsAuthorised"] = i.IsAuthorized;
                                FieldLookupValue lookup = editItem["RoleID"] as FieldLookupValue;
                                lookup.LookupId = Convert.ToInt32(i.RoleId);
                                editItem["RoleID"] = lookup;
                                editItem.Update();
                                this.context.ExecuteQuery();
                            }
                        }
                        status.IsSucceed = true;
                        status.Messages.Add("Role Activity Screen Matrix saved successfully.");
                    }
                }
                catch (Exception ex)
                {
                    status.IsSucceed = false;
                    //status.Messages = "Error occured while saving data";
                    Logger.Error("Error while UpdateRoleWiseActivity, Message =" + ex.Message + " StackTrace = " + ex.StackTrace);
                }
                return status;
            }
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public ActionStatus AddRole(string role)
        {
            lock (Padlock)
            {
                ActionStatus status = new ActionStatus();
                try
                {
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        List<NameValueData> roleMasterData = this.GetAllRoles();
                        if (roleMasterData.Where(m => m.Value.ToUpper().Trim() == role.ToUpper().Trim()).Any())
                        {
                            status.IsSucceed = false;
                            status.Messages.Add("This Role is already exists.");
                            return status;
                        }
                        else
                        {
                            List roleMaster = this.web.Lists.GetByTitle(Masters.ROLEMASTER);
                            ListItemCreationInformation info = new ListItemCreationInformation();
                            ListItem listItem = roleMaster.AddItem(info);
                            listItem["Role"] = role;
                            listItem.Update();
                            this.context.ExecuteQuery();

                            List emplist = this.web.Lists.GetByTitle(Masters.APPROVERMASTER);
                            Field rolefield = emplist.Fields.GetByTitle("Role");
                            FieldChoice fieldChoice = this.context.CastTo<FieldChoice>(rolefield);
                            this.context.Load(fieldChoice);
                            this.context.ExecuteQuery();

                            List<string> options = new List<string>(fieldChoice.Choices);
                            options.Add(role);
                            fieldChoice.Choices = options.ToArray();
                            fieldChoice.Update();
                            this.context.ExecuteQuery();

                            //  this.CreateSPGroup(role);

                            status.IsSucceed = true;
                            status.ExtraData = role;
                            status.Messages.Add("Role added Successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    status.IsSucceed = false;
                    status.Messages.Add("Sorry! Error while Adding Role.");

                    Logger.Error("Error while adding role name =" + role + " Message =" + ex.Message + " StackTrace = " + ex.StackTrace);
                }
                return status;
            }
        }

        ///// <summary>
        ///// Creates the sp group.
        ///// </summary>
        ///// <param name="role">The role.</param>
        //private void CreateSPGroup(string role)
        //{
        //    try
        //    {
        //        Group addgroup = null;
        //        try
        //        {
        //            addgroup = this.context.Web.SiteGroups.GetByName(role);
        //            this.context.Load(addgroup);
        //            this.context.ExecuteQuery();
        //        }
        //        catch
        //        {
        //            Logger.Error(role + " Group not found.Creating Group now.");
        //            addgroup = null;
        //        }

        //        if (addgroup == null)
        //        {
        //            //this.web.BreakRoleInheritance(true, false);
        //            User owner = this.web.EnsureUser("adm_sp@bajajelect.onmicrosoft.com");
        //            //   User member = this.web.EnsureUser("Tmsuser1@bajajelectricals.com");

        //            GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
        //            groupCreationInfo.Title = role;
        //            groupCreationInfo.Description = "Group Name : " + role;

        //            Group group = this.web.SiteGroups.Add(groupCreationInfo);
        //            group.Owner = owner;
        //            //  group.Users.AddUser(member);
        //            group.Update();
        //            this.context.ExecuteQuery();

        //            // Get the Role Definition (Permission Level)
        //            var customFullControlRoleDefinition = this.web.RoleDefinitions.GetByName("Contribute");
        //            this.context.Load(customFullControlRoleDefinition);
        //            this.context.ExecuteQuery();

        //            // Add it to the Role Definition Binding Collection
        //            RoleDefinitionBindingCollection collRDB = new RoleDefinitionBindingCollection(this.context);
        //            collRDB.Add(this.web.RoleDefinitions.GetByName("Contribute"));

        //            // Bind the Newly Created Permission Level to the new User Group
        //            this.web.RoleAssignments.Add(group, collRDB);

        //            this.context.Load(group);
        //            this.context.ExecuteQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("Error while adding Group name =" + role + " Message =" + ex.Message + " StackTrace = " + ex.StackTrace);
        //    }
        //}

        ///// <summary>
        ///// Deletes the sp group.
        ///// </summary>
        ///// <param name="role">The role.</param>
        //private void DeleteSPGroup(string role)
        //{
        //    Group deletegroup = null;
        //    try
        //    {
        //        deletegroup = this.context.Web.SiteGroups.GetByName(role);
        //        this.context.Load(deletegroup);
        //        this.context.ExecuteQuery();
        //    }
        //    catch
        //    {
        //        Logger.Error(role + " Group could not found in sharepoint Group.");
        //        deletegroup = null;
        //    }
        //    try
        //    {
        //        if (deletegroup != null)
        //        {
        //            this.web.RoleAssignments.GetByPrincipal(deletegroup).DeleteObject();
        //            this.web.Update();
        //            this.context.ExecuteQuery();

        //            GroupCollection groupColl = this.web.SiteGroups;
        //            groupColl.Remove(deletegroup);
        //            this.context.ExecuteQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("Error while deleting Group name =" + role + " Message =" + ex.Message + " StackTrace = " + ex.StackTrace);
        //    }
        //}

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <returns></returns>
        public ActionStatus DeleteRole(int roleID)
        {
            ActionStatus status = new ActionStatus();
            try
            {
                string roleName = string.Empty;
                if (roleID > 0)
                {
                    List roleMaster = this.web.Lists.GetByTitle(Masters.ROLEMASTER);
                    ListItem item = roleMaster.GetItemById(roleID);
                    this.context.Load(item);
                    this.context.ExecuteQuery();
                    if (item != null)
                    {
                        roleName = Convert.ToString(item["Role"]);

                        List emplist = this.web.Lists.GetByTitle(Masters.APPROVERMASTER);
                        CamlQuery camlEmpQuery = new CamlQuery();
                        camlEmpQuery.ViewXml = @"<View>
                                       <Query>
                                           <Where>
                                              <Eq>
                                                 <FieldRef Name='Role' />
                                                 <Value Type='Choice'>" + roleName + @"</Value>
                                              </Eq>
                                           </Where>
                                        </Query>
                                   </View>";
                        ListItemCollection empitems = emplist.GetItems(camlEmpQuery);
                        this.context.Load(empitems);
                        this.context.ExecuteQuery();
                        if (empitems != null && empitems.Count > 0)
                        {
                            status.IsSucceed = false;
                            status.Messages.Add("You can't delete this role because " + empitems.Count + " employee(s) are already assigned to this role.To delete this role please remove/reassign the other role to employee(s).");
                        }
                        else if (empitems == null || empitems.Count == 0)
                        {
                            List screenMapMaster = this.web.Lists.GetByTitle(ICCPListNames.ROLESCREENMAPPING);
                            CamlQuery camlQuery = new CamlQuery();
                            camlQuery.ViewXml = @"<View>
                                       <Query>
                                           <Where>
                                              <Eq>
                                                 <FieldRef Name='RoleID' />
                                                 <Value Type='Lookup'>" + roleName + @"</Value>
                                              </Eq>
                                           </Where>
                                        </Query>
                                   </View>";
                            ListItemCollection items = screenMapMaster.GetItems(camlQuery);
                            this.context.Load(items);
                            this.context.ExecuteQuery();
                            if (items != null && items.Count > 0)
                            {
                                foreach (ListItem deleteitem in items.ToList())
                                {
                                    deleteitem.DeleteObject();
                                }
                                this.context.ExecuteQuery();
                            }
                            if (items != null && items.Count == 0)
                            {
                                item.DeleteObject();
                                this.context.ExecuteQuery();
                                Field rolefield = emplist.Fields.GetByTitle("Role");
                                FieldChoice fieldChoice = this.context.CastTo<FieldChoice>(rolefield);
                                this.context.Load(fieldChoice);
                                this.context.ExecuteQuery();

                                List<string> options = new List<string>(fieldChoice.Choices);
                                options.Remove(roleName);
                                fieldChoice.Choices = options.ToArray();
                                fieldChoice.Update();
                                this.context.ExecuteQuery();

                                // this.DeleteSPGroup(Convert.ToString(roleName));

                                status.IsSucceed = true;
                                status.Messages.Add("Role Deleted Successfully.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.IsSucceed = true;
                status.Messages.Add("Sorry! Error while delete Role.");
                Logger.Error("Error while delete role having roleId =" + roleID + " Message =" + ex.Message + " StackTrace = " + ex.StackTrace);
            }
            return status;
        }
    }
}