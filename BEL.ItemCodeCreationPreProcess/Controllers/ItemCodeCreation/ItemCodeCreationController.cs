namespace BEL.ItemCodeCreationPreProcess.Controllers.ItemCodeCreation
{
    using Common;
    using CommonDataContract;
    using Models.Common;
    using Models.ItemCode;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// ItemCode CreationController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ItemCodeCreationController : ItemCodeCreationBaseController
    {
        #region GET SECTION DETAILS

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isRetrieve">if set to <c>true</c> [is retrieve].</param>
        /// <returns>
        /// returns ActionResult
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [SharePointContextFilter]
        public ActionResult Index(int id = 0, bool isRetrieve = false)
        {
            ItemCodeContract contract = null;
            Dictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("FormName", FormNameConstants.ICCPForm);
            objDict.Add("ItemId", id.ToString());
            objDict.Add(Parameter.USEREID, this.CurrentUser.UserId);
            objDict.Add(Parameter.USERNAME, this.CurrentUser.FullName);
            objDict.Add(Parameter.CREATOREMAIL, this.CurrentUser.UserEmail);
            ViewBag.UserID = this.CurrentUser.UserId;
            ViewBag.UserName = this.CurrentUser.FullName;

            Logger.Info("ViewBag.UserID =", ViewBag.UserID);
            Logger.Info("ViewBag.UserName = ", ViewBag.UserName);

            contract = this.GetItemCodeCreationDetails(objDict);

            if (contract != null)
            {
                if (!isRetrieve)
                {
                    contract.UserDetails = this.CurrentUser;
                    if (contract.Forms != null && contract.Forms.Count > 0)
                    {
                        var lumMktInchargeSection = contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.LUMMKTINCHARGESECTION) as LUMMktInchargeSection;
                        if (lumMktInchargeSection != null && string.IsNullOrWhiteSpace(lumMktInchargeSection.ProposedBy))
                        {
                            return this.RedirectToAction("NotAuthorize", "Master");
                        }

                        SCMLUMDesignInchargeSection scmInchargeSection = contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.SCMLUMDESIGNINCHARGESECTION) as SCMLUMDesignInchargeSection;
                        if (scmInchargeSection != null && scmInchargeSection.IsActive)
                        {
                            this.SetTranListintoTempData<VendorItemCode>(scmInchargeSection.VendorItemCodeList, TempKeys.ICCPVendor.ToString() + "_" + id);
                        }
                        SCMLUMDesignDelegateSection scmDelegateSection = contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.SCMLUMDESIGNDELEGATESECTION) as SCMLUMDesignDelegateSection;
                        if (scmDelegateSection != null && scmDelegateSection.IsActive)
                        {
                            this.SetTranListintoTempData<VendorItemCode>(scmDelegateSection.VendorItemCodeList, TempKeys.ICCPVendor.ToString() + "_" + id);
                        }
                        return this.View(contract);
                    }
                    Logger.Error("contract.Forms == null so, notauthorised");
                    return this.RedirectToAction("NotAuthorize", "Master");
                }
                else
                {
                    if (contract.Forms != null && contract.Forms.Count > 0)
                    {
                        contract.Forms[0].FormStatus = string.Empty;

                        Button btn = new Button();
                        btn.Name = "Resume";
                        btn.ButtonStatus = ButtonActionStatus.NextApproval;
                        btn.JsFunction = "ConfirmSubmit";
                        btn.IsVisible = true;
                        btn.Icon = "fa fa-save";
                        contract.Forms[0].Buttons.Add(btn);
                        contract.Forms[0].Buttons.Remove(contract.Forms[0].Buttons.FirstOrDefault(f => f.ButtonStatus == ButtonActionStatus.Print));

                        var lumMktInchargeSection = contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.LUMMKTINCHARGESECTION) as LUMMktInchargeSection;
                        if (lumMktInchargeSection != null)
                        {
                            //if (lumMktInchargeSection.ApproversList.FirstOrDefault(p => p.Role == ICCPRoles.CREATOR).Approver != this.CurrentUser.UserId)
                            //{
                            //    return this.RedirectToAction("NotAuthorize", "Master");
                            //}
                            if (lumMktInchargeSection != null && string.IsNullOrWhiteSpace(lumMktInchargeSection.ProposedBy))
                            {
                                return this.RedirectToAction("NotAuthorize", "Master");
                            }

                            lumMktInchargeSection.OldICCPNo = lumMktInchargeSection.ICCPNo;
                            lumMktInchargeSection.OldICCPCreatedDate = lumMktInchargeSection.RequestDate;
                            lumMktInchargeSection.OldICCPId = id;
                            lumMktInchargeSection.ProposedBy = CurrentUser.UserId;
                            lumMktInchargeSection.ProposedByName = CurrentUser.FullName;
                            lumMktInchargeSection.ICCPNo = string.Empty;
                            lumMktInchargeSection.RequestDate = null;
                            lumMktInchargeSection.WorkflowStatus = string.Empty;
                            lumMktInchargeSection.IsActive = true;
                            lumMktInchargeSection.ListDetails[0].ItemId = 0;
                            lumMktInchargeSection.CurrentApprover.Approver = CurrentUser.UserId;
                            lumMktInchargeSection.CurrentApprover.ApproverName = CurrentUser.FullName;
                        }

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.LUMMKTDELEGATEESECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.SCMLUMDESIGNINCHARGESECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.SCMLUMDESIGNDELEGATESECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.SMSINCHARGESECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.SMSDELEGATESECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.QAINCHARGESECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.QADELEGATESECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.FINALSMSINCHARGESECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.FINALSMSDELEGATESECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.COSTINGINCHARGESECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.COSTINGDELEGATE1SECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.COSTINGDELEGATE2SECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.TDSINCHARGESECTION));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.TDSDELEGATESECTION));

                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == ICCPSectionName.ACTIVITYLOG));
                        contract.Forms[0].SectionsList.Remove(contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == SectionNameConstant.APPLICATIONSTATUS));
                        return this.View(contract);
                    }
                    Logger.Error("contract.Forms == null so, notauthorised");
                    return this.RedirectToAction("NotAuthorize", "Master");
                }
            }
            Logger.Error("contract == null so, notauthorised");
            return this.RedirectToAction("NotAuthorize", "Master");
        }

        #endregion

        #region SAVE SECTION DETAILS

        #region SAVE LUM Marketing SECTION DETAILS
        /// <summary>
        /// Saves the lum MKT incharge section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>return ActionResult</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveLUMMarketingInchrageSection(LUMMktInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (model.ProposedBy == null)
                {
                    model.ProposedBy = this.CurrentUser.UserId;
                }
                if (this.ValidateModelState(model))
                {
                    //if (model.ActionStatus == ButtonActionStatus.SaveAsDraft && string.IsNullOrWhiteSpace(model.CurrentApprover.Comments))
                    //{
                    //    status.IsSucceed = false;
                    //    status.Messages = new List<string>() { this.GetResourceValue("Error_Comments", System.Web.Mvc.Html.ResourceName.Common) };
                    //    return this.Json(status);
                    //}
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.LUMMARKETINGDELEGATE)
                                {
                                    p.Approver = model.LUMMarketingDelegate;
                                }
                            });
                        }
                        else
                        {
                            model.LUMMarketingDelegate = this.CurrentUser.UserId;
                            model.LUMMarketingDelegateName = this.CurrentUser.FullName;
                        }
                    }

                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);

                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the lum marketing delegate section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>return ActionResult</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveLUMMarketingDelegateSection(LUMMktDelegateSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        #endregion

        #region SAVE SCM LUM Design SECTION DETAILS
        /// <summary>
        /// Saves the scmlum design inchrage section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveSCMLUMDesignInchrageSection(SCMLUMDesignInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (model.ActionStatus == ButtonActionStatus.NextApproval)
                {
                    if (!model.BISRegistrationApplicable)
                    {
                        ModelState.Remove("RNumber");
                    }
                    if (model.BuyMake == "Make")
                    {
                        if (string.IsNullOrWhiteSpace(model.BOMAttachment))
                        {
                            ModelState.AddModelError("BOMAttachment", "BOMAttachment");
                        }
                    }
                    if (model.BuyMake == "Buy")
                    {
                        ModelState.Remove("BOMAttachment");
                    }
                }
                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.SCMLUMDESIGNDELEGATE)
                                    p.Approver = model.SCMLUMDesignDelegate;
                            });
                        }
                        else
                        {
                            model.SCMLUMDesignDelegate = this.CurrentUser.UserId;
                            model.SCMLUMDesignDelegateName = this.CurrentUser.FullName;
                        }
                    }

                    model.Files = new List<FileDetails>();
                    if (!string.IsNullOrWhiteSpace(model.UploadArtworkAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.UploadArtworkAttachment));
                        model.UploadArtworkAttachment = string.Join(",", FileListHelper.GetFileNames(model.UploadArtworkAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.SpecificationSheetAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.SpecificationSheetAttachment));
                        model.SpecificationSheetAttachment = string.Join(",", FileListHelper.GetFileNames(model.SpecificationSheetAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.ProductDrawingAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.ProductDrawingAttachment));
                        model.ProductDrawingAttachment = string.Join(",", FileListHelper.GetFileNames(model.ProductDrawingAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.PackagingDataSheetAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.PackagingDataSheetAttachment));
                        model.PackagingDataSheetAttachment = string.Join(",", FileListHelper.GetFileNames(model.PackagingDataSheetAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.LM79Attachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.LM79Attachment));
                        model.LM79Attachment = string.Join(",", FileListHelper.GetFileNames(model.LM79Attachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.IESFileAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.IESFileAttachment));
                        model.IESFileAttachment = string.Join(",", FileListHelper.GetFileNames(model.IESFileAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.BOMAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.BOMAttachment));
                        model.BOMAttachment = string.Join(",", FileListHelper.GetFileNames(model.BOMAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.ADSAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.ADSAttachment));
                        model.ADSAttachment = string.Join(",", FileListHelper.GetFileNames(model.ADSAttachment));
                    }

                    var list = this.GetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl());
                    model.VendorItemCodeList = list.ToList<ITrans>();
                    if (!string.IsNullOrWhiteSpace(model.Vendors))
                    {
                        model.Vendors = model.Vendors.Trim(new char[] { ',' });
                        //model.Vendors = model.Vendors.Remove(model.Vendors.LastIndexOf(','));
                    }
                    //if (model.ActionStatus == ButtonActionStatus.NextApproval && (list == null || list.Count == 0 || !list.Any(m => m.ItemAction != ItemActionStatus.DELETED)))
                    //{
                    //    status.IsSucceed = false;
                    //    status.Messages = new List<string>() { this.GetResourceValue("Text_VendorRequired", System.Web.Mvc.Html.ResourceName.ICCPForm) };
                    //    return this.Json(status);
                    //}

                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the scmlum design delegate section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveSCMLUMDesignDelegateSection(SCMLUMDesignDelegateSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (model.ActionStatus == ButtonActionStatus.NextApproval)
                {
                    if (!model.BISRegistrationApplicable)
                    {
                        ModelState.Remove("RNumber");
                    }
                    if (model.BuyMake == "Make")
                    {
                        if (string.IsNullOrWhiteSpace(model.BOMAttachment))
                        {
                            ModelState.AddModelError("BOMAttachment", "BOMAttachment");
                        }
                    }
                    if (model.BuyMake == "Buy")
                    {
                        ModelState.Remove("BOMAttachment");
                    }
                }
                if (this.ValidateModelState(model))
                {
                    model.Files = new List<FileDetails>();
                    if (!string.IsNullOrWhiteSpace(model.UploadArtworkAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.UploadArtworkAttachment));
                        model.UploadArtworkAttachment = string.Join(",", FileListHelper.GetFileNames(model.UploadArtworkAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.SpecificationSheetAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.SpecificationSheetAttachment));
                        model.SpecificationSheetAttachment = string.Join(",", FileListHelper.GetFileNames(model.SpecificationSheetAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.ProductDrawingAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.ProductDrawingAttachment));
                        model.ProductDrawingAttachment = string.Join(",", FileListHelper.GetFileNames(model.ProductDrawingAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.PackagingDataSheetAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.PackagingDataSheetAttachment));
                        model.PackagingDataSheetAttachment = string.Join(",", FileListHelper.GetFileNames(model.PackagingDataSheetAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.LM79Attachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.LM79Attachment));
                        model.LM79Attachment = string.Join(",", FileListHelper.GetFileNames(model.LM79Attachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.IESFileAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.IESFileAttachment));
                        model.IESFileAttachment = string.Join(",", FileListHelper.GetFileNames(model.IESFileAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.BOMAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.BOMAttachment));
                        model.BOMAttachment = string.Join(",", FileListHelper.GetFileNames(model.BOMAttachment));
                    }
                    if (!string.IsNullOrWhiteSpace(model.ADSAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.ADSAttachment));
                        model.ADSAttachment = string.Join(",", FileListHelper.GetFileNames(model.ADSAttachment));
                    }

                    var list = this.GetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl());
                    model.VendorItemCodeList = list.ToList<ITrans>();
                    if (!string.IsNullOrWhiteSpace(model.Vendors))
                    {
                        model.Vendors = model.Vendors.Trim(new char[] { ',' });
                    }
                    //if (model.ActionStatus == ButtonActionStatus.NextApproval && (list == null || list.Count == 0 || !list.Any(m => m.ItemAction != ItemActionStatus.DELETED)))
                    //{
                    //    status.IsSucceed = false;
                    //    status.Messages = new List<string>() { this.GetResourceValue("Text_VendorRequired", System.Web.Mvc.Html.ResourceName.ICCPForm) };
                    //    return this.Json(status);
                    //}

                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }
        #endregion

        #region SAVE SMS SECTION DETAILS
        /// <summary>
        /// Saves the SMS inchrage section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveSMSInchrageSection(SMSInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (model.PilotLotPreparation == "No")
                {
                    ModelState.Remove("Quantity");
                    model.Quantity = null;
                }
                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.SMSDELEGATE)
                                    p.Approver = model.SMSDelegate;
                            });
                        }
                        else
                        {
                            model.SMSDelegate = this.CurrentUser.UserId;
                            model.SMSDelegateName = this.CurrentUser.FullName;
                        }
                    }

                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the SMS delegate section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveSMSDelegateSection(SMSDelegateSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (model.PilotLotPreparation == "No")
                {
                    ModelState.Remove("Quantity");
                    model.Quantity = null;
                }
                if (this.ValidateModelState(model))
                {
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }
        #endregion

        #region SAVE QA SECTION DETAILS
        /// <summary>
        /// Saves the qa inchrage section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveQAInchrageSection(QAInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                if (model.ActionStatus == ButtonActionStatus.NextApproval)
                {
                    model.ActionStatus = ButtonActionStatus.NextApproval;
                    model.OldICCPRejectedDate = null;
                }
                else if (model.ActionStatus == ButtonActionStatus.Rejected)
                {
                    model.ActionStatus = ButtonActionStatus.Rejected;
                    model.OldICCPRejectedDate = DateTime.Now;
                    ModelState.Remove("ReliabilityTestReportAttachment");
                }
                else if (model.ActionStatus == ButtonActionStatus.SendBack)
                {
                    model.ActionStatus = ButtonActionStatus.SendBack;
                    model.SendBackTo = "2";
                    objDict[Parameter.SENDTOLEVEL] = "2";
                    model.ReworkCount = model.ReworkCount + 1;
                    model.OldICCPRejectedDate = null;
                }

                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.QADELEGATE)
                                    p.Approver = model.QADelegate;
                            });
                        }
                        else
                        {
                            model.QADelegate = this.CurrentUser.UserId;
                            model.QADelegateName = this.CurrentUser.FullName;
                        }
                    }
                    model.Files = new List<FileDetails>();
                    if (!string.IsNullOrWhiteSpace(model.ReliabilityTestReportAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.ReliabilityTestReportAttachment));
                        model.ReliabilityTestReportAttachment = string.Join(",", FileListHelper.GetFileNames(model.ReliabilityTestReportAttachment));
                    }
                    //if (!string.IsNullOrWhiteSpace(model.JointInspectionAttachment))
                    //{
                    //    model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.JointInspectionAttachment));
                    //    model.JointInspectionAttachment = string.Join(",", FileListHelper.GetFileNames(model.JointInspectionAttachment));
                    //}

                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the qa delegate section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveQADelegateSection(QADelegateSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                if (model.ActionStatus == ButtonActionStatus.NextApproval)
                {
                    model.ActionStatus = ButtonActionStatus.NextApproval;
                    model.OldICCPRejectedDate = null;
                }
                else if (model.ActionStatus == ButtonActionStatus.Rejected)
                {
                    model.ActionStatus = ButtonActionStatus.Rejected;
                    model.OldICCPRejectedDate = DateTime.Now;
                    ModelState.Remove("ReliabilityTestReportAttachment");
                }
                else if (model.ActionStatus == ButtonActionStatus.SendBack)
                {
                    model.ActionStatus = ButtonActionStatus.SendBack;
                    model.SendBackTo = "2";
                    objDict[Parameter.SENDTOLEVEL] = "2";
                    model.ReworkCount = model.ReworkCount + 1;
                    model.OldICCPRejectedDate = null;
                }
                if (this.ValidateModelState(model))
                {
                    model.Files = new List<FileDetails>();
                    if (!string.IsNullOrWhiteSpace(model.ReliabilityTestReportAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.ReliabilityTestReportAttachment));
                        model.ReliabilityTestReportAttachment = string.Join(",", FileListHelper.GetFileNames(model.ReliabilityTestReportAttachment));
                    }
                    //if (!string.IsNullOrWhiteSpace(model.JointInspectionAttachment))
                    //{
                    //    model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.JointInspectionAttachment));
                    //    model.JointInspectionAttachment = string.Join(",", FileListHelper.GetFileNames(model.JointInspectionAttachment));
                    //}
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }
        #endregion

        #region SAVE Final SMS SECTION DETAILS
        /// <summary>
        /// Saves the final SMS inchrage section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveFinalSMSInchrageSection(FinalSMSInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.FINALSMSDELEGATE)
                                    p.Approver = model.FinalSMSDelegate;
                            });
                        }
                        else
                        {
                            model.FinalSMSDelegate = this.CurrentUser.UserId;
                            model.FinalSMSDelegateName = this.CurrentUser.FullName;
                        }
                    }

                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the final SMS delegate section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveFinalSMSDelegateSection(FinalSMSDelegateSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }
        #endregion

        #region SAVE Costing SECTION DETAILS
        /// <summary>
        /// Saves the costing inchrage section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveCostingInchrageSection(CostingInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.COSTINGDELEGATE1)
                                    p.Approver = model.CostingDelegate;
                            });
                        }
                        else if (model.ActionStatus == ButtonActionStatus.NextApproval)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.COSTINGDELEGATE2)
                                    p.Approver = model.CostingDelegate2;
                            });
                        }
                        else
                        {
                            model.CostingDelegate = this.CurrentUser.UserId;
                            model.CostingDelegateName = this.CurrentUser.FullName;
                        }
                    }
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);

                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the csosting delegate1 section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>return </returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveCostingDelegate1Section(CostingDelegate1Section model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.NextApproval)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.COSTINGDELEGATE2)
                                    p.Approver = model.CostingDelegate2;
                            });
                        }
                        else
                        {
                            model.CostingDelegate2 = this.CurrentUser.UserId;
                            model.CostingDelegate2Name = this.CurrentUser.FullName;
                        }
                    }
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the costing delegate2 section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveCostingDelegate2Section(CostingDelegate2Section model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }
        #endregion

        #region SAVE TDS SECTION DETAILS
        /// <summary>
        /// Saves the TDS inchrage section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveTDSInchrageSection(TDSInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    if (model.ActionStatus == ButtonActionStatus.Delegate || model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        if (model.ActionStatus == ButtonActionStatus.Delegate)
                        {
                            model.ApproversList.ForEach(p =>
                            {
                                if (p.Role == ICCPRoles.TDSDELEGATE)
                                    p.Approver = model.TDSDelegate;
                            });
                        }
                        else
                        {
                            model.TDSDelegate = this.CurrentUser.UserId;
                            model.TDSDelegateName = this.CurrentUser.FullName;
                        }
                    }

                    model.Files = new List<FileDetails>();
                    if (!string.IsNullOrWhiteSpace(model.TDSAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.TDSAttachment));
                        model.TDSAttachment = string.Join(",", FileListHelper.GetFileNames(model.TDSAttachment));
                    }
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Saves the TDS delegate section.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]//ValidateAntiForgeryTokenOnAllPosts
        public ActionResult SaveTDSDelegateSection(TDSDelegateSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null)
            {
                if (this.ValidateModelState(model))
                {
                    model.Files = new List<FileDetails>();
                    if (!string.IsNullOrWhiteSpace(model.TDSAttachment))
                    {
                        model.Files.AddRange(Common.FileListHelper.GenerateFileBytes(model.TDSAttachment));
                        model.TDSAttachment = string.Join(",", FileListHelper.GetFileNames(model.TDSAttachment));
                    }
                    Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                    status = this.SaveSection(model, objDict);
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }
        #endregion

        #endregion

        #region "CRUD Vendor Detail"

        /// <summary>
        /// Adds the edit vendor.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddEditVendor(int index = 0)
        {
            List<VendorItemCode> list = new List<VendorItemCode>();
            list = this.GetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl());
            VendorItemCode item = null;
            if (index == 0)
            {
                item = new VendorItemCode() { Index = 0, RequestDate = DateTime.Now, RequestBy = this.CurrentUser.UserId };
            }
            else
            {
                item = list.FirstOrDefault(x => x.Index == index);
            }
            return this.PartialView("_AddVendor", item);
        }

        /// <summary>
        /// Saves the vendor.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveVendor(VendorItemCode model)
        {
            ActionStatus status = new ActionStatus() { IsSucceed = true };
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    List<VendorItemCode> list = new List<VendorItemCode>();
                    list = this.GetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl());
                    if (list.Any(p => p.VendorName == model.VendorName && p.Index != model.Index))
                    {
                        status.IsSucceed = false;
                        status.Messages.Add(this.GetResourceValue("Text_UniqeVendor", ResourceName.ICCPForm));
                        status = this.GetMessage(status, ResourceName.ICCPForm);
                        return this.Json(status);
                    }
                    if (model.Index == 0)
                    {
                        model.Index = list.Count + 1;
                        model.ItemAction = ItemActionStatus.NEW;
                    }
                    else
                    {
                        list.RemoveAll(x => x.Index == model.Index);
                    }
                    if (model.ID > 0)
                    {
                        model.ItemAction = ItemActionStatus.UPDATED;
                    }
                    list.Add(model);
                    this.SetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl(), list.OrderBy(x => x.Index).ToList());
                    status.Messages.Add(this.GetResourceValue("Text_VendorSave", ResourceName.ICCPForm));
                    status = this.GetMessage(status, ResourceName.ICCPForm);
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.ICCPForm);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Gets the vendor grid.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetVendorGrid()
        {
            List<VendorItemCode> list = new List<VendorItemCode>();
            list = this.GetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl()).Where(x => x.ItemAction != ItemActionStatus.DELETED).ToList();
            return this.PartialView("_VendorGrid", list.ToList<ITrans>());
        }

        /// <summary>
        /// Deletes the vendor.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteVendor(int index)
        {
            ActionStatus status = new ActionStatus() { IsSucceed = true };
            List<VendorItemCode> list = new List<VendorItemCode>();
            list = this.GetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor.ToString() + "_" + GetFormIdFromUrl());
            VendorItemCode item = list.FirstOrDefault(x => x.Index == index);
            list.RemoveAll(x => x.Index == index);
            if (item != null && item.ID > 0)
            {
                item.ItemAction = ItemActionStatus.DELETED;
                list.Add(item);
            }
            this.SetTempData<List<VendorItemCode>>(TempKeys.ICCPVendor, list.OrderBy(x => x.Index).ToList());
            status.Messages.Add(this.GetResourceValue("Text_VendorDeleted", ResourceName.ICCPForm));
            return this.Json(status, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the vendor.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddVendor()
        {
            return this.PartialView("_AddVendor");
        }
        #endregion
    }
}