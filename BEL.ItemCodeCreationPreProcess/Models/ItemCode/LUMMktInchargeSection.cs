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
    /// LUM MarketingIncharge Section
    /// </summary>
    [DataContract, Serializable]
    public class LUMMktInchargeSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LUMMktInchargeSection"/> class.
        /// </summary>
        public LUMMktInchargeSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LUMMktInchargeSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public LUMMktInchargeSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(ICCPListNames.ICCPMAINLIST, true) };
                this.SectionName = ICCPSectionName.LUMMKTINCHARGESECTION;
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
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember, IsListColumn(false)]
        public int? ID { get; set; }

        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        /// <value>
        /// The item code.
        /// </value>
        [DataMember, FieldColumnName("Title")]
        public string ICCPNo { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Proposed By.
        /// </summary>
        /// <value>
        /// The Proposed By.
        /// </value>
        [DataMember, IsPerson(true, false, returnId: true), IsViewer]
        public string ProposedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the proposed by.
        /// </summary>
        /// <value>
        /// The name of the proposed by.
        /// </value>
        [DataMember, IsPerson(true, false, returnName: true), FieldColumnName("ProposedBy"), IsViewer]
        public string ProposedByName { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember, DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }

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
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        [DataMember, IsListColumn(true)]
        public string Month
        {
            get
            {
                if (this.RequestDate.HasValue)
                {
                    return Convert.ToDateTime(this.RequestDate).ToString("MMMM");
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [DataMember, IsListColumn(true)]
        public string Year
        {
            get
            {
                if (this.RequestDate.HasValue)
                {
                    return Convert.ToDateTime(this.RequestDate).Year.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the lum marketing delegate.
        /// </summary>
        /// <value>
        /// The lum marketing delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnId: true), RequiredOnDelegate, IsViewer]
        public string LUMMarketingDelegate { get; set; }

        /// <summary>
        /// Gets or sets the name of the lum marketing delegate.
        /// </summary>
        /// <value>
        /// The name of the lum marketing delegate.
        /// </value>
        [DataMember, IsPerson(true, true, returnName: true), FieldColumnName("LUMMarketingDelegate"), IsViewer]
        public string LUMMarketingDelegateName { get; set; }

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
                if (this.ApproversList.Any(p => p.Role == ICCPRoles.LUMMARKETINGDELEGATE && string.IsNullOrEmpty(p.Approver)))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is iccp request retrieved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is iccp request retrieved; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsICCPRequestRetrieved { get; set; }

        /// <summary>
        /// Gets or sets the old iccp no.
        /// </summary>
        /// <value>
        /// The old iccp no.
        /// </value>
        [DataMember]
        public string OldICCPNo { get; set; }

        /// <summary>
        /// Gets or sets the old iccp created date.
        /// </summary>
        /// <value>
        /// The old iccp created date.
        /// </value>
        [DataMember]
        public DateTime? OldICCPCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the old iccp rejected date.
        /// </summary>
        /// <value>
        /// The old iccp rejected date.
        /// </value>
        [DataMember]
        public DateTime? OldICCPRejectedDate { get; set; }

        /// <summary>
        /// Gets or sets the old iccp identifier.
        /// </summary>
        /// <value>
        /// The old iccp identifier.
        /// </value>
        [DataMember, IsListColumn(false)]
        public int OldICCPId { get; set; }
    }
}