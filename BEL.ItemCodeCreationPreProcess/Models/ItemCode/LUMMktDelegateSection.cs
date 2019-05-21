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
    /// LUMMkt Delegate Section
    /// </summary>
    [DataContract, Serializable]
    public class LUMMktDelegateSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LUMMktDelegateSection"/> class.
        /// </summary>
        public LUMMktDelegateSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LUMMktDelegateSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public LUMMktDelegateSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(ICCPListNames.ICCPMAINLIST, true) };
                this.SectionName = ICCPSectionName.LUMMKTDELEGATEESECTION;
                this.ApproversList = new List<ApplicationStatus>();
                this.CurrentApprover = new ApplicationStatus();
                ////Add All Master Object which required for this Section.
                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ApproverMaster());
                //this.MasterData.Add(new ProductGroupMaster());
                this.MasterData.Add(new ConfirmedOrderMaster());
                this.MasterData.Add(new TypeOfPackagingMaster());
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
        /// Gets or sets the item description.
        /// </summary>
        /// <value>
        /// The item description.
        /// </value>
        [DataMember, Required]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the product group.
        /// </summary>
        /// <value>
        /// The product group.
        /// </value>
        [DataMember, Required]
        public string ProductGroup { get; set; }

        /// <summary>
        /// Gets or sets the confirmed order from customer.
        /// </summary>
        /// <value>
        /// The confirmed order from customer.
        /// </value>
        [DataMember, Required]
        public string ConfirmedOrderFromCustomer { get; set; }

        /// <summary>
        /// Gets or sets the type of packaging.
        /// </summary>
        /// <value>
        /// The type of packaging.
        /// </value>
        [DataMember, Required]
        public string TypeOfPackaging { get; set; }

        /// <summary>
        /// Gets or sets the reference product.
        /// </summary>
        /// <value>
        /// The reference product.
        /// </value>
        [DataMember]
        public string ReferenceProduct { get; set; }

        /// <summary>
        /// Gets or sets the expected annual business volume.
        /// </summary>
        /// <value>
        /// The expected annual business volume.
        /// </value>
        [DataMember]
        public double? ExpectedAnnualBusinessVolume { get; set; }

        /// <summary>
        /// Gets or sets the item to be phased out.
        /// </summary>
        /// <value>
        /// The item to be phased out.
        /// </value>
        [DataMember]
        public string ItemToBePhasedOut { get; set; }

        /// <summary>
        /// Gets or sets the item phased out with effect from.
        /// </summary>
        /// <value>
        /// The item phased out with effect from.
        /// </value>
        [DataMember, FieldColumnName("ItemPhasedOutWithEffectFrom"), DataType(DataType.Date)]
        public DateTime? ItemPhasedOutWithEffectFrom { get; set; }
    }
}
