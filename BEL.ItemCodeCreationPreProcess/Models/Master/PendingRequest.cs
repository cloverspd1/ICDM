namespace BEL.ItemCodeCreationPreProcess.Models.Master
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Pending Request
    /// </summary>
    [DataContract, Serializable]
    public class PendingRequest
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int? ID { get; set; }

        /// <summary>
        /// Gets or sets the icdm no.
        /// </summary>
        /// <value>
        /// The icdm no.
        /// </value>
        [DataMember]
        public string ICDMNo { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        [DataMember]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the prepared by.
        /// </summary>
        /// <value>
        /// The prepared by.
        /// </value>
        [DataMember]
        public string PreparedBy { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        [DataMember]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the pending with.
        /// </summary>
        /// <value>
        /// The pending with.
        /// </value>
        [DataMember]
        public string PendingWith { get; set; }
    }
}