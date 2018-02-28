using PKS.DbModels;
using PKS.DbServices;
using PKS.DbServices.Models;
using System;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    /// <summary>公共图谱控制器</summary>
    public class KGraphPublicController : PKSBaseController
    {
        /// <summary>构造函数</summary>
        public KGraphPublicController(KG_PublicCatalogService service)
        {
            this.PublicCatalogService = service;
        }
        /// <summary>公共图谱服务</summary>
        private KG_PublicCatalogService PublicCatalogService { get; set; }
        /// <summary>公共图谱页面</summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>获得全部树节点</summary>
        public ActionResult GetAll()
        {
            var result = this.PublicCatalogService.GetAll();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>保存</summary>
        [HttpPost]
        public ActionResult Save(KG_CatalogNode model)
        {
            if (!this.ModelState.IsValid) return JsonTips();
            if (model.Id <= 0)
            {
                model.CreatedBy = this.CurrentUser.Name;
            }
            model.LastUpdatedBy = this.CurrentUser.Name;
            var result = this.PublicCatalogService.Save(model);
            return Json(result);
        }
        /// <summary>删除</summary>
        public ActionResult Delete(KG_CatalogNode model)
        {
            model.LastUpdatedBy = this.CurrentUser.Name;
            this.PublicCatalogService.Delete(model);
            return Json(model);
        }
        /// <summary>排序</summary>
        public ActionResult Sort(KG_CatalogNode[] models)
        {
            var userName = this.CurrentUser.Name;
            foreach (var model in models)
            {
                if (model == null) continue;
                model.LastUpdatedBy = userName;
            }
            this.PublicCatalogService.Sort(models);
            return Json(models);
        }
    }
}