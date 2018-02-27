using Jurassic.Com.Tools;
using Jurassic.CommonModels.Organization;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebQuery.Controllers
{
    /// <summary>
    /// 选择部门用户 wangjiaxin 2016/9/28
    /// </summary>
    public class SelectDepUserController : BaseController
    {

        /// <summary>
        /// 选择用户的视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 部门树
        /// </summary>
        /// <returns>部门树的json数据</returns>
        public JsonResult GetDepts()
        {
            List<DepartmentModel> data = OrgManager.mOrganizationManager.GetDepartmentData(0);
            return Json(data.Select(d => new { Id = d.Id, Name = d.Name, ParentId = d.ParentId }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有部门用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllDeptUsers(string key = null)
        {
            var list = OrgManager.mOrganizationManager.GetDepUserQuery()
               .GroupBy(u => u.UserId)
               .Select(g => g.FirstOrDefault());
            if (!key.IsEmpty())
            {
                list = list.Where(u => u.UserName.Contains(key));
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 指定部门下的用户列表
        /// </summary>
        /// <param name="deptIds">部门的ID，以,号分隔多个</param>
        /// <returns>用户列表json数据</returns>
        [AdvQuery]
        public JsonResult GetDeptUsers(string deptIds)
        {
            int[] deptIdArr = CommOp.ToIntArray(deptIds, ',');
            var list = OrgManager.mOrganizationManager.GetDepUserQuery();
            if (!deptIdArr.IsEmpty())
            {
                list = list
                   .Where(u => u.DepId != null && deptIdArr.Contains(u.DepId.Value));
            }

            var result = list
               .GroupBy(u => u.UserId)
               .Select(g => g.FirstOrDefault())
                // 在不需要高级查询，只需要模糊查询的情况下，也可以返回一个匿名对象查询
                // 这里只返回三个字段，是为了减少返回前台的json数据量
               .Select(u => new
               {
                   Id = u.UserId.Value,
                   UserName = u.UserName,
                   DepName = u.DepName,
               });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}