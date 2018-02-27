using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Jurassic.WebApi.Providers
{
    /// <summary>
    /// 创建安全令牌过程重构
    /// </summary>
    public class WebApiTokenProvider : AuthenticationTokenProvider
    {
        public WebApiTokenProvider()
        {   }

        /// <summary>
        /// 线程集合
        /// 记录用作刷新安全令牌的刷新key标识
        /// </summary>
        private static ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 创建安全令牌以及刷新key
        /// </summary>
        /// <param name="context"></param>
        public override void Create(AuthenticationTokenCreateContext context)
        {
            if (string.IsNullOrEmpty(context.Ticket.Identity.Name))
                return;

            var clientId = context.OwinContext.Get<string>("clientId");
            if (string.IsNullOrEmpty(clientId))
                return;

            //生成刷新用的key令牌
            RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[60];
            cryptoRandomDataGenerator.GetBytes(buffer);
            var refreshTokenId = Convert.ToBase64String(buffer).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            //设置刷新key的有效时间,单位:分钟
            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(WebApiProvidersConfig.RefreshTokenLifeTime);
            //保存到线程集合
            _refreshTokens[refreshTokenId] = context.SerializeTicket();
            //设置刷新key标识
            context.SetToken(refreshTokenId);
        }

        /// <summary>
        /// 发送刷新的token
        /// </summary>
        /// <param name="context"></param>
        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (_refreshTokens.TryRemove(context.Token, out value))
            {
                context.DeserializeTicket(value);
            }
        }


    }

}