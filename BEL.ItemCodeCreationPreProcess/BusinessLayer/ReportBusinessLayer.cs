namespace BEL.ItemCodeCreationPreProcess.BusinessLayer
{
    using BEL.CommonDataContract;
    using BEL.DataAccessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Reports;
    using Microsoft.SharePoint.Client;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Report Business Layer
    /// </summary>
    public class ReportBusinessLayer
    {
        /// <summary>
        /// The lazy
        /// </summary>
        private static readonly Lazy<ReportBusinessLayer> lazy = new Lazy<ReportBusinessLayer>(() => new ReportBusinessLayer());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ReportBusinessLayer Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ReportBusinessLayer" /> class from being created.
        /// </summary>
        private ReportBusinessLayer()
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
        /// Gets the report details.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>
        /// list of Reportdetails
        /// </returns>
        public List<ReportDetails> GetReportDetails(Report report)
        {
            try
            {
                List<ReportDetails> reportList = new List<ReportDetails>();
                string appendStatusClause = string.Empty, appendWhereClause = string.Empty, startStatusAndClause = string.Empty, endStatusAndClause = string.Empty, startAndClause = string.Empty, endAndClause = string.Empty;
                if (report != null)
                {
                    string fromDate = string.Format("{0:yyyy-MM-dd}", report.FromDate);
                    string toDate = string.Format("{0:yyyy-MM-dd}", report.ToDate);
                    string pendingWith = report.PendingWith;
                    string[] arrFilter = !string.IsNullOrWhiteSpace(pendingWith) ? pendingWith.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) : null;

                    string status = report.Status;
                    string[] arrStatusFilter = !string.IsNullOrWhiteSpace(status) ? status.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) : null;
                                      
                    List statusList = this.web.Lists.GetByTitle(ICCPListNames.ICCPMAINLIST);
                    if (statusList != null)
                    {
                        CamlQuery query = new CamlQuery();

                        //if (!string.IsNullOrWhiteSpace(report.Status))
                        //{
                        //    startStatusAndClause = "<And>";
                        //    EndStatusAndClause = "</And>";
                        //    appendStatusClause = @"<Eq>
                        //                               <FieldRef Name='Status' />
                        //                               <Value Type='Text'>" + report.Status + @"</Value>
                        //                            </Eq>";
                        //}

                        string paraStatusValue = string.Empty;
                        if (arrStatusFilter != null)
                        {
                            foreach (string item in arrStatusFilter)
                            {
                                paraStatusValue += "<Value Type='Text'>" + item + @"</Value>";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(paraStatusValue))
                        {
                            startStatusAndClause = "<And>";
                            endStatusAndClause = "</And>";

                            appendStatusClause = @"<In>
                                                    <FieldRef Name='Status' />
                                                    <Values>
                                                    " + paraStatusValue + @"
                                                    </Values>
                                                </In>";
                        }

                        string paraValue = string.Empty;
                        if (arrFilter != null)
                        {
                            foreach (string item in arrFilter)
                            {
                                paraValue += "<Value Type='Text'>" + item + @"</Value>";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(paraValue))
                        {
                            if (string.IsNullOrWhiteSpace(report.Status))
                            {
                                startStatusAndClause = "<And>";
                                endStatusAndClause = "</And>";
                            }
                            else
                            {
                                startAndClause = "<And>";
                                endAndClause = "</And>";
                            }
                            appendWhereClause = @"<In>
                                                    <FieldRef Name='PendingWith' />
                                                    <Values>
                                                    " + paraValue + @"
                                                    </Values>
                                                </In>";
                        }

                        query.ViewXml = @"<View>
                                        <Query> 
                                            <Where>
                                                <And>
                                                     <Geq>
                                                        <FieldRef Name='RequestDate' />
                                                        <Value IncludeTimeValue='FALSE' Type='DateTime'>" + fromDate + @"</Value>
                                                     </Geq>
                                                     " + startStatusAndClause + @"
                                                        <Leq>
                                                           <FieldRef Name='RequestDate' />
                                                           <Value IncludeTimeValue='FALSE' Type='DateTime'>" + toDate + @"</Value>
                                                        </Leq>
                                                       " + startAndClause + @"
                                                      " + appendStatusClause + @"
                                                      " + appendWhereClause + @"
                                                     " + endAndClause + @"
                                                    " + endStatusAndClause + @"
                                                  </And>
                                               </Where>                                                                                   
                                            <OrderBy>
                                                <FieldRef Name='ID' Ascending='True' />
                                            </OrderBy>                                            
                                        </Query>                                      
                                    </View>";
                        ListItemCollection items = statusList.GetItems(query);
                        this.context.Load(items);
                        this.context.ExecuteQuery();

                        if (items != null && items.Count > 0)
                        {
                            foreach (ListItem item in items)
                            {
                                ReportDetails reportDetails = new ReportDetails();
                                reportDetails.ICCPNo = Convert.ToString(item["Title"]);
                                reportDetails.ItemCode = Convert.ToString(item["ItemCode"]);
                                reportDetails.ItemDescription = Convert.ToString(item["ItemDescription"]);
                                reportDetails.BuyMake = Convert.ToString(item["BuyMake"]);
                                reportDetails.CostPrice = Convert.ToString(item["CostPrice"]);
                                reportDetails.GSTRate = Convert.ToString(item["GSTRate"]);
                                reportDetails.PendingWith = Convert.ToString(item["PendingWith"]);
                                reportDetails.ProductGroup = Convert.ToString(item["ProductGroup"]);
                                reportDetails.Quantity = Convert.ToString(item["Quantity"]);
                                reportDetails.Status = Convert.ToString(item["Status"]);
                                if (!string.IsNullOrWhiteSpace(reportDetails.Status))
                                {
                                    switch (reportDetails.Status.ToLower())
                                    {
                                        case "submitted":
                                            reportDetails.Status = "Pending";
                                            break;
                                        case "sent back":
                                            reportDetails.Status = "Rework";
                                            break;
                                    }
                                }
                                reportDetails.TypeofPackaging = Convert.ToString(item["TypeOfPackaging"]);
                                reportDetails.CreatedBy = BELDataAccessLayer.GetPersonValueFromPersonField(this.context, this.web, (FieldUserValue)item["ProposedBy"], Person.Name);
                                reportDetails.CreatedDate = Convert.ToString(item["RequestDate"]);
                                reportList.Add(reportDetails);
                            }
                        }
                    }
                }
                reportList.RemoveAll(m => m.Status == "Draft");
                return reportList;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetRoleList()
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
                roleList.RemoveAll(m => m.Value == "Admin");
                return roleList;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while get role list: Message = " + ex.Message + " ,Stack Trace = " + ex.StackTrace);
                return roleList;
            }
        }

        /// <summary>
        /// Gets the status list.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetStatusList()
        {
            List<NameValueData> statuses = new List<NameValueData>();
            Dictionary<string, string> statusArray = new Dictionary<string, string>();
            statusArray.Add("Completed", "Completed");
            statusArray.Add("Sent Back", "Rework");
            statusArray.Add("Submitted", "Pending");

            foreach (var item in statusArray)
            {
                NameValueData status = new NameValueData();
                status.Name = Convert.ToString(item.Key);
                status.Value = Convert.ToString(item.Value);
                statuses.Add(status);
            }
            //List statusList = this.web.Lists.GetByTitle(ICCPListNames.ICCPMAINLIST);
            //if (statusList != null)
            //{
            //    CamlQuery query = new CamlQuery();

            //    query.ViewXml = @"<View>
            //                            <Query>                                                                                    
            //                                <OrderBy>
            //                                    <FieldRef Name='ID' Ascending='True' />
            //                                </OrderBy>
            //                                <FieldRef Name='WorkflowStatus' />
            //                            </Query>                                      
            //                        </View>";
            //    ListItemCollection items = statusList.GetItems(query);
            //    this.context.Load(items);
            //    this.context.ExecuteQuery();

            //    if (items != null && items.Count > 0)
            //    {
            //        var distinctItems = items.ToList().Select(i => i.FieldValues["WorkflowStatus"]).Distinct().ToList();
            //        foreach (ListItem item in distinctItems)
            //        {
            //            NameValueData status = new NameValueData();
            //            status.Name = Convert.ToString(item["WorkflowStatus"]);
            //            status.Value = Convert.ToString(item["WorkflowStatus"]);
            //            statuses.Add(status);
            //        }
            //    }
            //}
            return statuses;
        }
    }
}