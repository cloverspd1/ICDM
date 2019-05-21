namespace BEL.ItemCodeCreationPreProcess.BusinessLayer
{
    using BEL.CommonDataContract;
    using BEL.DataAccessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using Microsoft.SharePoint.Client;
    using Models.ItemCode;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Item Code Creation Business Layer
    /// </summary>
    public class ItemCodeCreationBusinessLayer
    {
        /// <summary>
        /// The lazy
        /// </summary>
        private static readonly Lazy<ItemCodeCreationBusinessLayer> lazy = new Lazy<ItemCodeCreationBusinessLayer>(() => new ItemCodeCreationBusinessLayer());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ItemCodeCreationBusinessLayer Instance
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
        /// Prevents a default instance of the <see cref="ItemCodeCreationBusinessLayer"/> class from being created.
        /// </summary>
        private ItemCodeCreationBusinessLayer()
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
        /// Gets the item code creation details.
        /// </summary>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "Reviewed")]
        public ItemCodeContract GetItemCodeCreationDetails(Dictionary<string, string> objDict)
        {
            ItemCodeContract contract = new ItemCodeContract();

            if (objDict != null && objDict.ContainsKey(Parameter.FORMNAME) && objDict.ContainsKey(Parameter.ITEMID) && objDict.ContainsKey(Parameter.USEREID) && objDict.ContainsKey(Parameter.USERNAME))
            {
                string formName = objDict[Parameter.FORMNAME];
                int itemId = Convert.ToInt32(objDict[Parameter.ITEMID]);
                string userID = objDict[Parameter.USEREID];
                string userName = objDict[Parameter.USERNAME];
                IForm itemCodeForm = new ItemCodeForm(true);
                itemCodeForm = BELDataAccessLayer.Instance.GetFormData(this.context, this.web, ApplicationNameConstants.LUMITEMCODECREATION, formName, itemId, userID, itemCodeForm);
                if (itemCodeForm != null && itemCodeForm.SectionsList != null && itemCodeForm.SectionsList.Count > 0)
                {
                    var lumMktInchargeSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.LUMMKTINCHARGESECTION)) as LUMMktInchargeSection;
                    //var approvalMatrix = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(SectionNameConstant.APPLICATIONSTATUS)) as ApplicationStatusSection;
                    ProductGroupMaster productCategoryMaster = GetProductCategoryMasterFromNPD();
                    string itemDescription = string.Empty;
                    if (lumMktInchargeSection != null && lumMktInchargeSection.MasterData != null)
                    {
                        lumMktInchargeSection.MasterData.Add(productCategoryMaster);

                        if (itemId == 0)
                        {
                            List<IMasterItem> approvers = lumMktInchargeSection.MasterData.FirstOrDefault(p => p.NameOfMaster == Masters.APPROVERMASTER).Items;
                            if (approvers != null && approvers.Any())
                            {
                                List<ApproverMasterListItem> approverList = approvers.ConvertAll(p => (ApproverMasterListItem)p);
                                if (approverList != null && approverList.Any())
                                {
                                    approverList = approverList.Where(m => m.UserSelection).ToList();
                                    ////Check Current User Role is LUM Mkt. Incharge from Approver Master
                                    bool isValidRequester = false;
                                    foreach (ApproverMasterListItem listItem in approverList)
                                    {
                                        if (listItem.Role == ICCPRoles.LUMMARKETINGINCHARGE && listItem.UserID.Split(',').Contains(userID))
                                        {
                                            isValidRequester = true;
                                            break;
                                        }
                                    }
                                    if (isValidRequester)
                                    {
                                        lumMktInchargeSection.ProposedBy = userID;
                                        lumMktInchargeSection.ProposedByName = userName;
                                        lumMktInchargeSection.RequestDate = DateTime.Now;
                                        ////Get Approvers from Approver Master List
                                        lumMktInchargeSection.ApproversList.ForEach(p =>
                                        {
                                            if (p.Role == ICCPRoles.CREATOR)
                                            {
                                                p.Approver = userID;
                                                p.ApproverName = userName;
                                            }
                                            else if (p.Role == ICCPRoles.SCMLUMDESIGNINCHARGE)
                                            {
                                                p.Approver = approverList.Where(q => q.Role == ICCPRoles.SCMLUMDESIGNINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.SCMLUMDESIGNINCHARGE).UserID : string.Empty;
                                                p.ApproverName = approverList.Where(q => q.Role == ICCPRoles.SCMLUMDESIGNINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.SCMLUMDESIGNINCHARGE).UserName : string.Empty;
                                            }
                                            else if (p.Role == ICCPRoles.SMSINCHARGE || p.Role == ICCPRoles.FINALSMSINCHARGE)
                                            {
                                                p.Approver = approverList.Where(q => q.Role == ICCPRoles.SMSINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.SMSINCHARGE).UserID : string.Empty;
                                                p.ApproverName = approverList.Where(q => q.Role == ICCPRoles.SMSINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.SMSINCHARGE).UserName : string.Empty;
                                            }
                                            else if (p.Role == ICCPRoles.QAINCHARGE)
                                            {
                                                p.Approver = approverList.Where(q => q.Role == ICCPRoles.QAINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.QAINCHARGE).UserID : string.Empty;
                                                p.ApproverName = approverList.Where(q => q.Role == ICCPRoles.QAINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.QAINCHARGE).UserName : string.Empty;
                                            }
                                            else if (p.Role == ICCPRoles.COSTINGINCHARGE)
                                            {
                                                p.Approver = approverList.Where(q => q.Role == ICCPRoles.COSTINGINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.COSTINGINCHARGE).UserID : string.Empty;
                                                p.ApproverName = approverList.Where(q => q.Role == ICCPRoles.COSTINGINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.COSTINGINCHARGE).UserName : string.Empty;
                                            }
                                            else if (p.Role == ICCPRoles.TDSINCHARGE)
                                            {
                                                p.Approver = approverList.Where(q => q.Role == ICCPRoles.TDSINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.TDSINCHARGE).UserID : string.Empty;
                                                p.ApproverName = approverList.Where(q => q.Role == ICCPRoles.TDSINCHARGE).Any() ? approverList.FirstOrDefault(q => q.Role == ICCPRoles.TDSINCHARGE).UserName : string.Empty;
                                            }
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            lumMktInchargeSection.ApproversList.Remove(lumMktInchargeSection.ApproversList.FirstOrDefault(p => p.Role == UserRoles.VIEWER));
                        }
                        itemDescription = lumMktInchargeSection.ItemDescription;
                    }

                    var lumMktDelegateSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.LUMMKTDELEGATEESECTION)) as LUMMktDelegateSection;

                    if (lumMktDelegateSection != null)
                    {
                        if (lumMktDelegateSection.MasterData != null)
                            lumMktDelegateSection.MasterData.Add(productCategoryMaster);
                        itemDescription = lumMktDelegateSection.ItemDescription;
                    }

                    var scmLUMDesignInchargeSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.SCMLUMDESIGNINCHARGESECTION)) as SCMLUMDesignInchargeSection;
                    if (scmLUMDesignInchargeSection != null)
                    {
                        scmLUMDesignInchargeSection.UploadArtworkAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.UploadArtworkAttachment) && scmLUMDesignInchargeSection.UploadArtworkAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.UploadArtworkAttachment) && scmLUMDesignInchargeSection.UploadArtworkAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.BOMAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.BOMAttachment) && scmLUMDesignInchargeSection.BOMAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.BOMAttachment) && scmLUMDesignInchargeSection.BOMAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.ADSAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.ADSAttachment) && scmLUMDesignInchargeSection.ADSAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.ADSAttachment) && scmLUMDesignInchargeSection.ADSAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.SpecificationSheetAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.SpecificationSheetAttachment) && scmLUMDesignInchargeSection.SpecificationSheetAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.SpecificationSheetAttachment) && scmLUMDesignInchargeSection.SpecificationSheetAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.PackagingDataSheetAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.PackagingDataSheetAttachment) && scmLUMDesignInchargeSection.PackagingDataSheetAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.PackagingDataSheetAttachment) && scmLUMDesignInchargeSection.PackagingDataSheetAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.IESFileAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.IESFileAttachment) && scmLUMDesignInchargeSection.IESFileAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.IESFileAttachment) && scmLUMDesignInchargeSection.IESFileAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.LM79Attachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.LM79Attachment) && scmLUMDesignInchargeSection.LM79Attachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.LM79Attachment) && scmLUMDesignInchargeSection.LM79Attachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignInchargeSection.ProductDrawingAttachment = scmLUMDesignInchargeSection.Files != null && scmLUMDesignInchargeSection.Files.Count > 0 ? (scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.ProductDrawingAttachment) && scmLUMDesignInchargeSection.ProductDrawingAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignInchargeSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignInchargeSection.ProductDrawingAttachment) && scmLUMDesignInchargeSection.ProductDrawingAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;

                        if (!string.IsNullOrWhiteSpace(scmLUMDesignInchargeSection.Vendors))
                        {
                            scmLUMDesignInchargeSection.VendorList = this.GetSelectedVendorsFromMainList(scmLUMDesignInchargeSection.Vendors);
                        }
                    }
                    var scmLUMDesignDelegateSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.SCMLUMDESIGNDELEGATESECTION)) as SCMLUMDesignDelegateSection;
                    if (scmLUMDesignDelegateSection != null)
                    {
                        scmLUMDesignDelegateSection.UploadArtworkAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.UploadArtworkAttachment) && scmLUMDesignDelegateSection.UploadArtworkAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.UploadArtworkAttachment) && scmLUMDesignDelegateSection.UploadArtworkAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.BOMAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.BOMAttachment) && scmLUMDesignDelegateSection.BOMAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.BOMAttachment) && scmLUMDesignDelegateSection.BOMAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.ADSAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.ADSAttachment) && scmLUMDesignDelegateSection.ADSAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.ADSAttachment) && scmLUMDesignDelegateSection.ADSAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.SpecificationSheetAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.SpecificationSheetAttachment) && scmLUMDesignDelegateSection.SpecificationSheetAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.SpecificationSheetAttachment) && scmLUMDesignDelegateSection.SpecificationSheetAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.PackagingDataSheetAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.PackagingDataSheetAttachment) && scmLUMDesignDelegateSection.PackagingDataSheetAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.PackagingDataSheetAttachment) && scmLUMDesignDelegateSection.PackagingDataSheetAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.IESFileAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.IESFileAttachment) && scmLUMDesignDelegateSection.IESFileAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.IESFileAttachment) && scmLUMDesignDelegateSection.IESFileAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.LM79Attachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.LM79Attachment) && scmLUMDesignDelegateSection.LM79Attachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.LM79Attachment) && scmLUMDesignDelegateSection.LM79Attachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        scmLUMDesignDelegateSection.ProductDrawingAttachment = scmLUMDesignDelegateSection.Files != null && scmLUMDesignDelegateSection.Files.Count > 0 ? (scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.ProductDrawingAttachment) && scmLUMDesignDelegateSection.ProductDrawingAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(scmLUMDesignDelegateSection.Files.Where(x => !string.IsNullOrEmpty(scmLUMDesignDelegateSection.ProductDrawingAttachment) && scmLUMDesignDelegateSection.ProductDrawingAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        if (!string.IsNullOrWhiteSpace(scmLUMDesignDelegateSection.Vendors))
                        {
                            scmLUMDesignDelegateSection.VendorList = this.GetSelectedVendorsFromMainList(scmLUMDesignDelegateSection.Vendors);
                        }
                    }

                    var qaInchargeSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.QAINCHARGESECTION)) as QAInchargeSection;
                    if (qaInchargeSection != null)
                    {
                        qaInchargeSection.ReliabilityTestReportAttachment = qaInchargeSection.Files != null && qaInchargeSection.Files.Count > 0 ? (qaInchargeSection.Files.Where(x => !string.IsNullOrEmpty(qaInchargeSection.ReliabilityTestReportAttachment) && qaInchargeSection.ReliabilityTestReportAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(qaInchargeSection.Files.Where(x => !string.IsNullOrEmpty(qaInchargeSection.ReliabilityTestReportAttachment) && qaInchargeSection.ReliabilityTestReportAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        // qaInchargeSection.JointInspectionAttachment = qaInchargeSection.Files != null && qaInchargeSection.Files.Count > 0 ? (qaInchargeSection.Files.Where(x => !string.IsNullOrEmpty(qaInchargeSection.JointInspectionAttachment) && qaInchargeSection.JointInspectionAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(qaInchargeSection.Files.Where(x => !string.IsNullOrEmpty(qaInchargeSection.JointInspectionAttachment) && qaInchargeSection.JointInspectionAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        if (qaInchargeSection.ReworkCount >= Convert.ToInt32(BELDataAccessLayer.Instance.GetConfigVariable("ReworkCountLimit")))
                        {
                            if (itemCodeForm.Buttons.Where(m => m.ButtonStatus == ButtonActionStatus.SendBack).Any())
                            {
                                itemCodeForm.Buttons.FirstOrDefault(m => m.ButtonStatus == ButtonActionStatus.SendBack).IsVisible = false;
                            }
                        }
                    }
                    var qaDelegateSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.QADELEGATESECTION)) as QADelegateSection;
                    if (qaDelegateSection != null)
                    {
                        qaDelegateSection.ReliabilityTestReportAttachment = qaDelegateSection.Files != null && qaDelegateSection.Files.Count > 0 ? (qaDelegateSection.Files.Where(x => !string.IsNullOrEmpty(qaDelegateSection.ReliabilityTestReportAttachment) && qaDelegateSection.ReliabilityTestReportAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(qaDelegateSection.Files.Where(x => !string.IsNullOrEmpty(qaDelegateSection.ReliabilityTestReportAttachment) && qaDelegateSection.ReliabilityTestReportAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        //    qaDelegateSection.JointInspectionAttachment = qaDelegateSection.Files != null && qaDelegateSection.Files.Count > 0 ? (qaDelegateSection.Files.Where(x => !string.IsNullOrEmpty(qaDelegateSection.JointInspectionAttachment) && qaDelegateSection.JointInspectionAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(qaDelegateSection.Files.Where(x => !string.IsNullOrEmpty(qaDelegateSection.JointInspectionAttachment) && qaDelegateSection.JointInspectionAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                        if (qaDelegateSection.ReworkCount >= Convert.ToInt32(BELDataAccessLayer.Instance.GetConfigVariable("ReworkCountLimit")))
                        {
                            if (itemCodeForm.Buttons.Where(m => m.ButtonStatus == ButtonActionStatus.SendBack).Any())
                            {
                                itemCodeForm.Buttons.FirstOrDefault(m => m.ButtonStatus == ButtonActionStatus.SendBack).IsVisible = false;
                            }
                        }
                    }

                    var costingDelegate2Section = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.COSTINGDELEGATE2SECTION)) as CostingDelegate2Section;
                    if (costingDelegate2Section != null)
                    {
                        costingDelegate2Section.ItemDescription = itemDescription;
                    }

                    var tdsInchargeSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.TDSINCHARGESECTION)) as TDSInchargeSection;
                    if (tdsInchargeSection != null)
                    {
                        tdsInchargeSection.TDSAttachment = tdsInchargeSection.Files != null && tdsInchargeSection.Files.Count > 0 ? (tdsInchargeSection.Files.Where(x => !string.IsNullOrEmpty(tdsInchargeSection.TDSAttachment) && tdsInchargeSection.TDSAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(tdsInchargeSection.Files.Where(x => !string.IsNullOrEmpty(tdsInchargeSection.TDSAttachment) && tdsInchargeSection.TDSAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                    }
                    var tdsDelegateSection = itemCodeForm.SectionsList.FirstOrDefault(f => f.SectionName.Equals(ICCPSectionName.TDSDELEGATESECTION)) as TDSDelegateSection;
                    if (tdsDelegateSection != null)
                    {
                        tdsDelegateSection.TDSAttachment = tdsDelegateSection.Files != null && tdsDelegateSection.Files.Count > 0 ? (tdsDelegateSection.Files.Where(x => !string.IsNullOrEmpty(tdsDelegateSection.TDSAttachment) && tdsDelegateSection.TDSAttachment.Split(',').Contains(x.FileName)).ToList().Any() ? JsonConvert.SerializeObject(tdsDelegateSection.Files.Where(x => !string.IsNullOrEmpty(tdsDelegateSection.TDSAttachment) && tdsDelegateSection.TDSAttachment.Split(',').Contains(x.FileName)).ToList()) : string.Empty) : string.Empty;
                    }
                    contract.Forms.Add(itemCodeForm);
                }
            }
            return contract;
        }

        /// <summary>
        /// Gets the selected vendors from main list.
        /// </summary>
        /// <param name="vendors">The vendors.</param>
        /// <returns></returns>
        private List<NameValueData> GetSelectedVendorsFromMainList(string vendors)
        {
            try
            {
                List<NameValueData> vendorList = new List<NameValueData>();
                if (!string.IsNullOrWhiteSpace(vendors))
                {
                    string[] vendorIds = vendors.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < vendorIds.Length; i++)
                    {
                        List spList = this.web.Lists.GetByTitle(Masters.VENDORMASTERLIST);
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = @"<View>
                                         <Query>
                                                <Where>
                                                      <Eq>
                                                         <FieldRef Name='ID'/>
                                                         <Value Type='Number'>" + Convert.ToInt32(vendorIds[i]) + @"</Value>
                                                       </Eq>
                                                  </Where>
                                            </Query>
                                            </View>";
                        ListItemCollection items = spList.GetItems(query);
                        this.context.Load(items);
                        this.context.ExecuteQuery();
                        if (items != null && items.Count != 0)
                        {
                            foreach (ListItem item in items)
                            {
                                NameValueData obj = new NameValueData();

                                obj.Name = Convert.ToString(item["ID"]);
                                obj.Value = item["VendorCode"] + " - " + item["Title"];

                                vendorList.Add(obj);
                            }
                        }
                    }
                    return vendorList;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Saves the by section.
        /// </summary>
        /// <param name="sectionDetails">The section details.</param>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public ActionStatus SaveBySection(ISection sectionDetails, Dictionary<string, string> objDict)
        {
            lock (Padlock)
            {
                ActionStatus status = new ActionStatus();
                ItemCodeNoCount currentItemCodeNo = null;
                if (sectionDetails != null && objDict != null)
                {
                    objDict[Parameter.ACTIVITYLOG] = ICCPListNames.ITEMCODEACTIVITYLOG;
                    objDict[Parameter.APPLICATIONNAME] = ApplicationNameConstants.LUMITEMCODECREATION;
                    objDict[Parameter.FORMNAME] = FormNameConstants.ICCPForm;
                    LUMMktInchargeSection lumktInchargeSection = null;
                    ////AppApprovalMatrix approvalMatrix = ;
                    if (sectionDetails.SectionName == ICCPSectionName.LUMMKTINCHARGESECTION)
                    {
                        lumktInchargeSection = sectionDetails as LUMMktInchargeSection;
                        if (string.IsNullOrEmpty(lumktInchargeSection.ICCPNo) && sectionDetails.ActionStatus == ButtonActionStatus.SaveAsDraft)
                        {
                            lumktInchargeSection.ICCPNo = "View";
                        }
                        else if ((sectionDetails.ActionStatus == ButtonActionStatus.Delegate || sectionDetails.ActionStatus == ButtonActionStatus.NextApproval) && (string.IsNullOrEmpty(lumktInchargeSection.ICCPNo) || lumktInchargeSection.ICCPNo == "View"))
                        {
                            currentItemCodeNo = this.GetItemCodeNo();
                            lumktInchargeSection.RequestDate = DateTime.Now;
                            if (currentItemCodeNo != null)
                            {
                                currentItemCodeNo.CurrentValue = currentItemCodeNo.CurrentValue + 1;
                                Logger.Info("Item Code No Current Value + 1 = " + currentItemCodeNo.CurrentValue);
                                lumktInchargeSection.ICCPNo = string.Format("{0}-{1}-{2}-{3}", ICCPCommonConstant.ICCPREFERENCENOPREFIX, lumktInchargeSection.RequestDate.Value.ToString("yyyy"), lumktInchargeSection.RequestDate.Value.ToString("MM"), string.Format("{0:00000}", currentItemCodeNo.CurrentValue));
                                Logger.Info("Item Code No is " + lumktInchargeSection.ICCPNo);
                                status.ExtraData = lumktInchargeSection.ICCPNo;
                            }
                        }
                    }

                    if (sectionDetails.SectionName == ICCPSectionName.SCMLUMDESIGNINCHARGESECTION)
                    {
                        SCMLUMDesignInchargeSection scmLUMInchargeSection = sectionDetails as SCMLUMDesignInchargeSection;

                        if (scmLUMInchargeSection != null && (scmLUMInchargeSection.ActionStatus == ButtonActionStatus.NextApproval || scmLUMInchargeSection.ActionStatus == ButtonActionStatus.Submit || scmLUMInchargeSection.ActionStatus == ButtonActionStatus.Delegate) && (!string.IsNullOrWhiteSpace(scmLUMInchargeSection.WorkflowStatus) && scmLUMInchargeSection.WorkflowStatus.ToUpper() == "SENT BACK BY QA DELEGATE"))
                        {
                            scmLUMInchargeSection.ApproversList = this.UpdateApproverMatrixForQADelegateReworkCase(sectionDetails.ListDetails[0].ItemId);
                        }
                    }

                    if (sectionDetails.SectionName == ICCPSectionName.QAINCHARGESECTION)
                    {
                        QAInchargeSection qaInchargeSection = sectionDetails as QAInchargeSection;

                        if (qaInchargeSection != null && qaInchargeSection.ActionStatus == ButtonActionStatus.SendBack)
                        {
                            qaInchargeSection.ApproversList = this.UpdateApproverMatrixForReworkCase(sectionDetails.ListDetails[0].ItemId);
                        }
                    }

                    if (sectionDetails.SectionName == ICCPSectionName.QADELEGATESECTION)
                    {
                        QADelegateSection qaDelegateSection = sectionDetails as QADelegateSection;
                        if (qaDelegateSection != null && qaDelegateSection.ActionStatus == ButtonActionStatus.SendBack)
                        {
                            qaDelegateSection.ApproversList = this.UpdateApproverMatrixForReworkCase(sectionDetails.ListDetails[0].ItemId);
                        }
                    }

                    List<ListItemDetail> objSaveDetails = BELDataAccessLayer.Instance.SaveData(this.context, this.web, sectionDetails, objDict);
                    ListItemDetail itemDetails = objSaveDetails.Where(p => p.ListName.Equals(ICCPListNames.ICCPMAINLIST)).FirstOrDefault<ListItemDetail>();
                    if (sectionDetails.SectionName == ICCPSectionName.LUMMKTINCHARGESECTION)
                    {
                        lumktInchargeSection = sectionDetails as LUMMktInchargeSection;
                        if (lumktInchargeSection != null && !string.IsNullOrEmpty(lumktInchargeSection.OldICCPNo))
                        {
                            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();
                            values.Add("IsICCPRequestRetrieved", true);
                            BELDataAccessLayer.Instance.SaveFormFields(this.context, this.web, ICCPListNames.ICCPMAINLIST, lumktInchargeSection.OldICCPId, values);
                        }
                        if (itemDetails.ItemId > 0 && currentItemCodeNo != null)
                        {
                            this.UpdateItemCodeNoCount(currentItemCodeNo);
                            Logger.Info("Update Item Code No " + lumktInchargeSection.ICCPNo);
                        }
                    }
                    if (itemDetails.ItemId > 0)
                    {
                        status.IsSucceed = true;
                        status.ItemID = itemDetails.ItemId;
                        switch (sectionDetails.ActionStatus)
                        {
                            case ButtonActionStatus.SaveAsDraft:
                                status.Messages.Add("Text_SaveDraftSuccess");
                                break;
                            case ButtonActionStatus.SaveAndNoStatusUpdate:
                                status.Messages.Add("Text_SaveSuccess");
                                break;
                            case ButtonActionStatus.NextApproval:
                                status.Messages.Add(ApplicationConstants.SUCCESSMESSAGE);
                                break;
                            case ButtonActionStatus.Delegate:
                                status.Messages.Add("Text_DelegatedSuccess");
                                break;
                            case ButtonActionStatus.Complete:
                                status.Messages.Add("Text_CompleteSuccess");
                                break;
                            case ButtonActionStatus.Rejected:
                                status.Messages.Add("Text_RejectedSuccess");
                                break;
                            case ButtonActionStatus.SendBack:
                                status.Messages.Add("Text_Rework");
                                break;
                            case ButtonActionStatus.Hold:
                                status.Messages.Add("Text_HoldSuccess");
                                status.ItemID = itemDetails.ItemId;
                                break;
                            case ButtonActionStatus.Resume:
                                status.Messages.Add("Text_ResumeSuccess");
                                status.ItemID = itemDetails.ItemId;
                                break;
                            default:
                                status.Messages.Add(ApplicationConstants.SUCCESSMESSAGE);
                                break;
                        }
                    }
                    else
                    {
                        status.IsSucceed = false;
                        status.Messages.Add(ApplicationConstants.ERRORMESSAGE);
                    }
                }
                return status;
            }
        }

        /// <summary>
        /// Gets the approver matrix for rework case.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns>
        /// List of ApplicationStatus
        /// </returns>
        private List<ApplicationStatus> UpdateApproverMatrixForReworkCase(int itemId)
        {
            AppApprovalMatrixHelper approverHelper = new AppApprovalMatrixHelper();
            List<ApplicationStatus> appApproverMatrix = approverHelper.GetAppApprovalMatrix(this.context, this.web, itemId, ICCPListNames.ITEMCODEAPPROVALMATRIX);
            appApproverMatrix = appApproverMatrix.Where(p => p.Role == ICCPRoles.SCMLUMDESIGNDELEGATE || p.Role == ICCPRoles.SMSDELEGATE || p.Role == ICCPRoles.QADELEGATE).ToList();
            if (appApproverMatrix != null && appApproverMatrix.Count > 0)
            {
                List list = web.Lists.GetByTitle(ICCPListNames.ITEMCODEAPPROVALMATRIX);
                var approverItems = new ListItem[appApproverMatrix.Count];
                int i = 0;
                foreach (ApplicationStatus approveritem in appApproverMatrix)
                {
                    if (approveritem.ID > 0)
                    {
                        Logger.Info("Calling SaveAproverMatrix for Rework Case -> " + appApproverMatrix + "=>ID=" + approveritem.ID);
                        approverItems[i] = list.GetItemById(approveritem.ID);
                        approverItems[i]["Approver"] = string.Empty;
                        approverItems[i]["Status"] = ApproverStatus.NOTASSIGNED;
                        approverItems[i]["DueDate"] = null;
                        approverItems[i]["AssignDate"] = null;
                        approverItems[i].Update();
                        this.context.ExecuteQuery();

                        approveritem.Approver = string.Empty;
                        approveritem.Status = ApproverStatus.NOTASSIGNED;
                        approveritem.DueDate = null;
                        approveritem.AssignDate = null;

                        Logger.Info("Calling SaveAproverMatrix  for Rework Case-> Complete -> " + appApproverMatrix + "=>ID=" + approveritem.ID);
                        i++;
                        Logger.Info("Calling SaveAproverMatrix  for Rework Case-> Complete -> " + appApproverMatrix + "=>i" + i);
                    }
                }
            }
            return appApproverMatrix;
        }

        /// <summary>
        /// Gets the approver matrix for rework case.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns>
        /// List of ApplicationStatus
        /// </returns>
        private List<ApplicationStatus> UpdateApproverMatrixForQADelegateReworkCase(int itemId)
        {
            AppApprovalMatrixHelper approverHelper = new AppApprovalMatrixHelper();
            List<ApplicationStatus> appApproverMatrix = approverHelper.GetAppApprovalMatrix(this.context, this.web, itemId, ICCPListNames.ITEMCODEAPPROVALMATRIX);
            appApproverMatrix = appApproverMatrix.Where(p => p.Role == ICCPRoles.QADELEGATE).ToList();
            if (appApproverMatrix != null && appApproverMatrix.Count > 0)
            {
                List list = web.Lists.GetByTitle(ICCPListNames.ITEMCODEAPPROVALMATRIX);
                var approverItems = new ListItem[appApproverMatrix.Count];
                int i = 0;
                foreach (ApplicationStatus approveritem in appApproverMatrix)
                {
                    if (approveritem.ID > 0)
                    {
                        Logger.Info("Calling SaveAproverMatrix for Rework Case -> " + appApproverMatrix + "=>ID=" + approveritem.ID);
                        approverItems[i] = list.GetItemById(approveritem.ID);
                        approverItems[i]["Approver"] = string.Empty;
                        approverItems[i]["Status"] = ApproverStatus.NOTASSIGNED;
                        approverItems[i]["DueDate"] = null;
                        approverItems[i]["AssignDate"] = null;
                        approverItems[i].Update();
                        this.context.ExecuteQuery();

                        approveritem.Approver = string.Empty;
                        approveritem.Status = ApproverStatus.NOTASSIGNED;
                        approveritem.DueDate = null;
                        approveritem.AssignDate = null;

                        Logger.Info("Calling SaveAproverMatrix  for Rework Case-> Complete -> " + appApproverMatrix + "=>ID=" + approveritem.ID);
                        i++;
                        Logger.Info("Calling SaveAproverMatrix  for Rework Case-> Complete -> " + appApproverMatrix + "=>i" + i);
                    }
                }
            }
            return appApproverMatrix;
        }

        /// <summary>
        /// Gets the item code no.
        /// </summary>
        /// <returns></returns>
        public ItemCodeNoCount GetItemCodeNo()
        {
            try
            {
                List<ItemCodeNoCount> lstItemCodeCount = new List<ItemCodeNoCount>();
                List spList = this.web.Lists.GetByTitle(ICCPListNames.ITEMCODENOCOUNT);
                CamlQuery query = new CamlQuery();
                query.ViewXml = @"<View><ViewFields><FieldRef Name='Title' /><FieldRef Name='Year' /><FieldRef Name='CurrentValue' /></ViewFields></View>";
                ListItemCollection items = spList.GetItems(query);
                this.context.Load(items);
                this.context.ExecuteQuery();
                if (items != null && items.Count != 0)
                {
                    foreach (ListItem item in items)
                    {
                        ItemCodeNoCount obj = new ItemCodeNoCount();
                        obj.ID = item.Id;
                        obj.Title = Convert.ToString(item["Title"]);
                        obj.Year = Convert.ToInt32(item["Year"]);
                        obj.CurrentValue = Convert.ToInt32(item["CurrentValue"]);
                        if (obj.Year != DateTime.Today.Year)
                        {
                            obj.Year = DateTime.Today.Year;
                            obj.CurrentValue = 0;
                        }
                        lstItemCodeCount.Add(obj);
                    }
                }
                if (lstItemCodeCount != null)
                {
                    return lstItemCodeCount.FirstOrDefault(p => p.Year == DateTime.Today.Year);
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Updates the item code no count.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        public void UpdateItemCodeNoCount(ItemCodeNoCount currentValue)
        {
            if (currentValue != null && currentValue.ID != 0)
            {
                try
                {
                    Logger.Info("Async update Item Code No Current value currentValue : " + currentValue.CurrentValue + " Title : " + currentValue.Title);
                    List spList = this.web.Lists.GetByTitle(ICCPListNames.ITEMCODENOCOUNT);
                    ListItem item = spList.GetItemById(currentValue.ID);

                    item.RefreshLoad(); // Pooja Atkotiya - Added for Version Conflict!
                    this.context.Load(item);
                    this.context.ExecuteQuery();

                    item["CurrentValue"] = currentValue.CurrentValue;
                    item["Year"] = currentValue.Year;
                    item.Update();
                    //Version conflict error
                    item.RefreshLoad(); // Pooja Atkotiya - Added for Version Conflict!
                    this.context.Load(item);
                    this.context.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while Update Item Code No Current Value");
                    Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// Gets the vendor master data.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public string GetVendorMasterData(string title)
        {
            MasterDataHelper masterHelper = new MasterDataHelper();
            IMaster vendormaster = masterHelper.GetMasterData(this.context, this.web, new List<IMaster>() { new VendorMaster() }).FirstOrDefault();
            string vendorjson = JSONHelper.ToJSON<IMaster>(vendormaster);
            var filterdVendor = JSONHelper.ToObject<IMaster>(vendorjson);
            if (filterdVendor != null)
            {
                filterdVendor.Items = filterdVendor.Items.Where(x => (x.Title ?? string.Empty).ToLower().Trim().Contains((title ?? string.Empty).ToLower().Trim()) || (x.Value ?? string.Empty).ToLower().Trim().Contains((title ?? string.Empty).ToLower().Trim())).ToList();
                return JSONHelper.ToJSON<IMaster>(filterdVendor);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the product category master from NPD.
        /// </summary>
        /// <returns></returns>
        public ProductGroupMaster GetProductCategoryMasterFromNPD()
        {
            try
            {
                List<IMasterItem> masterItems = new List<IMasterItem>();
                ProductGroupMaster productGroupMaster = new ProductGroupMaster();
                string siteURL = BELDataAccessLayer.Instance.GetSiteURL(SiteURLs.NPDSITEURL);
                if (!string.IsNullOrEmpty(siteURL))
                {
                    ClientContext npdClientContext = BELDataAccessLayer.Instance.CreateClientContext(siteURL);
                    Web npdWeb = BELDataAccessLayer.Instance.CreateWeb(npdClientContext);
                    List spList = npdWeb.Lists.GetByTitle(NPDListNames.PRODUCTCATEGORYMASTERLIST);
                    CamlQuery query = new CamlQuery();
                    query.ViewXml = @"<View>
                                         <Query>
                                                <Where>
                                                      <Eq>
                                                         <FieldRef Name='BU'/>
                                                         <Value Type='Text'>LUM</Value>
                                                       </Eq>
                                                  </Where>
                                            </Query>
                                            </View>";
                    ListItemCollection items = spList.GetItems(query);
                    npdClientContext.Load(items);
                    npdClientContext.ExecuteQuery();
                    if (items != null && items.Count != 0)
                    {
                        foreach (ListItem item in items)
                        {
                            IMasterItem obj = new ProductGroupMasterListItem();
                            obj.Title = Convert.ToString(item["ProductCategoryCode"] + " - " + item["Title"]);
                            obj.Value = Convert.ToString(item["ProductCategoryCode"] + " - " + item["Title"]);
                            masterItems.Add(obj);
                        }
                        productGroupMaster.Items = masterItems;
                        return productGroupMaster;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        #region Approver Replacement 

        /// <summary>
        /// Gets the pending with whom user list.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        private List<NameValueData> GetPendingWithWhomUserList(string roleName)
        {
            try
            {
                List<NameValueData> pendingWithWhomList = new List<NameValueData>();
                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    List spList = this.web.Lists.GetByTitle(ICCPListNames.ITEMCODEAPPROVALMATRIX);
                    CamlQuery query = new CamlQuery();
                    query.ViewXml = @"<View>
                                         <Query>
                                                <Where>
                                                     <And>
                                                         <Eq>
                                                            <FieldRef Name='Role' />
                                                            <Value Type='Text'>" + roleName + @"</Value>
                                                         </Eq>                                                       
                                                            <And>
                                                                <Neq>
                                                                   <FieldRef Name='Status' />
                                                                   <Value Type='Text'>Approved</Value>
                                                                </Neq>
                                                                 <And>
                                                                    <Neq>
                                                                       <FieldRef Name='Status' />
                                                                       <Value Type='Text'>Completed</Value>
                                                                    </Neq>  
                                                                     <And>                                                                                                                            
                                                                        <Neq>
                                                                           <FieldRef Name='Status' />
                                                                           <Value Type='Text'>Not Required</Value>
                                                                        </Neq> 
                                                                         <IsNotNull>
                                                                              <FieldRef Name='Approver' />
                                                                         </IsNotNull>      
                                                                </And>   
                                                            </And>
                                                         </And>
                                                      </And>
                                                  </Where>
                                            </Query>
                                            <ViewFields><FieldRef Name='Approver' /></ViewFields>
                                     </View>";
                    ListItemCollection items = spList.GetItems(query);
                    this.context.Load(items);
                    this.context.ExecuteQuery();
                    if (items != null && items.Count != 0)
                    {
                        foreach (ListItem item in items)
                        {
                            NameValueData obj = new NameValueData();
                            foreach (FieldUserValue user in (FieldUserValue[])item["Approver"])
                            {
                                string approverName = BELDataAccessLayer.GetPersonValueFromPersonField(this.context, this.web, user, Person.Name);
                                obj.Name = approverName;
                                obj.Value = approverName;
                                pendingWithWhomList.Add(obj);
                            }
                        }
                    }
                    return pendingWithWhomList.GroupBy(m => m.Name).Select(g => g.First()).ToList();
                }
                return pendingWithWhomList;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the replace by whome user list.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public List<NameValueData> GetReplaceByWhomeUserList(string roleName)
        {
            try
            {
                List<NameValueData> replaceByWhomList = new List<NameValueData>();
                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    MasterDataHelper masterHelper = new MasterDataHelper();
                    IMaster approverMaster = masterHelper.GetMasterData(this.context, this.web, new List<IMaster>() { new ApproverMaster() }).FirstOrDefault();
                    string approverMasterjson = JSONHelper.ToJSON<IMaster>(approverMaster);
                    var filterdapproverMasterjson = JSONHelper.ToObject<IMaster>(approverMasterjson);
                    if (filterdapproverMasterjson != null)
                    {
                        List<ApproverMasterListItem> userList = filterdapproverMasterjson.Items.Where(x => (x as ApproverMasterListItem).Role == roleName && (x as ApproverMasterListItem).UserSelection == true).ToList().ConvertAll(p => (ApproverMasterListItem)p);
                        if (userList != null && userList.Count > 0)
                        {
                            userList = Helper.SplitUser(userList);
                            foreach (ApproverMasterListItem item in userList)
                            {
                                NameValueData obj = new NameValueData();

                                obj.Name = Convert.ToString(item.UserName);
                                obj.Value = Convert.ToString(item.UserName);

                                replaceByWhomList.Add(obj);
                            }
                        }
                        return replaceByWhomList.GroupBy(m => m.Name).Select(g => g.First()).ToList();
                    }
                }
                return replaceByWhomList;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the listof users.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public ChangeApproverMaster GetListofUsers(string roleName)
        {
            ChangeApproverMaster approverMaster = new Models.Master.ChangeApproverMaster();
            approverMaster.PendingWithWhomUserList = GetPendingWithWhomUserList(roleName);
            if (approverMaster.PendingWithWhomUserList != null && approverMaster.PendingWithWhomUserList.Count > 0)
                approverMaster.ReplaceByWhomUserList = GetReplaceByWhomeUserList(roleName);
            return approverMaster;
        }

        /// <summary>
        /// Gets all pending requests.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="approverName">Name of the approver.</param>
        /// <returns></returns>
        public List<PendingRequest> GetAllPendingRequests(string role, string approverName)
        {
            List<PendingRequest> pendingRequestsList = new List<PendingRequest>();
            PendingRequest pendingRequest = new PendingRequest();
            ////Get data from Local Approval Matrix 
            List approverList = this.web.Lists.GetByTitle(ICCPListNames.ITEMCODEAPPROVALMATRIX);

            CamlQuery query = new CamlQuery();
        
            ListItemCollectionPosition position = null;
            var page = 1;

            do
            {
                query.ViewXml = @"<View>
                                        <Query>
                                           <Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Role' />
                                                    <Value Type='Text'>" + role + @"</Value>
                                                 </Eq>
                                                 <And>
                                                    <Neq>
                                                         <FieldRef Name='RequestID_x003a_Status' />
                                                         <Value Type='Lookup'>Completed</Value>
                                                     </Neq>
                                                    <And>
                                                        <Contains>
                                                            <FieldRef Name='Approver' />
                                                            <Value Type='User'>" + approverName + @"</Value>
                                                        </Contains>
                                                         <And>
                                                              <Neq>
                                                                 <FieldRef Name='Status' />
                                                                 <Value Type='Choice'>Completed</Value>
                                                              </Neq>
                                                              <And>
                                                                 <Neq>
                                                                    <FieldRef Name='Status' />
                                                                    <Value Type='Choice'>Approved</Value>
                                                                 </Neq>
                                                                 <Neq>
                                                                    <FieldRef Name='Status' />
                                                                    <Value Type='Choice'>Not Required</Value>
                                                                 </Neq>
                                                              </And>
                                                         </And>
                                                    </And>
                                                 </And>
                                              </And>
                                           </Where>
                                           <OrderBy>
                                              <FieldRef Name='ID' Ascending='False' />
                                           </OrderBy>
                                        </Query>
                                        <RowLimit>5000</RowLimit>
                                 </View>";

                query.ListItemCollectionPosition = position;

                ListItemCollection approverDetails = approverList.GetItems(query);
                this.context.Load(approverDetails);
                this.context.ExecuteQuery();

                position = approverDetails.ListItemCollectionPosition;

                if (approverDetails != null)
                {
                    for (int i = 0; i < approverDetails.Count; i++)
                    {
                        pendingRequest = new PendingRequest();
                        FieldLookupValue lookup = approverDetails[i]["RequestID_x003a_Creation_x0020_D"] as FieldLookupValue;
                        pendingRequest.CreationDate = Convert.ToDateTime(lookup.LookupValue);

                        lookup = approverDetails[i]["RequestID_x003a_ICDM_x0020_No"] as FieldLookupValue;
                        pendingRequest.ICDMNo = (string.IsNullOrWhiteSpace(lookup.LookupValue) || lookup.LookupValue.ToLower().Trim() == "view") ? "-" : Convert.ToString(lookup.LookupValue);

                        string str = string.Empty;
                        foreach (FieldUserValue userValue in approverDetails[i]["Approver"] as FieldUserValue[])
                        {
                            str = str + userValue.LookupValue + ",";
                        }
                        pendingRequest.PendingWith = Convert.ToString(str.Remove(str.LastIndexOf(",")));
                        //pendingRequest.PreparedBy = Convert.ToString(approverDetails[i]["Status"]);

                        lookup = approverDetails[i]["RequestID_x003a_Status"] as FieldLookupValue;
                        pendingRequest.Status = Convert.ToString(lookup.LookupValue);

                        lookup = approverDetails[i]["RequestID"] as FieldLookupValue;
                        pendingRequest.ID = Convert.ToInt32(lookup.LookupValue);
                        pendingRequest.Role = Convert.ToString(approverDetails[i]["Role"]);
                        pendingRequestsList.Add(pendingRequest);
                    }
                    page++;
                }
            }
            while (position != null);
            return pendingRequestsList;
        }

        #region CRUD ApproverReplacement
        /// <summary>
        /// Gets the approver replacements.
        /// </summary>
        /// <param name="approverMaster">The approver master.</param>
        /// <param name="isExistCheck">if set to <c>true</c> [is exist check].</param>
        /// <returns></returns>
        public List<ChangeApproverMaster> GetApproverReplacements(ChangeApproverMaster approverMaster, bool isExistCheck = false)
        {
            try
            {
                List<ChangeApproverMaster> approverReplacementList = new List<ChangeApproverMaster>();
                List spList = this.web.Lists.GetByTitle(ICCPListNames.CHANGEAPPROVERMASTER);
                CamlQuery query = new CamlQuery();
                string appendIDWhereClause = string.Empty, appendRoleNameWhereClause = string.Empty, appendPendingWithWhomWhereClause = string.Empty, appendReplaceByWhomWhereClause = string.Empty, appendJobStatusWhereClause = string.Empty;
                string idStartAnd = string.Empty, idEndAnd = string.Empty, roleNameStartAnd = string.Empty, roleNameEndAnd = string.Empty, pendingWithWhomStartAnd = string.Empty, pendingWithWhomEndAnd = string.Empty, replaceByWhomStartAnd = string.Empty, replaceByWhomEndAnd = string.Empty;
                if (approverMaster != null)
                {
                    if (approverMaster.ID > 0 && !isExistCheck)
                    {
                        if (!string.IsNullOrWhiteSpace(approverMaster.RoleName))
                        {
                            idStartAnd = "<And>";
                            idEndAnd = "</And>";
                        }
                        appendIDWhereClause = @"<Eq>
                                                   <FieldRef Name='ID' />
                                                       <Value Type='Number'>" + approverMaster.ID + @"</Value>
                                               </Eq>";
                    }
                    if (!string.IsNullOrWhiteSpace(approverMaster.RoleName))
                    {
                        if (!string.IsNullOrWhiteSpace(approverMaster.PendingWithWhom))
                        {
                            roleNameStartAnd = "<And>";
                            roleNameEndAnd = "</And>";
                        }
                        else if (!string.IsNullOrWhiteSpace(approverMaster.JobStatus))
                        {
                            roleNameStartAnd = "<And>";
                            roleNameEndAnd = "</And>";
                        }
                        appendRoleNameWhereClause = @"<Eq>
                                                        <FieldRef Name='RoleName' />
                                                            <Value Type='Text'>" + approverMaster.RoleName + @"</Value>
                                                     </Eq>";
                    }
                    if (!string.IsNullOrWhiteSpace(approverMaster.PendingWithWhom))
                    {
                        if (string.IsNullOrWhiteSpace(approverMaster.RoleName))
                        {
                            roleNameStartAnd = "<And>";
                            roleNameEndAnd = "</And>";
                        }
                        if (!string.IsNullOrWhiteSpace(approverMaster.ReplaceByWhom) && !isExistCheck)
                        {
                            pendingWithWhomStartAnd = "<And>";
                            pendingWithWhomEndAnd = "</And>";
                        }
                        else if (!string.IsNullOrWhiteSpace(approverMaster.JobStatus))
                        {
                            pendingWithWhomStartAnd = "<And>";
                            pendingWithWhomEndAnd = "</And>";
                        }
                        appendPendingWithWhomWhereClause = @"<Eq>
                                                                 <FieldRef Name='PendingWithWhom' />
                                                                 <Value Type='User'>" + approverMaster.PendingWithWhom + @"</Value>
                                                              </Eq>";
                    }
                    if (!string.IsNullOrWhiteSpace(approverMaster.ReplaceByWhom) && !isExistCheck)
                    {
                        if (!string.IsNullOrWhiteSpace(approverMaster.JobStatus))
                        {
                            replaceByWhomStartAnd = "<And>";
                            replaceByWhomEndAnd = "</And>";
                        }
                        appendReplaceByWhomWhereClause = @"<Eq>
                                                            <FieldRef Name='ReplaceByWhom' />
                                                                <Value Type='User'>" + approverMaster.ReplaceByWhom + @"</Value>
                                                         </Eq>";
                    }
                    if (!string.IsNullOrWhiteSpace(approverMaster.JobStatus))
                    {
                        appendJobStatusWhereClause = @"<Eq>
                                                        <FieldRef Name='JobStatus' />
                                                            <Value Type='Choice'>" + approverMaster.JobStatus + @"</Value>
                                                     </Eq>";
                    }

                    query.ViewXml = @"<View>
                                         <Query>
                                                <Where>
                                                     " + idStartAnd + @"
                                                        " + appendIDWhereClause + @"
                                                            " + roleNameStartAnd + @"
                                                                  " + appendRoleNameWhereClause + @"  
                                                                        " + pendingWithWhomStartAnd + @"
                                                                              " + appendPendingWithWhomWhereClause + @"  
                                                                                     " + replaceByWhomStartAnd + @"
                                                                                             " + appendReplaceByWhomWhereClause + @" 
                                                                                             " + appendJobStatusWhereClause + @" 
                                                                                       " + replaceByWhomEndAnd + @" 
                                                                        " + pendingWithWhomEndAnd + @"
                                                            " + roleNameEndAnd + @"
                                                      " + idEndAnd + @"  
                                                  </Where>
                                                  <OrderBy>
                                                     <FieldRef Name='ID' Ascending='False' />
                                                  </OrderBy>  
                                            </Query>                                           
                                     </View>";
                }
                else
                {
                    query.ViewXml = @"<View>
                                         <Query>
                                               <OrderBy>
                                                  <FieldRef Name='ID' Ascending='False' />
                                               </OrderBy>
                                            </Query>                                           
                                     </View>";
                }

                ListItemCollection items = spList.GetItems(query);
                this.context.Load(items);
                this.context.ExecuteQuery();
                if (items != null && items.Count != 0)
                {
                    foreach (ListItem item in items)
                    {
                        ChangeApproverMaster approvermaster = new ChangeApproverMaster();
                        approvermaster.ID = Convert.ToInt32(item["ID"]);
                        approvermaster.RoleName = Convert.ToString(item["RoleName"]);
                        approvermaster.JobStatus = Convert.ToString(item["JobStatus"]);
                        approvermaster.PendingWithWhom = BELDataAccessLayer.GetPersonValueFromPersonField(this.context, this.web, (FieldUserValue)item["PendingWithWhom"], Person.Name);
                        approvermaster.ReplaceByWhom = BELDataAccessLayer.GetPersonValueFromPersonField(this.context, this.web, (FieldUserValue)item["ReplaceByWhom"], Person.Name);
                        approverReplacementList.Add(approvermaster);
                    }
                    return approverReplacementList;
                }
                return approverReplacementList;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the approver replacement by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ChangeApproverMaster GetApproverReplacementById(int id)
        {
            ChangeApproverMaster approver = new ChangeApproverMaster();
            if (id > 0)
            {
                approver.ID = id;
                approver = GetApproverReplacements(approver).FirstOrDefault();
            }
            approver.RoleNameList = ReportBusinessLayer.Instance.GetRoleList();
            approver.PendingWithWhomUserList = GetPendingWithWhomUserList(approver.RoleName);
            if (approver.PendingWithWhomUserList != null && approver.PendingWithWhomUserList.Count > 0)
                approver.ReplaceByWhomUserList = GetReplaceByWhomeUserList(approver.RoleName);
            return approver;
        }

        /// <summary>
        /// Saves the approver replacement details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionStatus SaveApproverReplacementDetails(ChangeApproverMaster model)
        {
            ActionStatus status = new ActionStatus();
            try
            {
                if (model != null)
                {
                    status = IsApproverReplacementAlreadyExists(model);
                    if (status.IsSucceed)
                    {
                        User userVal = this.context.Web.EnsureUser(model.PendingWithWhom);
                        this.context.Load(userVal);
                        this.context.ExecuteQuery();

                        FieldUserValue pendingWithWhomValue = new FieldUserValue();
                        pendingWithWhomValue.LookupId = userVal.Id;

                        User users = this.context.Web.EnsureUser(model.ReplaceByWhom);
                        this.context.Load(users);
                        this.context.ExecuteQuery();

                        FieldUserValue replaceByWhomValue = new FieldUserValue();
                        replaceByWhomValue.LookupId = users.Id;

                        List list = this.web.Lists.GetByTitle(ICCPListNames.CHANGEAPPROVERMASTER);
                        if (list != null)
                        {
                            ListItemCreationInformation itemInfo = new ListItemCreationInformation();
                            ListItem itemtoSave = null;
                            if (model.ID == 0)
                            {
                                itemtoSave = list.AddItem(itemInfo);
                            }
                            else if (model.ID > 0)
                            {
                                itemtoSave = list.GetItemById(model.ID);
                            }
                            itemtoSave["RoleName"] = model.RoleName;
                            itemtoSave["PendingWithWhom"] = pendingWithWhomValue;
                            itemtoSave["ReplaceByWhom"] = replaceByWhomValue;
                            itemtoSave.Update();
                            this.context.Load(itemtoSave);
                            this.context.ExecuteQuery();
                            status.IsSucceed = true;
                            status.Messages.Add("Record is saved successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status.IsSucceed = false;
                Logger.Error("Error while save items in listname " + ICCPListNames.CHANGEAPPROVERMASTER + " : ID =" + model.ID + ": Message = " + ex.Message + ": StackTrace = " + ex.StackTrace);
            }
            return status;
        }

        /// <summary>
        /// Determines whether [is approver replacement already exists] [the specified model].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private ActionStatus IsApproverReplacementAlreadyExists(ChangeApproverMaster model)
        {
            ActionStatus status = new ActionStatus();
            status.IsSucceed = true;
            ChangeApproverMaster master = GetApproverReplacements(model, true).FirstOrDefault();
            if (master != null)
            {
                if (model.ID != 0 && master.ID == model.ID)
                {
                    status.IsSucceed = true;
                }
                else if (master.PendingWithWhom == model.PendingWithWhom && master.RoleName == model.RoleName && master.JobStatus == "No")
                {
                    status.IsSucceed = false;
                    status.Messages.Add("Record is already Exists with Same Role Name & Approver.");
                }
                else if (master.PendingWithWhom == model.PendingWithWhom && master.RoleName == model.RoleName && master.JobStatus == "Yes")
                {
                    status.IsSucceed = true;
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages.Add("Record is already exists.");
                }
            }
            return status;
        }

        /// <summary>
        /// Deletes the approver replacement by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteApproverReplacementById(int id)
        {
            bool isdeleted = true;
            try
            {
                List list = this.web.Lists.GetByTitle(ICCPListNames.CHANGEAPPROVERMASTER);
                if (list != null)
                {
                    ListItem item = list.GetItemById(id);
                    if (item != null)
                    {
                        item.DeleteObject();
                    }
                    this.context.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                isdeleted = false;
                Logger.Error("Error while delete items in listname " + ICCPListNames.CHANGEAPPROVERMASTER + ": Message = " + ex.Message + ": StackTrace = " + ex.StackTrace);
            }
            return isdeleted;
        }
        #endregion CRUD ApproverReplacement
        #endregion Approver Replacement 
    }
}