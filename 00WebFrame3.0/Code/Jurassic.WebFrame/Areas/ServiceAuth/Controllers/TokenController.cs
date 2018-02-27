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
using Jurassic.Com.DB;
using Jurassic.CommonModels.EFProvider;
using Jurassic.CommonModels.ServerAuth;

namespace Jurassic.WebFrame.Areas.ServiceAuth.Controllers
{
    public class TokenController : BaseController
    {
        public ServerAuthManager mAuthTokenManager = ApiManager.mServerAuthManager;

        #region 用户组授权
        /// <summary>
        /// token授权页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// token首页数据列表
        /// </summary>
        /// <param name="pageIndex">分页序号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key">查询关键字</param>
        /// <returns></returns>
        public JsonResult GetMainPageData(int pageIndex, int pageSize, string key)
        {
            //mini-ui的分页是从0开始的
            pageIndex++;
            var authTokens = mAuthTokenManager.GetData();
            if (!key.IsEmpty())
            {
                key = key.ToLower();
                authTokens = authTokens.Where(u => u.ClientName.ToLower().Contains(key.ToLower()) || u.ClientId.ToLower().Contains(key.ToLower()));
            }
            //设置分页查询获取数据(此处才开始数据读取)
            var pager = new Pager<AuthToken>(authTokens.OrderBy(u => u.ToKeyId), pageIndex, pageSize);

            return Json(new
            {
                data = pager,
                total = pager.RecordCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 打开表单显示数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public ActionResult FormInfo(string id)
        {
            List<AuthToken> authTokenList = mAuthTokenManager.GetDataById(id);

            if (authTokenList.Any())
            {
                return View(authTokenList[0]);
            }
            //新增状态设置初始值
            AuthToken authTokenModel = new AuthToken();
            authTokenModel.IsvalId = 1;
            authTokenModel.AccreditBy = base.CurrentUser.Name;
            authTokenModel.TokeyCode = Guid.NewGuid().ToString("N");
            return View(authTokenModel);
        }

        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <returns>保存成功回首页</returns>
        [HttpPost]
        public ActionResult SaveForm()
        {
            bool flag = true;
            var formData = Request.Form["formData"];
            AuthToken authTokenModel = JsonHelper.FormJson(formData, typeof(AuthToken)) as AuthToken;
            if (authTokenModel == null)
            {
                return JsonTips("error", JStr.InvalidRequestData);
            }

            //验证授权用户组名称或授权key是否重复
            if (!mAuthTokenManager.VerifyClientRepeat(authTokenModel))
            {
                return JsonTips("warning", FStr.DuplicatedAccountOrKey);// "你设置的客户组账号或授权Key重复!");
            }
            //保存或修改数据
            if (string.IsNullOrEmpty(authTokenModel.ToKeyId))
            {
                //新增状态设置主键以及创建日期(授权日期)
                authTokenModel.ToKeyId = Guid.NewGuid().ToString();
                authTokenModel.AccreditDate = DateTime.Now;
                flag = mAuthTokenManager.Add(authTokenModel);
            }
            else
            {
                flag = mAuthTokenManager.Change(authTokenModel);
            }
            if (flag)
            {
                return JsonTips("success", JStr.SuccessSaved);
            }
            return JsonTips("error", JStr.SaveFailed);
        }

        /// <summary>
        /// 删除数据(支持批量删除)
        /// </summary>
        /// <param name="delList">需要删除的ID集合，用逗号隔开</param>
        /// <returns></returns>
        public ActionResult DelData(string delList)
        {
            if (string.IsNullOrEmpty(delList))
            {
                return JsonTips("warning", JStr.PlzSelectARecord);
            }

            string[] idArr = delList.Split(',');
            int i = 0;
            string msg1 = "";
            string msg2 = "";
            foreach (string str in idArr)
            {
                AuthToken authTokenModel = new AuthToken();
                authTokenModel.ToKeyId = str.ToStr();

                List<DataRelation> list = mAuthTokenManager.GetDataRelation(authTokenModel.ToKeyId);
                if (list.Any())
                {
                    msg1 = "," + FStr.PlzDeleteAuthroizeRelationFirst;
                    continue;
                }

                List<ServiceRelation> listTmp = mAuthTokenManager.GetServiceRelation(authTokenModel.ToKeyId);
                if (listTmp.Any())
                {
                    msg2 = "," + FStr.PlzDeleteAuthroizeRelationFirst;
                    continue;
                }
                bool flag = mAuthTokenManager.Delete(authTokenModel);
                if (flag)
                {
                    i++;
                }
            }
            if (i == 0)
            {
                return JsonTips("error", JStr.DeleteFailed + msg1 + msg2);
            }
            else
            {
                return JsonTips("success", null, JStr.SuccessDeleted0 + msg1 + msg2, (object)null, i);
            }
        }
        #endregion

        #region 数据授权

        /// <summary>
        /// 数据授权页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DataRelation(string id)
        {
            DataRelation dataRelationModel = new DataRelation() { TokeyID = id };
            return View("DataRelation", dataRelationModel);
        }

        /// <summary>
        /// 获取已授权的数据节点
        /// </summary>
        /// <returns></returns>
        public JsonResult InitTreeCheckStat(string id)
        {
            List<DataRelation> data = mAuthTokenManager.GetDataRelation(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存授权信息
        /// </summary>
        /// <returns>保存成功</returns>
        [HttpPost]
        public ActionResult SaveDataRelation()
        {
            string nowDataList = Request.Form["nowDataList"];
            string tokeyID = Request.Form["TokeyID"];
            if (string.IsNullOrEmpty(tokeyID))
            {
                return JsonTips("warning", "请提供授权ID");
            }
            List<DataRelation> nowList = JsonHelper.FormJson(nowDataList, typeof(List<DataRelation>)) as List<DataRelation>;
            bool flag = mAuthTokenManager.SaveDataRelation(nowList, tokeyID);
            if (flag)
            {
                return JsonTips("success", "保存成功");
            }
            return JsonTips("error", "保存失败");
        }

        #endregion

        #region 服务授权

        /// <summary>
        /// 数据授权页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceRelation(string id)
        {
            ServiceRelation serviceRelationModel = new ServiceRelation() { TokeyID = id };
            return View("ServiceRelation", serviceRelationModel);
        }

        /// <summary>
        /// 获取已授权的数据节点
        /// </summary>
        /// <returns></returns>
        public JsonResult InitServiceTreeCheckStat(string id)
        {

            mAuthTokenManager.GetServiceByClientId(id);

            List<ServiceRelation> data = mAuthTokenManager.GetServiceRelation(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存授权信息
        /// </summary>
        /// <returns>保存成功</returns>
        [HttpPost]
        public ActionResult SaveServiceRelation()
        {
            string nowDataList = Request.Form["nowDataList"];
            string tokeyID = Request.Form["TokeyID"];
            if (string.IsNullOrEmpty(tokeyID))
            {
                return JsonTips("warning", "请提供授权ID");
            }
            List<ServiceRelation> nowList = JsonHelper.FormJson(nowDataList, typeof(List<ServiceRelation>)) as List<ServiceRelation>;
            bool flag = mAuthTokenManager.SaveServiceRelation(nowList, tokeyID);
            if (flag)
            {
                //ServerAuthManager
                mAuthTokenManager.ServiceAuthList = null;
                mAuthTokenManager.GetAllAuthService();
                return JsonTips("success", "保存成功");
            }
            return JsonTips("error", "保存失败");
        }

        #endregion
    }
}
