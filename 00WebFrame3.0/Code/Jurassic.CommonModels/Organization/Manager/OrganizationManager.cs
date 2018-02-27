using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Organization
{
    public class OrganizationManager
    {

        private IOrganizationProvider organizationProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        public OrganizationManager(IOrganizationProvider organization)
        {
            organizationProvider = organization;
        }

        #region

        /// <summary>
        /// 查询组织结构数据
        /// </summary>
        /// <param name="isDisabled">1有效 0无效 null所有</param>
        /// <returns></returns>
        public List<DepartmentModel> GetDepartmentData(int? isDisabled)
        {
            return organizationProvider.GetDepartmentData(isDisabled);
        }

        /// <summary>
        /// 通过主键id查询部门数据对象
        /// </summary>
        /// <param name="orgID">机构ID(主键)</param>
        /// <returns></returns>
        public List<DepartmentModel> GetDepartmentById(int id)
        {
            return organizationProvider.GetDepartmentById(id);
        }

        /// <summary>
        /// 通过节点查询组织机构子节点
        /// </summary>
        /// <param name="orgNode">机构节点</param>
        /// <returns></returns>
        public List<DepartmentModel> GetOrgByOrgNode(string orgNode)
        {
            return organizationProvider.GetOrgByOrgNode(orgNode);
        }

        /// <summary>
        /// 验证机构编码是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="departmentModel">数据节点对象</param>
        /// <returns>true重复  false=不重复</returns>
        public bool VerifyOrgCode(DepartmentModel departmentModel)
        {
            return organizationProvider.VerifyOrgCode(departmentModel);
        }

        /// <summary>
        /// 获取员工的第一部门对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public ViewUserModel VerifyUserIsMainOrg(DepUserModel depUserModel)
        {
            return organizationProvider.VerifyUserIsMainOrg(depUserModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool SaveInfo(DepartmentModel organizationModel)
        {
            return organizationProvider.SaveInfo(organizationModel);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Delete(DepartmentModel organizationModel)
        {
            return organizationProvider.Delete(organizationModel);
        }

        /// <summary>
        /// 查询组织结构相关的岗位
        /// </summary>
        /// <returns></returns>
        public List<DepPostModel> GetDepPostData(DepartmentModel organizationModel)
        {
            return organizationProvider.GetDepPostData(organizationModel);
        }

        /// <summary>
        /// 查询岗位数据集合
        /// </summary>
        /// <returns></returns>
        public List<PostModel> GetPostData()
        {
            return organizationProvider.GetPostData();
        }

        /// <summary>
        /// 查询所有人员集合
        /// </summary>
        /// <returns></returns>
        public List<ViewUserModel> GetUserList()
        {
            return organizationProvider.GetUserList();
        }

        /// <summary>
        /// 查询组织结构相关的人员
        /// </summary>
        /// <returns></returns>
        public List<ViewDepUserModel> GetDepUserData(DepartmentModel organizationModel)
        {
            return organizationProvider.GetDepUserData(organizationModel);
        }

        /// <summary>
        /// 保存岗位数据
        /// </summary>
        /// <param name="postModelList"></param>
        /// <returns></returns>
        public bool SavePostInfo(List<PostModel> postModelList)
        {
            return organizationProvider.SavePostInfo(postModelList);
        }

        /// <summary>
        /// 验证该岗位是否存在引用关系
        /// 岗位与部门表
        /// 岗位与部门人员表
        /// </summary>
        /// <returns>true 没有引用关系  false 有引用关系</returns>
        public bool IsNotLinkPost(PostModel postModel)
        {
            return organizationProvider.IsNotLinkPost(postModel);
        }
        #endregion

        #region 组织机构接口数据

        /// <summary>
        /// 获取用户所属部门以及直属领导人对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public ViewUserModel GetUserInfo(string userName)
        {
            DepUserModel depUserModel = new DepUserModel();
            depUserModel.UserName = userName;
            return organizationProvider.GetUserInfo(depUserModel);
        }

        /// <summary>
        /// 获取指定员工的上级部门主管
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public List<ViewUserModel> GetParentDepInfo(string userName)
        {
            DepUserModel depUserModel = new DepUserModel();
            depUserModel.UserName = userName;
            return organizationProvider.GetParentDepInfo(depUserModel);
        }

        /// <summary>
        /// 获取部门对象以及部门的主管
        /// </summary>
        /// <param name="departmentModel"></param>
        /// <returns></returns>
        public ViewDepartmentModel GetDepInfo(int depId)
        {
            DepartmentModel departmentModel = new DepartmentModel();
            departmentModel.Id = depId;
            return organizationProvider.GetDepInfo(departmentModel);
        }

        /// <summary>
        /// 获取用户所属部门以及直属领导人对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public ViewUserModel GetUserInfo(DepUserModel depUserModel)
        {
            return organizationProvider.GetUserInfo(depUserModel);
        }

        /// <summary>
        /// 获取部门对象以及部门的主管
        /// </summary>
        /// <param name="departmentModel"></param>
        /// <returns></returns>
        public ViewDepartmentModel GetDepInfo(DepartmentModel departmentModel)
        {
            return organizationProvider.GetDepInfo(departmentModel);
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<ViewDepartmentModel> GetDepList()
        {
            return organizationProvider.GetDepList();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<ViewUserModel> GetUserInfoList()
        {
            return organizationProvider.GetUserInfoList();
        }

        /// <summary>
        /// 根据用户名称获取用户ID
        /// </summary>
        /// <returns></returns>
        public ViewUserModel GetUserInfoByName(string name)
        {
            return organizationProvider.GetUserInfoByName(name);
        }

        public IQueryable<ViewDepUserModel> GetDepUserQuery()
        {
            return organizationProvider.GetDepUserQuery();
        }
        #endregion
    }
}
