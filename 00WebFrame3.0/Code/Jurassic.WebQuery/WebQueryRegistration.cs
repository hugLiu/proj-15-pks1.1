using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jurassic.WebFrame
{
    public class WebQueryRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WebQuery";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            SiteManager.Catalog.InitStaticCatalogs(typeof(AdvQuery));
        }
    }
}
