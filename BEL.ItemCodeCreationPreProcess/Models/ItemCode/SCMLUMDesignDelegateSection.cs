namespace BEL.ItemCodeCreationPreProcess.Models.ItemCode
{
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// SCM LUM Design Delegate Section
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.ISection" />
    [DataContract, Serializable]
    public class SCMLUMDesignDelegateSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SCMLUMDesignDelegateSection"/> class.
        /// </summary>
        public SCMLUMDesignDelegateSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SCMLUMDesignDelegateSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public SCMLUMDesignDelegateSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(ICCPListNames.ICCPMAINLIST, true) };
                this.SectionName = ICCPSectionName.SCMLUMDESIGNDELEGATESECTION;
                this.ApproversList = new List<ApplicationStatus>();
                this.CurrentApprover = new ApplicationStatus();
                this.VendorItemCodeList = new List<ITrans>();
                this.Files = new List<FileDetails>();
                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ApproverMaster());
                this.MasterData.Add(new BuyMakeMaster());
                this.VendorList = new List<NameValueData>();
            }
        }

        /// <summary>
        /// Gets or sets the master data.
        /// </summary>
        /// <value>
        /// The master data.
        /// </value>
        [DataMember, IsListColumn(false), ContainsMasterData(true)]
        public List<IMaster> MasterData { get; set; }

        /// <summary>
        /// Gets or sets the vendor item code list.
        /// </summary>
        /// <value>
        /// The vendor item code list.
        /// </value>
        [DataMember, IsListColumn(false), IsTran(true, ICCPListNames.ITEMCODEVENDORLIST, typeof(VendorItemCode))]
        public List<ITrans> VendorItemCodeList { get; set; }

        /// <summary>
        /// Gets or sets the vendor ids.
        /// </summary>
        /// <value>
        /// The vendor ids.
        /// </value>
        [DataMember]
        public string Vendors { get; set; }

        /// <summary>
        /// Gets or sets the vendor list.
        /// </summary>
        /// <value>
        /// The vendor list.
        /// </value>
        [DataMember, IsListColumn(false)]
        public List<NameValueData> VendorList { get; set; }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        [DataMember, IsFile(true)]
        public List<FileDetails> Files { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string FileNameList { get; set; }

        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        /// <value>
        /// The division.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the workflow status.
        /// </summary>
        /// <value>
        /// The workflow status.
        /// </value>
        [DataMember]
        public string WorkflowStatus { get; set; }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets the form belong to.
        /// </summary>
        /// <value>
        /// The form belong to.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string FormBelongTo { get; set; }

        /// <summary>
        /// Gets or sets the application belong to.
        /// </summary>
        /// <value>
        /// The application belong to.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string ApplicationBelongTo { get; set; }

        /// <summary>
        /// Gets or sets the list details.
        /// </summary>
        /// <value>
        /// The list details.
        /// </value>
        [DataMember, IsListColumn(false)]
        public List<ListItemDetail> ListDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [DataMember, IsListColumn(false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the action status.
        /// </summary>
        /// <value>
        /// The action status.
        /// </value>
        [DataMember, IsListColumn(false)]
        public ButtonActionStatus ActionStatus { get; set; }

        /// <summary>
        /// Gets or sets the approvers list.
        /// </summary>
        /// <value>
        /// The approvers list.
        /// </value>
        [DataMember, IsListColumn(false), IsApproverDetails(true, ICCPListNames.ITEMCODEAPPROVALMATRIX)]
        public ApplicationStatus CurrentApprover { get; set; }

        /// <summary>
        /// Gets or sets the approvers list.
        /// </summary>
        /// <value>
        /// The approvers list.
        /// </value>
        [DataMember, IsListColumn(false), IsApproverMatrixField(true, ICCPListNames.ITEMCODEAPPROVALMATRIX)]
        public List<ApplicationStatus> ApproversList { get; set; }

        /// <summary>
        /// Gets or sets the button caption.
        /// </summary>
        /// <value>
        /// The button caption.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string ButtonCaption { get; set; }

        /// <summary>
        /// Gets or sets the action status.
        /// </summary>
        /// <value>
        /// The action status.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string SendBackTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bis registration applicable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bis registration applicable]; otherwise, <c>false</c>.
        /// </value>
        [DataMember, Required]
        public bool BISRegistrationApplicable { get; set; }

        /// <summary>
        /// Gets or sets the r number.
        /// </summary>
        /// <value>
        /// The r number.
        /// </value>
        [DataMember, Required]
        public string RNumber { get; set; }

        /// <summary>
        /// Gets or sets the upload artwork attachment.
        /// </summary>
        /// <value>
        /// The upload artwork attachment.
        /// </value>
        [DataMember]
        public string UploadArtworkAttachment { get; set; }

        /// <summary>
        /// Gets or sets the buy make.
        /// </summary>
        /// <value>
        /// The buy make.
        /// </value>
        [DataMember, Required]
        public string BuyMake { get; set; }

        /// <summary>
        /// Gets or sets the bom attachment.
        /// </summary>
        /// <value>
        /// The bom attachment.
        /// </value>
        [DataMember]
        public string BOMAttachment { get; set; }

        /// <summary>
        /// Gets or sets the ads attachment.
        /// </summary>
        /// <value>
        /// The ads attachment.
        /// </value>
        [DataMember, Required]
        public string ADSAttachment { get; set; }

        /// <summary>
        /// Gets or sets the specification sheet attachment.
        /// </summary>
        /// <value>
        /// The specification sheet attachment.
        /// </value>
        [DataMember, Required]
        public string SpecificationSheetAttachment { get; set; }

        /// <summary>
        /// Gets or sets the packaging data sheet attachment.
        /// </summary>
        /// <value>
        /// The packaging data sheet attachment.
        /// </value>
        [DataMember, Required]
        public string PackagingDataSheetAttachment { get; set; }

        /// <summary>
        /// Gets or sets the ies file attachment.
        /// </summary>
        /// <value>
        /// The ies file attachment.
        /// </value>
        [DataMember]
        public string IESFileAttachment { get; set; }

        /// <summary>
        /// Gets or sets the l M79 attachment.
        /// </summary>
        /// <value>
        /// The l M79 attachment.
        /// </value>
        [DataMember]
        public string LM79Attachment { get; set; }

        /// <summary>
        /// Gets or sets the product drawing attachment.
        /// </summary>
        /// <value>
        /// The product drawing attachment.
        /// </value>
        [DataMember]
        public string ProductDrawingAttachment { get; set; }
    }
}