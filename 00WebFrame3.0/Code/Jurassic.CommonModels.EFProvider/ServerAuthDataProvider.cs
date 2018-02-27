using Jurassic.AppCenter;
using Jurassic.CommonModels.ServerAuth;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Jurassic.CommonModels.EFProvider
{
    public class ServerAuthDataProvider
    {
    }

    /// <summary>
    /// 安全令牌数据访问对象
    /// </summary>
    public class ServerAuthProvider : IServerAuthProvider
    {
        ModelContext _context;
        public ServerAuthProvider()
        { }

        #region 用户组授权
        /// <summary>
        /// 查询所有授权客户组数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<AuthToken> GetData()
        {
            _context = SiteManager.Kernel.Get<ModelContext>();
            return _context.Set<AuthToken>();
        }

        /// <summary>
        /// 通过主键id查询数据对象
        /// </summary>
        /// <param name="toKeyId">授权表主键</param>
        /// <returns></returns>
        public List<AuthToken> GetDataById(string toKeyId)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<AuthToken>().Where(u => u.ToKeyId == toKeyId).ToList();
            }
        }

        /// <summary>
        /// 通过客户组id查询数据对象
        /// 有效的数据
        /// </summary>
        /// <param name="clientId">客户id</param>
        /// <returns></returns>
        public List<AuthToken> GetDataByClientId(string clientId)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<AuthToken>().Where(u => u.ClientId == clientId && u.IsvalId == 1).ToList();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool Delete(AuthToken authTokenModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<AuthToken>(authTokenModel).State = EntityState.Deleted;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool Add(AuthToken authTokenModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<AuthToken>(authTokenModel).State = EntityState.Added;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool Change(AuthToken authTokenModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<AuthToken>(authTokenModel).State = EntityState.Modified;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 验证客户id与授权key是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool VerifyClientRepeat(AuthToken authTokenModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                AuthToken authToken = null;
                if (string.IsNullOrEmpty(authTokenModel.ToKeyId))
                {
                    authToken = _context.Set<AuthToken>().FirstOrDefault(o => o.ClientId == authTokenModel.ClientId || o.TokeyCode == authTokenModel.TokeyCode);
                }
                else
                {
                    authToken = _context.Set<AuthToken>().FirstOrDefault(o => (o.ClientId == authTokenModel.ClientId || o.TokeyCode == authTokenModel.TokeyCode) && o.ToKeyId != authTokenModel.ToKeyId);
                }
                if (authToken != null && !string.IsNullOrEmpty(authToken.ToKeyId))
                {
                    return false;
                }
                return true;
            }
        }
        #endregion

        #region 数据授权
        /// <summary>
        /// 查询指定客户的数据授权关系
        /// </summary>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        public List<DataRelation> GetDataRelation(string tokeyID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<DataRelation>().Where(u => u.TokeyID == tokeyID).ToList();
            }
        }

        /// <summary>
        /// 保存数据授权关系
        /// </summary>
        /// <param name="dataRelationList">关系集合</param>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        public bool SaveDataRelation(List<DataRelation> dataRelationList, string tokeyID)
        { 
            List<DataRelation> oldList = GetDataRelation(tokeyID);

            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                DbContextTransaction _trans = _context.Database.BeginTransaction();
                #region 首先删除该授权用户已设置的授权数据
                foreach (DataRelation model in oldList)
                {
                    _context.Entry<DataRelation>(model).State = EntityState.Deleted;
                    int row = _context.SaveChanges();
                    if (row < 0)
                    {
                        _trans.Rollback();
                        return false;
                    }
                }
                #endregion
                #region 添加新设置的授权信息
                foreach (DataRelation model in dataRelationList)
                {
                    model.RID = Guid.NewGuid().ToString();

                    _context.Entry<DataRelation>(model).State = EntityState.Added;
                    int row = _context.SaveChanges();
                    if (row < 0)
                    {
                        _trans.Rollback();
                        return false;
                    }
                }
                _trans.Commit();
                #endregion
            }
            return true;
        }

        /// <summary>
        /// 通过客户组Id查询该客户授权的数据节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public List<ViDataAuth> GetDataNodesByClientId(string clientId)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = from dr in _context.DataRelations
                            join d in _context.DataNodeInfos on dr.DataID equals d.DataID
                            join a in _context.AuthTokens on dr.TokeyID equals a.ToKeyId
                            where a.ClientId == clientId
                            select new ViDataAuth
                            {
                                RID = dr.RID,
                                ClientName = a.ClientName,
                                ClientId = a.ClientId,
                                TokeyCode = a.TokeyCode,
                                ValidityDate = a.ValidityDate,
                                DataNodeID = d.DataNodeID,
                                DataNodeName = d.DataNodeName,
                                DataParentID = d.DataParentID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取该客户的数据节点是否授权
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public bool IsAuthData(string clientId, string dataNodeID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = from dr in _context.DataRelations
                            join d in _context.DataNodeInfos on dr.DataID equals d.DataID
                            join a in _context.AuthTokens on dr.TokeyID equals a.ToKeyId
                            where a.ClientId == clientId && d.DataNodeID.ToUpper() == dataNodeID.ToUpper()
                            select new ViDataAuth
                            {
                                RID = dr.RID,
                                ClientName = a.ClientName,
                                ClientId = a.ClientId,
                                TokeyCode = a.TokeyCode,
                                ValidityDate = a.ValidityDate,
                                DataNodeID = d.DataNodeID,
                                DataNodeName = d.DataNodeName,
                                DataParentID = d.DataParentID
                            };
                return query.Any() ? true : false;
            }
        }

        #endregion

        #region 服务授权
        /// <summary>
        /// 查询指定客户的服务授权关系
        /// </summary>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        public List<ServiceRelation> GetServiceRelation(string tokeyID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<ServiceRelation>().Where(u => u.TokeyID == tokeyID).ToList();
            }
        }

        /// <summary>
        /// 保存服务授权关系
        /// </summary>
        /// <param name="serviceRelationList">关系集合</param>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        public bool SaveServiceRelation(List<ServiceRelation> serviceRelationList,string tokeyID)
        {
            List<ServiceRelation> oldList = GetServiceRelation(tokeyID);

            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                DbContextTransaction _trans = _context.Database.BeginTransaction();
                #region 首先删除该授权用户已设置的授权数据
                foreach (ServiceRelation model in oldList)
                {
                    _context.Entry<ServiceRelation>(model).State = EntityState.Deleted;
                    int row = _context.SaveChanges();
                    if (row < 0)
                    {
                        _trans.Rollback();
                        return false;
                    }
                }
                #endregion
                #region 添加新设置的授权信息
                foreach (ServiceRelation model in serviceRelationList)
                {
                    model.SID = Guid.NewGuid().ToString();

                    _context.Entry<ServiceRelation>(model).State = EntityState.Added;
                    int row = _context.SaveChanges();
                    if (row < 0)
                    {
                        _trans.Rollback();
                        return false;
                    }
                }
                _trans.Commit();
                #endregion
            }
            return true;
        }

        /// <summary>
        /// 通过客户组Id查询该客户授权的服务节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public List<ViServiceAuth> GetServiceByClientId(string clientId)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = from sr in _context.ServiceRelations
                            join s in _context.ServiceInfos on sr.ServiceID equals s.ServiceID
                            join a in _context.AuthTokens on sr.TokeyID equals a.ToKeyId
                            where a.ClientId == clientId
                            select new ViServiceAuth
                            {
                                SID = sr.SID,
                                ClientName = a.ClientName,
                                ClientId = a.ClientId,
                                TokeyCode = a.TokeyCode,
                                ServiceID = s.ServiceID,
                                ServiceName = s.ServiceName,
                                ServiceFunctionName = s.ServiceFunctionName,
                                ServiceFullName = s.ServiceFullName,
                                AuthWay = s.AuthWay,
                                RequestWay = s.RequestWay
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取该客户的服务节点是否授权
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="actionName">服务名称</param>
        /// <param name="serviceFullName">服务完成方法名称(包含命名空间名称)</param>
        /// <returns></returns>
        public bool IsAuthService(string clientId, string actionName, string serviceFullName)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
               var query = from sr in _context.ServiceRelations
                            join s in _context.ServiceInfos on sr.ServiceID equals s.ServiceID
                            join a in _context.AuthTokens on sr.TokeyID equals a.ToKeyId
                           where a.ClientId == clientId 
                           && s.ServiceFullName == serviceFullName
                           && s.ServiceFunctionName == actionName 
                            select new ViServiceAuth
                            {
                                SID = sr.SID,
                                ClientName = a.ClientName,
                                ClientId = a.ClientId,
                                TokeyCode = a.TokeyCode,
                                ServiceID = s.ServiceID,
                                ServiceName = s.ServiceName,
                                ServiceFunctionName = s.ServiceFunctionName,
                                ServiceFullName = s.ServiceFullName,
                                AuthWay = s.AuthWay,
                                RequestWay = s.RequestWay
                            };
                return query.Any() ? true : false;
            }
        }

        /// <summary>
        /// 获取所有的授权服务关系
        /// 有效的授权服务
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public List<ViServiceAuth> GetAllAuthService()
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = from sr in _context.ServiceRelations
                            join s in _context.ServiceInfos on sr.ServiceID equals s.ServiceID
                            join a in _context.AuthTokens on sr.TokeyID equals a.ToKeyId
                            where s.IsvalId == 1
                            select new ViServiceAuth
                            {
                                SID = sr.SID,
                                ClientName = a.ClientName,
                                ClientId = a.ClientId,
                                TokeyCode = a.TokeyCode,
                                ServiceID = s.ServiceID,
                                ServiceName = s.ServiceName,
                                ServiceFunctionName = s.ServiceFunctionName,
                                ServiceFullName = s.ServiceFullName,
                                AuthWay = s.AuthWay,
                                RequestWay = s.RequestWay
                            };
                return query.ToList();
            }
        }

        #endregion
    }

    /// <summary>
    /// 数据权限节点对象
    /// </summary>
    public class DataAuthorizeProvider : IDataAuthorizeProvider
    {
        ModelContext _context;
        public DataAuthorizeProvider()
        { }

        #region
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="isvalId">1有效 0无效 null所有</param>
        /// <returns></returns>
        public List<DataNodeInfo> GetData(int? isvalId)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                if (isvalId == null)
                {
                    return _context.Set<DataNodeInfo>().ToList();
                }
                else
                {
                    return _context.Set<DataNodeInfo>().Where(u => u.IsvalId == isvalId).ToList();
                }
            }
        }

        /// <summary>
        /// 通过主键id查询有效的数据对象
        /// </summary>
        /// <param name="toKeyId">授权表主键</param>
        /// <returns></returns>
        public List<DataNodeInfo> GetDataById(string dataID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<DataNodeInfo>().Where(u => u.DataID == dataID).ToList();
            }
        }

        /// <summary>
        /// 通过数据节点id查询有效的数据对象
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public List<DataNodeInfo> GetDataByDataNodeID(string dataNodeID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                return _context.Set<DataNodeInfo>().Where(u => u.DataNodeID == dataNodeID && u.IsvalId == 1).ToList();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Delete(DataNodeInfo dataNodeInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<DataNodeInfo>(dataNodeInfoModel).State = EntityState.Deleted;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Add(DataNodeInfo dataNodeInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<DataNodeInfo>(dataNodeInfoModel).State = EntityState.Added;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Change(DataNodeInfo dataNodeInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<DataNodeInfo>(dataNodeInfoModel).State = EntityState.Modified;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 验证数据节点id是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool VerifyDataNodeRepeat(DataNodeInfo dataNodeInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                DataNodeInfo dataNodeInfo;
                if (string.IsNullOrEmpty(dataNodeInfoModel.DataID))
                {
                    dataNodeInfo = _context.Set<DataNodeInfo>().FirstOrDefault(o => o.DataNodeID.ToUpper() == dataNodeInfoModel.DataNodeID.ToUpper());
                }
                else
                {
                    dataNodeInfo = _context.Set<DataNodeInfo>().FirstOrDefault(o => o.DataNodeID.ToUpper() == dataNodeInfoModel.DataNodeID.ToUpper() && o.DataID.ToUpper() != dataNodeInfoModel.DataID.ToUpper());
                }
                if (dataNodeInfo != null && !string.IsNullOrEmpty(dataNodeInfo.DataID))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 查询该数据节点是否存在授权关系
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public bool IsDataRelations(string dataNodeID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = _context.Set<DataRelation>().Where(u => u.DataID == dataNodeID).ToList();
                return query.Any() ? true : false;
            }
        }
        #endregion

    }

    /// <summary>
    /// 控制器Api服务授权对象
    /// </summary>
    public class ServiceInfoProvider : IServiceInfoProvider
    {
        ModelContext _context;
        public ServiceInfoProvider()
        { }

        #region
        /// <summary>
        /// 获取服务信息
        /// </summary>
        /// <param name="isvalId">1有效 0无效 null所有</param>
        /// <returns></returns>
        public List<ServiceInfo> GetData(int? isvalId)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                if (isvalId == null)
                {
                    return _context.Set<ServiceInfo>().ToList();
                }
                else
                {
                    return _context.Set<ServiceInfo>().Where(u => u.IsvalId == isvalId).ToList();
                }
            }
        }

        /// <summary>
        /// 通过主键id查询有效的服务对象
        /// </summary>
        /// <param name="serviceID">授权表主键</param>
        /// <returns></returns>
        public List<ServiceInfo> GetDataById(string serviceID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                //左链接查询
                var query = from sr in _context.ServiceInfos
                             join s in _context.ServiceInfos on sr.ParentID equals s.ServiceID 
                             into sLeft from sl in sLeft.DefaultIfEmpty()   
                             where sr.ServiceID == serviceID
                             select new
                             {
                                 ServiceID = sr.ServiceID,
                                 ParentID = sr.ParentID,
                                 ParentName = sl.ServiceName,
                                 ServiceName = sr.ServiceName,
                                 ServiceFunctionName = sr.ServiceFunctionName,
                                 ServiceFullName = sr.ServiceFullName,
                                 RequestWay = sr.RequestWay,
                                 AuthWay = sr.AuthWay,
                                 IsvalId = sr.IsvalId,
                                 CreatedDate = sr.CreatedDate,
                                 CreatedBy = sr.CreatedBy,
                                 Memo = sr.Memo
                             };
            
                var data = query.ToList().Select(m => new ServiceInfo
                {
                    ServiceID = m.ServiceID,
                    ParentID = m.ParentID,
                    ServiceName = m.ServiceName,
                    ParentName = m.ParentName,
                    ServiceFunctionName = m.ServiceFunctionName,
                    ServiceFullName = m.ServiceFullName,
                    RequestWay = m.RequestWay,
                    AuthWay = m.AuthWay,
                    IsvalId = m.IsvalId,
                    Memo = m.Memo,
                    CreatedDate = m.CreatedDate,
                    CreatedBy = m.CreatedBy
                }); 

                return data.ToList();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="serviceInfoModel">服务对象</param>
        /// <returns></returns>
        public bool Delete(ServiceInfo serviceInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<ServiceInfo>(serviceInfoModel).State = EntityState.Deleted;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="serviceInfoModel">数据节点对象</param>
        /// <returns></returns>
        public bool Add(ServiceInfo serviceInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<ServiceInfo>(serviceInfoModel).State = EntityState.Added;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Change(ServiceInfo serviceInfoModel)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                _context.Entry<ServiceInfo>(serviceInfoModel).State = EntityState.Modified;
                int res = _context.SaveChanges();
                return res >= 0 ? true : false;
            }
        }

        /// <summary>
        /// 查询该数服务点是否存在授权关系
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public bool IsServiceRelations(string serviceID)
        {
            using (_context = SiteManager.Kernel.Get<ModelContext>())
            {
                var query = _context.Set<ServiceRelation>().Where(u => u.ServiceID == serviceID).ToList();
                return query.Any() ? true : false;
            }
        }
        #endregion

    }


}
