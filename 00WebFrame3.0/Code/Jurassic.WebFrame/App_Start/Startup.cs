using System;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Web.Routing;
using System.Reflection;
using Jurassic.AppCenter.Resources;

[assembly: OwinStartup(typeof(Jurassic.WebFrame.Startup))]
namespace Jurassic.WebFrame
{

    /// <summary>
    /// 利用Owin对应用程序的初始化操作
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 应用程序模块通过此集合加入初始化类，为Owin只能执行一次
        /// </summary>
        List<IStartupConfig> _configs = new List<IStartupConfig>();

        public void Configuration(IAppBuilder app)
        {
            GetAllConfigs();

            foreach (var cfg in _configs)
            {
                cfg.Config(app);
            }
            _configs.Clear();
            ResHelper.InitStartupStr(typeof(JStr));
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void GetAllConfigs()
        {
            foreach(var ass in MvcApplication.Assemblys)
            {
                foreach(Type type in ass.GetExportedTypes())
                {
                    if (typeof(IStartupConfig).IsAssignableFrom(type) && type.IsClass)
                    {
                       _configs.Add(Activator.CreateInstance(type) as IStartupConfig);
                    }
                    if (typeof(IStartupStr).IsAssignableFrom(type) && type.IsClass)
                    {
                        ResHelper.InitStartupStr(type);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 应用程序初始化接口。
    /// </summary>
    public interface IStartupConfig
    {
        void Config(IAppBuilder app);
    }
}