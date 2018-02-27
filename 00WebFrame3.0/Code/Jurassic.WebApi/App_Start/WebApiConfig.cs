using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Jurassic.WebApi
{
    public static class WebApiConfig 
    {
        public static void Register(HttpConfiguration config)
        {
            //设置跨域访问
            EnableCrossSiteRequests(config);

            //config.Routes.MapHttpRoute(
            //    name: "WebAppApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.MapHttpAttributeRoutes();
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));


            // 取消注释下面的代码行可对具有 IQueryable 或 IQueryable<T> 返回类型的操作启用查询支持。
            // 若要避免处理意外查询或恶意查询，请使用 QueryableAttribute 上的验证设置来验证传入查询。
            // 有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=279712。
            //config.EnableQuerySupport();

            // 若要在应用程序中禁用跟踪，请注释掉或删除以下代码行
            // 有关详细信息，请参阅: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }

        /// <summary>
        /// 注册全局服务都允许跨域处理
        /// </summary>
        /// <param name="config"></param>
        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new System.Web.Http.Cors.EnableCorsAttribute(
            origins: "*",
            headers: "*",
            methods: "*"            
            );
            cors.SupportsCredentials = true;
            config.EnableCors(cors);
        }


    }
}
