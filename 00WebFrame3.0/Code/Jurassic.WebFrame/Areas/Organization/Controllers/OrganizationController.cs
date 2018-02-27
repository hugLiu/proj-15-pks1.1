using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Organization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebFrame.Areas.Organization.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OrganizationController : BaseController
    {
        public OrganizationManager organizationManager =  OrgManager.mOrganizationManager;

        /// <summary>
        /// 初始页面部门表单
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DepartmentModel organizationModel = new DepartmentModel() { IsDisabled = 0, Ord = 1, DepType = "2" };
            return View(organizationModel);
        }

        /// <summary>
        /// 获取机构节点集合
        /// 获取所有数据
        /// </summary>
        /// <param name="isDisabled">是否禁用 1是 0否 null所有</param>
        /// <returns></returns>
        public ActionResult GetTreeData(int? isDisabled)
        {
            List<DepartmentModel> data = organizationManager.GetDepartmentData(isDisabled);
            return JsonNT(data);
        }
         
        /// <summary>
        /// 查询组织结构相关的岗位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDepPostData(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            DepartmentModel model = new DepartmentModel();
            model.Id = Convert.ToInt32(id);
            List<DepPostModel> list = organizationManager.GetDepPostData(model);
            return JsonNT(list);
        }

        /// <summary>
        /// 查询组织结构相关的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDepUserData(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            DepartmentModel model = new DepartmentModel();
            model.Id = Convert.ToInt32(id);
            List<ViewDepUserModel> list = organizationManager.GetDepUserData(model);
            return JsonNT(list);
        }
         
        /// <summary>
        /// 根据Id获取组织机构数据对象
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataInfo(int id)
        {
            List<DepartmentModel> data = organizationManager.GetDepartmentById(id);
            DepartmentModel organizationModel = new DepartmentModel();
            if (data.Any())
            {
                organizationModel = data[0];
            }
            return JsonNT(organizationModel);
        }

        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <returns>保存成功回首页</returns>
        [HttpPost]
        public ActionResult SaveForm()
        {  
            #region 业务数据验证
            //主数据业务
            var formData = Request.Form["formData"];
            DepartmentModel organizationModel = JsonHelper.FormJson(formData, typeof(DepartmentModel)) as DepartmentModel;

            if (organizationModel == null)
                return JsonTips("error", FStr.InvalidRequestData);

            if (organizationManager.VerifyOrgCode(organizationModel))
                return JsonTipsLang("warning", FStr.DuplicatedOrgCode);

            //岗位明细数据
            var grid1 = Request.Form["grid1"];
            //用户明细数据
            var grid2 = Request.Form["grid2"];

            organizationModel.DepPostModelList = JsonHelper.FromJson<List<DepPostModel>>(grid1);
            organizationModel.DepUserModelList = JsonHelper.FromJson<List<DepUserModel>>(grid2);
            #endregion

            #region 保存或修改数据

            if (organizationModel.Id == null || organizationModel.Id <= 0)
            {
                //新增状态设置主键以及创建日期
                organizationModel.OrgNode = organizationModel.ParentOrgNode + organizationModel.DepHID + ".";
                organizationModel.IsActive = 1;
                organizationModel.IsDeleted = 0;
                organizationModel.IsDisabled = 0;
                organizationModel.CreateDatetime = DateTime.Now;
                organizationModel.ModifiedDateTime = DateTime.Now;
                organizationModel.ExamineType = "";
            }
            else
            {
                organizationModel.ModifiedDateTime = DateTime.Now;
            }
            #endregion

            bool flag = organizationManager.SaveInfo(organizationModel);
            if (flag)
            {
                return JsonTips("success", JStr.SuccessSaved, Json(new { data = organizationModel }));
            }
            return JsonTips("error", JStr.SaveFailed);
        }

        /// <summary>
        /// 删除组织机构数据
        /// </summary>
        /// <param name="id">需要删除的ID</param>
        /// <returns></returns>
        public ActionResult DelData(int? id)
        {
            if (id == null || id <= 0)
            {
                return JsonTipsLang("warning", "请选择一条数据");
            }
            DepartmentModel departmentModel = new DepartmentModel();
            departmentModel.Id = id;
 
            bool flag = organizationManager.Delete(departmentModel);
            if (flag)
            {
                return JsonTips("success", JStr.SuccessDeleted);
            }
            else
            {
                return JsonTips("error", JStr.DeleteFailed);
            }
        }

        /// <summary>
        /// 获取岗位数据集合
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPostDataInfo()
        {
            List<PostModel> data = organizationManager.GetPostData();
            return JsonNT(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyUserIsMainOrg(string name, string id)
        {
            DepUserModel model = new DepUserModel();
            model.UserName = name;
            model.DepId = id.ToInt();
            ViewUserModel data = organizationManager.VerifyUserIsMainOrg(model);
            return JsonNT(data);
        }

        /// <summary>
        /// 人员合同类型(字典)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDictDataHT()
        {
            #region 
            DataTable dt=new DataTable("DICT");
            dt.Columns.Add("text");
            dt.Columns.Add("id");

            DataRow dr1 = dt.NewRow();
            dr1["text"] = "录用合同";
            dr1["id"] = "1";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["text"] = "聘用合同";
            dr2["id"] = "2";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr2["text"] = "借调合同";
            dr2["id"] = "3";
            dt.Rows.Add(dr3);
            #endregion
            return JsonNT(dt);
        }
        
        /// <summary>
        /// 人员基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetUserList()
        {
            List<ViewUserModel> data = organizationManager.GetUserList();
            return JsonNT(data);
        }

        /// <summary>
        /// 初始页面岗位表格
        /// </summary>
        /// <returns></returns>
        public ActionResult PostInfo()
        {
            return View();
        }

        /// <summary>
        /// 保存表单信息
        /// </summary>
        /// <returns>保存成功回首页</returns>
        [HttpPost]
        public ActionResult PostSave()
        {
            #region 业务数据验证
            //岗位数据
            var grid1 = Request.Form["grid1"];
            List<PostModel> postModelList = JsonHelper.FromJson<List<PostModel>>(grid1);

            if (!postModelList.Any())
                return JsonTips("warning", FStr.NoDataToSave);
            #endregion

            foreach (PostModel item in postModelList)
            {
                item.OperatorID = base.UserConfig.Id.ToInt();
                item.PostType = string.IsNullOrEmpty(item.PostType) ? "" : item.PostType;
                item.PostEngageType = string.IsNullOrEmpty(item.PostEngageType) ? "" : item.PostEngageType;
                item.PostLevelType = string.IsNullOrEmpty(item.PostLevelType) ? "" : item.PostLevelType;
                item.IsDeleted = 0;
                item.CreateDatetime = DateTime.Now;
            }

            bool flag = organizationManager.SavePostInfo(postModelList);
            if (flag)
            {
                return JsonTips("success", JStr.SuccessSaved);
            }
            return JsonTips("error", JStr.SaveFailed);
        }

        /// <summary>
        /// 验证该岗位是否存在引用关系
        /// 岗位与部门表
        /// 岗位与部门人员表
        /// </summary>
        /// <returns>true 没有引用关系  false 有引用关系</returns>
        public ActionResult IsNotLinkPost(int? id)
        {
            PostModel postModel = new PostModel() { Id = id };
            bool flag = organizationManager.IsNotLinkPost(postModel);
            return JsonNT(flag);
        }



    }
}
