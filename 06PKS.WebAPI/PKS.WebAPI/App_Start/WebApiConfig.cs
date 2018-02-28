using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using PKS.Core;
using PKS.Utils;
using PKS.WebAPI.Controllers;
using PKS.WebAPI.Filter;

namespace PKS.WebAPI
{
    /// <summary>API配置</summary>
    public static class WebApiConfig
    {
        /// <summary>注册配置</summary>
        public static void Register(HttpConfiguration config)
        {
            // 设置JSON序列化命名策略CamelCase
            JsonUtil.DefaultUseCamelCaseNamingStrategy();
            config.Formatters.JsonFormatter.SerializerSettings.UseCamelCaseNamingStrategy();
            // 跨域
            var cors = new EnableCorsAttribute("*", "*", "*");
            cors.PreflightMaxAge = 1 * 60 * 60 * 24;
            cors.SupportsCredentials = true;
            config.EnableCors(cors);
            // 过滤器
            var exceptionFilterAttribute = Bootstrapper.Get<PKSExceptionFilterAttribute>();
            config.Filters.Add(exceptionFilterAttribute);
            config.Filters.Add(Bootstrapper.Get<PKSAuthorizeAttribute>());
            config.Filters.Add(new PKSParameterValidationAttribute());
            // 异常日志
            config.Services.Add(typeof(IExceptionLogger), exceptionFilterAttribute);
            //config.Services.Replace(typeof(IExceptionHandler), new PKSExceptionHandler());
            // 路由
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
            );
            //删除
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}