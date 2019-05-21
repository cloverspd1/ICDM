namespace BEL.ItemCodeCreationPreProcess.Models.Master
{
    using BEL.CommonDataContract;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Type Of Packaging Master ListItem
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.IMasterItem" />
    [DataContract, Serializable]
    public class TypeOfPackagingMasterListItem : IMasterItem
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
    }
}