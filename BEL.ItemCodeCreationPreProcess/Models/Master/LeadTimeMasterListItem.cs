namespace BEL.ItemCodeCreationPreProcess.Models.Master
{
    using BEL.CommonDataContract;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// LeadTime Master ListItem
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.IMasterItem" />
    [DataContract, Serializable]
    public class LeadTimeMasterListItem : IMasterItem
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
        [DataMember, FieldColumnName("Title")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the rm lead time.
        /// </summary>
        /// <value>
        /// The rm lead time.
        /// </value>
        [DataMember, FieldColumnName("RMLeadTime")]
        public double? RMLeadTime { get; set; }

        /// <summary>
        /// Gets or sets the bm lead time.
        /// </summary>
        /// <value>
        /// The bm lead time.
        /// </value>
        [DataMember, FieldColumnName("BMLeadTime")]
        public double? BMLeadTime { get; set; }
    }
}