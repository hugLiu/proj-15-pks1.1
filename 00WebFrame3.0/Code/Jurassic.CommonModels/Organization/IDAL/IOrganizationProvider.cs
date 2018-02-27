using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Organization
{
    public interface IOrganizationProvider
    {
        #region

        /// <summary>
        /// 查询组织结构数据
        /// </summary>
        /// <param name="isDisabled">1有效 0无效 null所有</param>
        /// <returns></returns>
        List<DepartmentModel> GetDepartmentData(int? isDisabled);

        /// <summary>
        /// 通过主键id查询部门数据对象
        /// </summary>
        /// <param name="orgID">机构ID(主键)</param>
        /// <returns></returns>
        List<DepartmentModel> GetDepartmentById(int id);

        /// <summary>
        /// 通过节点查询组织机构子节点
        /// </summary>
        /// <param name="orgNode">机构节点</param>
        /// <returns></returns>
        List<DepartmentModel> GetOrgByOrgNode(string orgNode);

        /// <summary>
        /// 验证机构编码是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="organizationModel">数据节点对象</param>
        /// <returns>true重复  false=不重复</returns>
        bool VerifyOrgCode(DepartmentModel departmentModel);

        /// <summary>
        /// 获取员工的第一部门对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        ViewUserModel VerifyUserIsMainOrg(DepUserModel depUserModel);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool SaveInfo(DepartmentModel organizationModel);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool Delete(DepartmentModel organizationModel);

        /// <summary>
        /// 查询组织结构相关的岗位
        /// </summary>
        /// <returns></returns>
        List<DepPostModel> GetDepPostData(DepartmentModel organizationModel);

        /// <summary>
        /// 查询岗位数据集合
        /// </summary>
        /// <returns></returns>
        List<PostModel> GetPostData();

        /// <summary>
        /// 查询所有人员集合
        /// </summary>
        /// <returns></returns>
        List<ViewUserModel> GetUserList();

        /// <summary>
        /// 查询组织结构相关的人员
        /// </summary>
        /// <returns></returns>
        List<ViewDepUserModel> GetDepUserData(DepartmentModel organizationModel);

        /// <summary>
        /// 保存岗位数据
        /// </summary>
        /// <param name="postModelList"></param>
        /// <returns></returns>
        bool SavePostInfo(List<PostModel> postModelList);

        /// <summary>
        /// 验证该岗位是否存在引用关系
        /// 岗位与部门表
        /// 岗位与部门人员表
        /// </summary>
        /// <returns>true 没有引用关系  false 有引用关系</returns>
        bool IsNotLinkPost(PostModel postModel);
        #endregion

        #region 组织机构接口数据

        /// <summary>
        /// 获取用户所属部门以及直属领导人对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        ViewUserModel GetUserInfo(DepUserModel depUserModel);

        /// <summary>
        /// 获取指定员工的上级部门主管
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        List<ViewUserModel> GetParentDepInfo(DepUserModel depUserModel);

        /// <summary>
        /// 获取部门对象以及部门的主管
        /// </summary>
        /// <param name="departmentModel"></param>
        /// <returns></returns>
        ViewDepartmentModel GetDepInfo(DepartmentModel departmentModel);

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        List<ViewDepartmentModel> GetDepList();

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        List<ViewUserModel> GetUserInfoList();

        /// <summary>
        /// 根据用户名称获取用户ID
        /// </summary>
        /// <returns></returns>
        ViewUserModel GetUserInfoByName(string name);


        /// <summary>
        /// 返回一个可以附加其他条件的查询
        /// </summary>
        /// <returns>部门用户查询对象</returns>
        IQueryable<ViewDepUserModel> GetDepUserQuery();
        #endregion

    }
}
