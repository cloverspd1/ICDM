﻿namespace BEL.ItemCodeCreationPreProcess
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config
    /// </summary>
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862

        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-1.10.2.min.js",
                            "~/Scripts/jquery-ui.js",
                            "~/Scripts/jquery-migrate-1.2.1.min.js",
                            "~/Scripts/select2.full.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Scripts/bootstrap-hover-dropdown.js",
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/jquery.menu.js",
                        "~/Scripts/jquery.tokeninput.js",                  
                        "~/Scripts/responsive-tabs.js"));

            bundles.Add(new ScriptBundle("~/bundles/spcontext").Include(
                        "~/Scripts/jquery.datatable.js",
                        "~/Scripts/spcontext.js",
                        "~/Scripts/fileuploader.js",
                        "~/Scripts/ProjectScripts/SessionUpdater.js",
                        "~/Scripts/ProjectScripts/common.js",
                        "~/Scripts/ProjectScripts/resources.js"));

            //Cache
            bundles.Add(new ScriptBundle("~/bundles/adminindex").Include("~/Scripts/ProjectScripts/Admin/admin.js"));

            //Role Activity
            bundles.Add(new ScriptBundle("~/bundles/RoleActivityScript").Include("~/Scripts/ProjectScripts/RoleActivity/RoleActivity.js"));

            //ICCP Form
            bundles.Add(new ScriptBundle("~/bundles/ICCPPojectScript").Include("~/Scripts/ProjectScripts/ICCPForm/ICCPRequest.js"));

            //ICDM Approvers 
            bundles.Add(new ScriptBundle("~/bundles/ICDMApproversScript").Include("~/Scripts/ProjectScripts/ICDMApprovers/ICDMApprovers.js"));            

            bundles.Add(new ScriptBundle("~/bundles/Reports").Include("~/Scripts/ProjectScripts/Reports/Report.js"));

            //CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/jquery-ui-1.10.4.custom.min.css",
                      "~/Content/fileuploader.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/main.css",
                      "~/Content/token-input.css",
                      "~/Content/style-responsive.css",
                      "~/Content/select2.css"));
        }
    }
}
