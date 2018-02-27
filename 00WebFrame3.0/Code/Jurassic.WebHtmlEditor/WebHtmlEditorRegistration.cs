using System.Web.Mvc;
using System.Web.Routing;

namespace Jurassic.WebHtmlEditor
{
    public class WebHtmlEditorRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebHtmlEditor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //在别的项目引用该程序集时，自动执行这个操作，而不需要其他程序手动写这一行
        
        }
    }
}
