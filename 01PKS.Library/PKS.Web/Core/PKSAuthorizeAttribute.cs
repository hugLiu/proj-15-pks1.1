using System.Web;
using System.Web.Mvc;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Services;
using PKS.Core;
#pragma warning disable 1591

namespace PKS.Web.MVC.Filter
{
    /// <summary>PKS授权验证特性</summary>
    public class PKSAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>PKS授权验证特性</summary>
        public PKSAuthorizeAttribute(ISecurityServiceWrapper securityService)
        {
            SecurityService = securityService;
        }

        /// <summary>安全服务实例</summary>
        private ISecurityServiceWrapper SecurityService { get; set; }
        /// <summary>Called when a process requests authorization.</summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext" /> parameter is null.</exception>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.SetControllerContext();
            base.OnAuthorization(filterContext);
        }
        /// <summary>When overridden, provides an entry point for custom authorization checks.</summary>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpContext" /> parameter is null.</exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string token = null;
            IPKSPrincipal principal = null;
            if (!httpContext.IsLogined(this.SecurityService, ref token, out principal)) return false;
            if (httpContext.Request.IsAjaxRequest()) return true;
            // TODO: 用于测试
            if (httpContext.Request.IsLocal) return true;
            var requestUrl = httpContext.GetControllerContext().GetAuthorizeUrl();
            return this.SecurityService.HasMenuPermission(principal.Identity.Id.ToInt32(), requestUrl);
        }
        /// <summary>Processes HTTP requests that fail authorization.</summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Request.IsAjaxRequest())
            {
                ExceptionCodes.OperationUnauthorized.ThrowUserFriendly("请求未授权！", "Token认证失败！");
                return;
            }
            var principal = context.GetPrincipal(this.SecurityService);
            string redirectUrl = null;
            if (principal == null)
            {
                redirectUrl = context.GetRedirectUrlToPortalLogin();
            }
            else
            {
                redirectUrl = context.GetRedirectUrlToPortalNoAuthorize();
            }
            filterContext.Result = new RedirectResult(redirectUrl);
        }
    }
}
#pragma warning restore 1591
