using Jurassic.AppCenter;
using Jurassic.CommonModels.EFProvider;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.ServerAuth;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using Jurassic.AppCenter.Logs;

namespace Jurassic.WebApi.Providers
{
    /// <summary>
    /// WebApi服务授权信息验证
    /// </summary>
    public class WebApiServerOAuthProvider : OAuthAuthorizationServerProvider
    {

        /// <summary>
        /// 验证客户端身份
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            //获取Headers包含的验证信息
            context.TryGetBasicCredentials(out clientId, out clientSecret);

            context.Validated(clientId );
            context.OwinContext.Set<string>("clientId", clientId);
            context.OwinContext.Set<string>("clientSecret", clientSecret);
           
            return base.ValidateClientAuthentication(context);
        }

        /// <summary>
        /// 对客户端进行授权
        /// 客户端访问方式必须是"grant_type", "client_credentials"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var clientId = context.OwinContext.Get<string>("clientId");
            var clientSecret = context.OwinContext.Get<string>("clientSecret");
            //验证是授权信息是否正确,并发放授权信息
            bool isAuth = ApiManager.mServerAuthManager.IsServerAuthValid(clientId, clientSecret);
            if (isAuth)
            {
                var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                //创建授权对象,并记录到HttpContext.Current.User.Identity
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.ClientId ));
                //授权安全令牌后,记录当前授权用户id 到会话对象中
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:clientId", context.ClientId },
                    { "as:clientSecret", clientSecret }
                });
                //发布授权
                var ticket = new AuthenticationTicket(oAuthIdentity, props);
                context.Validated(ticket);
            }
            //return Task.FromResult<object>(null);
            return base.GrantClientCredentials(context);
        }

        /// <summary>
        /// (目前还未使用该部分)
        /// 对具体用户账号与密码进行授权
        /// 客户端访问方式必须是"grant_type" = "password"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);

            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.ClientId));
            //授权安全令牌后,记录当前授权用户id 到安全令牌对象中
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:clientId", context.ClientId }
                });
            //
            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
            //return Task.FromResult<object>(null);
            return base.GrantResourceOwnerCredentials(context);
        }

        /// <summary>
        /// 刷新安全令牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override  Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //当前服务器对应票据中的客户对象名称
            var originalClient = context.Ticket.Properties.Dictionary["as:clientId"];
            //当前请求的客户对象名称
            var currentClient = context.ClientId;
            //判断请求刷新key的客户id是否来自安全令牌对象的客户id
            if (originalClient != currentClient)
            {
                context.Rejected();
                return base.GrantRefreshToken(context); 
            }

            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));
            //
            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
            return base.GrantRefreshToken(context);
        }

        /// <summary>
        /// 添加安全令牌相关信息到客户响应请求中
        /// (返回信息到客户端)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            if (context.Properties.IssuedUtc != null)
            {
                //安全令牌发布日期.
                DateTimeOffset Issued = new DateTimeOffset();
                bool flag = DateTimeOffset.TryParse(context.Properties.IssuedUtc.ToString(), out Issued);
                if (flag)
                    context.AdditionalResponseParameters.Add("issued", Issued.LocalDateTime);
            }

            if (context.Properties.ExpiresUtc != null)
            {
                //安全令牌有效期截止日期
                DateTimeOffset expires = new DateTimeOffset();
                bool flag = DateTimeOffset.TryParse(context.Properties.ExpiresUtc.ToString(), out expires);
                if (flag)
                    context.AdditionalResponseParameters.Add("expires", expires.LocalDateTime);
            }

            return Task.FromResult<object>(null);
        }
























       




    }
}