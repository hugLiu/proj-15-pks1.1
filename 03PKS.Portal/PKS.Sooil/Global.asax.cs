using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PKS.Web;

namespace PKS.Sooil.Web
{
    /// <summary>Sooil Web MVC应用程序</summary>
    public class SooilMvcApplication : PKSMvcBaseApplication
    {
        /// <summary>启动</summary>
        protected override void Application_Start()
        {
            base.Application_Start();
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
