using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Jurassic.WebApi.Providers;

//using WebApplication2.Models;

namespace Jurassic.WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            // 针对基于 OAuth 的流配置应用程序
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                //AuthorizeEndpointPath = new PathString("/api/apidemo"),

                TokenEndpointPath = new PathString("/Token"),
                
                //注册授权验证业务类实现 
                Provider = new WebApiServerOAuthProvider(),

                //注册安全令牌创建于刷新业务类实现
                RefreshTokenProvider = new WebApiTokenProvider(),

                //token有效期时间
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(WebApiProvidersConfig.TokenLifeTime),

                //在生产模式下设 AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // 使应用程序可以使用不记名令牌来验证用户身份
            app.UseOAuthBearerTokens(OAuthOptions);
        }


    }
}
