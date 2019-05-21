namespace BEL.ItemCodeCreationPreProcess.Models.ItemCode
{
    using BEL.CommonDataContract;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Vendor ItemCode
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.ITrans" />
    [DataContract, Serializable]
    public class VendorItemCode : ITrans
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember, IsListColumn(false)]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the vendor.
        /// </summary>
        /// <value>
        /// The name of the vendor.
        /// </value>
        [DataMember, Required, MaxLength(255, ErrorMessage = "Max Allow 255 character.")]
        public string VendorName { get; set; }

        /// <summary>
        /// Gets or sets the fg stock.
        /// </summary>
        /// <value>
        /// The fg stock.
        /// </value>
        [DataMember]
        public double? FGStock { get; set; }

        /// <summary>
        /// Gets or sets the existing component stock.
        /// </summary>
        /// <value>
        /// The existing component stock.
        /// </value>
        [DataMember]
        public double? ExistingComponentStock { get; set; }

        /// <summary>
        /// Gets or sets the date of implementation.
        /// </summary>
        /// <value>
        /// The date of implementation.
        /// </value>
        [DataMember]
        public DateTime? DateOfImplementation { get; set; }

        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        [DataMember, FieldColumnName("RequestID", true, false, "ID")]
        public int RequestID { get; set; }

        /// <summary>
        /// Gets or sets the request by.
        /// </summary>
        /// <value>
        /// The request by.
        /// </value>
        [DataMember, IsPerson(true, false, returnId: true), IsViewer]
        public string RequestBy { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember]
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        [DataMember, IsListColumn(false)]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the item action.
        /// </summary>
        /// <value>
        /// The item action.
        /// </value>
        [DataMember, IsListColumn(false)]
        public ItemActionStatus ItemAction { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }
    }
}