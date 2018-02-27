using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebFrame.Controllers
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 角色管理，对角色基本信息和角色对应功能权限的维护
    /// 除了首页Index以外其他的都是输出Json的ReturnValueWithTips
    /// </summary>
    [JAuth(Name = "RolesManager", Ord = 2)]
    public class RolesController : BaseController
    {
        //
        // GET: /App_Center/Roles/
        DataManager<AppRole> mRoleMgr = AppManager.Instance.RoleManager;

        /// <summary>
        /// 首页，展示角色列表
        /// </summary>
        /// <param name="id">要定痊的角色ID，将在列表中自动选中该角色</param>
        /// <returns>展示所有角色的视图</returns>
        public ActionResult Index(string id)
        {
            ViewBag.CurrentId = id;
            return View();
        }

        public JsonResult GetAll(string key)
        {
            if (key.IsEmpty())
            {
                return Json(mRoleMgr.GetAll(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                key = key.ToLower();
                return Json(mRoleMgr.GetAll().Where(r=>r.Name.ToLower().Contains(key)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 返回编辑或新增的对象
        /// </summary>
        /// <param name="id">对象ID，为空时表示新增对象</param>
        /// <returns>要修改或新增的对象Json数据</returns>
        public JsonResult Edit(string id)
        {
            var role = mRoleMgr.GetById(id) ?? new AppRole();
            //存一个临时值到Sessioin用于验证名称是否重复
            Session["EditingRole"] = role;
            return JsonTips(role, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查角色名称是否合法
        /// </summary>
        /// <param name="name">要检查的角色名称</param>
        /// <returns>是否</returns>
        [OutputCache(Duration = 0)]
        [JAuth(JAuthType.Ignore)]
        [Authorize]
        public JsonResult CheckRoleName(string name)
        {
            var role = Session["EditingRole"] as AppRole;
            var tRole = mRoleMgr.GetByName(name);
            return Json(tRole == null || role == null || tRole.Id == role.Id, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交并返回编辑或新增过的对象
        /// </summary>
        /// <param name="role">提交的AppRole对象</param>
        /// <param name="funcIds">角色能操作的功能ID列表</param>
        /// <returns>返回角色管理首页</returns>
        [HttpPost]
        public ActionResult Edit(AppRole role, string funcIds)
        {
            if (ModelState.IsValid)
            {

                if (!funcIds.IsEmpty())
                {
                    var funcIdArr = funcIds.Split(',');
                    var funcs = AppManager.Instance.FunctionManager.GetAll().Where(func => funcIdArr.Contains(func.Id));
                    foreach (var func in funcs)
                    {
                        //将勾选的功能模块和它关联的功能模块ID一起加到FunctionIds中
                        role.FunctionIds.Add(func.Id);
                        role.FunctionIds.AddRange(func.RelatedIds ?? new List<string>());
                    }
                    role.FunctionIds = role.FunctionIds.Distinct().ToList();
                }
                if (role.Id.IsEmpty())
                {
                    mRoleMgr.Add(role);
                    return JsonTips("success", JStr.SuccessAdded, JStr.SuccessAdded0, role, role.Name);
                }
                else
                {
                    mRoleMgr.Change(role);
                    return JsonTipsLang("success", JStr.SuccessUpdated, JStr.SuccessUpdated0, role, role.Name);
                }
            }
            return JsonTips();
        }

        /// <summary>
        /// 删除指定ID的角色
        /// </summary>
        /// <param name="id">要删除的角色ID</param> 
        /// <returns>成功或失败的提示信息</returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            if (mRoleMgr.Remove(id))
            {
                return JsonTipsLang("success", null,JStr.SuccessDeleted);
            }
            else
            {
                return JsonTipsLang("error", JStr.DeleteFailed);
            }
        }
    }
}
