using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CacheManager.Core;
using PKS.Core;
using PKS.DbServices;
using PKS.DbServices.SysFrame;
using PKS.Models;
using PKS.WebAPI.Models;
using System.Configuration;
using PKS.Utils;

namespace PKS.WebAPI.Services
{
    /// <summary>安全服务实现</summary>
    public class SecurityService : AppService, ISecurityService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public SecurityService(ICacheProvider provider)
        {
            this.TokenExpireSettings = TokenExpireSettings.Parse(ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_TokenExpireSettings]);
            provider.ExternalCacher.TryPut(CacheConst.TokenExpireSettings, this.TokenExpireSettings, CacheConst.AuthenticationRegion);
        }
        /// <summary>Token过期参数</summary>
        private TokenExpireSettings TokenExpireSettings { get; set; }
        /// <summary>获得Token过期参数</summary>
        public TokenExpireSettings GetTokenExpireSettings()
        {
            this.MemcachedCacher.TryPut(CacheConst.TokenExpireSettings, this.TokenExpireSettings, CacheConst.AuthenticationRegion);
            return this.TokenExpireSettings;
        }
        /// <summary>登录</summary>
        public LoginResult Login(LoginRequest request)
        {
            return Task.Run(() => LoginAsync(request)).Result;
        }

        /// <summary>登录</summary>
        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            var tokenExpireInterval = this.TokenExpireSettings.ExpireInterval;
            var result = await GetService<IdentityService>().LoginAsync(request, tokenExpireInterval);
            if (result.Succeed)
            {
                var cacheItem = new CacheItem<object>(result.Token, CacheConst.AuthenticationRegion, result.Principal, ExpirationMode.Absolute, tokenExpireInterval);
                this.MemcachedCacher.TryPut(cacheItem);
            }
            return result;
        }
        /// <summary>续期</summary>
        public LoginResult Renew(string token)
        {
            return Task.Run(() => RenewAsync(token)).Result;
        }
        /// <summary>续期</summary>
        public async Task<LoginResult> RenewAsync(string token)
        {
            if (token.IsNullOrEmpty()) return null;
            var principal = GetPrincipal(token);
            if (principal == null) return null;
            var tokenExpireInterval = this.TokenExpireSettings.ExpireInterval;
            var result = await GetService<IdentityService>().RenewAsync(token, principal, tokenExpireInterval);
            if (result != null && result.Succeed)
            {
                principal.As<PKSPrincipal>().NewToken = token;
                var cacheItem = new CacheItem<object>(token, CacheConst.AuthenticationRegion, principal, ExpirationMode.Absolute, tokenExpireInterval);
                this.MemcachedCacher.TryPut(cacheItem);
                cacheItem = new CacheItem<object>(result.Token, CacheConst.AuthenticationRegion, result.Principal, ExpirationMode.Absolute, tokenExpireInterval);
                this.MemcachedCacher.TryPut(cacheItem);
            }
            return result;
        }
        /// <summary>获取登录用户信息</summary>
        public IPKSPrincipal GetPrincipal(string token)
        {
            return this.MemcachedCacher.TryGetOrAddValue<IPKSPrincipal>(token, CacheConst.AuthenticationRegion, GetCacheItem_Principal);
        }
        /// <summary>获取登录用户信息</summary>
        public Task<IPKSPrincipal> GetPrincipalAsync(string token)
        {
            return Task.FromResult(GetPrincipal(token));
        }
        /// <summary>获得缓存项_登录用户信息</summary>
        private CacheItem<object> GetCacheItem_Principal(string token, string region)
        {
            var tokenExpireInterval = this.TokenExpireSettings.ExpireInterval;
            var principal = GetPrincipalFromDb(token);
            if (principal == null) return null;
            return new CacheItem<object>(token, region, principal, ExpirationMode.Absolute, tokenExpireInterval);
        }
        /// <summary>从数据库获取登录用户信息</summary>
        private IPKSPrincipal GetPrincipalFromDb(string token)
        {
            return Task.Run(() => GetService<IdentityService>().GetPrincipalAsync(token)).Result;
        }
        /// <summary>注销用户</summary>
        public void Logout(string token)
        {
            Task.Run(() => LogoutAsync(token));
        }

        /// <summary>注销用户</summary>
        public async Task LogoutAsync(string token)
        {
            await GetService<IdentityService>().LogoutAsync(token);
            this.MemcachedCacher.TryRemove(token, CacheConst.AuthenticationRegion);
        }
        /// <summary>获得指定角色的门户菜单</summary>
        public PortalMenu GetPortalMenu(int roleId)
        {
            return Task.Run(() => GetPortalMenuAsync(roleId)).Result;
        }

        /// <summary>获得指定角色的门户菜单</summary>
        public async Task<PortalMenu> GetPortalMenuAsync(int roleId)
        {
            return await GetService<RolePermissionsService>().GetPortalMenuAsync(roleId);
        }
        /// <summary>获得指定用户的权限集合</summary>
        public Dictionary<string, bool> GetPermissions(int userId)
        {
            return Task.Run(() => GetPermissionsAsync(userId)).Result;
        }
        /// <summary>获得指定用户的权限集合</summary>
        public async Task<Dictionary<string, bool>> GetPermissionsAsync(int userId)
        {
            return await GetService<RolePermissionsService>().GetPermissionsAsync(userId);
        }
        /// <summary>获得门户底部菜单</summary>
        public PortalFooterMenu GetPortalFooterMenu()
        {
            return GetService<RolePermissionsService>().GetPortalFooterMenu();
        }
        /// <summary>获得门户底部菜单</summary>
        public async Task<PortalFooterMenu> GetPortalFooterMenuAsync()
        {
            return await Task.Run(() => GetPortalFooterMenu()).ConfigureAwait(false);
        }
    }
}