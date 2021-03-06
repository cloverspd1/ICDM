﻿namespace BEL.ItemCodeCreationPreProcess.Controllers.Role
{
    using BEL.CommonDataContract;
    using BEL.ItemCodeCreationPreProcess.BusinessLayer;
    using BEL.ItemCodeCreationPreProcess.Models.Role;
    using System.Collections.Generic;

    /// <summary>
    /// RoleA ctivity Base Controller
    /// </summary>
    /// <seealso cref="BEL.ItemCodeCreationPreProcess.Controllers.BaseController" />
    public class RoleActivityBaseController : BaseController
    {
        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns></returns>
        public List<NameValueData> GetAllRoles()
        {
            return RoleActivityBusinessLayer.Instance.GetAllRoles();
        }

        /// <summary>
        /// Gets all screens.
        /// </summary>
        /// <returns></returns>
        public List<MenuDetails> GetAllScreens()
        {
            return RoleActivityBusinessLayer.Instance.GetAllScreens();
        }

        /// <summary>
        /// Gets the role activity details by role identifier.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public List<RoleScreenMappingDetails> GetRoleActivityDetailsByRoleId(string roleName)
        {
            return RoleActivityBusinessLayer.Instance.GetRoleActivityDetailsByRoleId(roleName);
        }

        /// <summary>
        /// Updates the role wise activity.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public ActionStatus UpdateRoleWiseActivity(List<RoleScreenMappingDetails> list)
        {
            return RoleActivityBusinessLayer.Instance.UpdateRoleWiseActivity(list);
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public ActionStatus AddRole(string roleName)
        {
            return RoleActivityBusinessLayer.Instance.AddRole(roleName);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public ActionStatus DeleteRole(int roleId)
        {
            return RoleActivityBusinessLayer.Instance.DeleteRole(roleId);
        }
    }
}