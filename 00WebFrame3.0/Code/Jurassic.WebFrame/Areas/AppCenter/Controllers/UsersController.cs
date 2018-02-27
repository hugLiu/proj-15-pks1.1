using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Resources;
using Jurassic.AppCenter.Models;
using Jurassic.AppCenter;
using Jurassic.CommonModels;
using Jurassic.AppCenter.Caches;
using Jurassic.CommonModels.Articles;

namespace Jurassic.WebFrame.Controllers
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用户管理，对用户基本信息和角色及对应功能权限的维护
    /// </summary>
    [JAuth(Name = "UsersManager", Ord = 3)]
    public class UsersController : BaseController
    {
        DataManager<AppUser> mUserMgr = AppManager.Instance.UserManager;

        /// <summary>
        /// 首页，返回用户列表
        /// </summary>
        /// <param name="page">页号</param>
        /// <returns>分页用户列表</returns>
        public ActionResult Index(int page = 0)
        {
            return View();
        }

        #region 设置用户与模块功能的直接关系
        /// <summary>
        /// 初始显示用户与功能模块页面并显示用户信息 by_zjf
        /// </summary>
        /// <returns></returns>
        public ActionResult UserToAction(string id)
        {
            var userProfile = mUserMgr.GetById(id) ?? (AppUser)mUserMgr.Create();
            return View(userProfile);
        }

        /// <summary>
        /// 恢复默认角色授权 by_zjf
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RestoreAction(AppUser user)
        {
            //获取所设置用户对象
            AppUser appUserModel = AppManager.Instance.UserManager.GetByName(user.Name);
            appUserModel.IsDefaultRole = true;
            appUserModel.FunctionIds = new List<string>();
            AppManager.Instance.UserManager.Change(appUserModel);
            AppManager.Instance.UserManager.Save();
            return JsonTips("success", FStr.DefaultRoleReseted);
        }

        /// <summary>
        /// 保存设置的功能模块 by_zjf
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAction(AppUser user)
        {
            //获取所设置用户对象
            AppUser appUserModel = AppManager.Instance.UserManager.GetByName(user.Name);
            appUserModel.IsDefaultRole = false;

            #region 获取所设置模块功能,并添加到用户对象
            string funcIds = Request.Form["FuncIds"];
            string[] strList = funcIds.Split(',');
            appUserModel.FunctionIds.Clear();
            foreach (string item in strList)
            {
                if (!string.IsNullOrEmpty(item))
                    appUserModel.FunctionIds.Add(item);
            }
            #endregion
            AppManager.Instance.UserManager.Change(appUserModel);
            AppManager.Instance.UserManager.Save();
            ////保存数据到缓存变量
            //IList<AppUser> appUserList = mUserMgr.ChangeCached(appUserModel);

            //#region 保存数据到缓存文件(APP_DATA)
            //CachedList<AppUser> cachedObject = new CachedList<AppUser>();
            //cachedObject.Clear();
            //foreach (AppUser item in appUserList)
            //{
            //    cachedObject.Insert(0, item);
            //}
            //cachedObject.Save();
            //#endregion
            return JsonTips("success", JStr.SuccessSaved);
        }

        /// <summary>
        /// 获取所选择用户设置的模块功能,并设置树节点为选中状态 by_zjf
        /// </summary>
        /// <param name="uName">用户</param>
        /// <returns>功能模块Json数据</returns>
        public JsonResult InitTreeCheckStat(string uName)
        {
            //获取所设置用户对象
            AppUser appUserModel = AppManager.Instance.UserManager.GetByName(uName);

            ICollection<string> role = AppManager.Instance.GetRightIds(uName).ToList();
            return JsonTips(role, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 用户基本信息
        /// <summary>
        /// 根据用户角色ID获取相应所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public JsonResult GetRoleUsers(string roleId)
        {
            var users = mUserMgr.GetAll().Where(u => u.IsInRoleId(roleId));
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有用户的分页json数据
        /// </summary>
        /// <param name="page">查询分页信息，包括当前页、页大小、关键词等</param>
        /// <returns></returns>
        public JsonResult GetAll(PageModel page)
        {
            //mini-ui的分页是从0开始的
            page.PageIndex++;

            //因userMgr.GetAll()不是IQueryable对象，所以不能直接用在分页API中
            //需要一个转换
            var users = mUserMgr.GetAll().AsQueryable();

            if (!page.Key.IsEmpty())
            {
                page.Key = page.Key.ToLower();
                users = users.Where(u => u.Name.ToLower().Contains(page.Key)
                    || u.Id.Equals(page.Key) || (u.Email != null && u.Email.Contains(page.Key))
                    || (u.PhoneNumber != null && u.PhoneNumber.Contains(page.Key))
                    || u.RoleNames.ToLower().Contains(page.Key));
            }

            var pager = new Pager<AppUser>(users.OrderByDescending(u => CommOp.ToInt(u.Id)), page.PageIndex, page.PageSize);

            //多重排序：
            //ViewBag.Pager = Pager.GetPagedList(ref users, us =>
            //    us.OrderByDescending(u => u.IsOnline)
            //    .ThenByDescending(u => u.Id), page);

            // var vre = this.ViewEngineCollection[1].FindView(this.ControllerContext., "Index_Spc", "_AdminPartial", false); //MVC3不支持

            return Json(new
            {
                data = pager,
                total = pager.RecordCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户编辑页面</returns>
        public ActionResult Edit(string id)
        {
            var user = mUserMgr.GetById(id) ?? (AppUser)mUserMgr.Create();
            Session["EditingUser"] = user;
            //ViewBag.ShowSearchBox = false;
            ViewBag.ShowBreadCrumb = false;
            //var vre = this.ViewEngineCollection[1].FindView(this.ControllerContext, "Edit_Spc", "_AdminPartial", false);//MVC3不支持
            return View(user);
        }

        /// <summary>
        /// 检查用户名是否合法，用于前台validate远程验证
        /// </summary>
        /// <param name="name">要检查的用户名</param>
        /// <returns>可用/不可用（有重复）</returns>
        [OutputCache(Duration = 0)]
        [JAuth(JAuthType.Ignore)]
        [Authorize]
        public JsonResult CheckUserName(string name)
        {
            var user = Session["EditingUser"] as AppUser;
            var tUser = mUserMgr.GetByName(name);
            return Json(tUser == null || user == null || tUser.Id == user.Id, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交编辑
        /// </summary>
        /// <param name="user">用户AppUser实体</param>
        /// <param name="rolesIds">用户对应的角色ID串</param>
        /// <returns>如果是新增用户，则继续新增页面，如果是修改用户，则返回用户列表</returns>
        [HttpPost]
        public ActionResult Edit(AppUser user, string[] rolesIds)
        {
            if (!ModelState.IsValid) return ViewTips(user);

            var tUser = mUserMgr.GetById(user.Id) ?? mUserMgr.Create();

            tUser.Id = user.Id;
            tUser.Name = user.Name;

            //添加其他程序AppUser子类中的特殊属性
            Request.Form.AssignFormValues(tUser);

            tUser.RoleIds = rolesIds;

            if (!user.Id.IsEmpty())
            {
                mUserMgr.Change(tUser);
                return JsonTips("success", JStr.SuccessUpdated, FStr.UpdateUserInfoSucceed0, user, user.Name);
            }
            else //如果是新增用户信息, 则保存成功后可继续新增
            {
                mUserMgr.Add(tUser);
                return JsonTips("success", JStr.SuccessAdded, FStr.UserSuccessAdded0, new { Url = Url.Action("Edit") }, tUser.Name);
            }
        }

        /// <summary>
        /// 删除用户，这个如果用户和其他的记录关联会删不掉
        /// tips:可以在UserProvider实现的数据操作中采用逻辑删除以避免这种情况
        /// </summary>
        /// <param name="ids">逗号分隔的要删除的用户ID列表串</param>
        /// <returns>回到用户的主界面</returns>
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            int[] idArr = CommOp.ToIntArray(ids, ',');

            int i = 0;
            foreach (int item in idArr)
            {
                bool flag = AppManager.Instance.UserManager.Remove(item.ToStr());
                if (flag)
                    i++;
            }
            //未进行执行调整成foreach循环
            //idArr.Each(id => AppManager.Instance.UserManager.Remove(id.ToStr()));
            return JsonTips("success", null, JStr.SuccessDeleted0, (object)null, i);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids">逗号分隔的要重置密码的用户ID列表串</param>
        /// <returns>回到用户的主界面</returns>
        [HttpPost]
        public ActionResult ResetPass(string ids)
        {
            string[] idArr = ids.Split(',');
            //idArr.Each(id => AppManager.Instance.UserManager.Remove(id.ToStr()));

            string msg = "";
            int i = 0;
            foreach (var userSub in idArr)
            {
                if (string.IsNullOrEmpty(userSub))
                    continue;
                PasswordResetModel model = new PasswordResetModel() { UserName = userSub, Password = "123456", IsResetPass = 1 };
                LoginState resState = AppManager.Instance.StateProvider.ResetPassword(model);
                if (LoginState.OK != resState)
                {
                    msg = msg + userSub + " ";
                    i++;
                }
            }

            if (i == 0)
            {
                return JsonTipsLang("success", null, "F_Reset_Password_Success");
            }
            return JsonTips("error", null, FStr.ResetPasswordFailed0, (object)null, msg);
        }

        #endregion
    }
}
