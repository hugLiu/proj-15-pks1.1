using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <summary>
    /// 数据节点业务访问类
    /// </summary>
    public class ServiceInfoManager
    {
        private IServiceInfoProvider serviceInfoDataProvider;

        public ServiceInfoManager(IServiceInfoProvider mServiceInfo)
        {
            serviceInfoDataProvider = mServiceInfo;
        }

        #region  
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="isvalId">1有效 0无效 null所有</param>
        /// <returns></returns>
        public List<ServiceInfo> GetData(int? isvalId)
        {
            return serviceInfoDataProvider.GetData(isvalId);
        }

        /// <summary>
        /// 通过主键id查询有效的服务对象
        /// </summary>
        /// <param name="serviceID">授权表主键</param>
        /// <returns></returns>
        public List<ServiceInfo> GetDataById(string serviceID)
        {
            return serviceInfoDataProvider.GetDataById(serviceID);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="serviceInfoModel">服务对象</param>
        /// <returns></returns>
        public bool Delete(ServiceInfo serviceInfoModel)
        {
            return serviceInfoDataProvider.Delete(serviceInfoModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="serviceInfoModel">数据节点对象</param>
        /// <returns></returns>
        public bool Add(ServiceInfo serviceInfoModel)
        {
            return serviceInfoDataProvider.Add(serviceInfoModel);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="authTokenModel">数据节点对象</param>
        /// <returns></returns>
        public bool Change(ServiceInfo serviceInfoModel)
        {
            return serviceInfoDataProvider.Change(serviceInfoModel);
        }

        /// <summary>
        /// 查询该数服务点是否存在授权关系
        /// </summary>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public bool IsServiceRelations(string serviceID)
        {
            return serviceInfoDataProvider.IsServiceRelations(serviceID);
        }


        /// <summary>
        /// 获取客户id与访问的方法所授权的对象
        /// </summary>
        /// <param name="serviceFunctionName">访问的请求方法</param>
        /// <param name="serviceFullName">访问的请求全称(命名空间名称)</param>
        /// <returns></returns>
        public List<ServiceInfo> GetServiceInfo(string serviceFunctionName, string serviceFullName)
        {
            List<ServiceInfo> serviceAuthList = GetDataList();
            var q = from m in serviceAuthList
                    where m.ServiceFullName.ToStr().Equals(serviceFullName, StringComparison.OrdinalIgnoreCase) 
                    && m.ServiceFunctionName.ToStr().Equals(serviceFunctionName, StringComparison.OrdinalIgnoreCase) 
                    select m;
            return q.ToList();
        }
         
        /// <summary>
        /// 查询获取服务信息数据到本地
        /// </summary>
        /// <returns></returns>
        public List<ServiceInfo> GetDataList()
        {
            if (ServiceInfoList != null && ServiceInfoList.Any())
            {
                return ServiceInfoList;
            }
            ServiceInfoList = serviceInfoDataProvider.GetData(1);
            return ServiceInfoList;
        }

        public List<ServiceInfo> ServiceInfoList
        {
            get;
            set;
        }
        
        #endregion








    }
}
