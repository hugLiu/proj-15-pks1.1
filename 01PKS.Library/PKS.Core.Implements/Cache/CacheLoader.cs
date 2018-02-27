using CacheManager.Core;
using Enyim.Caching;
using Ninject;
using PKS.Utils;

namespace PKS.Core
{
    /// <summary>缓存加载器</summary>
    public class CacheLoader : IServiceLoader<CacheLoaderConfig>
    {
        /// <summary>初始化</summary>
        public void Initialize(IKernel kernel, CacheLoaderConfig config)
        {
            var provider = new CacheProvider();
            //内部缓存
            var cacheManager = CacheFactory.Build(config.InternalName, settings =>
            {
                settings.WithSystemRuntimeCacheHandle(config.InternalName);
                settings.WithMaxRetries(config.MaxRetries);
                settings.WithLogging(typeof(CacheManager.Logging.NLogLoggerFactoryAdapter));
            });
            kernel.Bind<ICacheManager<object>>().ToConstant(cacheManager);
            provider.InternalCacher = cacheManager;
            if (!config.ExternalName.IsNullOrEmpty())
            {
                //外部缓存
                cacheManager = CacheFactory.Build(config.ExternalName, settings =>
                {
                    settings.WithMaxRetries(config.MaxRetries);
                    settings.WithMemcachedCacheHandle(config.ExternalSectionName);
                    settings.WithLogging(typeof(CacheManager.Logging.NLogLoggerFactoryAdapter));
                    //settings.WithJsonSerializer(JsonUtil.CamelCaseJsonSerializerSettings, JsonUtil.CamelCaseJsonSerializerSettings);
                });
                provider.ExternalCacher = cacheManager;
            }
            kernel.Bind<ICacheProvider>().ToConstant(provider);
            //kernel.Bind<ICacheInterceptor>().To<CacheInterceptor>().InSingletonScope();
        }
    }
}