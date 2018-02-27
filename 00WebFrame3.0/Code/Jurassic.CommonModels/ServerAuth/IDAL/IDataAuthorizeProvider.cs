using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <summary>
    /// 数据权限节点对象
    /// </summary>
    public interface IDataAuthorizeProvider
    {
        #region
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="isvalId">1有效 0无效 null所有</param>
        /// <returns></returns>
        List<DataNodeInfo> GetData(int? isvalid);

        /// <summary>
        /// 通过主键id查询有效的数据对象
        /// </summary>
        /// <param name="toKeyId">授权表主键</param>
        /// <returns></returns>
        List<DataNodeInfo> GetDataById(string dataID);

        /// <summary>
        /// 通过数据节点id查询有效的数据对象
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        List<DataNodeInfo> GetDataByDataNodeID(string dataNodeID);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool Delete(DataNodeInfo dataNodeInfoModel);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool Add(DataNodeInfo dataNodeInfoModel);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool Change(DataNodeInfo dataNodeInfoModel);

        /// <summary>
        /// 验证数据节点id是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        bool VerifyDataNodeRepeat(DataNodeInfo dataNodeInfoModel);

        /// <summary>
        /// 查询该数据节点是否存在授权关系
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        bool IsDataRelations(string dataNodeID);
        #endregion

    }
}
