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
    /// Costing Incharge Section
    /// </summary>
    [DataContract, Serializable]
    public class CostingInchargeSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CostingInchargeSection"/> class.
        /// </summary>
        public CostingInchargeSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CostingInchargeSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public CostingInchargeSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(ICCPListNames.ICCPMAINLIST, true) };
                this.SectionName = ICCPSectionName.COSTINGINCHARGESECTION;
                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ApproverMaster());
                this.MasterData.Add(new WarrantyMaster());
                this.MasterData.Add(new CurrencyMaster());
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
        /// Gets or sets the costing delegate.
        /// </summary>
        /// <value>
        /// The costing delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnId: true), RequiredOnDelegate, IsViewer]
        public string CostingDelegate { get; set; }

        /// <summary>
        /// Gets or sets the name of the costing delegate.
        /// </summary>
        /// <value>
        /// The name of the costing delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnName: true), FieldColumnName("CostingDelegate"), IsViewer]
        public string CostingDelegateName { get; set; }

        /// <summary>
        /// Gets or sets the cost price.
        /// </summary>
        /// <value>
        /// The cost price.
        /// </value>
        [DataMember, Required]
        public double? CostPrice { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [DataMember, Required]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the HSN code.
        /// </summary>
        /// <value>
        /// The HSN code.
        /// </value>
        [DataMember, Required]
        public string HSNCode { get; set; }

        /// <summary>
        /// Gets or sets the GST rate.
        /// </summary>
        /// <value>
        /// The GST rate.
        /// </value>
        [DataMember, Required]
        public double? GSTRate { get; set; }

        /// <summary>
        /// Gets or sets the warranty.
        /// </summary>
        /// <value>
        /// The warranty.
        /// </value>
        [DataMember, Required]
        public double? Warranty { get; set; }

        /// <summary>
        /// Gets or sets the costing delegate2.
        /// </summary>
        /// <value>
        /// The costing delegate2.
        /// </value>
        [DataMember, IsPerson(true, true, returnId: true), Required, IsViewer]
        public string CostingDelegate2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the costing delegate2.
        /// </summary>
        /// <value>
        /// The name of the costing delegate2.
        /// </value>
        [DataMember, IsPerson(true, true, returnName: true), FieldColumnName("CostingDelegate2"), IsViewer]
        public string CostingDelegate2Name { get; set; }

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
                if (this.ApproversList.Any(p => p.Role == ICCPRoles.COSTINGDELEGATE1 && string.IsNullOrEmpty(p.Approver)))
                {
                    return true;
                }            
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is submitted1.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is submitted1; otherwise, <c>false</c>.
        /// </value>
        [DataMember, IsListColumn(false)]
        public bool IsSubmitted1
        {
            get
            {
                if (this.ApproversList.Any(p => p.Role == ICCPRoles.COSTINGDELEGATE2 && string.IsNullOrEmpty(p.Approver)))
                {
                    return true;
                }
                return false;
            }
        }
    }
}