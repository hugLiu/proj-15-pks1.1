using PKS.DbServices;
using PKS.SZZSK.IService.Common;
using PKS.SZZSK.Web.Models;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace PKS.SZZSK.Web.Controllers
{
    public class CaseController : SZZSKController
    {
        private KCaseService kCaseService;
        private KCaseInputService kCaseInputService;

        public CaseController(KCaseService kCaseService, KCaseInputService kCaseInputService)
        {
            this.kCaseService = kCaseService;
            this.kCaseInputService = kCaseInputService;
        }

        // GET: Case
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string iiid, string dataid)
        {
            ViewBag.InstanceId = dataid;
            return View();
        }

        public JsonResult GetCaseTree()
        {
            var result = kCaseService.GetCaseTree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCase(string theme, int from = 1, int size = 16)
        {
            var service = GetService<IViewService>();
            service.SearchConfig = SearchConfig;
            var qParam = new string[] { theme, from.ToString(), size.ToString() };
            var G1 = service.GetIndexDatasByQuery("G1", qParam, false);
            var total = service.GetCountByQuery("G2", qParam);
            var cases = new List<CaseItem>();
            if (G1 != null && G1.Count() > 0)
            {
                foreach(var item in G1)
                {
                    var dataId = item["dataid"].ToString().ToInt32();
                    cases.Add(new CaseItem
                    {
                        Id = dataId,
                        IIId = item["iiid"].ToString(),
                        Name = item["title"].ToString(),
                        HasChart = HasChart(dataId),
                        Contents = item["abstract"].ToString(),
                        //Image = ObjectToBytes(item["thumbnail"])
                    });
                }
            }
            var model = new
            {
                Data = cases,
                Total = total
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchInstance(string text, int from = 1, int size = 16)
        {
            var service = GetService<IViewService>();
            service.SearchConfig = SearchConfig;
            var qParam = new string[] { text, from.ToString(), size.ToString() };
            var G1 = service.GetIndexDatasByQuery("G2", qParam, false);
            var total = service.GetCountByQuery("G2", qParam);
            var cases = new List<CaseItem>();
            if (G1 != null && G1.Count() > 0)
            {
                foreach (var item in G1)
                {
                    var dataId = item["dataid"].ToString().ToInt32();
                    cases.Add(new CaseItem
                    {
                        Id = dataId,
                        IIId = item["iiid"].ToString(),
                        Name = item["title"].ToString(),
                        HasChart = HasChart(dataId),
                        Contents = item["abstract"].ToString(),
                        //Image = ObjectToBytes(item["thumbnail"])
                    });
                }
            }
            var model = new
            {
                Data = cases,
                Total = total
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInstanceChart(int instanceId)
        {
            var result = kCaseService.GetInstanceChart(instanceId);
            if (result == null) return null;
            return new FileContentResult(result, "image/jpeg");           
        }

        public bool HasChart(int instanceId)
        {
            return kCaseService.HasChart(instanceId);
        }

        public JsonResult GetParamTreeGrid(int instanceId)
        {
            var result = kCaseService.GetParamTreeGrid(instanceId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCharts(int instanceId)
        {
            var charts = kCaseService.GetCharts(instanceId)
                .Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    charttype = t.ChartType == 1 ? "图版" : "公式",
                    parameters = t.Parameters
                });
            return Json(charts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetailChart(int instanceId, int chartId)
        {
            var result = kCaseInputService.GetInstanceChart(instanceId, chartId);
            if (result == null) return null;
            return new FileContentResult(result, "image/jpeg");
        }

        public JsonResult GetInstanceInfo(int instanceId)
        {
            var result = kCaseService.GetInstanceInfo(instanceId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}