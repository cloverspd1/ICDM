namespace BEL.ItemCodeCreationPreProcess.Controllers.Master
{
    using System.Web.Helpers;
    using System.Web.Mvc;

    /// <summary>
    /// Master Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.BaseController" />
    public class MasterController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        public JsonResult GetUsers(string q, string d = null)
        {
            return null;
        }

        /// <summary>
        /// Gets the users for sales.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <param name="d">The d.</param>
        /// <param name="g">The g.</param>
        /// <returns></returns>
        public JsonResult GetUsersForSales(string q, string d = "", string g = "")
        {
            return null;
        }

        /// <summary>
        /// Gets the approver information.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        public JsonResult GetApproverInfo(string division)
        {
            return null;
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <returns>
        /// User Information
        /// </returns>
        public JsonResult GetUserInfo(string userEmail)
        {
            return null;
        }

        /// <summary>
        /// Errors the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>
        /// error view
        /// </returns>
        public ActionResult Error(string msg)
        {
            return this.View();
        }

        /// <summary>
        /// Nots the authorize.
        /// </summary>
        /// <returns></returns>
        public ActionResult NotAuthorize()
        {
            return this.View();
        }

        /// <summary>
        /// Gets the tocken.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTocken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            HttpContext.Cache[this.CurrentUser.UserEmail + "_formToken"] = formToken;
            return this.Json(cookieToken + ":" + formToken, JsonRequestBehavior.AllowGet);
        }
    }
}