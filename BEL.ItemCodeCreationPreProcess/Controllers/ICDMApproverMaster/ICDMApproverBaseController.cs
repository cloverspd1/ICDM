namespace BEL.ItemCodeCreationPreProcess.Controllers.ICDMApproverMaster
{
    using BEL.CommonDataContract;
    using BEL.ItemCodeCreationPreProcess.BusinessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Master;
    using System.Collections.Generic;

    /// <summary>
    /// ICD MApprover Base Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.BaseController" />
    public class ICDMApproverBaseController : BaseController
    {
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetRoleList()
        {
            return ReportBusinessLayer.Instance.GetRoleList();
        }

        /// <summary>
        /// Gets the listof users.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public ChangeApproverMaster GetListofUsers(string roleName)
        {
            return ItemCodeCreationBusinessLayer.Instance.GetListofUsers(roleName);
        }

        /// <summary>
        /// Gets the approver replacements.
        /// </summary>
        /// <param name="approverMaster">The approver master.</param>
        /// <returns></returns>
        public List<ChangeApproverMaster> GetApproverReplacements(ChangeApproverMaster approverMaster)
        {
            return ItemCodeCreationBusinessLayer.Instance.GetApproverReplacements(approverMaster);
        }

        /// <summary>
        /// Gets all pending requests.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="approverName">Name of the approver.</param>
        /// <returns></returns>
        public List<PendingRequest> GetAllPendingRequests(string roleName, string approverName)
        {
            return ItemCodeCreationBusinessLayer.Instance.GetAllPendingRequests(roleName, approverName);
        }

        /// <summary>
        /// Gets the approver replacement by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ChangeApproverMaster GetApproverReplacementById(int id)
        {
            return ItemCodeCreationBusinessLayer.Instance.GetApproverReplacementById(id);
        }

        /// <summary>
        /// Saves the approver replacement details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionStatus SaveApproverReplacementDetails(ChangeApproverMaster model)
        {
            return ItemCodeCreationBusinessLayer.Instance.SaveApproverReplacementDetails(model);
        }

        /// <summary>
        /// Deletes the approver replacement by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteApproverReplacementById(int id)
        {
            return ItemCodeCreationBusinessLayer.Instance.DeleteApproverReplacementById(id);
        }
    }
}