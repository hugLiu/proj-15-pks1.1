using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PKS.Web.MVC.Filter;

namespace PKS.Web.MVC
{
    /// <summary>MVC全局配置</summary>
    public static class PKSMvcConfig
    {
        /// <summary>默认路由名</summary>
        public static string DefaultRouteName { get; } = "Default";
        /// <summary>注册全局过滤器</summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            PKSMvcExtension.AuthenticationConfig.Initialize();
            filters.Add(WebBootstrapper.Get<PKSAuthorizeAttribute>());
            filters.Add(WebBootstrapper.Get<PKSExceptionFilterAttribute>());
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
        /// <summary>注册全局过滤器</summary>
        public static void RegisterGlobalFiltersForPortalMgmt(GlobalFilterCollection filters)
        {
            //filters.Clear();
            filters.Add(WebBootstrapper.Get<PKSMgmtAuthorizeAttribute>());
            filters.Add(WebBootstrapper.Get<PKSMgmtExceptionFilterAttribute>());
        }
    }
}