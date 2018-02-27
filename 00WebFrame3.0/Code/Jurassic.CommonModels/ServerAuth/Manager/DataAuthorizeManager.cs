using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <summary>
    /// 数据节点业务访问类
    /// </summary>
    public class DataAuthorizeManager
    {
        private IDataAuthorizeProvider dataAuthorizeDataProvider;

        public DataAuthorizeManager(IDataAuthorizeProvider mDataAuthorize)
        {
            dataAuthorizeDataProvider = mDataAuthorize;
        }

        #region
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="isvalId">1有效 0无效 null所有</param>
        /// <returns></returns>
        public List<DataNodeInfo> GetData(int? isvalId)
        {
            return dataAuthorizeDataProvider.GetData(isvalId);
        }

        /// <summary>
        /// 通过主键id查询有效的数据对象
        /// </summary>
        /// <param name="toKeyId">授权表主键</param>
        /// <returns></returns>
        public List<DataNodeInfo> GetDataById(string dataID)
        {
            return dataAuthorizeDataProvider.GetDataById(dataID);
        }

        /// <summary>
        /// 通过数据节点id查询有效的数据对象
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public List<DataNodeInfo> GetDataByDataNodeID(string dataNodeID)
        {
            return dataAuthorizeDataProvider.GetDataByDataNodeID(dataNodeID);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Delete(DataNodeInfo dataNodeInfoModel)
        {
            return dataAuthorizeDataProvider.Delete(dataNodeInfoModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Add(DataNodeInfo dataNodeInfoModel)
        {
            return dataAuthorizeDataProvider.Add(dataNodeInfoModel);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Change(DataNodeInfo dataNodeInfoModel)
        {
            return dataAuthorizeDataProvider.Change(dataNodeInfoModel);
        }

        /// <summary>
        /// 验证数据节点id是否重复
        /// 通过主键ID来确定操作是新增还是编辑状态
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool VerifyDataNodeRepeat(DataNodeInfo dataNodeInfoModel)
        {
            return dataAuthorizeDataProvider.VerifyDataNodeRepeat(dataNodeInfoModel);
        }

        /// <summary>
        /// 查询该数据节点是否存在授权关系
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public bool IsDataRelations(string dataNodeID)
        {
            return dataAuthorizeDataProvider.IsDataRelations(dataNodeID);
        }
        #endregion








    }
}
