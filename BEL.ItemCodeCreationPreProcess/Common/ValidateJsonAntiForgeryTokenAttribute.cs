﻿namespace BEL.ItemCodeCreationPreProcess.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Validate Json AntiForgery Token Attribute
    /// </summary>
    /// <seealso cref="ActionFilterAttribute" />   
    public sealed class ValidateJsonAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called when [action executed].
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <exception cref="System.UnauthorizedAccessException">Unauthorized AccessException
        /// </exception>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            try
            {
                string cookieToken = string.Empty;
                string formToken = string.Empty;

                // if (filterContext.RequestContext.HttpContext.Request.Headers["UID"] != null)
                if (actionContext.Request.Headers.GetValues("UID") != null)
                {
                    IEnumerable<string> tokenHeaders = null;
                    tokenHeaders = actionContext.Request.Headers.GetValues("UID");
                    string[] tokens = tokenHeaders.First().Split(':');
                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }
                if (HttpContext.Current.Cache[HttpContext.Current.User.Identity.Name + "_formToken"].ToString().Trim().ToLower() != formToken.ToLower().Trim()) throw new UnauthorizedAccessException();
                AntiForgery.Validate(cookieToken, formToken);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new UnauthorizedAccessException();
                // Your error handling here
            }
        }
    }
}