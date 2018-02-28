using PKS.SZZSK.IService.ExplorationKB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.SZZSK.Web.Controllers
{
    public class ExplorationKBController : SZZSKController
    {
        // GET: ExplorationKB
        public ActionResult Index()
        {
            var service = GetService<IExplorationKBService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            //service.GetTopHots("G10", 8)
            var g1 = service.GetTopHots("G1", 8);
            var g2 = service.GetTopHots("G2", 8);
            var g3 = service.GetIndexDatasByQuery("G3", qParams, true);
            var g4 = service.GetTopHots("G4", 8);
            var g5 = service.GetIndexDatasByQuery("G5", qParams, true);
            var g6 = service.GetIndexDatasByQuery("G6", qParams, true);
            var g7 = service.GetIndexDatasByQuery("G7", qParams, true);
            var g8 = service.GetIndexDatasByQuery("G8", qParams, true);
            var model = new { g1, g2, g3, g4, g5, g6, g7, g8 };
            var str = SerializeObject(model);
     
            return View(model: str);
        }
    }
}