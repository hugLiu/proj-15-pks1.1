using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 定义一个简单的字典缓存接口
    /// </summary>
    public interface ICacheProvider : ICacheProvider<string, object>
    {
    }

    /// <summary>
    /// 关键字是字符串的字典接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheProvider<T> : ICacheProvider<string, T>
    {
    }

    /// <summary>
    /// 泛型缓存字典接口
    /// </summary>
    /// <typeparam name="TKey">键的数据类型</typeparam>
    /// <typeparam name="T">缓存的数据类型</typeparam>
    public interface ICacheProvider<TKey, T>
    {
        T this[TKey key] { get; set; }

        T Add(string key, T value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);
            //mCache.Add(key, value, null, DateTime.Now, TimeSpan.MinValue, CacheItemPriority.Normal, callback);

        T Remove(TKey key);
    }
}
