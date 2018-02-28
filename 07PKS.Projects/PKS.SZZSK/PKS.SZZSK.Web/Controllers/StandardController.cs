using PKS.SZZSK.IService.Standar;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.SZZSK.Web.Controllers
{
    public class StandardController : SZZSKController
    {
        // GET: Standard
        public ActionResult List()
        {
            var service = GetService<IStandard>();
            service.SearchConfig = SearchConfig;
            var G1 = service.GetIndexDatasByQuery("G1", new string[] { }, false);
            var G2 = service.GetIndexDatasByQuery("G2", new string[] { }, false);
            var G3 = service.GetIndexDatasByQuery("G3", new string[] { }, false);
            var G4 = service.GetIndexDatasByQuery("G4", new string[] { }, false);
            ViewBag.Model = new
            {
                G1,
                G2,
                G3,
                G4
            }.ToJson();
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}