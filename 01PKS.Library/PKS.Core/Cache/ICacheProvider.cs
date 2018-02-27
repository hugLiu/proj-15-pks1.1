using System;
using CacheManager.Core;

namespace PKS.Core
{
    /// <summary>缓存提供者</summary>
    public interface ICacheProvider
    {
        /// <summary>本地缓存</summary>
        ICacheManager<object> InternalCacher { get; }
        /// <summary>外部缓存</summary>
        ICacheManager<object> ExternalCacher { get; }
    }
}
