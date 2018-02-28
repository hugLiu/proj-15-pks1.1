using PKS.DbServices;
using PKS.DbServices.Standard.Model;
using PKS.Models;
using PKS.PortalMgmt.Common;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class StandardController : PKSBaseController
    {
        private StandardService standardService;
        private IIndexerService _indexService;

        public StandardController(StandardService standardService)
        {
            this.standardService = standardService;
            _indexService = GetService<IIndexerService>();
        }

        public ActionResult Index()
        {
            ViewBag.ShowSuccess = false;
            return View();
        }

        public ActionResult AddStandard()
        {
            return View();
        }

        public ActionResult EditStandard(string model)
        {
            var data = model.JsonTo<StandardModel>();
            ViewBag.Data = data;
            return View();
        }

        public JsonResult GetStandards()
        {
            var result = standardService.GetStandards();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AddNewStandard(string model)
        {
            var data = model.JsonTo<StandardModel>();
            var result = standardService.AddStandards(new List<StandardModel> { data }, CurrentUser.Name);
            IndexData(result);
        }

        [HttpPost]
        public void UpdateStandard(string model)
        {
            var data = model.JsonTo<StandardModel>();
            StandardModel result = standardService.UpdateStandard(data, CurrentUser.Name);
            IndexData(new List<StandardModel> { result });
        }

        /// <summary> 删除标准，同时删除索引 </summary>
        public void DeleteStandards(string data)
        {
            var ids = data.JsonTo<List<int>>();
            if (ids == null || ids.Count < 1) return;
            var iiids = new List<string>();
            foreach(var id in ids)
            {
                var resourcekey = "勘探知识库\\标准规范\\外部链接\\" + id;
                iiids.Add(resourcekey.ToMD5());
            }
            var count = _indexService.Delete(iiids);
            if (count != null && count.Length > 0)
            {
                standardService.RemoveStandards(ids);
            }
            else
            {
                //删除失败
            }
            
        }

        /// <summary> 重建索引 </summary>
        public void ReIndexData()
        {
            var list = standardService.GetStandards();
            IndexData(list);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Browse(HttpPostedFileBase file)
        {
            if(string.Empty.Equals(file.FileName) || ".xlsx" != Path.GetExtension(file.FileName))
            {
                throw new ArgumentException("当前文件格式不正确，请确保正确的Excel文件格式！");
            }

            var severPath = this.Server.MapPath("/files/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }

            var savePath = Path.Combine(severPath, file.FileName);

            try
            {
                file.SaveAs(savePath);
                List<StandardModel> models = ExcelHelper.ReadExcelToEntityList<StandardModel>(savePath).ToList();
                var list = standardService.AddStandards(models, CurrentUser.Name);
                IndexData(list);
                ViewBag.ShowSuccess = true;
                return View("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                System.IO.File.Delete(savePath);
            }
        }

        /// <summary> 创建索引 </summary>
        public void IndexData(List<StandardModel> list)
        {
            MetadataCollection collection = new MetadataCollection();

            foreach (var item in list)
            {
                Metadata metadata = new Metadata();
                var resourcekey = "勘探知识库\\标准规范\\外部链接\\" + item.Id;
                metadata.IIId = resourcekey.ToMD5();
                metadata.IndexedDate = DateTime.Now;
                metadata.Thumbnail = null;
                metadata.Fulltext = null;
                metadata.PageId = "23";
                metadata.DataId = item.Url;
                metadata["dsn"] = "勘探知识库";
                metadata.ShowType = IndexShowType.Mixing.ToString();
                metadata["title"] = item.Name;
                metadata["subject"] = null;
                metadata["abstract"] = item.Name;
                metadata["catalogue"] = item.Type;
                metadata["author"] = null;
                metadata["submitter"] = null;
                metadata["auditor"] = null;
                metadata["createddate"] = null;
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
                metadata["pc"] = item.Type;
                metadata["pt"] = "标准规范";
                metadata["bd"] = "勘探";
                metadata["bt"] = "标准规范";
                metadata["bp"] = item.Type;
                metadata["ba"] = null;
                metadata["bf"] = null;
                metadata["system"] = "勘探知识库";
                metadata["resourcetype"] = "勘探知识库\\标准规范\\外部链接\\" + item.Type;
                metadata["resourcekey"] = resourcekey;

                collection.Add(metadata);
            }
            if (collection.Count() > 0)
            {
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
}