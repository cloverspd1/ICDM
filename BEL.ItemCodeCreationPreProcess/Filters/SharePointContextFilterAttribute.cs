namespace BEL.ItemCodeCreationPreProcess
{
    using BEL.ItemCodeCreationPreProcess.Common;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// SharePoint action filter attribute.
    /// </summary>
    public class SharePointContextFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <exception cref="System.ArgumentNullException">filterContext</exception>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            Uri redirectUrl;
            switch (SharePointContextProvider.CheckRedirectionStatus(filterContext.HttpContext, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    Logger.Info("Get OK");
                    return;
                case RedirectionStatus.ShouldRedirect:
                    Logger.Info("ShouldRedirect");
                    filterContext.Result = new RedirectResult(redirectUrl.AbsoluteUri);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    if (this.EnvironmentLive)
                    {
                        Logger.Info("CanNotRedirect");
                        filterContext.Result = new ViewResult { ViewName = "Error" };
                    }

                    break;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [environment live].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [environment live]; otherwise, <c>false</c>.
        /// </value>
        public bool EnvironmentLive
        {
            get
            {
                bool environmentLive = false;
                bool.TryParse(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EnvironmentLive"]), out environmentLive);
                return environmentLive;
            }
        }
    }
}
