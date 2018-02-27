using System;
using CacheManager.Core;

namespace PKS.Core
{
    /// <summary>缓存基类特性</summary>
    [AttributeUsage(AttributeTargets.All)]
    public abstract class CacheBaseAttribute : Attribute
    {
        /// <summary>级别</summary>
        public CacheLevel Level { get; set; } = CacheLevel.Internal;
        /// <summary>区域</summary>
        public string Region { get; set; } = CacheConst.DefaultInternalRegion;
        /// <summary>键</summary>
        public string Key { get; set; } = string.Empty;
    }

    /// <summary>缓存特性</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheItemAttribute : CacheBaseAttribute
    {
        /// <summary>默认缓存特性</summary>
        public static CacheItemAttribute Default { get; private set; } = new CacheItemAttribute();
        /// <summary>过期方式,默认为绝对过期</summary>
        public ExpirationMode Mode { get; set; } = ExpirationMode.Absolute;
        /// <summary>过期时间,默认为10分钟</summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(10);
    }

    /// <summary>缓存删除特性</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CacheRemoveAttribute : CacheBaseAttribute
    {
    }
}
