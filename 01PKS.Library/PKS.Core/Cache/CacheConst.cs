using System;
using CacheManager.Core;

namespace PKS.Core
{
    /// <summary>缓存级别</summary>
    public enum CacheLevel
    {
        /// <summary>内部</summary>
        Internal,
        /// <summary>外部默认区域</summary>
        External,
    }

    /// <summary>缓存常数</summary>
    public static class CacheConst
    {
        /// <summary>内部默认区域</summary>
        public static string DefaultInternalRegion { get; } = "PKS.Cache.Internal";
        /// <summary>外部默认区域</summary>
        public static string DefaultExternalRegion { get; } = "PKS.Cache.External";
        /// <summary>认证区域</summary>
        public static string AuthenticationRegion { get; } = "PKS.Authentication";
        /// <summary>Token过期参数</summary>
        public static readonly string TokenExpireSettings = "PKS.TokenExpireSettings";
        /// <summary>子系统区域</summary>
        public static string SubSystemRegion { get; } = "PKS.SubSystems";
        /// <summary>子系统URL键</summary>
        public static string SubSystemUrlsKey { get; } = "PKS.SubSystems.URLS";
        /// <summary>权限区域</summary>
        public static string PermissionRegion { get; } = "PKS.Permissions";
        /// <summary>角色权限键</summary>
        public static string RoleKey { get; } = "PKS.RolePermissions-";
        /// <summary>用户权限键</summary>
        public static string UserKey { get; } = "PKS.UserPermissions-";
        /// <summary>底部菜单键</summary>
        public static string FooterMenuKey { get; } = "PKS.FooterMenu";
        /// <summary>尝试获取缓存项值</summary>
        public static T TryGet<T>(this ICacheManager<object> cacher, string key, string region)
        {
            try
            {
                return (T)cacher.Get(key, region);
            }
            catch (Exception ex)
            {
                var message = $"获取缓存[{region}:{key}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
            return default(T);
        }
        /// <summary>尝试设置缓存项值(如果不存在则添加)</summary>
        public static void TryPut(this ICacheManager<object> cacher, string key, object value, string region)
        {
            try
            {
                cacher.Put(key, value, region);
            }
            catch (Exception ex)
            {
                var message = $"设置缓存[{region}:{key}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
        }
        /// <summary>尝试设置缓存项值(如果不存在则添加)</summary>
        public static void TryPut(this ICacheManager<object> cacher, CacheItem<object> cacheItem)
        {
            try
            {
                cacher.Put(cacheItem);
            }
            catch (Exception ex)
            {
                var message = $"设置缓存[{cacheItem.Region}:{cacheItem.Key}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
        }
        /// <summary>尝试获取或加入缓存项值</summary>
        public static T TryGetOrAddValue<T>(this ICacheManager<object> cacher, string key, string region, Func<string, string, CacheItem<object>> valueFactory)
        {
            CacheItem<object> cacheItem = null;
            var success = false;
            try
            {
                CacheItem<object> newItem = null;
                Func<string, string, CacheItem<object>> valueFactoryWrapper = (key2, region2) =>
                {
                    if (!success && newItem == null)
                    {
                        newItem = valueFactory(key2, region2);
                        success = true;
                    }
                    return newItem;
                };
                if (!cacher.TryGetOrAdd(key, region, valueFactoryWrapper, out cacheItem))
                {
                    if (newItem != null) cacheItem = newItem;
                }
            }
            catch (Exception ex)
            {
                var message = $"获取或加入缓存[{region}:{key}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
            if (!success && cacheItem == null) cacheItem = valueFactory(key, region);
            if (cacheItem == null || cacheItem.Value == null) return default(T);
            return (T)cacheItem.Value;
        }
        /// <summary>尝试加入或更新缓存项值</summary>
        public static bool TryAddOrUpdate(this ICacheManager<object> cacher, CacheItem<object> cacheItem)
        {
            try
            {
                cacher.AddOrUpdate(cacheItem, oldValue => cacheItem.Value);
                return true;
            }
            catch (Exception ex)
            {
                var message = $"加入或更新缓存[{cacheItem.Region}:{cacheItem.Key}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
            return false;
        }
        /// <summary>尝试删除缓存项值</summary>
        public static bool TryRemove(this ICacheManager<object> cacher, string key, string region)
        {
            try
            {
                return cacher.Remove(key, region);
            }
            catch (Exception ex)
            {
                var message = $"删除缓存[{region}:{key}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
            return false;
        }
        /// <summary>尝试清除某区域缓存项</summary>
        public static bool TryClearRegion(this ICacheManager<object> cacher, string region)
        {
            try
            {
                cacher.ClearRegion(region);
                return true;
            }
            catch (Exception ex)
            {
                var message = $"清除区域缓存[{region}]失败:";
                Bootstrapper.Get<Common.Logging.ILog>().Error(message, ex);
            }
            return false;
        }
        /// <summary>加入随机生成的缓存</summary>
        public static string AddRandom(this ICacheManager<object> cacher, object data)
        {
            var token = Guid.NewGuid().ToString();
            var cacheItem = new CacheItem<object>(token, DefaultExternalRegion, data, ExpirationMode.Absolute, TimeSpan.FromSeconds(30));
            cacher.Add(cacheItem);
            return token;
        }
        /// <summary>获得随机生成的缓存</summary>
        public static T GetRandom<T>(this ICacheManager<object> cacher, string token)
        {
            return cacher.Get<T>(token, DefaultExternalRegion);
        }
    }
}
