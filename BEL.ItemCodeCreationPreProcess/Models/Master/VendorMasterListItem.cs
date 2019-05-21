namespace BEL.ItemCodeCreationPreProcess.Models.Master
{
    using BEL.CommonDataContract;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Department Master List Item
    /// </summary>
    [DataContract, Serializable]
    public class VendorMasterListItem : IMasterItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>  
        [DataMember, FieldColumnName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember, FieldColumnName("VendorCode")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember, FieldColumnName("ID")]
        public int ID { get; set; }

        /// <summary>
        /// Gets the name of the vendor.
        /// </summary>
        /// <value>
        /// The name of the vendor.
        /// </value>
        [DataMember]
        public string VendorName
        {
            get
            {
                return Value + " - " + Title;
            }
        }
    }
}