using Jurassic.CommonModels.ServerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace Jurassic.WebApi.Providers
{
    /// <summary>
    /// WebApi服务配置文件
    /// </summary>
    //该配置等效于在WebApiConfig所添加的方法注册所有服务都允许跨域处理EnableCrossSiteRequests
    //[System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// 获取该客户组的数据节点是否授权
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns>返回数据授权的结果 允许访问=true  不允许访问=false</returns>
        public virtual bool IsAuthData(string dataNodeID)
        {
            IIdentity identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated || string.IsNullOrEmpty(identity.Name))
            {
                return false;
            }
            return ApiManager.mServerAuthManager.IsAuthData(identity.Name, dataNodeID);
        }




    }
}