using Newtonsoft.Json;
using PKS.Core;
using PKS.DbModels;
using PKS.DbServices;
using PKS.DbServices.Models;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using PKS.DbServices.IndexApp;
using PKS.WebAPI.Models;

namespace PKS.SZZSK.Web.Controllers
{
    /// <summary>知识图谱控制器</summary>
    public class KGraphController : SZZSKController
    {
        /// <summary>构造函数</summary>
        public KGraphController(KG_PublicCatalogService publicCatalogService, KG_TopicService topicService)
        {
            this.PublicCatalogService = publicCatalogService;
            this.TopicService = topicService;
        }

        /// <summary>公共图谱服务</summary>
        private KG_PublicCatalogService PublicCatalogService { get; set; }

        /// <summary>公共主题服务</summary>
        private KG_TopicService TopicService { get; set; }

        /// <summary>公共图谱页面</summary>
        public ActionResult Index()
        {
            var models = this.PublicCatalogService.GetFirstLevel();
            return View(models);
        }

        /// <summary>显示某个公共图谱</summary>
        public ActionResult Public(int id)
        {
            var models = this.PublicCatalogService.GetChildren(id);
            var root = models[0];
            if (root.ParentId.HasValue)
            {
                var parents = this.PublicCatalogService.GetParents(root.ParentId.Value);
                models = this.PublicCatalogService.GetChildren(parents.First().Id);
            }
            return View(models);
        }

        /// <summary>获得公共图谱分类树</summary>
        public ActionResult GetPublicCatalogTree()
        {
            return NewtonJson(this.PublicCatalogService.GetAll().ToTreeNodes());
        }

        /// <summary>显示某个公共图谱的全部主题</summary>
        public ActionResult PublicTopics(int? page, int id = 0)
        {
            var iiid = Convert.ToString(Request.Params["iiid"]);
            var dataid = Convert.ToInt32(Request.Params["dataid"]);
            if (!string.IsNullOrWhiteSpace(iiid)&& dataid>0)
            {
                var topic=TopicService.GetTopicById(dataid);
                if (topic == null)
                {
                    throw  new Exception("未找到主题");
                }
                var url = topic.LinkUrl.StartsWith("http") ? topic.LinkUrl : ("http://" + topic.LinkUrl);
                return Redirect(url);
            }


            var models = this.PublicCatalogService.GetParents(id);
            ViewBag.Root = models.First();
            ViewBag.Current = models.Last();
            ViewBag.Parents = models;
            var children = this.PublicCatalogService.GetChildren(id);
            var childrenIds = children.Select(e => e.Id).ToArray();
            var pageNumber = (!page.HasValue || page < 1) ? 1 : page.Value;
            var pageSize = 10;
            var total = 0;
            var topics = this.TopicService.GetPublicListByPage(childrenIds, pageNumber, pageSize, out total);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            return View(topics);
        }

        /// <summary>提交主题到某个公共图谱</summary>
        public ActionResult PublicSubmit(int id)
        {
            var parents = this.PublicCatalogService.GetParents(id);
            var root = parents.First();
            ViewBag.Root = root;
            ViewBag.Current = parents.Last();
            ViewBag.Parents = parents;
            ViewBag.TreeData = this.PublicCatalogService.GetChildren(root.Id).ToTreeNodes().ToJson();
            return View();
        }

        /// <summary>提交主题到某个公共图谱</summary>
        [HttpPost]
        public ActionResult PublicSubmit(KG_NewTopic topic)
        {
            return SaveTopic(topic);
        }

        /// <summary>我的图谱</summary>
        public ActionResult PrivateIndex(int? catalogId)
        {
            if (catalogId == null) catalogId = 0;
            ViewBag.CatalogId = catalogId.Value;
            return View();
        }

        /// <summary>获得私有图谱分类树</summary>
        public ActionResult GetPrivateCatalogTree()
        {
            var service = GetService<KG_PrivateCatalogService>();
            var models = service.GetAll(this.PKSUser.Identity.Name);
            var root = new KG_CatalogNode();
            root.Id = 0;
            root.Name = "我的图谱";
            root.ParentId = null;
            root.Level = -1;
            root.Order = 0;
            models.Add(root);
            return Json(models);
        }

        /// <summary>保存私有图谱分类</summary>
        public ActionResult SavePrivateCatalog(KG_CatalogNode model)
        {
            var result = new WebActionResult();
            if (!ModelState.IsValid)
            {
                result.ErrorMessage = this.GetModelError();
                return Json(result);
            }
            if (model.Id <= 0)
            {
                model.CreatedBy = this.PKSUser.Identity.Name;
            }
            if (model.ParentId.HasValue && model.ParentId.Value == 0)
            {
                model.ParentId = null;
            }
            model.Name = model.Name.Trim();
            model.Code = model.Name;
            model.LastUpdatedBy = model.CreatedBy;
            var service = GetService<KG_PrivateCatalogService>();
            result.Data = service.Save(model);
            result.Succeed = true;
            return Json(result);
        }

        /// <summary>删除</summary>
        public ActionResult DeletePrivateCatalog(KG_CatalogNode model)
        {
            this.PublicCatalogService.Delete(model);
            return Json(model);
        }

        /// <summary>排序</summary>
        public ActionResult SortPrivateCatalogs(KG_CatalogNode[] models)
        {
            var userName = this.PKSUser.Identity.Name;
            foreach (var model in models)
            {
                if (model == null) continue;
                model.LastUpdatedBy = userName;
            }
            this.PublicCatalogService.Sort(models);
            return Json(models);
        }

        /// <summary>获得某个私有图谱分类的全部主题</summary>
        public ActionResult GetPrivateTopics(int catalogId, int page)
        {
            var userName = this.PKSUser.Identity.Name;
            var service = GetService<KG_PrivateCatalogService>();
            //加载私有图谱分类
            int[] childrenIds = null;
            if (catalogId > 0)
            {
                service.GetParents(userName, catalogId);
                var children = service.GetChildren(userName, catalogId);
                childrenIds = children.Select(e => e.Id).ToArray();
            }
            if (page < 1) page = 1;
            var pageInfo = new PageInfo() {CurrentNumber = page, Size = 20};
            var service2 = GetService<KG_TopicService>();
            var topics = service2.GetPrivateListByPage(userName, pageInfo, childrenIds);
            //预加载私有图谱分类
            int[] ids;
            if (childrenIds == null)
            {
                ids = topics.Where(e => e.PrivateCatalogId.HasValue).Select(e => e.PrivateCatalogId.Value).ToArray();
                if (ids.Length > 0) service.GetParents(userName, ids);
            }
            //预加载公共图谱分类
            ids = topics.Where(e => e.PublicCatalogId.HasValue).Select(e => e.PublicCatalogId.Value).ToArray();
            if (ids.Length > 0) GetService<KG_PublicCatalogService>().GetParents(ids);
            pageInfo.Data = topics.MapTo<KG_TopicModel>().ToArray();
            return Json(pageInfo);
        }

        /// <summary>保存主题</summary>
        public ActionResult SaveTopic(KG_NewTopic topic)
        {
            var result = new WebActionResult();
            if (!ModelState.IsValid)
            {
                result.ErrorMessage = this.GetModelError();
                return Json(result);
            }
            if (!topic.PrivateCatalogId.HasValue && !topic.PublicCatalogId.HasValue)
            {
                result.ErrorMessage = "图谱主题必须属于私有图谱分类或公共图谱分类！";
                return Json(result);
            }
            var model = this.TopicService.Save(topic, this.PKSUser.Identity.Name);
            result.Data = model.MapTo<KG_NewTopicModel>();
            result.Succeed = true;
            SyncTopicToES(model);
            return Json(result);
        }

        /// <summary>删除私有图谱分类</summary>
        public ActionResult DeletePrivateCatalog(int id)
        {
            var result = new WebActionResult();
            var service = GetService<KG_PrivateCatalogService>();
            service.Delete(id, this.PKSUser.Identity.Name);
            result.Succeed = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>删除私有主题</summary>
        public ActionResult DeletePrivateTopic(int id)
        {
            var result = new WebActionResult();
            var service = GetService<KG_TopicService>();
            service.Delete(id);
            result.Succeed = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private bool SyncTopicToES(PKS_KG_Topic topic)
        {
            var indexMetadtaService = GetService<IndexMetadataService>();
            var metadata = GetMetadata(topic);
            Dictionary<Metadata, AppDataSaveRequest> metadatas = new Dictionary<Metadata, AppDataSaveRequest>();
            metadatas.Add(metadata, null);
            return indexMetadtaService.SaveAppIndex(metadatas);
        }

        private Metadata GetMetadata(PKS_KG_Topic topic)
        {
            Metadata metadata = new Metadata();
            var resourcekey = "勘探知识库\\知识图谱\\" + topic.Id;
            metadata.IIId = resourcekey.ToMD5();
            metadata.IndexedDate = DateTime.Now;
            metadata.Thumbnail = null;
            metadata.Fulltext = topic.Contents;
            metadata.PageId = "19"; //展示页面模板注册
            metadata.DataId = topic.Id.ToString();
            metadata["dsn"] = "勘探社区";
            metadata.ShowType = IndexShowType.Mixing.ToString();
            metadata["title"] = topic.Title;
            metadata["subject"] = null;
            metadata["abstract"] = null;
            metadata["catalogue"] = null;
            metadata["author"] = PKSUser.Identity.Name;
            metadata["submitter"] = topic.CreatedBy;
            metadata["auditor"] = null;
            metadata["createddate"] = DateTime.Now;
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
            metadata["pc"] = "勘探社区";
            metadata["pt"] = "知识图谱";
            metadata["bd"] = "勘探";
            metadata["bt"] = null;
            metadata["bp"] = null;
            metadata["ba"] = null;
            metadata["bf"] = null;
            metadata["system"] = "勘探知识库";
            metadata["resourcetype"] = "勘探知识库\\知识图谱";
            metadata["resourcekey"] = resourcekey;
            return metadata;
        }
    }
}