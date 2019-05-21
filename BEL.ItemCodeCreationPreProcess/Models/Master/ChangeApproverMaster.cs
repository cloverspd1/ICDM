namespace BEL.ItemCodeCreationPreProcess.Models.Master
{
    using CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Change Approver Master
    /// </summary>
    [DataContract, Serializable]
    public class ChangeApproverMaster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeApproverMaster"/> class.
        /// </summary>
        public ChangeApproverMaster()
        {
            RoleNameList = new List<NameValueData>();
            PendingWithWhomUserList = new List<NameValueData>();
            ReplaceByWhomUserList = new List<NameValueData>();
            ChangeApproverMasterList = new List<ChangeApproverMaster>();
        }

        /// <summary>
        /// Gets or sets the role name list.
        /// </summary>
        /// <value>
        /// The role name list.
        /// </value>
        [DataMember]
        public List<NameValueData> RoleNameList { get; set; }

        /// <summary>
        /// Gets or sets the pending with whom user list.
        /// </summary>
        /// <value>
        /// The pending with whom user list.
        /// </value>
        [DataMember]
        public List<NameValueData> PendingWithWhomUserList { get; set; }

        /// <summary>
        /// Gets or sets the replace by whom user list.
        /// </summary>
        /// <value>
        /// The replace by whom user list.
        /// </value>
        [DataMember]
        public List<NameValueData> ReplaceByWhomUserList { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [DataMember,Required]
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the pending with whom.
        /// </summary>
        /// <value>
        /// The pending with whom.
        /// </value>
        [DataMember, Required]
        public string PendingWithWhom { get; set; }

        /// <summary>
        /// Gets or sets the replace by whom.
        /// </summary>
        /// <value>
        /// The replace by whom.
        /// </value>
        [DataMember, Required]
        public string ReplaceByWhom { get; set; }

        /// <summary>
        /// Gets or sets the job status.
        /// </summary>
        /// <value>
        /// The job status.
        /// </value>
        [DataMember]
        public string JobStatus { get; set; }

        /// <summary>
        /// Gets or sets the change approver master list.
        /// </summary>
        /// <value>
        /// The change approver master list.
        /// </value>
        [DataMember]
        public List<ChangeApproverMaster> ChangeApproverMasterList { get; set; }
    }
}