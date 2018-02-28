using Jurassic.CommonModels;
using PKS.Data;
using PKS.DbModels;
using PKS.DBModels;
using PKS.PortalMgmt.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class RolePermissionController : PKSBaseController
    {
        // GET: RolePermission
        public ActionResult Index()
        {
            ViewBag.ShowToolBar = false;
            return View();
        }

        /// <summary>
        /// 加载角色列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoles()
        {
            var roles = GetService<IRepository<WEBPAGES_ROLES>>()
                        .GetQuery()
                        .OrderBy(r=>r.ROLEID)
                        .ToList();
            return Json(new { data=roles,pageSize=roles.Count,total=roles.Count}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPermissions(WEBPAGES_ROLES role)
        {
            var rps = GetService<IRepository<PKS_ROLE_PERMISSION>>()
                      .GetQuery()
                      .Where(r => r.RoleId == role.ROLEID);
            var ps = GetService<IRepository<PKS_PERMISSION>>()
                     .GetQuery();
            var data = (from p in ps
                        join r in rps on p.Id equals r.PermissionId into g
                        from d in g.DefaultIfEmpty() orderby p.LevelNumber orderby p.OrderNumber
                        select new SystemPermission
                        {
                            Id = p.Id,
                            ParentId = p.ParentId,
                            Title = p.Title,
                            SystemName = p.SubSystem.Name,
                            Description = p.Description,
                            Url = p.Url,
                            IsDefault = (d == null) ? 0 : d.IsDefault,
                            IsChecked = (d != null)&&!ps.Any(l=>l.ParentId == p.Id),
                            PermissionType = p.PermissionType.Name
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public void SaveRolePermission(WEBPAGES_ROLES role, List<SystemPermission> permissions)
        {
            var db = GetService<IRepository<PKS_ROLE_PERMISSION>>();
            var query = db.GetQuery();
            db.DeleteList(r => r.RoleId == role.ROLEID);
            PKS_ROLE_PERMISSION rp;
            permissions.ForEach(p =>
            {
                rp = p.ToRolePermission(role);
                db.Add(rp, false);
            });
            db.Submit();
        }
    }
}