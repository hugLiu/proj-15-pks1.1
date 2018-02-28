using System.Collections.Generic;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using CacheManager.Core;
using System;

namespace PKS.WebAPI.Services
{
    /// <summary>安全服务实现包装器</summary>
    public class SecurityServiceWrapper : ApiServiceWrapper, ISecurityService, ISecurityServiceWrapper, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public SecurityServiceWrapper(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>构造函数</summary>
        public SecurityServiceWrapper(IApiServiceConfig config) : base(config, nameof(ISecurityService).Substring(1))
        {
        }
        /// <summary>获得Token过期参数</summary>
        public TokenExpireSettings GetTokenExpireSettings()
        {
            var result = this.Cacher?.Get<TokenExpireSettings>(CacheConst.TokenExpireSettings, CacheConst.AuthenticationRegion);
            if (result == null)
            {
                result = this.MemcachedCacher?.TryGet<TokenExpireSettings>(CacheConst.TokenExpireSettings, CacheConst.AuthenticationRegion);
                if (result == null)
                {
                    var client = InitHttpClient();
                    result = client.Get<TokenExpireSettings>(GetActionUrl(nameof(GetTokenExpireSettings)));
                }
                var cacheItem = new CacheItem<object>(CacheConst.TokenExpireSettings, CacheConst.AuthenticationRegion, result, ExpirationMode.Absolute, TimeSpan.FromHours(1));
                this.Cacher?.Put(cacheItem);
            }
            return result;
        }
        /// <summary>登录</summary>
        public LoginResult Login(LoginRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => LoginAsyncInternal(client, request)).Result;
        }

        /// <summary>登录</summary>
        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            var client = InitHttpClient();
            return await LoginAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>登录</summary>
        public async Task<LoginResult> LoginAsyncInternal(HttpClientWrapper client, LoginRequest request)
        {
            return await client.PostObjectAsync<LoginResult>(GetActionUrl(nameof(Login)), request).ConfigureAwait(false);
        }
        /// <summary>续期</summary>
        public LoginResult Renew(string token)
        {
            var client = InitHttpClient();
            return Task.Run(() => RenewAsyncInternal(client, token)).Result;
        }
        /// <summary>续期</summary>
        public async Task<LoginResult> RenewAsync(string token)
        {
            var client = InitHttpClient();
            return await RenewAsyncInternal(client, token).ConfigureAwait(false);
        }
        /// <summary>续期</summary>
        public async Task<LoginResult> RenewAsyncInternal(HttpClientWrapper client, string token)
        {
            var url = GetActionUrl(nameof(Renew)) + nameof(token).GetFirstQueryString(token);
            return await client.GetAsync<LoginResult>(url);
        }
        /// <summary>获取登录用户信息</summary>
        public IPKSPrincipal GetPrincipal(string token)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetPrincipalAsyncInternal(client, token)).Result;
        }

        /// <summary>获取登录用户信息</summary>
        public async Task<IPKSPrincipal> GetPrincipalAsync(string token)
        {
            var client = InitHttpClient();
            return await GetPrincipalAsyncInternal(client, token).ConfigureAwait(false);
        }
        /// <summary>获取登录用户信息</summary>
        public async Task<IPKSPrincipal> GetPrincipalAsyncInternal(HttpClientWrapper client, string token)
        {
            var result = this.MemcachedCacher?.TryGet<IPKSPrincipal>(token, CacheConst.AuthenticationRegion);
            if (result == null)
            {
                var url = GetActionUrl(nameof(GetPrincipal)) + nameof(token).GetFirstQueryString(token);
                return await client.GetAsync<IPKSPrincipal>(url).ConfigureAwait(false);
            }
            return result;
        }
        /// <summary>注销用户</summary>
        public void Logout(string token)
        {
            var client = InitHttpClient();
            Task.Run(() => LogoutAsyncInternal(client, token));
        }

        /// <summary>注销用户</summary>
        public async Task LogoutAsync(string token)
        {
            var client = InitHttpClient();
            await LogoutAsyncInternal(client, token).ConfigureAwait(false);
        }
        /// <summary>注销用户</summary>
        public async Task LogoutAsyncInternal(HttpClientWrapper client, string token)
        {
            await client.GetAsync(GetActionUrl(nameof(Logout))).ConfigureAwait(false);
        }
        /// <summary>获得指定角色的门户菜单</summary>
        public PortalMenu GetPortalMenu(int roleId)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetPortalMenuAsyncInternal(client, roleId)).Result;
        }

        /// <summary>获得指定角色的门户菜单</summary>
        public async Task<PortalMenu> GetPortalMenuAsync(int roleId)
        {
            var client = InitHttpClient();
            return await GetPortalMenuAsyncInternal(client, roleId).ConfigureAwait(false);
        }
        /// <summary>获得指定角色的门户菜单</summary>
        public async Task<PortalMenu> GetPortalMenuAsyncInternal(HttpClientWrapper client, int roleId)
        {
            var key = CacheConst.RoleKey + roleId.ToString();
            var result = this.MemcachedCacher?.TryGet<PortalMenu>(key, CacheConst.PermissionRegion);
            if (result == null)
            {
                var url = GetActionUrl(nameof(GetPortalMenu)) + nameof(roleId).GetFirstQueryString(roleId.ToString());
                result = await client.GetAsync<PortalMenu>(url).ConfigureAwait(false);
            }
            return result;
        }

        /// <summary>获得指定用户的权限集合</summary>
        public Dictionary<string, bool> GetPermissions(int userId)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetPermissionsAsyncInternal(client, userId)).Result;
        }
        /// <summary>获得指定用户的权限集合</summary>
        public async Task<Dictionary<string, bool>> GetPermissionsAsync(int userId)
        {
            var client = InitHttpClient();
            return await GetPermissionsAsyncInternal(client, userId).ConfigureAwait(false);
        }
        /// <summary>获得指定用户的权限集合</summary>
        public async Task<Dictionary<string, bool>> GetPermissionsAsyncInternal(HttpClientWrapper client, int userId)
        {
            var key = CacheConst.UserKey + userId.ToString();
            var result = this.MemcachedCacher?.TryGet<Dictionary<string, bool>>(key, CacheConst.PermissionRegion);
            if (result == null)
            {
                var url = GetActionUrl(nameof(GetPermissions)) + nameof(userId).GetFirstQueryString(userId.ToString());
                result = await client.GetAsync<Dictionary<string, bool>>(url).ConfigureAwait(false);
            }
            return result;
        }
        /// <summary>判断指定用户对指定Url是否有权限</summary>
        public bool HasMenuPermission(int userId, string url)
        {
            var client = InitHttpClient();
            return Task.Run(() => HasMenuPermissionAsyncInternal(client, userId, url)).Result;
        }
        /// <summary>判断指定用户对指定Url是否有权限</summary>
        public async Task<bool> HasMenuPermissionAsync(int userId, string url)
        {
            var client = InitHttpClient();
            return await HasMenuPermissionAsyncInternal(client, userId, url).ConfigureAwait(false);
        }
        /// <summary>判断指定用户对指定Url是否有权限</summary>
        public async Task<bool> HasMenuPermissionAsyncInternal(HttpClientWrapper client, int userId, string url)
        {
            var permissions = await GetPermissionsAsyncInternal(client, userId).ConfigureAwait(false);
            var url2 = url.RemoveQueryString().TrimEnd('/').ToLowerInvariant();
            return permissions.GetValueOrDefaultBy(url2, false);
        }
        /// <summary>获得门户底部菜单</summary>
        public PortalFooterMenu GetPortalFooterMenu()
        {
            var client = InitHttpClient();
            return Task.Run(() => GetPortalFooterMenuAsyncInternal(client)).Result;
        }
        /// <summary>获得门户底部菜单</summary>
        public async Task<PortalFooterMenu> GetPortalFooterMenuAsync()
        {
            var client = InitHttpClient();
            return await GetPortalFooterMenuAsyncInternal(client).ConfigureAwait(false);
        }
        /// <summary>获得门户底部菜单</summary>
        private async Task<PortalFooterMenu> GetPortalFooterMenuAsyncInternal(HttpClientWrapper client)
        {
            return await client.GetAsync<PortalFooterMenu>(GetActionUrl(nameof(GetPortalFooterMenu))).ConfigureAwait(false);
        }
    }
}