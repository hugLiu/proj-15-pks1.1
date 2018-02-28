using PKS.DbServices;
using PKS.DbServices.KCase.Model;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class KCaseInputController : PKSBaseController
    {
        private readonly KCaseInputService kCaseInputService;
        private IIndexerService _indexService;

        public KCaseInputController(KCaseInputService kCaseInputService)
        {
            this.kCaseInputService = kCaseInputService;
            _indexService = GetService<IIndexerService>();
        }

        // GET: KCaseInput
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCase(int themeId, int instanceId, string theme)
        {
            ViewBag.ThemeId = themeId;
            ViewBag.InstanceId = instanceId;
            ViewBag.Theme = theme;           
            return View();
        }

        public JsonResult GetInstancesByThemeId(int themeId)
        {
            var result = kCaseInputService.GetInstancesByThemeId(themeId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParamTreeGrid(int themeId, int instanceId)
        {
            var result = kCaseInputService.GetParamTreeGrid(themeId, instanceId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstance(int id)
        {
            var result = kCaseInputService.GetInstance(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int UpdateInstance(string data)
        {
            InstanceModel model = data.JsonTo<InstanceModel>();
            if(model.Id == -1)
            {
                return kCaseInputService.AddInstance(model, CurrentUser.Name);
            } else
            {
                return kCaseInputService.UpdateInstance(model, CurrentUser.Name);
            }
        }

        public void DeleteInstance(int id)
        {
            kCaseInputService.DeleteInstance(id);
            var list = new List<string>();
            var resourcekey = "勘探知识库\\知识案例\\" + id;
            list.Add(resourcekey.ToMD5());
            _indexService.Delete(list);
        }

        public void UpdateInstanceParam(int instanceId, string data)
        {
            var model = data.JsonTo<List<ParamTreeRow>>();
            kCaseInputService.UpdateInstanceParam(instanceId, model, CurrentUser.Name);
        }

        public ActionResult GetInstanceChart(int instanceId, int chartId)
        {
            var result = kCaseInputService.GetInstanceChart(instanceId, chartId);
            if (result == null) return null;
            return new FileContentResult(result, "image/jpeg");
        }

        [HttpPost]
        public void ChartUpload(int instanceId, int chartId, HttpPostedFileBase file)
        {
            if (chartId == -1) return;
            byte[] bytes = new byte[file.ContentLength];
            using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
            {
                bytes = reader.ReadBytes(file.ContentLength);
            }
            kCaseInputService.UpdateInstanceChart(instanceId, chartId, bytes, CurrentUser.Name);
        }

        public void DeleteInstanceChart(int instanceId, int chartId)
        {
            kCaseInputService.DeleteInstanceChart(instanceId, chartId);
        }

        public void IndexInstance(int instanceId)
        {
            var instance = kCaseInputService.GetInstanceIndexModel(instanceId);
            if (instance == null) return;
            //获取实例信息
            MetadataCollection collection = new MetadataCollection();

            Metadata metadata = new Metadata();
            var resourcekey = "勘探知识库\\知识案例\\" + instance.Id;
            metadata.IIId = resourcekey.ToMD5();
            metadata.IndexedDate = DateTime.Now;
            metadata.Thumbnail = instance.Chart?.ToString();
            metadata.Fulltext = instance.Description;
            metadata.PageId = "21";
            metadata.DataId = instance.Id.ToString();
            metadata["dsn"] = "勘探知识库";
            metadata.ShowType = IndexShowType.Mixing.ToString();
            metadata["title"] = instance.Name;
            metadata["subject"] = null;
            metadata["abstract"] = instance.Description;
            metadata["catalogue"] = instance.KCaseCategory;
            metadata["author"] = instance.Author;
            metadata["submitter"] = null;
            metadata["auditor"] = instance.Auditor;
            metadata["createddate"] = instance.CreateDate;
            metadata["submitteddate"] = null;
            metadata["auditteddate"] = null;
            metadata["status"] = "已审核";
            metadata["frequency"] = null;
            metadata["period"] = null;
            metadata["basin"] = null;
            metadata["firstlevel"] = null;
            metadata["secondlevel"] = null;
            metadata["trap"] = null;
            metadata["well"] = null;
            metadata["swa"] = null;
            metadata["miningarea"] = null;
            metadata["cozone"] = null;
            metadata["project"] = null;
            metadata["pc"] = instance.KCaseCategory;
            metadata["pt"] = "知识案例";
            metadata["bd"] = "勘探";
            metadata["bt"] = "知识案例";
            metadata["bp"] = instance.KCaseTheme;
            metadata["ba"] = null;
            metadata["bf"] = null;
            metadata["system"] = "勘探知识库";
            metadata["resourcetype"] = "勘探知识库\\知识案例\\" + instance.KCaseCategory;
            metadata["resourcekey"] = resourcekey;

            collection.Add(metadata);

            IndexSaveRequest indexrequest = new IndexSaveRequest()
            {
                Replace = true,
                Metadatas = collection
            };
            var iiids = _indexService.Save(indexrequest);
            var indexCount = iiids.Count();
        }
    }
}