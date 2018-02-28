using PKS.DbServices.SysFrame;
using PKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PKS.Utils;
using PKS.PortalMgmt.Models.DTO;

namespace PKS.PortalMgmt.Controllers
{
    public class MetadataManagerController : PKSBaseController
    {

        private RoleMetadataPermissionService metaPermissionService { get; }

        public MetadataManagerController(RoleMetadataPermissionService service)
        {
            metaPermissionService = service;
        }
        // GET: MetadataManager
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取元数据标签
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMetas()
        {
            var metas = metaPermissionService.GetMetas();
            var viewModel = metas.ToViewModel<List<MetadataDefinition>>();
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        public void Save(string data)
        {
            var meats = data.JsonTo<List<MetadataDefinition>>();
            metaPermissionService.SaveMetas(meats);
        }

        public void Delete(int[] ids)
        {
            metaPermissionService.DeleteMetas(ids);
        }

        public JsonResult GetMetaItems(int metaId)
        {
            var items = metaPermissionService.GetMetaItems(metaId);
            var metaItemsDTO = items.Select(p => new MetaItemDTO
            {
                Id = p.Id,
                MetadataItemName = p.Text
            });
            return Json(metaItemsDTO, JsonRequestBehavior.AllowGet);
        }


        public void SaveMetaItems(int metaId, string values)
        {
            var items = values.Split(new char[] { '\n' });
            metaPermissionService.SaveMetaItems(metaId, items.ToList<string>());
        }
    }
}