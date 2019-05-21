namespace BEL.ItemCodeCreationPreProcess.Models.ItemCode
{
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// TDS Incharge Section
    /// </summary>
    [DataContract, Serializable]
    public class TDSInchargeSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TDSInchargeSection"/> class.
        /// </summary>
        public TDSInchargeSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TDSInchargeSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public TDSInchargeSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(ICCPListNames.ICCPMAINLIST, true) };
                this.SectionName = ICCPSectionName.TDSINCHARGESECTION;
                this.ApproversList = new List<ApplicationStatus>();
                this.CurrentApprover = new ApplicationStatus();
                this.Files = new List<FileDetails>();
                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ApproverMaster());
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
        /// Gets or sets the TDS delegate.
        /// </summary>
        /// <value>
        /// The TDS delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnId: true), RequiredOnDelegate, IsViewer]
        public string TDSDelegate { get; set; }

        /// <summary>
        /// Gets or sets the name of the TDS delegate.
        /// </summary>
        /// <value>
        /// The name of the TDS delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnName: true), FieldColumnName("TDSDelegate"), IsViewer]
        public string TDSDelegateName { get; set; }

        /// <summary>
        /// Gets or sets the TDS attachment.s
        /// </summary>
        /// <value>
        /// The TDS attachment.
        /// </value>
        [DataMember, Required]
        public string TDSAttachment { get; set; }

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
        /// Gets a value indicating whether this instance is submitted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is submitted; otherwise, <c>false</c>.
        /// </value>
        [DataMember, IsListColumn(false)]
        public bool IsSubmitted
        {
            get
            {
                if (this.ApproversList.Any(p => p.Role == ICCPRoles.TDSDELEGATE && string.IsNullOrEmpty(p.Approver)))
                {
                    return true;
                }
                return false;
            }
        }
    }
}