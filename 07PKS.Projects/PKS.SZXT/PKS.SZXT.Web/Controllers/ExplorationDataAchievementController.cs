using PKS.SZXT.IService.ExplorationDataAchievement;
using PKS.SZXT.IService.ExprorationOverview;
using PKS.SZXT.Service.ExplorationDataAchievement;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Newtonsoft.Json.JsonConvert;
using PKS.WebAPI;

namespace PKS.SZXT.Web.Controllers
{
    public class ExplorationDataAchievementController : SZXTBaseController
    {
        #region 探井资料
        // GET: ExplorationDataAchievement
        public ActionResult ExploratoryWellData()
        {
            var service = GetService<IExploratoryWellDataService>();
            service.SearchConfig = SearchConfig;
            var conditions = service.GetWellSearchCondition() as Dictionary<string, BOTPropertyDefinition>;
            ViewBag.filterData = SerializeObject(conditions.Select(kv => new
            {
                catelog = kv.Key,
                displayName = kv.Value.DisplayName,
                type = "checkbox",
                list = kv.Value.Options
            }));
            return View();
        }

        public ActionResult GetExploratoryWellData(string year)
        {
            string annual = year == null ? DateTime.Now.Year.ToString() : year;
            var service = GetService<IExploratoryWellDataService>();
            service.SearchConfig = SearchConfig;
            var g1 = service.GetAnnualExploratoryWellStatistics(annual);
            var g2 = service.GetAnnualExploratoryWellStatisticsTable(annual);
            return NewtonJson(new
            {
                g1 = g1,
                g2 = g2
            });
        }
        /// <summary>
        /// 点击搜索按钮后返回查询结果
        /// </summary>
        /// <param name="key">搜索框文本</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExploratoryWellSearchResult(string key, int? from, int? size)
        {
            var service = GetService<IExploratoryWellDataService>();
            service.SearchConfig = SearchConfig;

            var r2 = service.GetExploratoryWellListByName(key, from ?? 0, size ?? 9);
            return Content(SerializeObject(r2), "text/json");
        }
        /// <summary>
        /// 点击查询按钮根据选中的条件搜索
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExploratoryWellFeatureResult(string feature, int? from, int? size)
        {
            var service = GetService<IExploratoryWellDataService>();
            service.SearchConfig = SearchConfig;
            var properties = DeserializeObject<Dictionary<string, List<string>>>(feature);
            var r3 = service.GetExploratoryWellList(properties, from ?? 0, size ?? 9);
            return Content(SerializeObject(r3), "text/json");
        }

        public ActionResult ExploratoryWellDetail(string well)
        {
            var service = GetService<IExploratoryWellDataService>();
            service.SearchConfig = SearchConfig;
            var dictionary = new Dictionary<string, object>();

            //获取二维表数据
            for (var i = 1; i <= 2; i++)
            {
                dictionary.Add("G" + i, service.GetTableData(well, "G" + i));
            }
            //获取列表数据
            for (var i = 4; i <= 10; i++)
            {
                dictionary.Add("G" + i, service.GetExploratoryWellDetailData(well, "G" + i));
            }
            //获取图片数据
            for (var i = 11; i <= 14; i++)
            {
                dictionary.Add("G" + i, service.GetImageData(well, "G" + i));
            }
            //获取html数据
            for (var i = 15; i <= 16; i++)
            {
                dictionary.Add("G" + i, service.GetHtmlData(well, "G" + i));
            }
            ViewBag.Well = well;
            ViewBag.data = SerializeObject(dictionary);
            //ViewBag.nearWells = SerializeObject(service.GetNearWells(well).Select(item => new { title = item }));
            return View();
        }

        #endregion
        #region 分析化验
        public ActionResult AnalysisTest()
        {
            var service = GetService<IAnalysisTestService>();
            service.SearchConfig = SearchConfig;
            var conditions = service.GetWellSearchCondition() as Dictionary<string, BOTPropertyDefinition>;
            ViewBag.filterData = SerializeObject(conditions.Select(kv => new
            {
                catelog = kv.Key,
                displayName = kv.Value.DisplayName,
                type = "checkbox",
                list = kv.Value.Options
            }));
            return View();
        }

        /// <summary>
        /// 点击搜索按钮后返回查询结果
        /// </summary>
        /// <param name="key">搜索框文本</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnalysisTestSearchResult(string key, int? from, int? size)
        {
            var service = GetService<IAnalysisTestService>();
            service.SearchConfig = SearchConfig;

            var r2 = service.GetExploratoryWellListByName(key, from ?? 0, size ?? 10);
            return Content(SerializeObject(r2), "text/json");
        }
        /// <summary>
        /// 点击查询按钮根据选中的条件搜索
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnalysisTestFeatureResult(string feature, int? from, int? size)
        {
            var service = GetService<IAnalysisTestService>();
            service.SearchConfig = SearchConfig;
            var properties = DeserializeObject<Dictionary<string, List<string>>>(feature);
            var r3 = service.GetExploratoryWellList(properties, from ?? 0, size ?? 10);
            return Content(SerializeObject(r3), "text/json");
        }


        public ActionResult AnalysisTestDetail(string well)
        {
            var service = GetService<IAnalysisTestService>();
            service.SearchConfig = SearchConfig;
            var dictionary = new Dictionary<string, IEnumerable<object>>();
            for (var i = 1; i <= SearchConfig.Count; i++)
            {
                dictionary.Add("G" + i, service.GetAnalysisTestDetailData(well, "G" + i));
            }
            ViewBag.Well = well;
            ViewBag.data = SerializeObject(new
            {
                G1 = dictionary["G1"].Concat(dictionary["G2"]),
                G2 = dictionary["G2"],
                G3 = dictionary["G3"].Concat(dictionary["G4"]),
                G4 = dictionary["G4"],
                G5 = dictionary["G5"].Concat(dictionary["G6"]),
                G6 = dictionary["G6"],
                G7 = dictionary["G7"].Concat(dictionary["G8"]),
                G8 = dictionary["G8"],
                G9 = dictionary["G9"].Concat(dictionary["G10"]),
                G10 = dictionary["G10"],
                G11 = dictionary["G11"].Concat(dictionary["G12"]),
                G12 = dictionary["G12"],
            });
            //ViewBag.nearWells = SerializeObject(service.GetNearWells(Request.Params["well"]).Select(item => new { title = item }));
            return View();
        }
        #endregion
        #region 圈闭
        public ActionResult TripReserve()
        {
            var service = GetService<ITripReserveService>();
            service.SearchConfig = SearchConfig;
            //汇总
            //var dictionary = new Dictionary<string, IEnumerable<object>>();
            //for (var i = 1; i <= 6; i++)
            //{
            //    dictionary.Add("G" + i, service.GeTripReserveSummaryData(Request.Params["well"], "G" + i));
            //}
            //ViewBag.summaryData = SerializeObject(dictionary);
            //ViewBag.summaryImg = SerializeObject(service.GetSummaryImageUrl("G0"));

            var BOs = service.GetSecondaryStructurals("二级构造单元");
            
            // ViewBag.years = SerializeObject(years);
            //ViewBag.targets= SerializeObject(targets);
            ViewBag.BOs = BOs;
            ViewBag.DetailData = GetTrapImageByYear(DateTime.Now.Year.ToString(),"白云凹陷");
            return View();
        }

        public string GetTrapImageByYear(string year,string bo)
        {
            var iyear = string.IsNullOrEmpty(year) ? DateTime.Now.Year.ToString() : year;
            var service = GetService<ITripReserveService>();
            service.SearchConfig = SearchConfig;
            var image = service.GetDetailChart(year);
            var table = service.GetTrapStatisticsTable(year,bo, "G12");
            var traps = service.GetAliasTraps("圈闭");
            return SerializeObject(new {  image = image, table = table,traps= traps });
        }

        public ActionResult TripReserveDetail(string trap,string alias)
        {
            var service = GetService<ITripReserveService>();
            service.SearchConfig = SearchConfig;
            var dictionary = new Dictionary<string, object>();
            var g1 = service.GetTrapTable(trap, "G1");
            var g2 = service.GetTrapTable(trap, "G2");
            var g3 = service.GetTrapTable(trap, "G3");
            var g4 = service.GetTrapItems(trap, "G4");
            dictionary.Add("G1",g1);
            dictionary.Add("G2", g2);
            dictionary.Add("G3", g3);
            dictionary.Add("G4", g4);

            ViewBag.Trap = trap; 
            ViewBag.Alias = (trap == alias || alias == null) ? trap : $"{trap}({alias})";
            ViewBag.Data = SerializeObject(dictionary);
            return View();
        }

        #endregion
        #region 物探化工程
        /// <summary>
        /// 物探化工程主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GeoEngineering()
        {
            var service = GetService<IGeoEngineeringService>();
            service.SearchConfig = SearchConfig;

            var r1 = service.GetSWAProperties() as Dictionary<string, BOTPropertyDefinition>;
            //var r2 = service.GetSWAPTListByName("", 0, 9);

            return View(model: SerializeObject(new
            {
                r1 = r1.Select(kv => new
                {
                    catelog = kv.Key,
                    displayName = kv.Value.DisplayName,
                    type = "checkbox",
                    list = kv.Value.Options
                }),
                //r2
            }));
        }

        /// <summary>
        /// 点击搜索按钮后返回查询结果
        /// </summary>
        /// <param name="key">搜索框文本</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GeoEngineeringSearchResult(string key, int? from, int? size)
        {
            var service = GetService<IGeoEngineeringService>();
            service.SearchConfig = SearchConfig;

            var r2 = service.GetSWAListByName(key, from.GetValueOrDefault(0), size.GetValueOrDefault(9));
            return NewtonJson(r2);
        }

        /// <summary>
        /// 点击筛选按钮后返回查询结果
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GeoEngineeringFeatureResult(string feature, int? from, int? size)
        {
            var service = GetService<IGeoEngineeringService>();
            service.SearchConfig = SearchConfig;

            var properties = DeserializeObject<Dictionary<string, List<string>>>(feature);

            // Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();
            //properties.Add("目标区", new List<string>() { "白云深水", "惠州" });
            //properties.Add("作业方式", new List<string>() { "自营" });
            var r3 = service.GetSWAListByProperties(properties, from ?? 0, size ?? 9);
            return Content(SerializeObject(r3), "text/json");
        }

        /// <summary>
        /// 物探化详细信息页
        /// </summary>
        /// <param name="iiid"></param>
        /// <returns></returns>
        public ActionResult GeoEngineeringDetail(string swa)
        {
            var service = GetService<IGeoEngineeringService>();
            service.SearchConfig = SearchConfig;
            var r = service.GetSWAPTByName(swa);
            ViewBag.Swa = swa;
            return View(model: SerializeObject(r));
        }

        #endregion

        public string GetWellThumbnail(string well)
        {
            return GetThumbnail(SearchConfig, well);
        }

        public string GetWellThumbnail_WellData(string well)
        {
            return GetThumbnail(SearchConfig, well);
        }

        public string GetSwaThumbnail(string swa)
        {
            return GetThumbnail(SearchConfig, swa);
        }
    }
}