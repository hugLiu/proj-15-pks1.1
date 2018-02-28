using Newtonsoft.Json;
using PKS.Utils;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace PKS.Web
{
    /// <summary>Windows集成登录</summary>
    public class WindowsLoginHandler : HttpTaskAsyncHandler, IRequiresSessionState
    {
        /// <summary>集成登录路径,必须与web.config中的路径一致</summary>
        public static string Path { get; } = "WindowsLogin";
        /// <summary>加入混合认证路由</summary>
        public static void AddMixedAuthRoute(RouteCollection routes)
        {
            if (PKSMvcExtension.AuthenticationConfig.IsMixedAuthentication)
            {
                routes.IgnoreRoute(Path);
            }
        }
        /// <summary>处理Windows集成登录</summary>
        public override Task ProcessRequestAsync(HttpContext httpContext)
        {
            var result = httpContext.GetHttpContextWrapper().WindowsLogin(httpContext.Request.LogonUserIdentity);
            httpContext.Response.ContentType = MimeTypes.JSON;
            httpContext.Response.Write(result.ToJson(new JsonSerializerSettings()));
            httpContext.Response.End();
            return Utility.CompletedTask;
        }
    }
}
