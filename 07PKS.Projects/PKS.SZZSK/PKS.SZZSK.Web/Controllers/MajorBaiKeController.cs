using PKS.SZZSK.Core.Common;
using PKS.SZZSK.IService.Common;
using PKS.SZZSK.IService.MajorBaiKe;
using PKS.SZZSK.Service.Common;
using PKS.SZZSK.Web.Models;
using PKS.Utils;
using PKS.WebAPI;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.SZZSK.Web.Controllers
{
    public class MajorBaiKeController : SZZSKController
    {
       
        #region Stratum

        public ActionResult SearchStratum()
        {
            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var GBO = GetBotFilters("井").Data;
            ViewBag.Model = new
            {
                G1,
                GBO
            }.ToJson();

            return View();
        }

        public ActionResult Stratum(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("地层百科", bo);

            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var ImageList = new List<string>();
            var qParamsbo = new string[] { bo };
            var G1 = service.GetIndexDatasByQuery("G1", qParamsbo, false);
            if (G1 != null && G1.Count() > 0)
            {
                foreach(var item in G1)
                {
                    ImageList.Add(item["iiid"].ToString());
                }
            }

            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var qParams2 = new string[] { bo, "8" };
            var G3 = service.GetIndexDatasByQuery("G3", qParams2, true);

            ViewBag.Model = new
            {
                ImageList,
                G2,
                G3
            }.ToJson();
            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/MajorBaiKe/Stratum");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }
        
        #region Structure
        public ActionResult SearchStructure()
        {
            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var GBO = GetBotFilters("井").Data;
            ViewBag.Model = new
            {
                G1,
                GBO
            }.ToJson();

            return View();
        }

        public ActionResult Structure(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("构造百科", bo);

            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var ImageList = new List<string>();
            var qParamsbo = new string[] { bo };
            var G1 = service.GetIndexDatasByQuery("G1", qParamsbo, false);
            if (G1 != null && G1.Count() > 0)
            {
                foreach (var item in G1)
                {
                    ImageList.Add(item["iiid"].ToString());
                }
            }
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var qParams2 = new string[] { bo, "8" };
            var G3 = service.GetIndexDatasByQuery("G3", qParams2, true);

            ViewBag.Model = new
            {
                ImageList,
                G2,
                G3
            }.ToJson();
            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/MajorBaiKe/Structure");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region Sedimentation

        public ActionResult SearchSedimentation()
        {
            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var GBO = GetBotFilters("井").Data;
            ViewBag.Model = new
            {
                G1,
                GBO
            }.ToJson();

            return View();
        }

        public ActionResult Sedimentation(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("沉积百科", bo);

            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var ImageList = new List<string>();
            var qParamsbo = new string[] { bo };
            var G1 = service.GetIndexDatasByQuery("G1", qParamsbo, false);
            if (G1 != null && G1.Count() > 0)
            {
                foreach (var item in G1)
                {
                    ImageList.Add(item["iiid"].ToString());
                }
            }
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var qParams2 = new string[] { bo, "8" };
            var G3 = service.GetIndexDatasByQuery("G3", qParams2, true);

            ViewBag.Model = new
            {
                ImageList,
                G2,
                G3
            }.ToJson();
            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/MajorBaiKe/Sedimentation");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region Reservoir

        public ActionResult SearchReservoir()
        {
            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var GBO = GetBotFilters("井").Data;
            ViewBag.Model = new
            {
                G1,
                GBO
            }.ToJson();

            return View();
        }

        public ActionResult Reservoir(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("储层百科", bo);

            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var ImageList = new List<string>();
            var qParamsbo = new string[] { bo };
            var G1 = service.GetIndexDatasByQuery("G1", qParamsbo, false);
            if (G1 != null && G1.Count() > 0)
            {
                foreach (var item in G1)
                {
                    ImageList.Add(item["iiid"].ToString());
                }
            }
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var qParams2 = new string[] { bo, "8" };
            var G3 = service.GetIndexDatasByQuery("G3", qParams2, true);

            ViewBag.Model = new
            {
                ImageList,
                G2,
                G3
            }.ToJson();
            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/MajorBaiKe/Reservoir");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region OilGenerating

        public ActionResult SearchOilGenerating()
        {
            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var GBO = GetBotFilters("井").Data;
            ViewBag.Model = new
            {
                G1,
                GBO
            }.ToJson();

            return View();
        }

        public ActionResult OilGenerating(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("生油百科", bo);

            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var ImageList = new List<string>();
            var qParamsbo = new string[] { bo };
            var G1 = service.GetIndexDatasByQuery("G1", qParamsbo, false);
            if (G1 != null && G1.Count() > 0)
            {
                foreach (var item in G1)
                {
                    ImageList.Add(item["iiid"].ToString());
                }
            }
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var qParams2 = new string[] { bo, "8" };
            var G3 = service.GetIndexDatasByQuery("G3", qParams2, true);

            ViewBag.Model = new
            {
                ImageList,
                G2,
                G3
            }.ToJson();
            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/MajorBaiKe/OilGenerating");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }

        #endregion

        #region PoolForming

        public ActionResult SearchPoolForming()
        {
            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;

            var qParams = new string[] { "8" };
            var G1 = service.GetIndexDatasByQuery("G1", qParams, true);
            var GBO = GetBotFilters("井").Data;
            ViewBag.Model = new
            {
                G1,
                GBO
            }.ToJson();

            return View();
        }

        public ActionResult PoolForming(string iiid, string dataid, string bo)
        {
            if (dataid != null) bo = dataid;
            else ReocrdBaikeUserebavior("成藏百科", bo);

            var service = GetService<IMajorBaiKeDataService>();
            service.SearchConfig = SearchConfig;
            var ImageList = new List<string>();
            var qParamsbo = new string[] { bo };
            var G1 = service.GetIndexDatasByQuery("G1", qParamsbo, false);
            if (G1 != null && G1.Count() > 0)
            {
                foreach (var item in G1)
                {
                    ImageList.Add(item["iiid"].ToString());
                }
            }
            var qParams = new string[] { "8" };
            var G2 = service.GetIndexDatasByQuery("G2", qParams, true);
            var qParams2 = new string[] { bo, "8" };
            var G3 = service.GetIndexDatasByQuery("G3", qParams2, true);

            ViewBag.Model = new
            {
                ImageList,
                G2,
                G3
            }.ToJson();
            ViewBag.BO = bo;
            var templateUrl = GetTemplateByUrl("/MajorBaiKe/PoolForming");
            ViewBag.TemplateUrl = templateUrl;
            return View();
        }


        #endregion

        /// <summary>
        /// 点击搜索按钮后返回查询结果
        /// </summary>
        /// <param name="key">搜索框文本</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSearchResult(string type, string name, int from = 1, int size = 16)
        {
            var service = GetService<IMajorBaiKeDataService>();
            //var total = GetSearchTotal(SearchConfig, "GSearch", type, name, 100, 1);
            //var viewModel = ToMajorViewModel(type, SearchConfig, total, "GSearch", type, name, size, from);

            var total = service.GetBOCountByProperties("二级构造单元", new Dictionary<string, List<string>>());
            var bos = service.GetBoListByName("二级构造单元", name, from, size);

            var viewModel = BosToMajorViewModel(type,  total, bos);
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
        public JsonResult GetFeatureResult(string type, string bot, string feature, int from = 1, int size = 16)
        {
            var service = GetService<IMajorBaiKeDataService>();
            var properties = feature.JsonTo<Dictionary<string, List<string>>>();
            var bos = service.GetBoListByFeature(bot, properties, from, size); 
            var total = service.GetBOCountByProperties(bot, properties);
            var viewModel = ToMajorViewModel(type, SearchConfig, total, "GFeature", type, bos, size, 1);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetBos(string bot)
        {
            IViewService _viewService = GetService<IViewService>();
            return Json(_viewService.GetTargets(bot), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBots()
        {
            var service = GetService<IMajorBaiKeDataService>();
            var bots = service.GetBots().Select(t => t.Name);
            return Json(bots, JsonRequestBehavior.AllowGet);
            
        }



        public JsonResult GetBotFilters(string bot)
        {
            var service = GetService<IMajorBaiKeDataService>();

            var data = service.GetBot(bot);
            var model = data.Properties.Where(t => t.Scenario == BOTScenarioType.Filter&& t.Options!=null).Select(t => new
            {
                catelog = data.Name,
                displayName = t.DisplayName ??t.Name,
                type = "checkbox",
                list = t.Options
            });
            var temp = model.ToJson();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private int GetSearchTotal(Dictionary<string, string> SearchConfig,  string grid, params object[] args)
        {
            var serviceBase = GetService<ViewServiceBase>();
            var query = SearchConfig[grid];
            query = query.ToEsQuery(args);
            var model = serviceBase.GetEsList(query);
            return model.Count();
        }

        

        private MajorViewModel BosToMajorViewModel(string type, long total, List<string> bos)
        {
            var data = new List<object>();
            foreach (var bo in bos)
            {
                var a = new ThumbnailViewModel
                {
                    iiid = bo,
                    title = bo + type + "专业研究",
                    thumbnail = "/Content/images/Objects/tb" + type + ".jpg"
                };
                //if (bo.ContainsKey("thumbnail"))
                //    if (bo["thumbnail"].ToString() != "[]")
                //        a.thumbnail = bo["thumbnail"];
                data.Add(a);
            }

            return new MajorViewModel
            {
                Data = data,
                Total = total
            };
        }

        private MajorViewModel ToMajorViewModel(string type, Dictionary<string, string> SearchConfig, long total, string grid,params object[] args)
        {
            var data = new List<object>();

            var serviceBase = GetService<ViewServiceBase>();
            var query = SearchConfig[grid];
            query = query.ToEsQuery(args);
            var model = serviceBase.GetEsList(query);

            foreach(var item in model)
            {
                var a = new ThumbnailViewModel
                {
                    iiid = item["iiid"],
                    title = item["title"],
                    thumbnail = "/Content/images/Objects/tb" + type + ".jpg"
                };
                if (item.ContainsKey("thumbnail"))
                    if (item["thumbnail"].ToString() != "[]")
                        a.thumbnail = item["thumbnail"];
                data.Add(a);
            }

            return new MajorViewModel
            {
                Data = data,
                Total = total
            };
        }
    }
}