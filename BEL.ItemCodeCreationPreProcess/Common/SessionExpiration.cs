namespace BEL.ItemCodeCreationPreProcess.Common
{
    using System.Web.Mvc;

    /// <summary>
    /// Session Expiration Class.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public sealed class SessionExpiration : ActionFilterAttribute
    {
        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
          base.OnActionExecuting(filterContext);
        }
    }
}