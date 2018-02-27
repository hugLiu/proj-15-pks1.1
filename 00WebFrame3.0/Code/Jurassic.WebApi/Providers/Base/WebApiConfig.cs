using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebApi.Providers
{
    /// <summary>
    /// WebApi服务配置文件
    /// </summary>
    public class WebApiProvidersConfig
    {
        /// <summary>
        /// RefreshToken(刷新key)生命有效期
        /// (刷新Key主要作用是对服务发布的ToKen安全令牌过期后的重置,并生成新的刷新Key与安全令牌Token)
        /// 单位:分钟
        /// </summary>
        public static double RefreshTokenLifeTime = 60;

        /// <summary>
        /// 安全令牌的有效期
        /// 单位:分钟
        /// </summary>
        public static double TokenLifeTime = 60;




    }
}