using PKS.Models;
using PKS.WebAPI.Services;
using System;
using System.Web;
using System.Web.Mvc;

namespace PKS.Web.MVC.Filter
{
    /// <summary>后台管理授权验证特性</summary>
    public class PKSMgmtAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>PKS授权验证特性</summary>
        public PKSMgmtAuthorizeAttribute(ISecurityService securityService)
        {
            SecurityService = securityService;
        }

        /// <summary>安全服务实例</summary>
        public ISecurityService SecurityService { get; set; }

        /// <summary>When overridden, provides an entry point for custom authorization checks.</summary>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        /// <param name="context">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
        protected override bool AuthorizeCore(HttpContextBase context)
        {
            if (context.Request.Url.AbsolutePath.StartsWith("/Account/Login", StringComparison.OrdinalIgnoreCase)) return true;
            string token = null;
            IPKSPrincipal principal = null;
            if (!context.IsLogined(this.SecurityService, ref token, out principal)) return false;
            return true;
        }
        /// <summary>Processes HTTP requests that fail authorization.</summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Request.IsAjaxRequest())
            {
                base.HandleUnauthorizedRequest(filterContext);
                return;
            }
            filterContext.Result = new RedirectResult("~/Account/Login");
        }
    }
}