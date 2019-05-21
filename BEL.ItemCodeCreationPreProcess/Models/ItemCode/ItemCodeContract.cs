﻿namespace BEL.ItemCodeCreationPreProcess.Models.ItemCode
{
    using BEL.CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Item Code Contract
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.IContract"/>
    [DataContract, Serializable]
    [KnownType(typeof(ItemCodeForm))]
    public class ItemCodeContract : IContract
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCodeContract"/> class.
        /// </summary>
        public ItemCodeContract()
        {
            this.UserDetails = new UserDetails();
            this.Forms = new List<IForm>();
        }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        /// <value>
        /// The user details.
        /// </value>
        [DataMember]
        public UserDetails UserDetails { get; set; }

        /// <summary>
        /// Gets or sets the forms.
        /// </summary>
        /// <value>
        /// The forms.
        /// </value>
        [DataMember]
        public List<IForm> Forms { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.Forms = null;
                this.UserDetails = null;
            }
        }
    }
}