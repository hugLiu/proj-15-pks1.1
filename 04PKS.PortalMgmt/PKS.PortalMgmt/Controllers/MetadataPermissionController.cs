using Jurassic.WebFrame;
using Newtonsoft.Json;
using PKS.Data;
using PKS.DbModels;
using PKS.DbModels.PortalMgmt;
using PKS.DbServices.SysFrame;
using PKS.Models;
using PKS.PortalMgmt.Models.DTO;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class MetadataPermissionController : PKSBaseController
    {
        private RoleMetadataPermissionService metaPermissionService { get; }

        public MetadataPermissionController(RoleMetadataPermissionService service)
        {
            metaPermissionService = service;
        }

        // GET: MetadataPermission
        public ActionResult Index()
        {
            metaPermissionService.InitMetas();
            return View();
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoles()
        {
            var roles = GetService<IRepository<WEBPAGES_ROLES>>()
                        .GetQuery()
                        .OrderBy(r => r.ROLEID)
                        .ToList();
            return Json(roles, JsonRequestBehavior.AllowGet);
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


        /// <summary>
        /// 获取角色的标签
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public JsonResult GetPermissionRoleMetas(int roleId)
        {
            var roleMetas = metaPermissionService.GetPermissionRoleMetas(roleId);

            var allItem = metaPermissionService.GetMetas();

            var permissons = (from a in allItem
                              join p in roleMetas on a.Id equals p.MetadataId into g
                              from d in g.DefaultIfEmpty()
                              where d != null
                              orderby a.Id
                              select new RoleMetaPermissionDTO
                              {
                                  Id = a.Id,
                                  RoleId = d.RoleId,
                                  MetaId = d.Id,//此处为MetaId
                                  Title = a.Title,
                                  Name = a.Name,
                                  Description = a.Description,
                                  IsChecked = d.IsValid,
                              }).ToList();
            return Json(permissons, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据metaID获取标签项
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns></returns>
        public JsonResult GetMetaItems(int metaId)
        {
            var metaItems = metaPermissionService.GetMetaItems(metaId);

            var metaItemsDTO = metaItems.Select(p=>new MetaItemDTO
            {
                Id=p.Id,
                MetadataItemName=p.Text
            });

            return Json(metaItemsDTO, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取角色的标签的黑名单
        /// </summary>
        /// <param name="roleId" ></ param >
        /// < param name="metaId"></param>
        /// <returns></returns>
        public JsonResult GetBlacklist(int roleId, int metaId)
        {
            var items = metaPermissionService.GetPermissionRoleMetaItems(roleId, metaId);
            if (items == null) return null;
            var blackList = items.Where(p => p.IsAble == false).ToViewModel<List<PKS_ROLE_METADATAITEMPERMISSION>>();

            return Json(blackList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色的标签的白名单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="metaId"></param>
        /// <returns></returns>
        public JsonResult GetWhitelist(int roleId, int metaId)
        {
            var items = metaPermissionService.GetPermissionRoleMetaItems(roleId, metaId);
            if (items == null) return null;
            var whiteList = items.Where(p => p.IsAble == true).ToViewModel<List<PKS_ROLE_METADATAITEMPERMISSION>>();

            return Json(whiteList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="metaId"></param>
        /// <param name="whiteList"></param>
        /// <param name="blackList"></param>
        public void SavePermission(int roleId, List<RoleMetaPermissionDTO> roleMetas, List<RoleMetaItemPermissionDTO> roleMetaItems)
        {
            if (roleMetas == null)
            {
                GetService<IRepository<PKS_ROLE_METADATAPERMISSION>>().DeleteList(p => p.RoleId == roleId);
                return;
            }

            var metaList = roleMetas.Select(m => new PKS_ROLE_METADATAPERMISSION
            {
                RoleId = roleId,
                MetadataId = m.Id,
                IsValid = m.IsChecked
            }).ToList();

            var re = metaPermissionService.SaveRoleMetas(roleId, metaList);

            if (roleMetaItems == null) return;
            foreach (var rm in roleMetaItems)
            {
                rm.RoleMetaId = re.Where(r=>r.MetadataId==rm.MetaId).ToList()[0].Id;
                metaPermissionService.SaveRoleMetaItems(rm.RoleMetaId, rm.WhiteList, rm.BlackList);
            }
        }
    }
}