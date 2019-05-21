namespace BEL.ItemCodeCreationPreProcess.Models.Common
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Item Code No Count
    /// </summary>
    [Serializable]
    public class ItemCodeNoCount
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>
        /// The current value.
        /// </value>
        [DataMember]
        public int CurrentValue { get; set; }
    }
}