using Ninject;

namespace PKS.Core
{
    /// <summary>组件加载器配置</summary>
    public class LoaderConfig
    {
        /// <summary>是否启用某组件，默认为True</summary>
        public bool Enable { get; set; } = true;
        /// <summary>是否支持WEB</summary>
        public bool EnableWeb { get; set; }
        /// <summary>组件名称，可用于区分各个程序或站点</summary>
        public string Name { get; set; }
    }

    /// <summary>组件加载器</summary>
    public interface IServiceLoader<T>
    {
        /// <summary>初始化</summary>
        void Initialize(IKernel kernel, T config);
    }

    /// <summary>日志组件加载器配置</summary>
    public class LoggingLoaderConfig : LoaderConfig
    {
        /// <summary>构造函数</summary>
        public LoggingLoaderConfig()
        {
            this.Name = "PKS.Logging";
        }
    }

    /// <summary>异常组件加载器配置</summary>
    public class ExceptionLoaderConfig : LoaderConfig
    {
        /// <summary>构造函数</summary>
        public ExceptionLoaderConfig()
        {
            this.Name = "PKS.Exception";
        }
        /// <summary>是否自动加载,默认是False</summary>
        /// <remarks>
        /// 如果是自动加载，则扫描所有程序集并自动加载，否则从配置文件中加载
        /// </remarks>
        public bool AutoLoad { get; set; } = false;
        /// <summary>配置文件节名</summary>
        public string SectionName { get; set; } = "pks.exception";
    }

    /// <summary>缓存组件加载器配置</summary>
    public class CacheLoaderConfig : LoaderConfig
    {
        /// <summary>构造函数</summary>
        public CacheLoaderConfig()
        {
            this.Name = "PKS.Cache";
        }
        /// <summary>内部名称</summary>
        public string InternalName { get; set; } = CacheConst.DefaultInternalRegion;
        /// <summary>外部名称</summary>
        public string ExternalName { get; set; } = CacheConst.DefaultExternalRegion;
        /// <summary>外部配置节</summary>
        public string ExternalSectionName { get; set; } = "pks.cache/memcached";
        /// <summary>最大重试次数</summary>
        public int MaxRetries { get; set; } = 1;
    }

    /// <summary>对象映射组件加载器配置</summary>
    public class ObjectMapperLoaderConfig : LoaderConfig
    {
        /// <summary>构造函数</summary>
        public ObjectMapperLoaderConfig()
        {
            this.Name = "PKS.ObjectMapper";
        }
        /// <summary>是否自动加载,默认是false</summary>
        /// <remarks>
        /// 如果是自动加载，则扫描所有程序集并自动加载，否则从配置文件中加载
        /// </remarks>
        public bool AutoLoad { get; set; } = false;
        /// <summary>配置文件节名</summary>
        public string SectionName { get; set; } = "pks.autoMapper";
    }

    /// <summary>绑定注入组件加载器配置</summary>
    public class BindingInjectLoaderConfig : LoaderConfig
    {
        /// <summary>构造函数</summary>
        public BindingInjectLoaderConfig()
        {
            this.Name = "PKS.BindingInject";
        }
        /// <summary>是否自动加载,默认是false</summary>
        /// <remarks>
        /// 如果是自动加载，则扫描所有程序集并自动加载，否则从配置文件中加载
        /// </remarks>
        public bool AutoLoad { get; set; } = false;
        /// <summary>配置文件节名</summary>
        public string SectionName { get; set; } = "pks.bindingInject";
    }
}