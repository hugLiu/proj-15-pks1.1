using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <summary>
    /// 安全令牌数据访问对象
    /// </summary>
    public class ServerAuthManager
    {
        private IServerAuthProvider serverAuthProvider;

        public ServerAuthManager(IServerAuthProvider mServerAuth)
        {
            serverAuthProvider = mServerAuth;
        }

        #region 用户组授权
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<AuthToken> GetData()
        {
            return serverAuthProvider.GetData();
        }

        /// <summary>
        /// 通过主键id查询数据对象
        /// </summary>
        /// <param name="toKeyId">授权表主键</param>
        /// <returns></returns>
        public List<AuthToken> GetDataById(string toKeyId)
        {
            return serverAuthProvider.GetDataById(toKeyId);
        }

        /// <summary>
        /// 通过客户id查询数据对象
        /// </summary>
        /// <param name="clientId">客户id</param>
        /// <returns></returns>
        public List<AuthToken> GetDataByClientId(string clientId)
        {
            return serverAuthProvider.GetDataByClientId(clientId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool Delete(AuthToken authTokenModel)
        {
            return serverAuthProvider.Delete(authTokenModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool Add(AuthToken authTokenModel)
        {
            return serverAuthProvider.Add(authTokenModel);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool Change(AuthToken authTokenModel)
        {
            return serverAuthProvider.Change(authTokenModel);
        }

        /// <summary>
        /// 验证客户id与授权key是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        public bool VerifyClientRepeat(AuthToken authTokenModel)
        {
            return serverAuthProvider.VerifyClientRepeat(authTokenModel);
        }
        #endregion

        #region 数据授权
        /// <summary>
        /// 查询数据制定客户的数据授权关系
        /// </summary>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        public List<DataRelation> GetDataRelation(string tokeyID)
        {
            return serverAuthProvider.GetDataRelation(tokeyID);
        }

        /// <summary>
        /// 保存数据授权关系
        /// </summary>
        /// <param name="dataRelationList">关系集合</param>
        /// <returns></returns>
        public bool SaveDataRelation(List<DataRelation> dataRelationList, string tokeyID)
        {
            return serverAuthProvider.SaveDataRelation(dataRelationList, tokeyID);
        }

        /// <summary>
        /// 通过客户组Id查询该客户授权的数据节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public List<ViDataAuth> GetDataNodesByClientId(string clientId)
        {
            return serverAuthProvider.GetDataNodesByClientId(clientId);
        }

        /// <summary>
        /// 获取该客户的数据节点是否授权
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public bool IsAuthData(string clientId, string dataNodeID)
        {
            return serverAuthProvider.IsAuthData(clientId, dataNodeID);
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
            return serverAuthProvider.GetServiceRelation(tokeyID);
        }

        /// <summary>
        /// 保存服务授权关系
        /// </summary>
        /// <param name="dataRelationList">关系集合</param>
        /// <returns></returns>
        public bool SaveServiceRelation(List<ServiceRelation> serviceRelationList, string tokeyID)
        {
            return serverAuthProvider.SaveServiceRelation(serviceRelationList, tokeyID);
        }

        /// <summary>
        /// 通过客户组Id查询该客户授权的服务节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public List<ViServiceAuth> GetServiceByClientId(string clientId)
        {
            return serverAuthProvider.GetServiceByClientId(clientId);
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
            return serverAuthProvider.IsAuthService(clientId, actionName, serviceFullName);
        }
        
        /// <summary>
        /// 获取客户id与访问的方法所授权的对象
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="serviceFullName">客户组编码ID</param>
        /// <returns></returns>
        public bool GetAuthService(string clientId, string actionName, string serviceFullName)
        {
            List<ViServiceAuth> serviceAuthList = GetAllAuthService();
            var q = from m in serviceAuthList
                    where m.ClientId.ToStr().Equals(clientId, StringComparison.OrdinalIgnoreCase)
                    && m.ServiceFullName.ToStr().Equals(serviceFullName, StringComparison.OrdinalIgnoreCase)
                    && m.ServiceFunctionName.ToStr().Equals(actionName, StringComparison.OrdinalIgnoreCase)
                    select m;

            return q.Any() ? true : false;
        }

        /// <summary>
        /// 获取所有的授权服务关系
        /// 判断是否本地有数据如果没有将重新获取最新数据
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public List<ViServiceAuth> GetAllAuthService()
        {
            if (ServiceAuthList != null && ServiceAuthList.Any())
            {
                return ServiceAuthList;
            }
            ServiceAuthList = serverAuthProvider.GetAllAuthService();
            return ServiceAuthList;
        }
        

        public List<ViServiceAuth> ServiceAuthList
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 验证客户组与授权Key是否合法
        /// </summary>
        /// <param name="clientId">用户组ID</param>
        /// <param name="clientSecret">授权Key</param>
        /// <returns></returns>
        public bool IsServerAuthValid(string clientId, string clientSecret)
        {
            //通过客户端请求的客户id获取相关授权授权信息(客户组id,安全令牌ToKen)
            List<AuthToken> list = GetDataByClientId(clientId);
            if (list.Any())
            {
                #region 验证是否属于合法的授权对象(以及是否过期)
                var q = from c in list where c.ClientId == clientId && c.TokeyCode == clientSecret select c;
                if (q.Any())
                {
                    if (q.ToList()[0].ValidityDate != null)
                    {
                        DateTime nowDate = DateTime.Now;
                        bool isDate = DateTime.TryParse(q.ToList()[0].ValidityDate.ToString(), out nowDate);
                        int isValidity = DateTime.Compare(nowDate, DateTime.Now);
                        if (isValidity >= 0)
                        {
                            return true;
                        }
                    }
                }
                #endregion
            }
            return false;
        }
    }
}
