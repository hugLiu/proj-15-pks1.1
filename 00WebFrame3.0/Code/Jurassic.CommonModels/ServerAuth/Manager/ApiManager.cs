using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ServerAuth
{
    public class ApiManager
    {

        /// <summary>
        /// 安全令牌数据访问对象
        /// </summary>
        public static ServerAuthManager serverAuthManager;
        public static ServerAuthManager mServerAuthManager
        {
            get
            {
                if (serverAuthManager == null)
                {
                    serverAuthManager = SiteManager.Get<ServerAuthManager>();
                }
                return serverAuthManager;
            }
        }

        /// <summary>
        /// 数据节点业务访问类
        /// </summary>
        public static DataAuthorizeManager dataAuthorizeManager;
        public static DataAuthorizeManager mDataAuthorizeManager
        {
            get
            {
                if (dataAuthorizeManager == null)
                {
                    dataAuthorizeManager = SiteManager.Get<DataAuthorizeManager>();
                }
                return dataAuthorizeManager;
            }
        }

        /// <summary>
        /// 服务节点业务访问类
        /// </summary>
        public static ServiceInfoManager serviceInfoManager;
        public static ServiceInfoManager mServiceInfoManager
        {
            get
            {
                if (serviceInfoManager == null)
                {
                    serviceInfoManager = SiteManager.Get<ServiceInfoManager>();
                }
                return serviceInfoManager;
            }
        }

    }
}
