using Jurassic.WebFrame;
using System.Web.Mvc;

namespace Jurassic.WebFrame
{
    public class AppCenterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AppCenter";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AppCenter_default",
                "AppCenter/{controller}/{action}/{id}",
                new { controller = "AppManager", action = "Index", id = UrlParameter.Optional }
            );

            //将bundle的注册从Global.asax移到此处，是为了在别的项目引用该程序集时，自动执行这个操作，而不需要
            //其他程序手动写这一行
            //BundleConfig.RegisterBundles(BundleTable.Bundles);//在MVC3中这个无效

            GlobalFilters.Filters.Add(new JGlobalAuthAttribute());
        }
    }
}
