using Jurassic.AppCenter;
using Jurassic.CommonModels.Organization;
using Jurassic.CommonModels.ServerAuth;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Jurassic.CommonModels.EFProvider
{

    public class OrganizationProvider : IOrganizationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public OrganizationProvider()
        { }

        #region "组织机构"
        /// <summary>
        /// 查询组织结构数据
        /// </summary>
        /// <param name="isDisabled">是否禁用 1否 0是 null所有</param>
        /// <returns></returns>
        public List<DepartmentModel> GetDepartmentData(int? isDisabled)
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                if (isDisabled == null)
                {
                    return _context.Set<DepartmentModel>().Where(u => u.IsDeleted == 0).OrderBy(u => u.Ord).ToList();
                }
                else
                {
                    return _context.Set<DepartmentModel>().Where(u => u.IsDeleted == 0 && u.IsDisabled == isDisabled).OrderBy(u => u.Ord).ToList();
                }
            }
        }

        /// <summary>
        /// 通过主键id查询部门数据对象
        /// </summary>
        /// <param name="orgID">机构ID(主键)</param>
        /// <returns></returns>
        public List<DepartmentModel> GetDepartmentById(int id)
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<DepartmentModel>().Where(u => u.Id == id).ToList();
            }
        }

        /// <summary>
        /// 通过节点查询组织机构子节点
        /// </summary>
        /// <param name="orgNode">机构节点</param>
        /// <returns></returns>
        public List<DepartmentModel> GetOrgByOrgNode(string orgNode)
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<DepartmentModel>().Where(u => u.OrgNode.StartsWith(orgNode)).ToList();
            }
        }

        /// <summary>
        /// 验证机构编码是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="departmentModel">数据节点对象</param>
        /// <returns>true重复  false=不重复</returns>
        public bool VerifyOrgCode(DepartmentModel departmentModel)
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                DepartmentModel organization;
                //新增情况下
                if (departmentModel.Id == null || departmentModel.Id == 0)
                {
                    organization = _context.Set<DepartmentModel>().FirstOrDefault(o => o.DepHID.ToUpper() == departmentModel.DepHID.ToUpper());
                }
                else
                {
                    organization = _context.Set<DepartmentModel>().FirstOrDefault(o => o.DepHID.ToUpper() == departmentModel.DepHID.ToUpper() && o.Id != departmentModel.Id);
                }
                if (organization != null && !string.IsNullOrEmpty(organization.DepHID))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取员工的第一部门对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public ViewUserModel VerifyUserIsMainOrg(DepUserModel depUserModel)
        {
            List<ViewUserModel> data = new List<ViewUserModel>();
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {

                var list = from du in _context.Set<DepUserModel>()
                           join d in _context.Set<DepartmentModel>() on du.DepId equals d.Id
                           where du.UserName == depUserModel.UserName && du.DepId != depUserModel.DepId && du.IsMain == 1
                           select new ViewUserModel
                           {
                               DepId = d.Id,
                               DepName = d.Name,
                               UserId = du.UserId,
                               UserName = du.UserName
                           };
                data = list.ToList();
            }
            if (data.Any())
            {
                return data[0];
            }
            return new ViewUserModel();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool SaveInfo(DepartmentModel organizationModel)
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();

            EFAuditDataService<DepartmentModel> efMain = new EFAuditDataService<DepartmentModel>(_context);
            EFAuditDataService<DepPostModel> efPost = new EFAuditDataService<DepPostModel>(_context);
            EFAuditDataService<DepUserModel> efUser = new EFAuditDataService<DepUserModel>(_context);

            try
            {
                #region 组织机构主表数据
                efMain.BeginTrans();
                int row = 0;
                if (organizationModel.Id == null || organizationModel.Id <= 0)
                    row = efMain.Add(organizationModel);
                else
                    row = efMain.Change(organizationModel);

                if (row < 0)
                {
                    efMain.RollbackTrans();
                    return false;
                }
                #endregion

                #region 组织机构与岗位信息
                foreach (DepPostModel item in organizationModel.DepPostModelList)
                {
                    item.DepId = organizationModel.Id;
                    item.IsActive = 1;
                    item.ExamineType = "";
                    item.IsDeleted = 0;
                    item.IsDisabled = 0;
                    item.Name = "";
                    item.CreateDatetime = DateTime.Now;

                    if (item._State.ToUpper() == "ADDED")
                        row = efPost.Add(item);
                    else if (item._State.ToUpper() == "MODIFIED")
                        row = efPost.Change(item);
                    else if (item._State.ToUpper() == "REMOVED")
                        row = efPost.Delete(item);

                    if (row < 0)
                    {
                        efMain.RollbackTrans();
                        return false;
                    }
                }
                #endregion

                #region 组织机构与用户信息

                foreach (DepUserModel item in organizationModel.DepUserModelList)
                {
                    #region 设置用户的主部门,清除已存在其他主部门标识,确定一个用户只有一个主要部门
                    //如果某个用户设置了主部门首先查询是否在其他部门下已经存在设置,如果存在清除掉,重新写入主部门标识
                    if (item.IsMain == 1)
                    {
                        List<DepUserModel> dList = efUser.GetQuery().Where(u => u.UserName == item.UserName && u.DepId != item.DepId).ToList();
                        foreach (DepUserModel m in dList)
                        {
                            m.IsMain = 0;
                            row = efUser.Change(m);
                            if (row < 0)
                            {
                                efMain.RollbackTrans();
                                return false;
                            }
                        }
                    }
                    #endregion

                    item.DepId = organizationModel.Id;
                    item.ContractLenght = item.ContractLenght == null ? 0 : item.ContractLenght;
                    item.ExamineType = "";
                    item.IsDeleted = 0;
                    item.JoinDateTime = item.JoinDateTime == null ? DateTime.Now : item.JoinDateTime;
                    item.IsSuspension = item.IsSuspension == null ? 0 : item.ContractLenght;
                    item.IsLeader = item.IsLeader == null ? 0 : item.IsLeader;
                    item.IsMain = item.IsMain == null ? 0 : item.IsMain;

                    item.CreateDatetime = DateTime.Now;

                    if (item._State.ToUpper() == "ADDED")
                        row = efUser.Add(item);
                    else if (item._State.ToUpper() == "MODIFIED")
                        row = efUser.Change(item);
                    else if (item._State.ToUpper() == "REMOVED")
                        row = efUser.Delete(item);

                    if (row < 0)
                    {
                        efMain.RollbackTrans();
                        return false;
                    }
                }


                #endregion

            }
            catch (Exception)
            {
                efMain.RollbackTrans();
                throw;
            }

            efMain.EndTrans();
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Delete(DepartmentModel organizationModel)
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();
            EFAuditDataService<DepartmentModel> efMain = new EFAuditDataService<DepartmentModel>(_context);
            EFAuditDataService<DepPostModel> efPost = new EFAuditDataService<DepPostModel>(_context);
            EFAuditDataService<DepUserModel> efUser = new EFAuditDataService<DepUserModel>(_context);

            try
            {
                int row = 0;
                #region 组织机构与岗位信息
                List<DepPostModel> postList = efPost.GetQuery().Where(p => p.DepId == organizationModel.Id).ToList();
                foreach (DepPostModel item in postList)
                {
                    row = efPost.Delete(item);
                    if (row < 0)
                    {
                        efMain.RollbackTrans();
                        return false;
                    }
                }
                #endregion

                #region 组织机构与用户信息
                List<DepUserModel> userList = efUser.GetQuery().Where(p => p.DepId == organizationModel.Id).ToList();
                foreach (DepUserModel item in userList)
                {
                    row = efUser.Delete(item);
                    if (row < 0)
                    {
                        efMain.RollbackTrans();
                        return false;
                    }
                }
                #endregion

                #region 组织机构主表数据
                row = efMain.Delete(organizationModel);
                if (row < 0)
                {
                    efMain.RollbackTrans();
                    return false;
                }
                #endregion
            }
            catch (Exception)
            {
                efMain.RollbackTrans();
                throw;
            }

            efMain.EndTrans();
            return true;
        }

        /// <summary>
        /// 查询组织结构相关的岗位
        /// </summary>
        /// <returns></returns>
        public List<DepPostModel> GetDepPostData(DepartmentModel organizationModel)
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<DepPostModel>().Where(u => u.IsDeleted == 0 && u.DepId == organizationModel.Id).OrderBy(u => u.PostId).ToList();
            }
        }

        /// <summary>
        /// 查询组织结构相关的人员
        /// </summary>
        /// <returns></returns>
        public List<ViewDepUserModel> GetDepUserData(DepartmentModel organizationModel)
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                //查询过滤掉删除的用户信息
                var list1 = from deu in _context.Set<DepUserModel>()
                            from ms in _context.Set<MemberShip>()
                            where deu.UserId == ms.UserId &&
                            ms.IsConfirmed == true &&
                            deu.IsDeleted == 0 &&
                            deu.DepId == organizationModel.Id
                            orderby deu.UserName
                            select new ViewDepUserModel()
                            {
                                Id = deu.Id,
                                DepId = deu.DepId,
                                UserId = deu.UserId,
                                UserName = deu.UserName,
                                ExamineType = deu.ExamineType,
                                ContractType = deu.ContractType,
                                ContractLenght = deu.ContractLenght,
                                JoinDateTime = deu.JoinDateTime,
                                CreateDatetime = deu.CreateDatetime,
                                IsSuspension = deu.IsSuspension,
                                IsLeader = deu.IsLeader,
                                IsMain = deu.IsMain,
                                IsDeleted = deu.IsDeleted,
                                PostId = deu.PostId
                            };

                return list1.ToList();
            }
        }

        /// <summary>
        /// 查询岗位数据集合
        /// </summary>
        /// <returns></returns>
        public List<PostModel> GetPostData()
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<PostModel>().Where(u => u.IsDeleted == 0).OrderBy(u => u.PostName).ToList();
            }
        }

        /// <summary>
        /// 查询所有人员集合
        /// </summary>
        /// <returns></returns>
        public List<ViewUserModel> GetUserList()
        {
            using (var _context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = from u in _context.Set<UserProfile>()
                            join ms in _context.Set<MemberShip>() on u.UserId equals ms.UserId

                            join ddu in _context.Set<DepUserModel>() on new { UId = (int?)u.UserId, IsMain = (int?)1 } equals new { UId = ddu.UserId, IsMain = ddu.IsMain } into ddul
                            from dum in ddul.DefaultIfEmpty()

                            join dd in _context.Set<DepartmentModel>() on dum.DepId equals dd.Id into ddm
                            from d in ddm.DefaultIfEmpty()
                            where ms.IsConfirmed == true
                            orderby u.UserName
                            select new ViewUserModel
                            {
                                UserId = u.UserId,
                                UserName = u.UserName,
                                ParentId = d.ParentId,
                                DepId = d.Id,
                                DepName = d.Name,
                                DepHID = d.DepHID 
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 保存岗位数据
        /// </summary>
        /// <param name="postModelList"></param>
        /// <returns></returns>
        public bool SavePostInfo(List<PostModel> postModelList)
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();
            EFAuditDataService<PostModel> efMain = new EFAuditDataService<PostModel>(_context);
            try
            {
                int row = 0;
                #region 岗位信息
                foreach (PostModel item in postModelList)
                {
                    if (item._State.ToUpper() == "ADDED")
                        row = efMain.Add(item);
                    else if (item._State.ToUpper() == "MODIFIED")
                        row = efMain.Change(item);
                    else if (item._State.ToUpper() == "REMOVED")
                        row = efMain.Delete(item);

                    if (row < 0)
                    {
                        efMain.RollbackTrans();
                        return false;
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                efMain.RollbackTrans();
                throw;
            }

            efMain.EndTrans();
            return true;
        }

        /// <summary>
        /// 验证该岗位是否存在引用关系
        /// 岗位与部门表
        /// 岗位与部门人员表
        /// </summary>
        /// <returns>true 没有引用关系  false 有引用关系</returns>
        public bool IsNotLinkPost(PostModel postModel)
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();
            EFAuditDataService<DepPostModel> efPost = new EFAuditDataService<DepPostModel>(_context);
            EFAuditDataService<DepUserModel> efUser = new EFAuditDataService<DepUserModel>(_context);

            List<DepPostModel> postLsit = efPost.GetQuery().Where(u => u.IsDeleted == 0 && u.PostId == postModel.Id).ToList();
            List<DepUserModel> userLsit = efUser.GetQuery().Where(u => u.IsDeleted == 0 && u.PostId == postModel.Id).ToList();
            if (!postLsit.Any() && !userLsit.Any())
            {
                return true;
            }
            return false;
        }


        #endregion

        #region 组织机构接口数据

        /// <summary>
        /// 获取所有部门对象以及部门的主管
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns>返回一个可以附加其他条件的查询</returns>
        private IQueryable<ViewDepartmentModel> GetDepAllList()
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();

            var list = from dep in _context.Set<DepartmentModel>()
                       join depf in _context.Set<DepartmentModel>() on dep.ParentId equals depf.Id into da
                       from depx in da.DefaultIfEmpty()
                       join depUser in _context.Set<DepUserModel>()
                       on new { Id = dep.Id, IsLeader = (int?)1 } equals new { Id = depUser.DepId, IsLeader = depUser.IsLeader } into depUserl
                       from epUsera in depUserl.DefaultIfEmpty()
                       select new ViewDepartmentModel
                       {
                           DepId = dep.Id,
                           DepName = dep.Name,
                           OrgNode = dep.OrgNode,
                           DepType = dep.DepType,
                           ParentId = depx.Id,
                           ParentDepName = depx.Name,
                           ParentDepHID = depx.DepHID,
                           DepHID = dep.DepHID,
                           LeaderUserId = epUsera.UserId,
                           LeaderUserName = epUsera.UserName
                       };
            return list;
        }

        /// <summary>
        /// 获取指定员工的上级部门主管
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public List<ViewUserModel> GetParentDepInfo(DepUserModel depUserModel)
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();
            var list = 

                       from ddu in _context.Set<DepUserModel>()
                       join dd in _context.Set<DepartmentModel>() on ddu.DepId equals dd.Id into ddm
                       from d in ddm.DefaultIfEmpty()

                       join depf in _context.Set<DepartmentModel>() on d.ParentId equals depf.Id into da
                       from depx in da.DefaultIfEmpty()

                       join depUser in _context.Set<DepUserModel>()
                       on new { Id = depx.Id, IsLeader = (int?)1 } equals new { Id = depUser.DepId, IsLeader = depUser.IsLeader } into depUserl
                       from epUsera in depUserl.DefaultIfEmpty()
                       where ddu.UserName == depUserModel.UserName 
                       select new ViewUserModel
                       {
                           UserId = epUsera.UserId,
                           UserName = epUsera.UserName,
                           ParentId = depx.ParentId,
                           DepId = depx.Id,
                           DepName = depx.Name,
                           DepHID = depx.DepHID 
                       };
            List<ViewUserModel> data = list.ToList();
            if (data.Any())
            {
                return data;
            }
            return new List<ViewUserModel>();
        }

        /// <summary>
        /// 获取指定部门对象以及部门的主管
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public ViewDepartmentModel GetDepInfo(DepartmentModel departmentModel)
        {
            var model = GetDepAllList().FirstOrDefault(p => p.DepId == departmentModel.Id);
            if (model != null)
                return model;
            return new ViewDepartmentModel();
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public List<ViewDepartmentModel> GetDepList()
        {
            List<ViewDepartmentModel> data = new List<ViewDepartmentModel>();

            var list = from dd in GetDepAllList() select dd;
            data = list.ToList();

            if (data.Any())
            {
                foreach (ViewDepartmentModel item in data)
                {
                    if (!string.IsNullOrEmpty(item.OrgNode))
                        item.DepLevel = item.OrgNode.Split('.').Length - 1;
                }
            }
            return data;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>返回一个可以附加其他条件的查询</returns>
        private IQueryable<ViewUserModel> GetUserInfoAllList()
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();
            var list = from u in _context.Set<UserProfile>()
                       join ms in _context.Set<MemberShip>() on u.UserId equals ms.UserId

                       join ddu in _context.Set<DepUserModel>() on new { UId = (int?)u.UserId, IsMain = (int?)1 } equals new { UId = ddu.UserId, IsMain = ddu.IsMain } into ddul
                       from dum in ddul.DefaultIfEmpty()

                       join dd in _context.Set<DepartmentModel>() on dum.DepId equals dd.Id into ddm
                       from d in ddm.DefaultIfEmpty()

                       join dle in _context.Set<DepUserModel>() on new { Id = d.Id, IsLeader = (int?)1 } equals new { Id = dle.DepId, IsLeader = dle.IsLeader } into dlea
                       from dea in dlea.DefaultIfEmpty()

                       where ms.IsConfirmed == true
                       orderby u.UserName
                       select new ViewUserModel
                       {
                           UserId = u.UserId,
                           UserName = u.UserName,
                           ParentId = d.ParentId,
                           DepId = d.Id,
                           DepName = d.Name,
                           DepHID = d.DepHID,
                           LeaderUserName = dea.UserName,
                           IsLeader = dum.IsLeader
                       };
            return list;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<ViewUserModel> GetUserInfoList()
        {
            var list = from dd in GetUserInfoAllList() select dd;
            List<ViewUserModel> data = list.ToList();
            if (data.Any())
                return data;
            return new List<ViewUserModel>();
        }

        /// <summary>
        /// 获取用户所属部门以及直属领导人对象
        /// </summary>
        /// <param name="depUserModel"></param>
        /// <returns></returns>
        public ViewUserModel GetUserInfo(DepUserModel depUserModel)
        {
            List<ViewUserModel> data = new List<ViewUserModel>();
            var model = GetUserInfoAllList().FirstOrDefault(p => p.UserName == depUserModel.UserName);
            if (model != null)
                return model;
            return new ViewUserModel();
        }

        /// <summary>
        /// 根据用户名称获取用户ID
        /// 此方法是冗余工作流接口使用
        /// </summary>
        /// <returns></returns>
        public ViewUserModel GetUserInfoByName(string name)
        {
            List<ViewUserModel> data = new List<ViewUserModel>();
            var model = GetUserInfoAllList().FirstOrDefault(p => p.UserName == name);
            if (model != null)
                return model;
            return new ViewUserModel();
        }

        /// <summary>
        /// 返回一个可以附加其他条件的查询
        /// </summary>
        /// <returns>部门用户查询Queryable对象</returns>
        public IQueryable<ViewDepUserModel> GetDepUserQuery()
        {
            var _context = SiteManager.Kernel.Get<ModelContext>();
            //查询过滤掉删除的用户信息
            var query = from deu in _context.Set<DepUserModel>()
                        from ms in _context.Set<MemberShip>()
                        from dep in _context.DepartmentModels
                        where deu.UserId == ms.UserId &&
                        dep.Id == deu.DepId &&
                        ms.IsConfirmed == true &&
                        deu.IsDeleted == 0
                        orderby deu.UserName
                        select new ViewDepUserModel()
                        {
                            Id = deu.Id,
                            DepId = deu.DepId,
                            DepName = dep.Name,
                            UserId = deu.UserId,
                            UserName = deu.UserName,
                            ExamineType = deu.ExamineType,
                            ContractType = deu.ContractType,
                            ContractLenght = deu.ContractLenght,
                            JoinDateTime = deu.JoinDateTime,
                            CreateDatetime = deu.CreateDatetime,
                            IsSuspension = deu.IsSuspension,
                            IsLeader = deu.IsLeader,
                            IsMain = deu.IsMain,
                            IsDeleted = deu.IsDeleted,
                            OrgNode = dep.OrgNode,
                            PostId = deu.PostId
                        };
            return query;
        }

        #endregion


    }
}
