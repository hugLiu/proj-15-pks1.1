using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PKS.Web;
using PKS.SZXT.Web.Config;

namespace PKS.SZXT.Web
{
    /// <summary>SZXT Web MVC应用程序</summary>
    public class SZXTMvcApplication : PKSMvcBaseApplication
    {
        /// <summary>启动</summary>
        protected override void Application_Start()
        {
            base.Application_Start();
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ConfigService.Run(this);
        }
    }
}
