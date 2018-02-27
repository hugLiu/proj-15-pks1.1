using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <summary>
    /// 安全令牌数据访问对象
    /// </summary>
    public interface IServerAuthProvider
    {
        #region 用户组授权
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        IQueryable<AuthToken> GetData();

        /// <summary>
        /// 通过主键id查询数据对象
        /// </summary>
        /// <param name="toKeyId">授权表主键</param>
        /// <returns></returns>
        List<AuthToken> GetDataById(string toKeyId);

        /// <summary>
        /// 通过客户id查询数据对象
        /// </summary>
        /// <param name="clientId">客户id</param>
        /// <returns></returns>
        List<AuthToken> GetDataByClientId(string clientId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        bool Delete(AuthToken authTokenModel);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        bool Add(AuthToken authTokenModel);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        bool Change(AuthToken authTokenModel);

        /// <summary>
        /// 验证客户id与授权key是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="authTokenModel"></param>
        /// <returns></returns>
        bool VerifyClientRepeat(AuthToken authTokenModel);
        #endregion

        #region 数据授权
        /// <summary>
        /// 查询数据制定客户的数据授权关系
        /// </summary>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        List<DataRelation> GetDataRelation(string tokeyID);

        /// <summary>
        /// 保存数据授权关系
        /// </summary>
        /// <param name="dataRelationList">关系集合</param>
        /// <returns></returns>
        bool SaveDataRelation(List<DataRelation> dataRelationList, string tokeyID);

        /// <summary>
        /// 通过客户组Id查询该客户授权的数据节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        List<ViDataAuth> GetDataNodesByClientId(string clientId);

        /// <summary>
        /// 获取该客户的数据节点是否授权
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        bool IsAuthData(string clientId, string dataNodeID);

        #endregion

        #region 服务授权
        /// <summary>
        /// 查询指定客户的服务授权关系
        /// </summary>
        /// <param name="tokeyID">授权主键ID</param>
        /// <returns></returns>
        List<ServiceRelation> GetServiceRelation(string tokeyID);

        /// <summary>
        /// 保存服务授权关系
        /// </summary>
        /// <param name="dataRelationList">关系集合</param>
        /// <returns></returns>
        bool SaveServiceRelation(List<ServiceRelation> serviceRelationList, string tokeyID);

        /// <summary>
        /// 通过客户组Id查询该客户授权的服务节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        List<ViServiceAuth> GetServiceByClientId(string clientId);

        /// <summary>
        /// 获取该客户的服务节点是否授权
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="actionName">服务名称</param>
        /// <param name="serviceFullName">服务完成方法名称(包含命名空间名称)</param>
        /// <returns></returns>
        bool IsAuthService(string clientId, string actionName, string serviceFullName);

        /// <summary>
        /// 获取所有的授权服务关系
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        List<ViServiceAuth> GetAllAuthService();
        #endregion

    }
}
