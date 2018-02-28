using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Controllers;
using PKS.WebAPI.Services;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PKS.WebAPI.Filter
{
    /// <summary>PKS授权验证特性</summary>
    public class PKSAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>PKS授权验证特性</summary>
        public PKSAuthorizeAttribute(ISecurityService securityService)
        {
            SecurityService = securityService;
        }

        /// <summary>安全服务实例</summary>
        public ISecurityService SecurityService { get; set; }

        /// <summary>是否授权</summary>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var token = actionContext.Request.GetToken();
            if (token.IsNullOrEmpty()) return false;
            var principal = this.SecurityService.GetPrincipal(token);
            if (principal == null) return false;
            actionContext.ControllerContext.Controller.As<PKSApiController>().Token = token;
            actionContext.RequestContext.Principal = principal;
            return base.IsAuthorized(actionContext);
        }
        /// <summary>处理未授权的请求</summary>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            ExceptionCodes.OperationUnauthorized.ThrowUserFriendly("请求未授权！", "Token认证失败！");
        }
    }
}