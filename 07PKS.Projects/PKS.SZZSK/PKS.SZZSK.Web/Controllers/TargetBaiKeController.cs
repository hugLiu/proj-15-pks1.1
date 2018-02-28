using PKS.SZZSK.IService.TargetBaiKe;
using PKS.SZZSK.Service.TargetBaiKe.Model;
using PKS.Utils;
using PKS.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.SZZSK.Web.Controllers
{
    public class TargetBaiKeController : SZZSKController
    {
        #region Well

        public ActionResult SearchWell()
        {
            var service = GetService<IWellDataService>();
            service.SearchConfig = SearchConfig;
            var conditions = service.GetWellSearchCondition() as Dictionary<string, BOTPropertyDefinition>;
            var G1 = conditions.Select(kv => new
            {
                catelog = kv.Key,
                displayName = kv.Value.DisplayName ?? kv.Value.Name,
                type = "checkbox",
                list = kv.Value.Options
            });
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            return View();
        }

        /// <summary>
        /// 点击搜索按钮后返回查询结果
        /// </summary>
        /// <param name="key">搜索框文本</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWellSearchResult(string wellName, int from = 1, int size = 100)
        {
            var service = GetService<IWellDataService>();
            var wells = service.GetWellListByName(wellName, from, size);
            var total = service.GetBOCountByName("井", wellName);
            var viewModel = ToTargetViewModel(SearchConfig, wells, total);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 点击查询按钮根据选中的条件搜索
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWellFeatureResult(string feature, int from = 1, int size = 100)
        {
            var service = GetService<IWellDataService>();
            var properties = feature.JsonTo<Dictionary<string, List<string>>>();
            var wells = service.GetWellListByFeature(properties, from, size);
            var total = service.GetBOCountByProperties("井", properties);
            var viewModel = ToTargetViewModel(SearchConfig, wells, total);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Well(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("井百科", bo);

            var service = GetService<IWellDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var G2 = GetNearBos(bo).Select(w => new { name = w });

            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/TargetBaiKe/Well");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }
        #endregion

        #region SWA

        public ActionResult SearchSWA()
        {
            var service = GetService<ISWADataService>();
            service.SearchConfig = SearchConfig;
            var conditions = service.GetSWASearchCondition() as Dictionary<string, BOTPropertyDefinition>;
            var G1 = conditions.Select(kv => new
            {
                catelog = kv.Key,
                displayName = kv.Value.DisplayName ?? kv.Value.Name,
                type = "checkbox",
                list = kv.Value.Options
            });
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            return View();
        }

        public JsonResult GetSWASearchResult(string swaName, int from = 1, int size = 100)
        {
            var service = GetService<ISWADataService>();
            var swas = service.GetSWAListByName(swaName, from, size);
            var total = service.GetBOCountByName("地震工区", swaName);
            var viewModel = ToTargetViewModel(SearchConfig, swas, total, "swa");
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSWAFeatureResult(string feature, int from = 1, int size = 100)
        {
            var service = GetService<ISWADataService>();
            var properties = feature.JsonTo<Dictionary<string, List<string>>>();
            var swas = service.GetSWAListByFeature(properties, from, size);
            var total = service.GetBOCountByProperties("地震工区", properties);
            var viewModel = ToTargetViewModel(SearchConfig, swas, total, "swa");
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SWA(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("地震工区百科", bo);

            var service = GetService<ISWADataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);

            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/TargetBaiKe/SWA");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region Trap

        public ActionResult SearchTrap()
        {
            var service = GetService<ITrapDataService>();
            service.SearchConfig = SearchConfig;
            var conditions = service.GetTrapSearchCondition() as Dictionary<string, BOTPropertyDefinition>;
            var G1 = conditions.Select(kv => new
            {
                catelog = kv.Key,
                displayName = kv.Value.DisplayName ?? kv.Value.Name,
                type = "checkbox",
                list = kv.Value.Options
            });
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            return View();
        }

        public JsonResult GetTrapSearchResult(string trapName, int from = 1, int size = 100)
        {
            var service = GetService<ITrapDataService>();
            var traps = service.GetTrapListByName(trapName, from, size);
            var total = service.GetBOCountByName("圈闭", trapName);
            var viewModel = ToTargetViewModel(SearchConfig, traps, total, "trap");
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTrapFeatureResult(string feature, int from = 1, int size = 100)
        {
            var service = GetService<ITrapDataService>();
            var properties = feature.JsonTo<Dictionary<string, List<string>>>();
            var traps = service.GetTrapListByFeature(properties, from, size);
            var total = service.GetBOCountByProperties("圈闭", properties);
            var viewModel = ToTargetViewModel(SearchConfig, traps, total, "trap");
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Trap(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("圈闭百科", bo);

            var service = GetService<ITrapDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var service2 = GetService<IWellDataService>();

            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);


            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/TargetBaiKe/Trap");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region Unit

        public ActionResult SearchUnit()
        {
            var service = GetService<IUnitDataService>();
            service.SearchConfig = SearchConfig;
            var conditions = service.GetUnitSearchCondition() as Dictionary<string, BOTPropertyDefinition>;
            var G1 = conditions.Select(kv => new
            {
                catelog = kv.Key,
                displayName = kv.Value.DisplayName ?? kv.Value.Name,
                type = "checkbox",
                list = kv.Value.Options
            });
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            ViewBag.Model = new
            {
                G1,
                G2
            }.ToJson();

            return View();
        }

        public JsonResult GetUnitSearchResult(string unitName, int from = 1, int size = 100)
        {
            var service = GetService<IUnitDataService>();
            var units = service.GetUnitListByName(unitName, from, size);
            var total = service.GetBOCountByName("二级构造单元", unitName);
            var viewModel = ToTargetViewModel(SearchConfig, units, total, "unit");
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUnitFeatureResult(string feature, int from = 1, int size = 100)
        {
            var service = GetService<IUnitDataService>();
            var properties = feature.JsonTo<Dictionary<string, List<string>>>();
            var units = service.GetUnitListByFeature(properties, from, size);
            var total = service.GetBOCountByProperties("二级构造单元", properties);
            var viewModel = ToTargetViewModel(SearchConfig, units, total, "unit");
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Unit(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("构造单元百科", bo);

            var service = GetService<IUnitDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);

            ViewBag.Model = new
            {
                G1,
            }.ToJson();

            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/TargetBaiKe/Unit");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region Basin

        public ActionResult Basin(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("盆地百科", bo);
            if (string.IsNullOrEmpty(bo))
                bo = "珠江口盆地";
            var service = GetService<IBasinDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);

            ViewBag.Model = new
            {
                G1,
            }.ToJson();

            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/TargetBaiKe/Basin");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }
        #endregion

        #region 私有方法
        private TargetViewModel ToTargetViewModel(Dictionary<string, string> SearchConfig, IList<string> names, long total, string bot = "well")
        {
            var defalutThumbnail = "/Content/images/Objects/井_缩略.png";
            switch (bot)
            {
                case "swa":
                    defalutThumbnail = "/Content/images/Objects/地震工区_缩略.png";
                    break;
                case "trap":
                    defalutThumbnail = "/Content/images/Objects/圈闭_缩略.png";
                    break;
                case "unit":
                    defalutThumbnail = "/Content/images/Objects/构造单元_缩略.jpg";
                    break;
            }
            var data = new List<object>();
            names.ForEach(w =>
            {
                var th = GetThumbnail(SearchConfig, w).JsonTo<List<Thumbnail>>().FirstOrDefault();
                data.Add(new
                {
                    name = w,
                    title = w,
                    thumbnail = th == null ? defalutThumbnail : "data:image/png;base64," + th.thumbnail
                });
            });

            return new TargetViewModel
            {
                Data = data,
                Total = total
            };
        }

        public class Thumbnail
        {
            public string thumbnail { get; set; }
        }
        #endregion

    }
}