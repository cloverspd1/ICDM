namespace BEL.CommonDataContract
{
    using System;

    /// <summary>
    /// Is Person Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class IsPersonAttribute : Attribute
    {
        /// <summary>
        /// The is person
        /// </summary>
        private bool isPerson = true;

        /// <summary>
        /// The is multiple
        /// </summary>
        private bool isMultiple;

        /// <summary>
        /// The is returnName
        /// </summary>
        private bool returnName;

        /// <summary>
        /// The is returnName
        /// </summary>
        private bool returnAlias;

        /// <summary>
        /// The is returnAliasWithUserTag
        /// </summary>
        private bool employeeTags;

        /// <summary>
        /// The is returnAliasWithUserTag
        /// </summary>
        private bool id;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsPersonAttribute" /> class.
        /// </summary>
        /// <param name="isPerson">if set to <c>true</c> [is person].</param>
        /// <param name="isMultiple">if set to <c>true</c> [is multiple].</param>
        /// <param name="returnName">if set to <c>true</c> [return name].</param>
        /// <param name="returnAlias">if set to <c>true</c> [return alias].</param>
        /// <param name="returnEmployeeTags">if set to <c>true</c> [return employee tags].</param>
        /// <param name="returnId">if set to <c>true</c> [return identifier].</param>
        public IsPersonAttribute(bool isPerson, bool isMultiple, bool returnName = false, bool returnAlias = false, bool returnEmployeeTags = false, bool returnId = false)
        {
            this.isPerson = isPerson;
            this.isMultiple = isMultiple;
            this.returnName = returnName;
            this.returnAlias = returnAlias;
            this.employeeTags = returnEmployeeTags;
            this.id = returnId;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is person.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is person; otherwise, <c>false</c>.
        /// </value>
        public bool IsPerson
        {
            get
            {
                return this.isPerson;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is multiple.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is multiple; otherwise, <c>false</c>.
        /// </value>
        public bool IsMultiple
        {
            get
            {
                return this.isMultiple;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is multiple.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is multiple; otherwise, <c>false</c>.
        /// </value>
        public bool ReturnName
        {
            get
            {
                return this.returnName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is multiple.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is multiple; otherwise, <c>false</c>.
        /// </value>
        public bool ReturnAlias
        {
            get
            {
                return this.returnAlias;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [employee tags].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [employee tags]; otherwise, <c>false</c>.
        /// </value>
        public bool EmployeeTags
        {
            get
            {
                return this.employeeTags;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [return identifier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [return identifier]; otherwise, <c>false</c>.
        /// </value>
        public bool ReturnID
        {
            get
            {
                return this.id;
            }
        }        
    }
}
