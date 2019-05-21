namespace BEL.ItemCodeCreationPreProcess.Models.Role
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Role Screen Mapping Details
    /// </summary>
    [DataContract, Serializable]
    public class RoleScreenMappingDetails
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
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        [DataMember]
        public int? RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [DataMember]
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent menu.
        /// </summary>
        /// <value>
        /// The name of the parent menu.
        /// </value>
        [DataMember]
        public string ParentMenuName { get; set; }

        /// <summary>
        /// Gets or sets the name of the child menu.
        /// </summary>
        /// <value>
        /// The name of the child menu.
        /// </value>
        [DataMember]
        public string ChildMenuName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is authorized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authorized; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsAuthorized { get; set; }
    }
}