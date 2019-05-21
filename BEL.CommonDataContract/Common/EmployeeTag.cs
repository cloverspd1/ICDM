namespace BEL.CommonDataContract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// User Details
    /// </summary>  
    [DataContract, Serializable]
    public class EmployeeTag
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>      
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>     
        public string Name { get; set; }      
    }
}
