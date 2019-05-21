namespace BEL.ItemCodeCreationPreProcess.Models.Role
{
    using BEL.CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Role Activities
    /// </summary>
    [DataContract, Serializable]
    public class RoleActivities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActivities"/> class.
        /// </summary>
        public RoleActivities()
        {
            this.RoleList = new List<NameValueData>();
            this.MenuList = new List<MenuDetails>();
            this.RoleActivityData = new List<RoleScreenMappingDetails>();
        }

        /// <summary>
        /// Gets or sets the role list.
        /// </summary>
        /// <value>
        /// The role list.
        /// </value>
        [DataMember]
        public List<NameValueData> RoleList { get; set; }

        /// <summary>
        /// Gets or sets the menu list.
        /// </summary>
        /// <value>
        /// The menu list.
        /// </value>
        [DataMember]
        public List<MenuDetails> MenuList { get; set; }

        /// <summary>
        /// Gets or sets the role activity data.
        /// </summary>
        /// <value>
        /// The role activity data.
        /// </value>
        [DataMember]
        public List<RoleScreenMappingDetails> RoleActivityData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is all menu authorized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is all menu authorized; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsAllMenuAuthorized { get; set; }
    }
}