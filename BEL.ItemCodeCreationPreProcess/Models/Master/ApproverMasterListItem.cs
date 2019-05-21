namespace BEL.ItemCodeCreationPreProcess.Models.Master
{
    using BEL.CommonDataContract;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Approver Master ListItem
    /// </summary>
    [DataContract, Serializable]
    public class ApproverMasterListItem : IMasterItem
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
        /// Gets or sets the Role.
        /// </summary>
        /// <value>
        /// The Role.
        /// </value>
        [DataMember, FieldColumnName("Role")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        [DataMember, IsPerson(true, true, returnId: true), FieldColumnName("UserName")]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        [DataMember, IsPerson(true, true, returnName: true), FieldColumnName("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [user selection].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [user selection]; otherwise, <c>false</c>.
        /// </value>
        [DataMember, FieldColumnName("UserSelection")]
        public bool UserSelection { get; set; }
    }
}