
using PKS.SZXT.IService.ExplorationResearchAchievement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Newtonsoft.Json.JsonConvert;
using PKS.SZXT.IService.ExplorationDecision;
using PKS.Web;
using PKS.Models;
using PKS.WebAPI;
using PKS.SZXT.Infrastructure;

namespace PKS.SZXT.Web.Controllers
{
    /// <summary>
    /// 研究成果
    /// </summary>
    public class ExplorationResearchAchievementController : SZXTBaseController
    {
        // GET: ExplorationResearchAchievement
        public ActionResult BasinResearch()
        {
            return View();
        }
        private static object s_Lock = new object();
        public static bool s_Loaded = false;
        public ActionResult AreaResearch()
        {
            var svc = GetService<IAreaSearchService>();
            var content = SearchConfig;
            if (!s_Loaded)
            {
                lock (s_Lock)
                {
                    if (!s_Loaded)
                    {
                        CheckConfig_AreaResearch(svc, content);
                        s_Loaded = true;
                    }
                }
            }
            svc.SearchConfig = content;
            var targets = svc.GetTargetsByConfig("g0");
            var years = GetYears();
            var model = new
            {
                filters = new List<object> {
                    new {
                        catelog = "二级构造单元",
                        displayName = "目标区",
                        type = "checkbox",
                        list = targets
                    },
                    new {
                        catelog = "年份",
                        displayName = "年份",
                        type = "checkbox",
                        list = years
                    }
                }
            };
            ViewBag.Title = "区域研究";
            ViewBag.viewConfig = SearchConfig["viewConfig"];
            return View("ExplorationResearch", model: SerializeObject(model));
        }
        private void CheckConfig_AreaResearch(IAreaSearchService svc, Dictionary<string, string> content)
        {
            var basins = svc.GetBOsByProperties("盆地", null, 0, short.MaxValue)
                .Select(e => @"""" + e + @"""").ToArray();
            var basins2 = string.Join(",", basins);
            var flevels = svc.GetBOsByProperties("一级构造单元", null, 0, short.MaxValue)
                .Select(e => @"""" + e + @"""").ToArray();
            var flevels2 = string.Join(",", flevels);
            var keys = content.Keys.ToList();
            foreach (var key in keys)
            {
                var value = content[key];
                value = value.Replace(@"""%BOT-Basin%""", basins2);
                value = value.Replace(@"""%BOT-FirstLevel%""", flevels2);
                content[key] = value;
            }
        }
        public ActionResult GeochemicalResearch()
        {
            var svc = GetService<IResearchAchievementService>();
            svc.SearchConfig = SearchConfig;
            var targets = svc.GetTargetsByConfig("g0");
            var years = GetYears();
            var model = new
            {
                filters = new List<object> {
                    new {
                        catelog = "二级构造单元",
                        displayName = "目标区",
                        type = "checkbox",
                        list = targets,
                        checkAll = true
                    },
                    new {
                        catelog = "年份",
                        displayName = "年份",
                        type = "checkbox",
                        list = years,
                        checkAll = true
                    }
                }
            };
            ViewBag.Title = "地化研究";
            ViewBag.viewConfig = SearchConfig["viewConfig"];
            return View("ExplorationResearch", model: SerializeObject(model));
        }

        public ActionResult GetTreeData(List<string> bos ,List<int> years, Dictionary<string, string> searchConfig)
        {
            var svc = GetService<IResearchAchievementService>();
            svc.SearchConfig = searchConfig;
            var tree = svc.GetSearchConfigTreeWithQuantity("tree", bos, years);
            return Content(SerializeObject(tree));
        }


        public ActionResult DepositionResearch()
        {
            var svc = GetService<IDepositionResearchService>();
            svc.SearchConfig = SearchConfig;
            var targets = svc.GetTargetsByConfig("g0");
            var years = GetYears();
            var model = new
            {
                filters = new List<object> {
                    new {
                        catelog = "二级构造单元",
                        displayName = "目标区",
                        type = "checkbox",
                        list = targets
                    },
                    new {
                        catelog = "年份",
                        displayName = "年份",
                        type = "checkbox",
                        list = years
                    }
                }
            };
            ViewBag.Title = "沉积储层研究";
            ViewBag.viewConfig = SearchConfig["viewConfig"];
            return View("ExplorationResearch", model: SerializeObject(model));
        }
        public string FragInfoOfArea(List<string> bos, string grid, List<int> years)
        {
            return GetFragInfoOfBo(SearchConfig, bos, grid, years);
        }
        public string FragInfoOfGeochemical(List<string> bos, string grid, List<int> years)
        {
            return GetFragInfoOfBo(SearchConfig, bos, grid, years);
        }
        public string FragInfoOfDisposition(List<string> bos, string grid, List<int> years)
        {
            return GetFragInfoOfBo(SearchConfig, bos, grid, years);
        }
        public string FragInfoOfTarget(List<string> bos, string grid, List<int> years)
        {
            return GetFragInfoOfBo(SearchConfig, bos, grid, years);
        }
        public ActionResult GetTreeDataOfArea(List<string> bos, List<int> years)
        {
            return GetTreeData(bos, years, SearchConfig);
        }
        public ActionResult GetTreeDataOfGeochemical(List<string> bos, List<int> years)
        {
            return GetTreeData(bos, years, SearchConfig);
        }
        public ActionResult GetTreeDataOfDisposition(List<string> bos, List<int> years)
        {
            return GetTreeData(bos, years, SearchConfig);
        }
        public ActionResult GetTreeDataOfTarget(List<string> bos, List<int> years)
        {
            return GetTreeData(bos, years, SearchConfig);
        }
        /// <summary>
        /// 目标&油气藏评价主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TargetEvaluation()
        {
            var service = GetService<ITargetEvaluationService>();
            service.SearchConfig = SearchConfig;
            var r1 = service.GetTrapProperties() as Dictionary<string, BOTPropertyDefinition>;
            return View(model: SerializeObject(new
            {
                r1 = r1.Select(kv => new
                {
                    catelog = kv.Key,
                    displayName = kv.Value.DisplayName,
                    type = "checkbox",
                    list = kv.Value.Options
                })
            }));
        }

        public string GetTargetEvaluationSearchResult(string key, int? from, int? size)
        {
            var svc = GetService<ITargetEvaluationService>();
            svc.SearchConfig = SearchConfig;
            var model = svc.GetTrapListByName(key, from, size);
            return SerializeObject(model);
        }

        public string GetTargetEvaluationFeatureResult(string feature, int? from, int? size)
        {
            var svc = GetService<ITargetEvaluationService>();
            svc.SearchConfig = SearchConfig;
            var model = svc.GetTrapListByProperties(DeserializeObject<Dictionary<string, List<string>>>(feature), from, size);
            return SerializeObject(model);
        }

        public ActionResult TargetEvaluationDetail(string trap)
        {
            var svc = GetService<ITargetEvaluationService>();
            svc.SearchConfig = SearchConfig;
            var model = trap;
            ViewBag.trap = trap;
            ViewBag.Title = $"{trap}目标评价";
            ViewBag.tree = SearchConfig["tree"];
            ViewBag.viewConfig = SearchConfig["viewConfig"].ToEsQuery("圈闭名称", trap, trap);
            return View("ExplorationResearch", model: SerializeObject(model));
        }

        public string GetTrapThumbnail(string trap)
        {
            return GetThumbnail(SearchConfig, trap);
        }

        public ActionResult ReservoirEvaluation()
        {
            return View();
        }


        #region 地化研究

        public ActionResult GeophysicalResearch()
        {
            return View();
        }

        #endregion
        private IEnumerable<int> GetYears(int count = 10)
        {
            var curYear = DateTime.Now.Year;
            for (int i = 0; i < count; i++)
            {
                yield return curYear - i;
            }
        }

    }
}