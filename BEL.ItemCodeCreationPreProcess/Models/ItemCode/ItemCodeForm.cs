namespace BEL.ItemCodeCreationPreProcess.Models.ItemCode
{
    using BEL.CommonDataContract;
    using BEL.ItemCodeCreationPreProcess.Models.Common;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Item Code Form
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.IForm" />
    [DataContract, Serializable]
    public class ItemCodeForm : IForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCodeForm"/> class.
        /// </summary>
        public ItemCodeForm()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCodeForm" /> class.
        /// </summary>
        /// <param name="setSections">if set to <c>true</c> [set sections].</param>
        public ItemCodeForm(bool setSections)
        {
            if (setSections)
            {
                this.ApprovalMatrixListName = ICCPListNames.ITEMCODEAPPROVALMATRIX;
                this.SectionsList = new List<ISection>();
                this.SectionsList.Add(new LUMMktInchargeSection(true));              ////LEVEL 0
                this.SectionsList.Add(new LUMMktDelegateSection(true));              ////LEVEL 0
                this.SectionsList.Add(new SCMLUMDesignInchargeSection(true));           ////LEVEL 1
                this.SectionsList.Add(new SCMLUMDesignDelegateSection(true));           ////LEVEL 1
                this.SectionsList.Add(new SMSInchargeSection(true));                ////LEVEL 2
                this.SectionsList.Add(new SMSDelegateSection(true));                ////LEVEL 2
                this.SectionsList.Add(new QAInchargeSection(true));               ////LEVEL 3
                this.SectionsList.Add(new QADelegateSection(true));               ////LEVEL 3
                this.SectionsList.Add(new FinalSMSInchargeSection(true));               //// LEVEL 4
                this.SectionsList.Add(new FinalSMSDelegateSection(true));               //// LEVEL 4
                this.SectionsList.Add(new CostingInchargeSection(true));                //// LEVEL 4
                this.SectionsList.Add(new CostingDelegate1Section(true));               //// LEVEL 4
                this.SectionsList.Add(new CostingDelegate2Section(true));               //// LEVEL 4
                this.SectionsList.Add(new TDSInchargeSection(true));      //// LEVEL 5
                this.SectionsList.Add(new TDSDelegateSection(true));      //// LEVEL 5

                this.SectionsList.Add(new ApplicationStatusSection(true) { SectionName = SectionNameConstant.APPLICATIONSTATUS });
                this.SectionsList.Add(new ActivityLogSection(ICCPListNames.ITEMCODEACTIVITYLOG));
                this.Buttons = new List<Button>();
                this.MainListName = ICCPListNames.ICCPMAINLIST;
            }
        }

        /// <summary>
        /// Gets the name of the form.
        /// </summary>
        /// <value>
        /// The name of the form.
        /// </value>
        [DataMember]
        public string FormName
        {
            get { return FormNames.ITEMCODEPREPROCESSFORM; }
        }

        /// <summary>
        /// Gets or sets the form status.
        /// </summary>
        /// <value>
        /// The form status.
        /// </value>
        [DataMember]
        public string FormStatus { get; set; }

        /// <summary>
        /// Gets or sets the form approval level.
        /// </summary>
        /// <value>
        /// The form approval level.
        /// </value>
        [DataMember]
        public int FormApprovalLevel { get; set; }

        /// <summary>
        /// Gets or sets the total approval required.
        /// </summary>
        /// <value>
        /// The total approval required.
        /// </value>
        [DataMember]
        public int TotalApprovalRequired { get; set; }

        /// <summary>
        /// Gets or sets the sections list.
        /// </summary>
        /// <value>
        /// The sections list.
        /// </value>
        [DataMember]
        public List<ISection> SectionsList { get; set; }

        /// <summary>
        /// Gets or sets the buttons.
        /// </summary>
        /// <value>
        /// The buttons.
        /// </value>
        [DataMember]
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// Gets or sets the name of the approval matrix list.
        /// </summary>
        /// <value>
        /// The name of the approval matrix list.
        /// </value>
        [DataMember]
        public string ApprovalMatrixListName { get; set; }

        /// <summary>
        /// Gets or sets the name of the main list.
        /// </summary>
        /// <value>
        /// The name of the main list.
        /// </value>
        [DataMember]
        public string MainListName { get; set; }
    }
}