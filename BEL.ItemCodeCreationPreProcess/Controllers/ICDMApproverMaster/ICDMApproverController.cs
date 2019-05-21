namespace BEL.ItemCodeCreationPreProcess.Controllers.ICDMApproverMaster
{
    using BEL.CommonDataContract;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// ICDM Approver Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.ICDMApproverMaster.ICDMApproverBaseController" />
    public class ICDMApproverController : ICDMApproverBaseController
    {
        /// <summary>
        /// Icdms the approver list.
        /// </summary>
        /// <returns></returns>
        [SharePointContextFilter]
        public ActionResult ICDMApproverList()
        {
            ChangeApproverMaster approvers = new Models.Master.ChangeApproverMaster();
            approvers.RoleNameList = this.GetRoleList();
            return View("ICDMApprover", approvers);
        }

        /// <summary>
        /// Gets the name of the user list by role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public JsonResult GetUserListByRoleName(string roleName)
        {
            ChangeApproverMaster approvers = this.GetListofUsers(roleName);
            return this.Json(approvers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the approver replacement list.
        /// </summary>
        /// <param name="approverMaster">The approver master.</param>
        /// <returns></returns>
        public ActionResult GetApproverReplacementList(ChangeApproverMaster approverMaster)
        {
            List<ChangeApproverMaster> list = this.GetApproverReplacements(approverMaster);
            return PartialView("_ICDMApproverList", list);
        }

        /// <summary>
        /// Shows all pending requests.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="approverName">Name of the approver.</param>
        /// <returns></returns>
        public ActionResult ShowAllPendingRequests(string roleName, string approverName)
        {
            List<PendingRequest> list = this.GetAllPendingRequests(roleName, approverName);
            return PartialView("_PendingRequestList", list);
        }

        #region "CRUD Approver Replacement Detail"

        /// <summary>
        /// Adds the edit vendor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApproverReplacementApproverById(int id = 0)
        {
            ChangeApproverMaster approvers = this.GetApproverReplacementById(id);
            return this.PartialView("_ICDMApproverReplacement", approvers);
        }

        /// <summary>
        /// Saves the vendor.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveApproverReplacement(ChangeApproverMaster model)
        {
            ActionStatus status = new ActionStatus() { IsSucceed = true };
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    status = this.SaveApproverReplacementDetails(model);                  
                }
                else
                {
                    status.IsSucceed = false;
                    status.Messages = this.GetErrorMessage(ResourceName.Common);
                }
            }
            return this.Json(status);
        }

        /// <summary>
        /// Deletes the vendor.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteApproverReplacement(int id)
        {
            ActionStatus status = new ActionStatus() { IsSucceed = true };
            status.IsSucceed = this.DeleteApproverReplacementById(id);
            if (status.IsSucceed)
                status.Messages.Add(this.GetResourceValue("Text_ApproverReplacementDeleted", ResourceName.ChangeApproverForm));
            else
                status.Messages.Add(this.GetResourceValue("Text_ApproverReplacementNotDeleted", ResourceName.ChangeApproverForm));
            return this.Json(status, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}