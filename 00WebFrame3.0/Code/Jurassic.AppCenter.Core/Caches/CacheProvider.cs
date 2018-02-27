using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Jurassic.AppCenter.Caches
{
    /// <summary>
    /// 默认基于.net Cahce的缓存实现
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private Cache mCache;

        /// <summary>
        /// 创建默认基于.net Cahce的缓存实现
        /// </summary>
        public CacheProvider()
        {
            if (HttpContext.Current == null)
            {
                mCache = new Cache();
            }
            else
            {
                mCache = HttpContext.Current.Cache;
            }
        }

        public object this[string key]
        {
            get
            {
                return mCache[key];
            }
            set
            {
                mCache.Add(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return mCache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public object Remove(string key)
        {
            return mCache.Remove(key);
        }
    }
}
