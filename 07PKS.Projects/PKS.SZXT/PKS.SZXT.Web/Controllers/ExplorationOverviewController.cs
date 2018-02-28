using System.Web.Mvc;
using PKS.SZXT.IService.ExprorationOverview;
using PKS.SZXT.Web.Config.PageSearchService;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.SZXT.Web.Controllers
{
    public class ExplorationOverviewController : SZXTBaseController
    {
        // GET: ExplorationOverview
        public ActionResult Index()
        {
            var service = GetService<IExprorationOverviewService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var g1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var g2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var g3 = service.GetCompletionDrilling();
            var g3_1 = service.GetCompletionDrilling_1();
            var g4 = service.GetCompletion2DSeismic();
            var g5 = service.GetCompletion3DSeismic();
            var g6 = service.GetCompletionProject();
            var g7 = service.GetIndexDatasByQuery("G7", qParams, true);
            var g8 = service.GetIndexDatasByQuery("G8", qParams, true);
            var g9 = service.GetIndexDatasByQuery("G9", qParams, true);
            var g10 = service.GetTopHots("G10", 8);
            var g11 = service.GetRecentlyView("G11", PKSUser.Identity.Name, 8);
            var g12_1 = service.GetIndexDataByQuery("G12_1", null, false);
            var g12_2 = BuildTables(service, g12_1, "well", "Well", "G12_2");
            var g13_1 = service.GetIndexDataByQuery("G13_1", null, false);
            var g13_2 = BuildTables(service, g13_1, "well", "Well", "G13_2");
            var g14_1 = service.GetIndexDataByQuery("G14_1", null, false);
            var g14_2 = BuildTables(service, g14_1, "well", "Well", "G14_2");
            var g15_1 = service.GetIndexDataByQuery("G15_1", null, false);
            var g15_2 = BuildTables(service, g15_1, "well", "Well", "G15_2");
            var g16_1 = service.GetIndexDataByQuery("G16_1", null, false);
            var g16_2 = BuildTables(service, g16_1, "swa", "Block", "G16_2");
            //var g13 = service.GetLogging();
            //var g14 = service.GetFormationTestYieldResults();
            //var g15 = service.GetFormationTestDaily();
            //var g16 = service.GetSeismicDynamic();
            var g17 = service.GetIndexDatasByQuery("G17", null, true);
            var g18 = service.GetIndexDatasByQuery("G18", null, true);

            var model = new
            {
                g1 = g1,
                g2 = g2,
                g3 = g3,
                g3_1 = g3_1,
                g4 = g4,
                g5 = g5,
                g6 = g6,
                g7 = g7,
                g8 = g8,
                g9 = g9,
                g10 = g10,
                g11 = g11,
                g12 = g12_2,
                g13 = g13_2,
                g14 = g14_2,
                g15 = g15_2,
                g16 = g16_2,
                g17 = g17,
                g18 = g18,
            };
            var str = SerializeObject(model);
            return View(model: str);
        }
        /// <summary>
        /// 清除配置缓存
        /// </summary>
        public ActionResult ClearConfigCache()
        {
            ExplorationResearchAchievementController.s_Loaded = false;
            PageSearchService.Run(this.HttpContext.ApplicationInstance);
            return Content("OK");
        }
    }
}