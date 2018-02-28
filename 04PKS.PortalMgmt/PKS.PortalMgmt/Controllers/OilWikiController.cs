using PKS.DbModels.OilWiki;
using PKS.DbServices.OilWiki;
using PKS.DbServices.OilWiki.Model;
using PKS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using PKS.DbServices.IndexApp;
using PKS.Models;
using PKS.WebAPI.Models;

namespace PKS.PortalMgmt.Controllers
{
    public class OilWikiController : PKSBaseController
    {

        private OilWikiService _oilWikiService;
        public OilWikiController(OilWikiService oilWikiService)
        {
            _oilWikiService = oilWikiService;
        }

        // GET: OilWiki
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetGridData()
        {
            int pageIndex = Request.Form["PageIndex"].ToInt32() + 1;
            int pageSize = Request.Form["PageSize"].ToInt32();
            string filter = Request.Form["Filter"];
            string catalogIdStr = Request.Form["CatalogId"];
            int recordCount = 0;

            Expression<Func<PKS_OILWIKI_ENTRY, bool>> whereExpr = null;

            if (!string.IsNullOrEmpty(catalogIdStr))
            {
                var catalogId = catalogIdStr.ToInt32();
                if (string.IsNullOrEmpty(filter))
                {
                    whereExpr = e => e.CATALOGID == catalogId;
                }
                else
                {
                    whereExpr = e => e.CATALOGID == catalogId && e.NAME.Contains(filter);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(filter))
                {
                    whereExpr = e => 1 == 1;
                }
                else
                {
                    whereExpr = e => 1 == 1 && e.NAME.Contains(filter);
                }
            }

            var entrys = _oilWikiService.FindEntryListByPage(whereExpr, pageSize, pageIndex, out recordCount);
            Hashtable result = new Hashtable();
            result["data"] = entrys;
            result["total"] = recordCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EntryNameExists(int id, string name)
        {
            bool exists = _oilWikiService.EntryNameExists(id, name);
            return Json(exists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteEntry(int entryId)
        {
            RemoveEsRecord(entryId);
            _oilWikiService.DeleteEntry(entryId);
        }

        [HttpPost]
        public void UpdateEntry()
        {
            string entryModel = Request.Form["EntryModel"];
            string aliasListStr = Request.Form["aliasList"];
            string relatedListStr = Request.Form["relatedList"];
            
            var entity = entryModel.JsonTo<EntryDTO>();
            entity.LastUpdatedBy = CurrentUser.Id;
            entity.LastUpdatedDate = DateTime.Now;

            entity.AliasEntry = aliasListStr.JsonTo<List<string>>();
            entity.RelatedEntry = relatedListStr.JsonTo<List<int>>();

            _oilWikiService.UpdateEntry(entity);
            SyncEntryToES(entity.Id);
        }

        [HttpPost]
        public void AddEntry()
        {
            string entryModel = Request.Form["EntryModel"];
            string aliasListStr = Request.Form["aliasList"];
            string relatedListStr = Request.Form["relatedList"];

            var entity = entryModel.JsonTo<EntryDTO>();
            entity.CreatedBy = CurrentUser.Id;
            entity.CreatedDate = DateTime.Now;
            entity.LastUpdatedBy = CurrentUser.Id;
            entity.LastUpdatedDate = DateTime.Now;

            entity.AliasEntry = aliasListStr.JsonTo<List<string>>();
            entity.RelatedEntry = relatedListStr.JsonTo<List<int>>();

            var entryId=_oilWikiService.AddEntry(entity);
            SyncEntryToES(entryId);
        }



        public ActionResult Category()
        {
            return View();
        }

        public JsonResult GetCatalog(int? levelNum = null)
        {
            var cats = levelNum == null
                        ? _oilWikiService.GetCatalog()
                        : _oilWikiService.GetCatalog().Where(c => c.LEVELNUMBER == levelNum);
            var viewModel = cats.OrderBy(c => c.ORDERNUMBER)
                .Select(c =>
                  new
                  {
                      Id = c.Id,
                      NAME = c.NAME,
                      PARENTID = c.PARENTID,
                      DESCRIPTION = c.DESCRIPTION,
                      CODE = c.CODE,
                      IMAGEURL = c.IMAGEURL,
                      LEVELNUMBER = c.LEVELNUMBER,
                      ORDERNUMBER = c.ORDERNUMBER,
                      KMD = c.KMD,
                      DOMAINID = c.DOMAINID,
                      CREATEDBY = c.CREATEDBY,
                      CREATEDDATE = c.CREATEDDATE,
                      LASTUPDATEDBY = c.LASTUPDATEDBY,
                      LASTUPDATEDDATE = c.LASTUPDATEDDATE
                  }
            );
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddCatalog(string model)
        {
            var catelogModel = model.JsonTo<PKS_OILWIKI_CATALOG>();
            catelogModel.CREATEDBY = CurrentUser.Id;
            catelogModel.CREATEDDATE = DateTime.Now;
            catelogModel.LASTUPDATEDBY = CurrentUser.Id;
            catelogModel.LASTUPDATEDDATE = DateTime.Now;
            catelogModel.NAME = catelogModel.NAME + "::" + Guid.NewGuid().ToString();
            int id = _oilWikiService.AddCatalog(catelogModel);
            return Json(new { state = true, id = id }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param model="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateCatalog(string model)
        {
            var catelogModel = model.JsonTo<PKS_OILWIKI_CATALOG>();
            catelogModel.LASTUPDATEDBY = CurrentUser.Id;
            catelogModel.LASTUPDATEDDATE = DateTime.Now;
            _oilWikiService.UpdateCatalog(catelogModel);
            return Json(new { state = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除分类
        /// </summary>
        /// <param models="models"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCatalog(string models)
        {
            var modelList = models.JsonTo<List<PKS_OILWIKI_CATALOG>>();
            _oilWikiService.DeleteCatalog(modelList);

            return Json(new { state = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断分类名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult CatalogNameExists(int id, string name)
        {
            bool exists = _oilWikiService.CatalogNameExists(id, name);
            return Json(exists, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 根据词条id获取同义词
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public JsonResult GetAliasEntrys(int entryId)
        {
            return Json(_oilWikiService.GetAliasEntrys(entryId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据词条id获取相关词条
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public JsonResult GetRelateEntrys(int entryId)
        {
            return Json(_oilWikiService.GetRelateEntrys(entryId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool RebuildIndex()
        {
            var entrys = _oilWikiService.FindAllEntryList();
            Dictionary<Metadata, AppDataSaveRequest> metadatas = new Dictionary<Metadata, AppDataSaveRequest>();
            foreach (var entry in entrys)
            {
                var metadata = GetMetadata(entry);
                metadatas.Add(metadata,null);
            }
            var indexMetadtaService = GetService<IndexMetadataService>();
            return indexMetadtaService.SaveAppIndex(metadatas);
        }

        private Metadata GetMetadata(EntryDetails entry)
        {
            var category = entry.ParentCatalogName;
            Metadata metadata = new Metadata();
            var resourcekey = "勘探知识库\\石油百科\\" + category + "\\" + entry.Id;
            metadata.IIId = resourcekey.ToMD5();
            metadata.IndexedDate = DateTime.Now;
            metadata.Thumbnail = null;
            metadata.Fulltext = entry.Contents;
            metadata.PageId = "20";//展示页面模板注册：石油百科词条
            metadata.DataId = entry.Id.ToString();
            metadata["dsn"] = "石油百科";
            metadata.ShowType = IndexShowType.Mixing.ToString();
            metadata["title"] = entry.Name;
            metadata["subject"] = null;
            metadata["abstract"] = null;
            metadata["catalogue"] = null;
            metadata["author"] = entry.Author;
            metadata["submitter"] = null;
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
            metadata["pc"] = "石油百科";
            metadata["pt"] = "石油百科词条";
            metadata["bd"] = "勘探";
            metadata["bt"] = null;
            metadata["bp"] = null;
            metadata["ba"] = null;
            metadata["bf"] = null;
            metadata["system"] = "勘探知识库";
            metadata["resourcetype"] = "勘探知识库\\石油百科词条";
            metadata["resourcekey"] = resourcekey;
            return metadata;
        }

        private bool SyncEntryToES(int dataid)
        {
            var indexMetadtaService = GetService<IndexMetadataService>();
            var entry=_oilWikiService.GetEntryById(dataid);
            var metadata=GetMetadata(entry);
            Dictionary<Metadata, AppDataSaveRequest> metadatas = new Dictionary<Metadata, AppDataSaveRequest>();
            metadatas.Add(metadata,null);
            return indexMetadtaService.SaveAppIndex(metadatas);
        }

        private bool RemoveEsRecord(int dataid)
        {
            var entry = _oilWikiService.GetEntryById(dataid);
            var resourcekey = "勘探知识库\\石油百科\\" + entry.ParentCatalogName + "\\" + dataid;
            var iiid = resourcekey.ToMD5();
            var indexMetadtaService = GetService<IndexMetadataService>();
            return indexMetadtaService.DeleteEsRecord(iiid);
        }
    }
}


