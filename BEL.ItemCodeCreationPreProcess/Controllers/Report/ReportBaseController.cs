namespace BEL.ItemCodeCreationPreProcess.Controllers.Report
{
    using BEL.CommonDataContract;
    using BEL.ItemCodeCreationPreProcess.BusinessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Reports;
    using System.Collections.Generic;

    /// <summary>
    /// Report Base Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.BaseController" />
    public class ReportBaseController : BaseController
    {
        /// <summary>
        /// Gets the report details.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns></returns>
        public List<ReportDetails> GetReportDetails(Models.Reports.Report report)
        {
            return ReportBusinessLayer.Instance.GetReportDetails(report);
        }

        /// <summary>
        /// Gets the status list.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetStatusList()
        {
            return ReportBusinessLayer.Instance.GetStatusList();
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetRoleList()
        {
            return ReportBusinessLayer.Instance.GetRoleList();
        }
    }
}