using System;
using System.Web.Mvc;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService;
using PKS.SZXT.IService.ExplorationDynamic;
using PKS.Utils;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.SZXT.Web.Controllers
{
    public class ExplorationDynamicController : SZXTBaseController
    {
        public ViewResult Drilling()
        {
            var svc = GetService<IDrillingService>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetIndexDatasByQuery("g1", null, true);
            var g2_1 = svc.GetIndexDataByQuery("g2_1", null, false);
            var g2_2 = BuildTables(svc, g2_1, "well", "Well", "g2_2");
            var model = new
            {
                g1 = g1,
                g2 = g2_2
            };
            return View(model: SerializeObject(model));
        }

        public string GetDrillingWellInfo(string wellId, string date)
        {
            var svc = GetService<IDrillingService>();
            svc.SearchConfig = SearchConfig;
            var qparams = new string[] { wellId };
            var g3_1 = svc.GetWellBaseForm(wellId);
            var g3_2 = svc.GetIndexDatasByQuery("g3_2", qparams, true);
            //var g3_3 = svc.GetIndexDatasByQuery("g3_3", qparams, true);
            var g3_4 = svc.GetWellDesignConstruct(wellId);
            //var g3_5 = svc.GetWellActualConstruct(wellId);
            var g3_6 = svc.GetIndexDatasByQuery("g3_6", qparams, true);
            //qparams = new string[] { wellId, DateTime.Now.ToEsDate() };
            //var g3_7 = svc.GetIndexDatasByQuery("g3_7", qparams, true);
            var g3_8 = svc.GetNearTargetIndexDatas("g3_8", "g3_9", wellId, "well");

            var model = new
            {
                g3_1 = g3_1,
                g3_2 = g3_2,
                //g3_3 = g3_3,
                g3_4 = g3_4,
                //g3_5 = g3_5,
                g3_6 = g3_6,
                //g3_7 = g3_7,
                g3_8 = g3_8
            };

            return SerializeObject(model);
        }

        public ViewResult Logging()
        {
            var svc = GetService<ILoggingService>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetIndexDatasByQuery("g1", null, true);
            var g2_1 = svc.GetIndexDataByQuery("g2_1", null, false);
            var g2_2 = BuildTables(svc, g2_1, "well", "Well", "g2_2");
            var model = new
            {
                g1 = g1,
                g2 = g2_2
            };
            return View(model: SerializeObject(model));
        }

        public ViewResult Detection()
        {
            var svc = GetService<IDetectionService>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetIndexDatasByQuery("g1", null, true);
            var g2 = svc.GetIndexDataByQuery("g2_1", null, false);
            g2["Content"] = BuildTables(svc, g2, "well", "Well", "g2_2");
            var model = new
            {
                g1 = g1,
                g2 = g2
            };
            return View(model: SerializeObject(model));
        }
        public ViewResult Testing()
        {
            var svc = GetService<ITestingService>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetIndexDatasByQuery("g1", null, true);
            var g2_1 = svc.GetIndexDataByQuery("g2_1", null, false);
            var g2_2 = BuildTables(svc, g2_1, "well", "Well", "g2_2");
            var model = new
            {
                g1 = g1,
                g2 = g2_2
            };
            return View(model: SerializeObject(model));
        }

        public ViewResult GeophysicalExploration()
        {
            var svc = GetService<IGeophysicalExpService>();
            svc.SearchConfig = SearchConfig;
            var g1 = svc.GetIndexDatasByQuery("g1", null, true);
            var g2_1 = svc.GetIndexDataByQuery("g2_1", null, false);
            var g2_2 = BuildTables(svc, g2_1, "swa", "Block", "g2_2");
            var model = new
            {
                g1 = g1,
                g2 = g2_2
            };
            return View(model: SerializeObject(model));
        }

        public string GetLoggingWellInfo(string wellId, string date)
        {
            var svc = GetService<ILoggingService>();
            svc.SearchConfig = SearchConfig;
            var dt = string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.Parse(date);
            var qparams = new string[] { wellId };
            var g3_1 = svc.GetIndexDatasByQuery("g3_1", qparams, true);
            var g3_2 = svc.GetIndexDatasByQuery("g3_2", qparams, true);
            var g3_3 = svc.GetOilGasForm(wellId);
            qparams = new string[] { wellId, dt.ToEsDate() };
            var g3_4 = svc.GetIndexDatasByQuery("g3_4", qparams, true);
            var g3_5 = svc.GetNearTargetIndexDatas("g3_5", "g3_6", wellId, "well");

            var model = new
            {
                g3_1 = g3_1,
                g3_2 = g3_2,
                g3_3 = g3_3,
                g3_4 = g3_4,
                g3_5 = g3_5
            };

            return SerializeObject(model);
        }
        public string GetDetectionWellInfo(string wellId, string date)
        {
            var svc = GetService<IDetectionService>();
            svc.SearchConfig = SearchConfig;
            var g3_1 = svc.GetPrimaryExplationPicture(wellId);
            var g3_2 = svc.GetPrimaryExplationForm(wellId);
            var g3_3 = svc.GetNearTargetIndexDatas("g3_2", "g3_4", wellId, "well");

            var model = new
            {
                g3_1 = g3_1,
                g3_2 = g3_2,
                g3_3 = g3_3
            };

            return SerializeObject(model);
        }

        public string GetTestingWellInfo(string wellId, string date)
        {
            var svc = GetService<ITestingService>();
            svc.SearchConfig = SearchConfig;
            var dt = string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.Parse(date);
            var qparams = new string[] { wellId };
            var g3_1 = svc.GetIndexDatasByQuery("g3_1", qparams, true);
            //var g3_2 = svc.GetIndexDatasByQuery("g3_2", qparams, true);
            var g3_3 = svc.GetFormationTestingProductData(wellId);
            qparams = new string[] { wellId, dt.ToEsDate() };
            //var g3_4 = svc.GetIndexDatasByQuery("g3_4", qparams, true);
            var g3_5 = svc.GetNearTargetIndexDatas("g3_5", "g3_6", wellId, "well");
            var model = new
            {
                g3_1 = g3_1,
                //g3_2 = g3_2,
                g3_3 = g3_3,
                //g3_4 = g3_4,
                g3_5 = g3_5
            };

            return SerializeObject(model);
        }
        public string GetGeophysicalInfo(string swa, string date)
        {
            var svc = GetService<IGeophysicalExpService>();
            svc.SearchConfig = SearchConfig;
            var dt = date.IsNullOrEmpty() ? DateTime.Now : DateTime.Parse(date);
            var qparams = new string[] { swa, dt.ToEsDate() };
            var g3_1 = svc.GetIndexDatasByQuery("g3_1", qparams, true);
            var g3_2 = svc.GetEarthquakeSamplingBaseForm(swa, dt);
            var g3_3 = svc.GetEarthquakeSamplingAreaPositionPicture(swa, dt);
            var g3_4 = svc.GetIndexDatasByQuery("g3_4", qparams, true);

            var model = new
            {
                g3_1 = g3_1,
                g3_2 = g3_2,
                g3_3 = g3_3,
                g3_4 = g3_4
            };

            return SerializeObject(model);
        }
    }
}