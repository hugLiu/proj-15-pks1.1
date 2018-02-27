using Jurassic.WebFrame;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jurassic.WebRepeater
{
    public class WebRepeaterRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebRepeater";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //在别的项目引用该程序集时，自动执行这个操作，而不需要其他程序手动写这一行
        }

        public void AppStart()
        {

        }
    }
}
