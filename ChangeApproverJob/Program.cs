namespace ChangeApproverJob
{
    using BEL.CommonDataContract;
    using BEL.DataAccessLayer;
    using BEL.ItemCodeCreationPreProcess.BusinessLayer;
    using BEL.ItemCodeCreationPreProcess.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using Microsoft.SharePoint.Client;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Program file
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                string siteUrl = BELDataAccessLayer.Instance.GetSiteURL(SiteURLs.ITEMCODECREATIONSITEURL);
                using (ClientContext clientContext = BELDataAccessLayer.Instance.CreateClientContext(siteUrl))
                {
                    log4net.Config.XmlConfigurator.Configure();
                    Logger.Info("Main method started");
                    Web web = BELDataAccessLayer.Instance.CreateWeb(clientContext);
                    List configList = web.Lists.GetByTitle(ICCPListNames.CHANGEAPPROVERMASTER);
                    CamlQuery configCaml = new CamlQuery();
                    configCaml.ViewXml = @"<View>
                                            <Query>
                                               <Where>
                                                  <Eq>
                                                     <FieldRef Name='JobStatus' />
                                                     <Value Type='Choice'>No</Value>
                                                  </Eq>
                                               </Where>
                                            </Query>
                                        </View>";
                    ListItemCollection items = configList.GetItems(configCaml);
                    clientContext.Load(items);
                    clientContext.ExecuteQuery();

                    if (items != null)
                    {
                        DataTable tblChangeApprover = GetChangeApproverTable();
                        foreach (ListItem item in items)
                        {
                            FieldUserValue approver = item["PendingWithWhom"] as FieldUserValue;
                            int approverID = approver.LookupId;
                            string approverName = approver.LookupValue;

                            FieldUserValue replaceByWhom = item["ReplaceByWhom"] as FieldUserValue;
                            int replaceByWhomID = replaceByWhom.LookupId;
                            string replaceByWhomName = replaceByWhom.LookupValue;

                            DataRow dr = tblChangeApprover.NewRow();
                            dr["ID"] = Convert.ToInt32(item["ID"]);
                            //dr["RequestID"] = Convert.ToInt32(item["RequestID"]);
                            dr["RoleName"] = Convert.ToString(item["RoleName"]);
                            dr["PendingWithWhomID"] = approverID;
                            dr["PendingWithWhomName"] = approverName;
                            dr["ReplaceByWhomID"] = replaceByWhomID;
                            dr["ReplaceByWhomName"] = replaceByWhomName;
                            dr["JobStatus"] = Convert.ToString(item["JobStatus"]);
                            tblChangeApprover.Rows.Add(dr);
                        }

                        if (tblChangeApprover != null && tblChangeApprover.Rows.Count != 0)
                        {
                            ChangeApprover(tblChangeApprover, clientContext, web);
                        }
                    }
                }
                Logger.Info("Main method completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error While Execute Main Method");
                Console.Write(ex.StackTrace + "==>" + ex.Message);
                Logger.Error("Error While Execute Main Method");
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Gets the change approver table.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetChangeApproverTable()
        {
            DataTable tblChangeApprover = new DataTable();
            tblChangeApprover.Columns.Add(new DataColumn("ID", typeof(int)));
            //tblChangeApprover.Columns.Add(new DataColumn("RequestID", typeof(int)));
            tblChangeApprover.Columns.Add(new DataColumn("RoleName", typeof(string)));
            tblChangeApprover.Columns.Add(new DataColumn("PendingWithWhomID", typeof(int)));
            tblChangeApprover.Columns.Add(new DataColumn("PendingWithWhomName", typeof(string)));
            tblChangeApprover.Columns.Add(new DataColumn("ReplaceByWhomID", typeof(int)));
            tblChangeApprover.Columns.Add(new DataColumn("ReplaceByWhomName", typeof(string)));
            tblChangeApprover.Columns.Add(new DataColumn("JobStatus", typeof(string)));

            return tblChangeApprover;
        }

        /// <summary>
        /// Changes the approver.
        /// </summary>
        /// <param name="changeApproverTbl">The change approver table.</param>
        /// <param name="clientContext">The client context.</param>
        /// <param name="web">The web.</param>
        private static void ChangeApprover(DataTable changeApproverTbl, ClientContext clientContext, Web web)
        {
            Logger.Info("ChangeApprover method started");
            try
            {
                foreach (DataRow dr in changeApproverTbl.Rows)
                {
                    string roleName = Convert.ToString(dr["RoleName"]);
                    string pendingWithWhomName = Convert.ToString(dr["PendingWithWhomName"]);
                    int id = Convert.ToInt32(dr["ID"]);

                    List<PendingRequest> pendingRequests = ItemCodeCreationBusinessLayer.Instance.GetAllPendingRequests(roleName, pendingWithWhomName);
                    DataTable emailBodyTable = GetEmailBodyTable();
                    try
                    {
                        if (pendingRequests != null && pendingRequests.Count > 0)
                        {
                            foreach (PendingRequest pendingReq in pendingRequests)
                            {
                                bool permissionUpdated = false, appMatrixUpdated = false;
                                bool isPendingStatus = false;

                                int requestID = Convert.ToInt32(pendingReq.ID);
                                List mainList = web.Lists.GetByTitle(ICCPListNames.ICCPMAINLIST);
                                if (mainList != null && requestID > 0)
                                {
                                    ListItem listItem = mainList.GetItemById(requestID);
                                    clientContext.Load(listItem);
                                    clientContext.ExecuteQuery();

                                    if (listItem != null)
                                    {
                                        FieldUserValue[] nextApprovers = listItem["NextApprover"] as FieldUserValue[];
                                        if (nextApprovers != null && Convert.ToInt32(dr["PendingWithWhomID"]) > 0)
                                        {
                                            foreach (FieldUserValue nextApprover in nextApprovers)
                                            {
                                                if (nextApprover.LookupId == Convert.ToInt32(dr["PendingWithWhomID"]))
                                                {
                                                    isPendingStatus = true;
                                                    nextApprover.LookupId = Convert.ToInt32(dr["ReplaceByWhomID"]);
                                                    listItem["NextApprover"] = nextApprovers;
                                                    listItem["_ModerationStatus"] = 0;

                                                    listItem.Update();
                                                    //listItem.RefreshLoad();
                                                    //clientContext.Load(listItem);
                                                }
                                            }
                                            clientContext.ExecuteQuery();
                                            Logger.Info("Next Approver Changed");
                                        }
                                        if (Convert.ToInt32(dr["PendingWithWhomID"]) > 0 && Convert.ToInt32(dr["ReplaceByWhomID"]) > 0 && !string.IsNullOrWhiteSpace(Convert.ToString(dr["RoleName"])))
                                        {
                                            permissionUpdated = UpdatePermission(listItem, clientContext, web, Convert.ToInt32(dr["PendingWithWhomID"]), Convert.ToInt32(dr["ReplaceByWhomID"]));
                                            appMatrixUpdated = UpdateApprovalMatrix(requestID, clientContext, web, Convert.ToInt32(dr["PendingWithWhomID"]), Convert.ToInt32(dr["ReplaceByWhomID"]), Convert.ToString(dr["RoleName"]));

                                            Logger.Info("permissionUpdated = " + permissionUpdated + " ,appMatrixUpdated = " + appMatrixUpdated);
                                            if (isPendingStatus && permissionUpdated && appMatrixUpdated)
                                            {
                                                DataRow emailRow = emailBodyTable.NewRow();
                                                emailRow["ID"] = requestID;
                                                // emailRow["PendingWithWhomID"] = Convert.ToInt32(dr["PendingWithWhomID"]);
                                                emailRow["ReplaceByWhomName"] = Convert.ToString(dr["ReplaceByWhomName"]);
                                                emailRow["ICDMNo"] = Convert.ToString(listItem["Title"]);
                                                emailRow["WorkFlowStatus"] = Convert.ToString(listItem["WorkflowStatus"]);

                                                emailBodyTable.Rows.Add(emailRow);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        emailBodyTable = null;
                        Console.WriteLine("Error While Execute ChangeApprover Inner Method");
                        Console.Write(ex.StackTrace + "==>" + ex.Message);
                        Logger.Error("Error While Execute ChangeApprover Inner Method");
                    }
                    if (emailBodyTable != null && emailBodyTable.Rows.Count > 0)
                    {
                        ////get Item by ID and update job status
                        List configList = web.Lists.GetByTitle(ICCPListNames.CHANGEAPPROVERMASTER);
                        ListItem item = configList.GetItemById(id);
                        item["JobStatus"] = "Yes";
                        item.Update();
                        clientContext.Load(item);
                        clientContext.ExecuteQuery();

                        Logger.Info("JobStatus Changed to Yes");

                        ////send mail to ReplaceByWhomID
                        SendMailToReplacer(Convert.ToInt32(dr["ReplaceByWhomID"]), Convert.ToString(dr["ReplaceByWhomName"]), clientContext, web, emailBodyTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error While Execute ChangeApprover Method");
                Console.Write(ex.StackTrace + "==>" + ex.Message);
                Logger.Error("Error While Execute ChangeApprover Method");
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Updates the permission.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="clientContext">The client context.</param>
        /// <param name="web">The web.</param>
        /// <param name="pendingWithWhomID">The pending with whom identifier.</param>
        /// <param name="replaceByWhomID">The replace by whom identifier.</param>
        /// <returns></returns>
        public static bool UpdatePermission(ListItem listItem, ClientContext clientContext, Web web, int pendingWithWhomID, int replaceByWhomID)
        {
            Logger.Info("UpdatePermission method started :  listItem.Id = " + listItem.Id);
            bool permissionUpdated = false;
            try
            {
                string permissionType = string.Empty;

                RoleAssignmentCollection roles = listItem.RoleAssignments;
                clientContext.Load(roles);
                clientContext.ExecuteQuery();

                User usr = BELDataAccessLayer.EnsureUser(clientContext, web, Convert.ToString(pendingWithWhomID));
                RoleAssignment role = roles.GetByPrincipal(usr);
                clientContext.Load(role.RoleDefinitionBindings);
                clientContext.ExecuteQuery();

                RoleDefinitionBindingCollection roleBindings = role.RoleDefinitionBindings;
                if (roleBindings != null)
                {
                    RoleDefinition contributorRole = roleBindings.Where(p => p.Name == SharePointPermission.CONTRIBUTOR).FirstOrDefault();
                    if (contributorRole != null)
                    {
                        Logger.Info("contributorRole is not null");
                        permissionType = SharePointPermission.CONTRIBUTOR;
                    }
                    else
                    {
                        RoleDefinition viewerRole = roleBindings.Where(p => p.Name == SharePointPermission.READER).FirstOrDefault();
                        if (viewerRole != null)
                        {
                            Logger.Info("ViewerRole is not null");
                            permissionType = SharePointPermission.READER;
                        }
                    }
                }

                try
                {
                    ////remove pendingWithWhomID's permission
                    roles.GetByPrincipal(usr).DeleteObject();
                    clientContext.ExecuteQuery();
                    Logger.Info("Permission deleted for user = " + usr.Email);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error While delete permission to user id : " + replaceByWhomID + "userEmail : " + usr.Email + " in list item id : " + listItem.Id);
                    Logger.Error(ex);
                }

                ////give permission to replaceByWhomID
                try
                {
                    if (!string.IsNullOrWhiteSpace(permissionType))
                    {
                        User replacer = BELDataAccessLayer.EnsureUser(clientContext, web, Convert.ToString(replaceByWhomID));
                        RoleDefinitionBindingCollection collRoleDefinitionBindingAssignee = new RoleDefinitionBindingCollection(clientContext);
                        collRoleDefinitionBindingAssignee.Add(web.RoleDefinitions.GetByName(permissionType));
                        listItem.RoleAssignments.Add(replacer, collRoleDefinitionBindingAssignee);
                        clientContext.ExecuteQuery();
                        permissionUpdated = true;
                        Logger.Info("Permission assign to user = " + replacer.Email + " ,permissionType = " + permissionType);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error While grant permission to user id : " + replaceByWhomID + " in list item id : " + listItem.Id);
                    Logger.Error(ex);
                    permissionUpdated = false;
                }

                permissionUpdated = true;
                Logger.Info("grant permission, type = " + permissionType + " to user id : " + replaceByWhomID + " in list item id : " + listItem.Id);
                Logger.Info("permissionUpdated = " + permissionUpdated);
            }
            catch (Exception ex)
            {
                Logger.Error("Error in UpdatePermission Method");
                Logger.Error(ex);
            }

            return permissionUpdated;
        }

        /// <summary>
        /// Updates the approval matrix.
        /// </summary>
        /// <param name="requestID">The request identifier.</param>
        /// <param name="clientContext">The client context.</param>
        /// <param name="web">The web.</param>
        /// <param name="pendingWithWhomID">The pending with whom identifier.</param>
        /// <param name="replaceByWhomID">The replace by whom identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public static bool UpdateApprovalMatrix(int requestID, ClientContext clientContext, Web web, int pendingWithWhomID, int replaceByWhomID, string roleName)
        {
            Logger.Info("UpdateApprovalMatrix method started : requestID = " + requestID);
            bool appMatrixUpdated = false;
            try
            {
                ////Get data from Local Approval Matrix 
                List approverList = web.Lists.GetByTitle(ICCPListNames.ITEMCODEAPPROVALMATRIX);

                CamlQuery query = new CamlQuery();
                string qry = string.Empty;
                ListItemCollectionPosition position = null;
                var page = 1;

                do
                {
                    query.ViewXml = @"<View>
                                        <Query>
                                           <Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='RequestID' />
                                                    <Value Type='Lookup'>" + requestID + @"</Value>
                                                 </Eq>
                                                 <Eq>
                                                    <FieldRef Name='Role' />
                                                    <Value Type='Text'>" + roleName + @"</Value>
                                                 </Eq>
                                              </And>
                                           </Where>
                                        </Query>
                                        <RowLimit>5000</RowLimit>
                                 </View>";

                    query.ListItemCollectionPosition = position;

                    ListItemCollection approverDetails = approverList.GetItems(query);
                    clientContext.Load(approverDetails);
                    clientContext.ExecuteQuery();

                    position = approverDetails.ListItemCollectionPosition;

                    if (approverDetails != null)
                    {
                        for (int i = 0; i < approverDetails.Count; i++)
                        {
                            approverDetails[i].RefreshLoad();
                            Logger.Info("Approvermatrix list item  : id = " + Convert.ToInt32(approverDetails[i]["ID"]));

                            FieldUserValue[] approvers = approverDetails[i]["Approver"] as FieldUserValue[];
                            foreach (FieldUserValue userValue in approvers)
                            {
                                if (userValue.LookupId == pendingWithWhomID)
                                {
                                    userValue.LookupId = replaceByWhomID;
                                    approverDetails[i]["Approver"] = approvers;
                                    approverDetails[i].Update();
                                    approverDetails[i].RefreshLoad();
                                    clientContext.Load(approverDetails[i]);
                                    clientContext.ExecuteQuery();
                                    Logger.Info("UpdateApprovalMatrix Method : requestId= " + requestID + "Approver Updated : Updated Approver id= " + userValue.LookupId);
                                }
                            }
                        }
                        page++;
                    }
                }
                while (position != null);

                appMatrixUpdated = true;
                Logger.Info("appMatrixUpdated = " + appMatrixUpdated);
                Logger.Info("UpdateApprovalMatrix Method Completed : requestId" + requestID);
            }
            catch (Exception ex)
            {
                appMatrixUpdated = false;
                Logger.Error("appMatrixUpdated = " + appMatrixUpdated);
                Console.WriteLine("Error While Execute UpdateApprovalMatrix Method");
                Console.Write(ex.StackTrace + "==>" + ex.Message);
                Logger.Error("Error While Execute UpdateApprovalMatrix Method");
                Logger.Error(ex);
            }

            return appMatrixUpdated;
        }

        /// <summary>
        /// Sends the mail to replacer.
        /// </summary>
        /// <param name="replaceByWhomID">The replace by whom identifier.</param>
        /// <param name="replaceByWhomName">Name of the replace by whom.</param>
        /// <param name="context">The context.</param>
        /// <param name="web">The web.</param>
        /// <param name="emailBodyTable">The email body table.</param>
        public static void SendMailToReplacer(int replaceByWhomID, string replaceByWhomName, ClientContext context, Web web, DataTable emailBodyTable)
        {
            Logger.Info("SendMailToReplacer Method Started");
            try
            {
                if (emailBodyTable != null && emailBodyTable.Rows.Count > 0)
                {
                    string htmlTbl;
                    htmlTbl = @"<table width='100%' cellspacing='2' cellpadding='2' border='2'>
                                    <tr>
                                    <th width='10%'>Sr. No.</th>
                                    <th width='20%'>ICDM No.</th>
                                    <th width='35%'>Pending With Whom</th>
                                    <th width='35%'>WorkFlow Status</th>
                                    </tr>";
                    int i = 1;

                    EmailHelper eHelper = new EmailHelper();

                    Dictionary<string, string> mailCustomValues = null;
                    List<FileDetails> emailAttachments = null;
                    Dictionary<string, string> email = new Dictionary<string, string>();
                    List<ListItemDetail> itemdetail = new List<ListItemDetail>();
                    string applicationName = ApplicationNameConstants.LUMITEMCODECREATION;
                    string formName = FormNameConstants.ICCPForm;

                    string from = string.Empty, to = string.Empty, cc = string.Empty, role = string.Empty, tmplName = string.Empty, strAllusers = string.Empty, nextApproverIds = string.Empty;

                    from = BELDataAccessLayer.Instance.GetConfigVariable("FromEmailAddress");
                    Logger.Info("SendMailToReplacer Method : from = " + from);
                    ListItem userInfo = web.SiteUserInfoList.GetItemById(replaceByWhomID);
                    context.Load(userInfo);
                    context.ExecuteQuery();
                    to = Convert.ToString(userInfo["EMail"]);

                    Logger.Info("SendMailToReplacer Method : to = " + to);

                    //to = BELDataAccessLayer.GetEmailUsingUserID(context, web, to);
                    cc = BELDataAccessLayer.Instance.GetAllUsersFromGroup(context, web, "Admin");
                    Logger.Info("SendMailToReplacer Method : cc = " + cc);

                    tmplName = EmailTemplateName.CHANGEAPPROVERMAIL;
                    if (mailCustomValues == null)
                    {
                        mailCustomValues = new Dictionary<string, string>();
                    }
                    mailCustomValues[Parameter.MAILRECEIVERNAME] = replaceByWhomName;
                    mailCustomValues[Parameter.CURRENTAPPROVERNAME] = string.Empty;

                    foreach (DataRow row in emailBodyTable.Rows)
                    {
                        itemdetail.Add(new ListItemDetail() { ItemId = Convert.ToInt32(row["ID"]), IsMainList = true, ListName = ICCPListNames.ICCPMAINLIST });
                        string icdmNo = Convert.ToString(row["ICDMNo"]) == "View" ? "-" : Convert.ToString(row["ICDMNo"]);
                        htmlTbl += @"<tr>
                                        <td>" + i.ToString() + @"</td>
                                        <td>" + icdmNo + @"</td>
                                        <td>" + row["ReplaceByWhomName"] + @"</td> 
                                        <td>" + row["WorkFlowStatus"] + @"</td>    
                              </tr>";
                        i++;
                    }
                    htmlTbl += "</table>";

                    Logger.Info("GetEmailBody Method started");
                    email = eHelper.GetEmailBody(context, web, tmplName, itemdetail, mailCustomValues, role, applicationName, formName);
                    Logger.Info("GetEmailBody Method Completed : email[Body] = " + email["Body"]);

                    email["Body"] = email["Body"].Replace("[[Table​]]", htmlTbl);
                    Logger.Info("GetEmailBody Method Completed : email = " + email["Body"]);

                    Logger.Info("SendMail Method started");
                    eHelper.SendMail(applicationName, formName, tmplName, email["Subject"], email["Body"], from, to, cc, false, emailAttachments);
                    Logger.Info("SendMail Method completed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error While Execute SendMailToReplacer Method");
                Console.Write(ex.StackTrace + "==>" + ex.Message);
                Logger.Error("Error While Execute SendMailToReplacer Method");
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Gets the email body table.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEmailBodyTable()
        {
            DataTable tblEmailBody = new DataTable();
            tblEmailBody.Columns.Add(new DataColumn("ID", typeof(int)));
            tblEmailBody.Columns.Add(new DataColumn("ICDMNo", typeof(string)));
            //tblEmailBody.Columns.Add(new DataColumn("PendingWithWhomID", typeof(int)));
            tblEmailBody.Columns.Add(new DataColumn("ReplaceByWhomName", typeof(string)));
            tblEmailBody.Columns.Add(new DataColumn("WorkFlowStatus", typeof(string)));

            return tblEmailBody;
        }
    }
}
