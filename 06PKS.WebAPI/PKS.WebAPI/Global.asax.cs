using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;
using Common.Logging;
using NLog;
using PKS.Core;
using PKS.Models;
using PKS.Web;
using PKS.WebAPI.Services;

namespace PKS.WebAPI
{
    /// <summary>PKS Web API应用程序</summary>
    public class PKSApiApplication : HttpApplication
    {
        /// <summary>WEB启动器</summary>
        protected static PKSWebBootstrapper s_Bootstrapper { get; set; } = new PKSWebBootstrapper();

        /// <summary>启动</summary>
        protected virtual void Application_Start()
        {
            s_Bootstrapper.Initialize();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            PKS.Core.Bootstrapper.Get<ILog>().Info(nameof(Application_Start));
        }

        /// <summary>处理应用程序错误</summary>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
            this.HandleApplicationError();
        }

        /// <summary>停止</summary>
        protected virtual void Application_End()
        {
            this.LogApplicationEnd();
            s_Bootstrapper.Stop();
        }
    }
}