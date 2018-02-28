using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbModels.PortalMgmt;
using PKS.PortalMgmt.Models.DTO;
using PKS.Web;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PKS.DbServices.SysFrame;
using Jurassic.AppCenter;

namespace PKS.PortalMgmt.Controllers
{
    public class RoleMapManageController : PKSBaseController
    {

        private RoleMapService _roleMapService;
        public RoleMapManageController(RoleMapService roleMapService)
        {
            _roleMapService = roleMapService;
        }
        public ViewResult Overview()
        {
            return View();
        }

        public JsonResult Roles()
        {
            var db = GetService<IRepository<WEBPAGES_ROLES>>();
            var model = db.GetAll().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Maps()
        {
            var db = GetService<IRepository<PKS_ROLE_MAP>>();
            var model = db.GetAll().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit(PKS_ROLE_MAP roleMap)
        {
            return View();
        }

        public void AddOrUpdateRoleMap(PKS_ROLE_MAP roleMap)
        {
            var db = GetService<IRepository<PKS_ROLE_MAP>>();
            var model = db.Find(rm => rm.Id == roleMap.Id);
            var userName = User.Identity.Name;
            var now = DateTime.Now;
            if (model == null)
            {
                roleMap.CreatedDate = now;
                roleMap.LastUpdatedDate = now;
                roleMap.CreatedBy = userName;
                roleMap.LastUpdatedBy = userName;
                db.Add(roleMap);
            }
            else
            {
                model.OrgName = roleMap.OrgName;
                model.RoleId = roleMap.RoleId;
                model.LastUpdatedBy = userName;
                model.LastUpdatedDate = now;
            }
            db.Submit();
        }

        public void SaveRoleMap(List<PKS_ROLE_MAP> roleMaps)
        {
            var userName = User.Identity.Name;
            var now = DateTime.Now;
            var db = GetService<IRepository<PKS_ROLE_MAP>>();
            var oldRoleMaps = db.GetAll().ToList();
            var roles = GetService<IRepository<WEBPAGES_ROLES>>().GetAll().ToList();
            roleMaps.ForEach(rm =>
            {
                var r = oldRoleMaps.FirstOrDefault(e => e.Id == rm.Id);
                if (rm.Role != null)
                {
                    r.RoleId = roles.FirstOrDefault(role => role.ROLENAME == rm.Role.ROLENAME).ROLEID;
                    r.LastUpdatedBy = userName;
                    r.LastUpdatedDate = now;
                    db.Update(r);
                }
            });
            db.Submit();
        }


        public void DeleteRoleMap(int roleMapId)
        {
            var db = GetService<IRepository<PKS_ROLE_MAP>>();
            var model = db.Find(rm => rm.Id == roleMapId);
            if (model != null)
            {
                db.Delete(model);
            }
        }

        /// <summary>
        /// 手动同步域部门结构
        /// </summary>
        public void SyncAD(string ous)
        {
            var ouNames = ous.Split('/').ToList();
            _roleMapService.Update(ouNames);
        }


        /// <summary>域用户自动注册</summary>
        [AllowAnonymous]
        [JAuth(JAuthType.Ignore)]
        public ActionResult AutoRegisterDomainUser(string token)
        {
            var result = new WebActionResult();
            if (token.IsNullOrEmpty())
            {
                result.ErrorMessage = "token无效!";
            }
            else
            {
                var userName = GetService<PKS.Core.ICacheProvider>().ExternalCacher.GetRandom<string>(token);
                if (userName.IsNullOrEmpty())
                {
                    result.ErrorMessage = "userName无效!";
                }
                else
                {
                    var adUserGroupId = GetService<IADIdentityService>().GetUserGroupId(userName);
                    var repo = GetService<IRepository<PKS_ROLE_MAP>>();
                    var model = repo.Find(e => e.OriginalId == adUserGroupId);
                    if (model == null) model = repo.Find(e => string.IsNullOrEmpty(e.OriginalPId));
                    var userManager = AppManager.Instance.UserManager;
                    var newUser = new Jurassic.WebFrame.Models.UserConfig();
                    newUser.Name = userName;
                    newUser.RoleIds = new string[] { model.RoleId.ToString() };
                    userManager.Add(newUser);
                    result.Succeed = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}