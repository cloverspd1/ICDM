namespace BEL.CommonDataContract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// User Details
    /// </summary>  
    [DataContract, Serializable]
    public class EmployeeDetails
    {
        ///// <summary>
        ///// Gets or sets the user identifier.
        ///// </summary>
        ///// <value>
        ///// The user identifier.
        ///// </value>
        //[DataMember]
        //public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember, IsPerson(true, false, returnId: true), FieldColumnName("UserName")]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        [DataMember, IsPerson(true, false, returnName: true), FieldColumnName("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the type of the role.
        /// </summary>
        /// <value>
        /// The type of the role.
        /// </value>
        [DataMember]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        [DataMember]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the employee code.
        /// </summary>
        /// <value>
        /// The employee code.
        /// </value>
        [DataMember]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>
        /// The name of the login.
        /// </value>
        [DataMember]
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is avtive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is avtive; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive { get; set; }
    }
}
