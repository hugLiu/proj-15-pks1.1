using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.AppCenter;
using Jurassic.CommonModels.ServerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebFrame.Areas.ServiceAuth.Controllers
{
    public class ServiceAuthorizeController : BaseController
    {
        public ServiceInfoManager serviceInfoManager = ApiManager.mServiceInfoManager;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ServiceInfo serviceInfoModel = new ServiceInfo() { IsvalId = 1, ParentID = "" };
            return View(serviceInfoModel);
        }

        /// <summary>
        /// 获取数据节点集合
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllTreeData()
        {
            List<ServiceInfo> data = serviceInfoManager.GetData(null);
            return JsonNT(data);
        }

        /// <summary>
        /// 获取数据节点集合
        /// 仅限获取有效的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData()
        {
            List<ServiceInfo> data = serviceInfoManager.GetData(1);
            return JsonNT(data);
        }

        /// <summary>
        /// 获取数据节点集合
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataInfo(string id)
        { 
            List<ServiceInfo> data = serviceInfoManager.GetDataById(id);
            ServiceInfo serviceInfoModel = new ServiceInfo();
            if (data.Any())
            {
                serviceInfoModel = data[0];
            }
            return JsonNT(serviceInfoModel);
        }

        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <returns>保存成功回首页</returns>
        [HttpPost]
        public ActionResult SaveForm(ServiceInfo serviceInfoModel)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(serviceInfoModel.ServiceID) && serviceInfoModel.ParentID == serviceInfoModel.ServiceID)
            {
                return JsonTips("warning", FStr.ParentAndCurrentSameName);
            }
            if (serviceInfoModel == null)
            {
                return JsonTips("error", JStr.InvalidRequestData);
            }

            //保存或修改数据
            if (string.IsNullOrEmpty(serviceInfoModel.ServiceID))
            {
                //新增状态设置主键以及创建日期
                serviceInfoModel.ServiceID = Guid.NewGuid().ToString();
                serviceInfoModel.CreatedDate = DateTime.Now;
                serviceInfoModel.CreatedBy = base.CurrentUser.Name;
                flag = serviceInfoManager.Add(serviceInfoModel);
            }
            else
            {
                flag = serviceInfoManager.Change(serviceInfoModel);
            }
            if (flag)
            {
                serviceInfoManager.ServiceInfoList = null;
                serviceInfoManager.GetDataList();
                return JsonTips("success", JStr.SuccessSaved, Json(new { data = serviceInfoModel }));
            }
            return JsonTips("error", JStr.SaveFailed);
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
                return JsonTips("warning", JStr.PlzSelectARecord);
            }
            ServiceInfo serviceInfoModel = new ServiceInfo();
            serviceInfoModel.ServiceID = id;

            bool isRelations = serviceInfoManager.IsServiceRelations(serviceInfoModel.ServiceID);
            if (isRelations)
            {
                return JsonTips("warning", FStr.PlzDeleteAuthroizeRelationFirst);// "提示:您所删除的服务节点,与客户组存在服务授权关系,请先删除该服务授权关系.");
            }
            bool flag = serviceInfoManager.Delete(serviceInfoModel);
            if (flag)
            {
                return JsonTips("success", JStr.SuccessDeleted);
            }
            else
            {
                return JsonTips("error", JStr.DeleteFailed);
            }
        }




    }
}

