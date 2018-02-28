using System.Security.Principal;
using System.Threading.Tasks;
using PKS.WebAPI.Models;
using System.Collections.Generic;
using PKS.Models;
using System;

namespace PKS.WebAPI.Services
{
    /// <summary>安全服务包装器接口</summary>
    public interface ISecurityServiceWrapper : ISecurityService, IApiServiceWrapper
    {
        /// <summary>判断指定用户对指定Url是否有权限</summary>
        bool HasMenuPermission(int userId, string url);
        /// <summary>判断指定用户对指定Url是否有权限</summary>
        Task<bool> HasMenuPermissionAsync(int userId, string url);
    }

    /// <summary>安全服务接口</summary>
    public interface ISecurityService
    {
        /// <summary>获得Token过期参数</summary>
        TokenExpireSettings GetTokenExpireSettings();
        /// <summary>登录</summary>
        LoginResult Login(LoginRequest request);

        /// <summary>登录</summary>
        Task<LoginResult> LoginAsync(LoginRequest request);

        /// <summary>续期</summary>
        LoginResult Renew(string token);
        /// <summary>续期</summary>
        Task<LoginResult> RenewAsync(string token);
        /// <summary>获取登录用户信息</summary>
        IPKSPrincipal GetPrincipal(string token);

        /// <summary>获取登录用户信息</summary>
        Task<IPKSPrincipal> GetPrincipalAsync(string token);

        /// <summary>注销用户</summary>
        void Logout(string token);

        /// <summary>注销用户</summary>
        Task LogoutAsync(string token);

        /// <summary>获得指定角色的门户菜单</summary>
        PortalMenu GetPortalMenu(int roleId);

        /// <summary>获得指定角色的门户菜单</summary>
        Task<PortalMenu> GetPortalMenuAsync(int roleId);
        /// <summary>获得门户底部菜单</summary>
        PortalFooterMenu GetPortalFooterMenu();
        /// <summary>获得门户底部菜单</summary>
        Task<PortalFooterMenu> GetPortalFooterMenuAsync();
        /// <summary>获得指定用户的权限集合</summary>
        Dictionary<string, bool> GetPermissions(int userId);
        /// <summary>获得指定用户的权限集合</summary>
        Task<Dictionary<string, bool>> GetPermissionsAsync(int userId);
    }
}