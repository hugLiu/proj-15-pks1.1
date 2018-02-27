using System;
using System.Reflection;
using CacheManager.Core;
using Common.Logging;
using EventBus;

namespace PKS.Core
{
    /// <summary>应用服务</summary>
    public abstract class AppService
    {
        /// <summary>注入服务提供者</summary>
        public IServiceProvider ServiceProvider { get; private set; } = Bootstrapper.Kernel;
        /// <summary>日志服务</summary>
        public Common.Logging.ILog Logger
        {
            get { return GetService<Common.Logging.ILog>(); }
        }
        /// <summary>本地缓存服务</summary>
        public ICacheManager<object> Cacher
        {
            get { return GetService<ICacheManager<object>>(); }
        }
        /// <summary>外部缓存服务</summary>
        public ICacheManager<object> MemcachedCacher
        {
            get { return GetService<ICacheProvider>().ExternalCacher; }
        }
        /// <summary>事件总线服务</summary>
        public IEventBus EventBus
        {
            get { return GetService<IEventBus>(); }
        }
        /// <summary>获得注入服务</summary>
        protected TService GetService<TService>()
        {
            return (TService)this.ServiceProvider.GetService(typeof(TService));
        }
    }
}
