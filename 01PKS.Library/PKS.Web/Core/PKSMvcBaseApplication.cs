using System;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using PKS.Web.MVC;

namespace PKS.Web
{
    /// <summary>PKS Web MVC应用程序</summary>
    public abstract class PKSMvcBaseApplication : HttpApplication
    {
        /// <summary>WEB启动器</summary>
        protected static PKSWebBootstrapper s_Bootstrapper { get; set; } = new PKSWebBootstrapper();

        /// <summary>启动</summary>
        protected virtual void Application_Start()
        {
            s_Bootstrapper.Initialize();
            PKSMvcConfig.RegisterGlobalFilters(GlobalFilters.Filters);
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