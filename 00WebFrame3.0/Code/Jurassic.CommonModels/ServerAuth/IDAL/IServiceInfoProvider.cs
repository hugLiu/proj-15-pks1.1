using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <summary>
    /// 控制器Api服务授权对象
    /// </summary>
    public interface IServiceInfoProvider
    {
        #region
        /// <summary>
        /// 获取服务信息
        /// </summary>
        /// <param name="isvalId">1有效 0无效 null所有</param>
        /// <returns></returns>
        List<ServiceInfo> GetData(int? isvalId);

        /// <summary>
        /// 通过主键id查询有效的服务对象
        /// </summary>
        /// <param name="serviceID">授权表主键</param>
        /// <returns></returns>
        List<ServiceInfo> GetDataById(string serviceID);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="serviceInfoModel">服务对象</param>
        /// <returns></returns>
        bool Delete(ServiceInfo serviceInfoModel);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="serviceInfoModel">数据节点对象</param>
        /// <returns></returns>
        bool Add(ServiceInfo serviceInfoModel);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool Change(ServiceInfo serviceInfoModel);

        /// <summary>
        /// 查询该数服务点是否存在授权关系
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        bool IsServiceRelations(string serviceID);
        #endregion

    }
}
