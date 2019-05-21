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
    /// Final SMS Incharge Section
    /// </summary>
    [DataContract, Serializable]
    public class FinalSMSInchargeSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinalSMSInchargeSection"/> class.
        /// </summary>
        public FinalSMSInchargeSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalSMSInchargeSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public FinalSMSInchargeSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(ICCPListNames.ICCPMAINLIST, true) };
                this.SectionName = ICCPSectionName.FINALSMSINCHARGESECTION;
                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ApproverMaster());
                this.MasterData.Add(new LeadTimeMaster());
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
        /// Gets or sets the final SMS delegate.
        /// </summary>
        /// <value>
        /// The final SMS delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnId: true),  RequiredOnDelegate, IsViewer]
        public string FinalSMSDelegate { get; set; }

        /// <summary>
        /// Gets or sets the final name of the SMS delegate.
        /// </summary>
        /// <value>
        /// The final name of the SMS delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnName: true), FieldColumnName("FinalSMSDelegate"), IsViewer]
        public string FinalSMSDelegateName { get; set; }

        /// <summary>
        /// Gets or sets the rm lead time.
        /// </summary>
        /// <value>
        /// The rm lead time.
        /// </value>
        [DataMember, Required]
        public double? RMLeadTime { get; set; }

        /// <summary>
        /// Gets or sets the bm lead time.
        /// </summary>
        /// <value>
        /// The bm lead time.
        /// </value>
        [DataMember, Required]
        public double? BMLeadTime { get; set; }

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
                if (this.ApproversList.Any(p => p.Role == ICCPRoles.FINALSMSDELEGATE && string.IsNullOrEmpty(p.Approver)))
                {
                    return true;
                }
                return false;
            }
        }
    }
}