using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using Common.Logging;
using Discuz.Config;
using Discuz.Forum;
using PKS.Web;

namespace PKS.Forum.Web
{
    /// <summary>Forum Web应用程序</summary>
    public class ForumApplication : PKSMvcBaseApplication
    {
        /// <summary>启动</summary>
        protected override void Application_Start()
        {
            base.Application_Start();
            PageBase.OnAuthorization = ForumExtension.OnAuthorization;
            PageBase.WebApiSiteUrl = PKSMvcExtension.GetWebApiSiteUrl(null);
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
