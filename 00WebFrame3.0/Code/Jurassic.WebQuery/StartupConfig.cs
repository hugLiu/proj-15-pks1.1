using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jurassic.WebFrame
{
    public class StartupConfig : IStartupConfig
    {
        public  void Config(IAppBuilder app)
        {
            SiteManager.Catalog.InitStaticCatalogs(typeof(AdvQuery));
        }
    }
}
