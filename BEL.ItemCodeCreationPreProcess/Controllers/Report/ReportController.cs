namespace BEL.ItemCodeCreationPreProcess.Controllers.Report
{
    using BEL.ItemCodeCreationPreProcess.Models.Reports;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Report Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.Report.ReportBaseController" />
    public class ReportController : ReportBaseController
    {
        /// <summary>
        /// Icdms the reports.
        /// </summary>
        /// <returns></returns>
        public ActionResult ICDMReports()
        {
            Models.Reports.Report report = new Models.Reports.Report();
            report.StatusList = this.GetStatusList();
            report.RoleList = this.GetRoleList();
            return View("Reports", report);
        }

        //public ActionResult ICDMReports(string fromDate, string toDate, string status, string pendingWith)

        /// <summary>
        /// Icdms the report search.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns></returns>
        public ActionResult ICDMReportSearch(Models.Reports.Report report)
        {
            if (report != null)
            {
                report.ReportList = this.GetReportDetails(report);
            }
            return PartialView("_ReportList", report.ReportList);
        }

        //public ActionResult ICDMReportExportToExcel(string fromDate, string toDate, string status, string pendingWith)

        /// <summary>
        /// Icdms the report export to excel.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="status">The status.</param>
        /// <param name="pendingWith">The pending with.</param>
        /// <returns></returns>
        public ActionResult ICDMReportExportToExcel(string fromDate, string toDate,string status, string pendingWith)
        {
            Models.Reports.Report report = new Models.Reports.Report();
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                report.FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                report.ToDate = Convert.ToDateTime(toDate);
            }
            report.PendingWith = pendingWith;
            report.Status = status;
            List<ReportDetails> reportDetails = this.GetReportDetails(report);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename='ICDM Report_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Millisecond + ".xls'");
            return PartialView("_ReportList", reportDetails);
        }
    }
}