namespace BEL.ItemCodeCreationPreProcess.Models.Reports
{
    using BEL.CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Report
    /// </summary>
    [DataContract, Serializable]
    public class Report
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        public Report()
        {
            ReportList = new List<ReportDetails>();
            StatusList = new List<NameValueData>();
            RoleList = new List<NameValueData>();
        }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        [DataMember, DataType(DataType.Date), Required]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets the date values.
        /// </summary>
        /// <value>
        /// The date values.
        /// </value>
        [DataMember, DataType(DataType.Date), Required]
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets the bu identifier.
        /// </summary>
        /// <value>
        /// The bu identifier.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the pending with.
        /// </summary>
        /// <value>
        /// The pending with.
        /// </value>
        [DataMember]
        public string PendingWith { get; set; }

        /// <summary>
        /// Gets or sets the report list.
        /// </summary>
        /// <value>
        /// The report list.
        /// </value>
        [DataMember]
        public List<ReportDetails> ReportList { get; set; }

        /// <summary>
        /// Gets or sets the bu list.
        /// </summary>
        /// <value>
        /// The bu list.
        /// </value>
        [DataMember]
        public List<NameValueData> StatusList { get; set; }

        /// <summary>
        /// Gets or sets the material location list.
        /// </summary>
        /// <value>
        /// The material location list.
        /// </value>
        [DataMember]
        public List<NameValueData> RoleList { get; set; }
    }
}