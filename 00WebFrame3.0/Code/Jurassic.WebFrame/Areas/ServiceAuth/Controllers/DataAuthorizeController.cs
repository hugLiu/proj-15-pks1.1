using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.CommonModels.ServerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebFrame.Areas.ServiceAuth.Controllers
{
    public class DataAuthorizeController : BaseController
    {
        public DataAuthorizeManager dataAuthorizeManager = ApiManager.mDataAuthorizeManager;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DataNodeInfo dataNodeInfoModel = new DataNodeInfo() { IsvalId = 1, DataParentID = "" };
            return View(dataNodeInfoModel);
        }

        /// <summary>
        /// 获取数据节点集合
        /// 获取所有的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllTreeData()
        {
            List<DataNodeInfo> data = dataAuthorizeManager.GetData(null);
            return JsonNT(data);
        }

        /// <summary>
        /// 获取数据节点集合
        /// 获取有效的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData()
        {
            List<DataNodeInfo> data = dataAuthorizeManager.GetData(1);
            return JsonNT(data);
        }

        /// <summary>
        /// 获取数据节点集合
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataInfo(string id)
        {
            List<DataNodeInfo> data = dataAuthorizeManager.GetDataById(id);
            DataNodeInfo dataNodeInfo = new DataNodeInfo();
            if (data.Any())
            {
                dataNodeInfo = data[0];
            }
            return JsonNT(dataNodeInfo);
        }

        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <returns>保存成功回首页</returns>
        [HttpPost]
        public ActionResult SaveForm(DataNodeInfo dataNodeInfoModel)
        {
            bool flag = true;
            if (dataNodeInfoModel.DataParentID == dataNodeInfoModel.DataNodeID)
            {
                return JsonTips("warning", FStr.ParentAndCurrentSameName);// "您所选择的上级数据节点与当前节点重名!");
            }
            if (dataNodeInfoModel == null)
            {
                return JsonTips("error", JStr.InvalidRequestData);
            }

            //验证授权用户组名称或授权key是否重复
            if (!dataAuthorizeManager.VerifyDataNodeRepeat(dataNodeInfoModel))
            {
                return JsonTips("warning", "你设置的数据节点编码重复!");
            }
            //保存或修改数据
            if (string.IsNullOrEmpty(dataNodeInfoModel.DataID))
            {
                //新增状态设置主键以及创建日期
                dataNodeInfoModel.DataID = Guid.NewGuid().ToString();
                dataNodeInfoModel.CreatedDate = DateTime.Now;
                dataNodeInfoModel.CreatedBy = base.CurrentUser.Name;
                flag = dataAuthorizeManager.Add(dataNodeInfoModel);
            }
            else
            {
                flag = dataAuthorizeManager.Change(dataNodeInfoModel);
            }
            if (flag)
            {
                return JsonTips("success", "保存成功", Json(new { data = dataNodeInfoModel }));
            }
            return JsonTips("error", "保存失败");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">需要删除的ID</param>
        /// <returns></returns>
        public ActionResult DelData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return JsonTips("warning", "请选择一条数据");
            }
            DataNodeInfo dataNodeInfoModel = new DataNodeInfo();
            dataNodeInfoModel.DataID = id;

            bool isRelations = dataAuthorizeManager.IsDataRelations(dataNodeInfoModel.DataID);
            if (isRelations)
            {
                return JsonTips("warning", "提示:您所删除的数据节点,与客户组存在数据授权关系,请先删除该数据授权关系.");
            }

            bool flag = dataAuthorizeManager.Delete(dataNodeInfoModel);
            if (flag)
            {
                return JsonTips("success", "删除成功");
            }
            else
            {
                return JsonTips("error", "删除失败");
            }
        }




    }
}
