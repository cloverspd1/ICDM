namespace BEL.ItemCodeCreationPreProcess
{
    using BEL.ItemCodeCreationPreProcess.Common;
    using System;
    using System.Web.Mvc;
    
    /// <summary>
    /// Mvc Exception Handler
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FilterAttribute" />
    /// <seealso cref="System.Web.Mvc.IExceptionFilter" />
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class MvcExceptionHandler : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                var currentController = string.Empty;
                var currentAction = string.Empty;
                var ex = filterContext.Exception;
                string id = Guid.NewGuid().ToString();

                Logger.Error("Id: " + id);
                Logger.Error("StatusCode: " + filterContext.HttpContext.Response.StatusCode);
                Logger.Error("Controller: " + currentController);
                Logger.Error("Action: " + currentAction);
                Logger.Error(ex);
                filterContext.HttpContext.Response.Redirect(string.Format("~/Master/Error?msg={0}&errorId={1}", filterContext.Exception.Message, id));
            }
        }
    }
}