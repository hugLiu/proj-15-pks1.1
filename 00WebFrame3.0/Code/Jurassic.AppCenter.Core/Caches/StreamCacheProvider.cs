using Jurassic.Com.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.Caches
{
    /// <summary>
    /// 默认使用本机内存的流缓存提供类
    /// </summary>
    public class StreamCacheProvider : ICacheProvider<Stream>
    {
        private CacheProvider innerProvider = new CacheProvider();

        public Stream this[string key]
        {
            get
            {
                byte[] bs = innerProvider[key] as byte[];
                if (bs == null) return null;
                return new MemoryStream(bs);
            }
            set
            {
                MemoryStream ms = new MemoryStream();
                value.CopyTo(ms);
                byte[] bs = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(bs, 0, (int) ms.Length);
                innerProvider[key] = bs;
            }
        }



        public Stream Add(string key, Stream value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
        }

        public Stream Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
