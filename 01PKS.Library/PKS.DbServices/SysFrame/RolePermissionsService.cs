using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using EventBus;
using Ninject;
using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DBModels;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;

namespace PKS.DbServices
{
    /// <summary>角色权限服务</summary>
    public class RolePermissionsService : AppService, IInitializable
    {
        /// <summary>初始化</summary>
        void IInitializable.Initialize()
        {
            this.EventBus.Register(this);
        }
        /// <summary>获得指定角色的门户菜单</summary>
        public async Task<PortalMenu> GetPortalMenuAsync(int roleId)
        {
            return await Task.Run(() => GetPortalMenuFromCache(roleId)).ConfigureAwait(false);
        }
        /// <summary>获得指定角色的门户菜单</summary>
        private PortalMenu GetPortalMenuFromCache(int roleId)
        {
            string key = CacheConst.RoleKey + roleId.ToString();
            return this.MemcachedCacher.TryGetOrAddValue<PortalMenu>(key, CacheConst.PermissionRegion, GetCahceItem_PortalMenu);
        }
        /// <summary>获得缓存项_指定角色的门户菜单</summary>
        private CacheItem<object> GetCahceItem_PortalMenu(string key, string region)
        {
            var roleId = key.Substring(CacheConst.RoleKey.Length).ToInt32();
            var value = GetPortalMenu(roleId);
            return new CacheItem<object>(key, region, value, ExpirationMode.Sliding, TimeSpan.FromHours(1));
        }
        /// <summary>获得指定角色的门户菜单</summary>
        private PortalMenu GetPortalMenu(int roleId)
        {
            //var permissions = GetService<IRepository<PKS_PERMISSION>>().GetQuery().Where(p=>p.PermissionTypeId==1).ToDictionary(e => e.Id);
            var systems = GetService<IRepository<PKS_SUBSYSTEM>>().GetAll().ToDictionary(e => e.Id);
            var rolePermissions = GetService<IRepository<PKS_ROLE_PERMISSION>>()
                .GetQuery().Include(e => e.Permission)
                .Where(r => r.RoleId == roleId && r.Permission.PermissionTypeId == 1)
                .ToList();
            foreach (var rolePermission in rolePermissions)
            {
                rolePermission.Permission.SubSystem = systems[rolePermission.Permission.SubSystemId];
            }
            rolePermissions = rolePermissions.OrderBy(e => e.Permission.LevelNumber).ThenBy(e => e.Permission.OrderNumber).ToList();
            var rootPermissions = rolePermissions.Where(e => e.Permission.LevelNumber == 1).ToArray();
            var rootNodes = rootPermissions.Select(e => BuildMenuInfo(e, rolePermissions)).ToList();
            foreach (var rootNode in rootNodes)
            {
                rootNode.HasThird = rootNode.Children.Any(e => e.Children.Count > 0);
            }
            var result = new PortalMenu();
            result.Menus = rootNodes;
            var defaultPermission = rolePermissions.FirstOrDefault(e => e.IsDefault == 1);
            if (defaultPermission != null)
            {
                result.DefaultUrl = GetMenuUrl(defaultPermission.Permission);
            }
            else
            {
                result.DefaultUrl = rootNodes.FirstOrDefault(e => e.URL.Length > 0)?.URL;
            }
            return result;
        }
        /// <summary>生成菜单节点</summary>
        private MenuInfo BuildMenuInfo(PKS_ROLE_PERMISSION rolePermission, List<PKS_ROLE_PERMISSION> rolePermissions)
        {
            var children = rolePermissions.Where(e => (int)e.Permission.ParentId.GetValueOrDefault() == rolePermission.PermissionId).ToArray();
            return new MenuInfo
            {
                Id = rolePermission.PermissionId,
                Key = rolePermission.Permission.Code,
                Name = rolePermission.Permission.Title,
                URL = GetMenuUrl(rolePermission.Permission),
                Target = GetUrlTarget(rolePermission.Permission),
                Children = children.Select(child => BuildMenuInfo(child, rolePermissions)).ToList()
            };
        }
        /// <summary>获取菜单的url</summary>
        private string GetMenuUrl(PKS_PERMISSION p)
        {
            if (p.Url.IsNullOrEmpty()) return string.Empty;
            var rootUrl = PKSSubSystemConfig.GetRootUrl(p.SubSystem);
            return new Uri(new Uri(rootUrl), p.Url).ToString();
        }
        /// <summary>生成菜单节点target</summary>
        private string GetUrlTarget(PKS_PERMISSION p)
        {
            return p.PermissionTypeId == PKSPermissionTypes.Menu ? "_self" : "_blank";
        }
        /// <summary>获得指定用户的权限集合</summary>
        public async Task<Dictionary<string, bool>> GetPermissionsAsync(int userId)
        {
            return await Task.Run(() => GetPermissionsFromCache(userId)).ConfigureAwait(false);
        }
        /// <summary>获得指定用户的权限集合</summary>
        private Dictionary<string, bool> GetPermissionsFromCache(int userId)
        {
            var key = CacheConst.UserKey + userId.ToString();
            return this.MemcachedCacher.TryGetOrAddValue<Dictionary<string, bool>>(key, CacheConst.PermissionRegion, GetCahceItem_Permissions);
        }
        /// <summary>获得缓存项_指定用户的权限集合</summary>
        private CacheItem<object> GetCahceItem_Permissions(string key, string region)
        {
            var userId = key.Substring(CacheConst.UserKey.Length).ToInt32();
            var value = GetPermissions(userId);
            return new CacheItem<object>(key, region, value, ExpirationMode.Sliding, TimeSpan.FromMinutes(PKSWebConsts.SlidingExpireInterval));
        }
        /// <summary>获得指定用户的权限集合</summary>
        private Dictionary<string, bool> GetPermissions(int userId)
        {
            var systems = GetService<IRepository<PKS_SUBSYSTEM>>()
                .GetQuery()
                .ToDictionary(e => e.Id);
            var userRoles = GetService<IRepository<WEBPAGES_USERSINROLES>>()
                .GetQuery()
                .Where(item => item.USERID == userId)
                .Select(item => item.ROLEID)
                .ToList();
            var rolePermissions = GetService<IRepository<PKS_ROLE_PERMISSION>>()
                .GetQuery()
                .Include(e => e.Permission)
                .Where(r => userRoles.Contains(r.RoleId))
                .ToList();
            var result = new Dictionary<string, bool>();
            foreach (var rolePermission in rolePermissions)
            {
                rolePermission.Permission.SubSystem = systems[rolePermission.Permission.SubSystemId];
                var url = GetMenuUrl(rolePermission.Permission).ToLowerInvariant();
                result[url] = true;
            }
            return result;
        }
        /// <summary>获得门户底部菜单</summary>
        public PortalFooterMenu GetPortalFooterMenu()
        {
            return this.MemcachedCacher.TryGetOrAddValue<PortalFooterMenu>(CacheConst.FooterMenuKey, CacheConst.PermissionRegion, GetPortalFooterMenuFromDb);
        }
        /// <summary>获得门户底部菜单</summary>
        private CacheItem<object> GetPortalFooterMenuFromDb(string key, string region)
        {
            var value = new PortalFooterMenu();
            var repo1 = GetService<IRepository<DbModels.PortalMgmt.PKS_PORTAL_LINKEDIN_TEXT>>();
            value.Content = repo1.GetAll().FirstOrDefault()?.Text;
            var repo2 = GetService<IRepository<DbModels.PortalMgmt.PKS_PORTAL_EXTERN_LINK>>();
            var links = repo2.GetAll().OrderBy(e => e.OrderNum).ThenBy(e => e.Id).ToList();
            value.Categories = links.GroupBy(e => e.Category)
                .Select(e => new LinkCategoryInfo
                {
                    Title = e.Key,
                    Links = e.Select(e2 => new LinkInfo
                    {
                        Title = e2.Name,
                        Url = e2.Url,
                        IconUrl = e2.IconUrl
                    })
                    .ToList()
                })
                .ToList();
            return new CacheItem<object>(key, region, value, ExpirationMode.Sliding, TimeSpan.FromMinutes(PKSWebConsts.SlidingExpireInterval));
        }
        /// <summary>处理变化事件</summary>
        [EventSubscriber]
        public void OnChanged(EntityChangedEventArgs<PKS_SUBSYSTEM> e)
        {
            if (e.Items.Any(item => item.State == EntityState.Modified))
            {
                this.MemcachedCacher.TryClearRegion(CacheConst.PermissionRegion);
            }
        }
        /// <summary>处理变化事件</summary>
        [EventSubscriber]
        public void OnChanged(EntityChangedEventArgs<PKS_PERMISSION> e)
        {
            if (e.Items.Any(item => item.State == EntityState.Modified))
            {
                this.MemcachedCacher.TryClearRegion(CacheConst.PermissionRegion);
            }
        }
        /// <summary>处理变化事件</summary>
        [EventSubscriber]
        public void OnChanged(EntityChangedEventArgs<PKS_ROLE_PERMISSION> e)
        {
            //var roleIds = e.Items.Select(item => item.Entity.RoleId).Distinct();
            //foreach (var roleId in roleIds)
            //{
            //    this.MemcachedCacher.Remove(CacheConst.RoleKey + roleId.ToString(), CacheConst.PermissionRegion);
            //}
            this.MemcachedCacher.TryClearRegion(CacheConst.PermissionRegion);
        }
        /// <summary>处理变化事件</summary>
        [EventSubscriber]
        public void OnChanged(EntityChangedEventArgs<DbModels.PortalMgmt.PKS_PORTAL_LINKEDIN_TEXT> e)
        {
            this.MemcachedCacher.TryRemove(CacheConst.FooterMenuKey, CacheConst.PermissionRegion);
        }
        /// <summary>处理变化事件</summary>
        [EventSubscriber]
        public void OnChanged(EntityChangedEventArgs<DbModels.PortalMgmt.PKS_PORTAL_EXTERN_LINK> e)
        {
            this.MemcachedCacher.TryRemove(CacheConst.FooterMenuKey, CacheConst.PermissionRegion);
        }
    }
}