using System;
using CacheManager.Core;
using Enyim.Caching;

namespace PKS.Core
{
    /// <summary>缓存提供者</summary>
    public class CacheProvider : ICacheProvider
    {
        /// <summary>本地缓存</summary>
        public ICacheManager<object> InternalCacher { get; set; }
        /// <summary>外部缓存</summary>
        public ICacheManager<object> ExternalCacher { get; set; }
    }
}
