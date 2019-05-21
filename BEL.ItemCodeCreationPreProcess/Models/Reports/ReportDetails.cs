namespace BEL.ItemCodeCreationPreProcess.Models.Reports
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Report Details
    /// </summary>
    [DataContract, Serializable]
    public class ReportDetails
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        [DataMember]
        public string ICCPNo { get; set; }

        /// <summary>
        /// Gets or sets the material category.
        /// </summary>
        /// <value>
        /// The material category.
        /// </value>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the bu.
        /// </summary>
        /// <value>
        /// The name of the bu.
        /// </value>
        [DataMember]
        public string ItemDescription { get; set; }     

        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        /// <value>
        /// The product category.
        /// </value>
        [DataMember]
        public string ProductGroup { get; set; }

        /// <summary>
        /// Gets or sets the inward identifier.
        /// </summary>
        /// <value>
        /// The inward identifier.
        /// </value>
        [DataMember]
        public string BuyMake { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        [DataMember]
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        [DataMember]
        public string CostPrice { get; set; }

        /// <summary>
        /// Gets or sets the material location.
        /// </summary>
        /// <value>
        /// The material location.
        /// </value>
        [DataMember]
        public string GSTRate { get; set; }

        /// <summary>
        /// Gets or sets the tester.
        /// </summary>
        /// <value>
        /// The tester.
        /// </value>
        [DataMember]
        public string Warranty { get; set; }

        /// <summary>
        /// Gets or sets the tester assigned date.
        /// </summary>
        /// <value>
        /// The tester assigned date.
        /// </value>
        [DataMember]
        public string TypeofPackaging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is outward done.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is outward done; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is outward done.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is outward done; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the outward identifier.
        /// </summary>
        /// <value>
        /// The outward identifier.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the inward date.
        /// </summary>
        /// <value>
        /// The inward date.
        /// </value>
        [DataMember]
        public string PendingWith { get; set; }       
    }
}