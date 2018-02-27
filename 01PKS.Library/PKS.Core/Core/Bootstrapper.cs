using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PKS.Utils;
using Ninject;
using EventBus;
using PKS.Models;
using CacheManager.Core.Logging;
using Ninject.Activation;

namespace PKS.Core
{
    /// <summary>启动器</summary>
    public class Bootstrapper
    {
        /// <summary>IOC核心</summary>
        public static IKernel Kernel { get; set; }
        /// <summary>核心功能实现程序集</summary>
        protected string ImplementsAssembly { get; } = "PKS.Core.Implements";

        /// <summary>是否支持WEB</summary>
        protected virtual bool EnableWeb { get { return false; } }
        /// <summary>初始化</summary>
        public virtual void Initialize()
        {
            //初始化容器
            if (Kernel == null)
            {
                if (!this.EnableWeb)
                {
                    var settings = new NinjectSettings();
                    settings.ExtensionSearchPatterns = new string[] { "Ninject.Extensions.*.dll" };
                    Kernel = new StandardKernel(settings);
                }
                else
                {
                    Kernel = new StandardKernel();
                }
            }
            var kernel = Kernel;
            kernel.Bind<IServiceProvider>().ToConstant(kernel);
            var implementsAssembly = Assembly.Load(this.ImplementsAssembly);
            //加载日志组件
            var loggingLoaderConfig = new LoggingLoaderConfig();
            SetLogggingConfig(loggingLoaderConfig);
            if (loggingLoaderConfig.Enable)
            {
                LoadLoggging(kernel, implementsAssembly, loggingLoaderConfig);
            }
            //加载异常组件
            var exceptionLoaderConfig = new ExceptionLoaderConfig();
            SetExceptionConfig(exceptionLoaderConfig);
            if (exceptionLoaderConfig.Enable)
            {
                LoadException(kernel, implementsAssembly, exceptionLoaderConfig);
            }
            //加载缓存组件
            var cacheLoaderConfig = new CacheLoaderConfig();
            SetCacheConfig(cacheLoaderConfig);
            if (cacheLoaderConfig.Enable)
            {
                LoadCache(kernel, implementsAssembly, cacheLoaderConfig);
            }
            //加载仓储数据访问组件
            var repositoryLoaderConfig = new RepositoryLoaderConfig();
            SetRepositoryConfig(repositoryLoaderConfig);
            if (repositoryLoaderConfig.Enable)
            {
                kernel.Bind<IEventBus>().ToConstant(SimpleEventBus.GetDefaultEventBus());
                LoadRepository(kernel, implementsAssembly, repositoryLoaderConfig);
            }
            //加载对象自动映射组件
            var objectMapperLoaderConfig = new ObjectMapperLoaderConfig();
            SetObjectMapperConfig(objectMapperLoaderConfig);
            if (objectMapperLoaderConfig.Enable)
            {
                LoadObjectMapper(kernel, implementsAssembly, objectMapperLoaderConfig);
            }
            //按约定注入绑定
            var bindingInjectLoaderConfig = new BindingInjectLoaderConfig();
            SetBindingInjectConfig(bindingInjectLoaderConfig);
            if (bindingInjectLoaderConfig.Enable)
            {
                LoadBindingInject(kernel, implementsAssembly, bindingInjectLoaderConfig);
            }
            //JSON序列化设置
            //JsonUtil.DefaultUseCamelCaseNamingStrategy();
        }

        /// <summary>设置日志配置</summary>
        public virtual void SetLogggingConfig(LoggingLoaderConfig config)
        {
            //自定义配置
        }
        /// <summary>加载日志组件</summary>
        private void LoadLoggging(IKernel kernel, Assembly implementsAssembly, LoggingLoaderConfig config)
        {
            var loader = implementsAssembly.CreateInterfaceInstance<IServiceLoader<LoggingLoaderConfig>>();
            loader.Initialize(kernel, config);
        }
        /// <summary>设置异常配置</summary>
        public virtual void SetExceptionConfig(ExceptionLoaderConfig config)
        {
            config.Enable = this.EnableWeb;
        }
        /// <summary>加载异常组件</summary>
        private void LoadException(IKernel kernel, Assembly implementsAssembly, ExceptionLoaderConfig config)
        {
            var loader = implementsAssembly.CreateInterfaceInstance<IServiceLoader<ExceptionLoaderConfig>>();
            loader.Initialize(kernel, config);
        }
        /// <summary>设置缓存配置</summary>
        public virtual void SetCacheConfig(CacheLoaderConfig config)
        {
            //自定义配置
        }
        /// <summary>加入缓存组件</summary>
        private void LoadCache(IKernel kernel, Assembly implementsAssembly, CacheLoaderConfig config)
        {
            var loader = implementsAssembly.CreateInterfaceInstance<IServiceLoader<CacheLoaderConfig>>();
            loader.Initialize(kernel, config);
        }
        /// <summary>设置仓储数据访问配置</summary>
        public virtual void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            config.EnableWeb = this.EnableWeb;
        }
        /// <summary>加入仓储数据访问组件</summary>
        private void LoadRepository(IKernel kernel, Assembly implementsAssembly, RepositoryLoaderConfig config)
        {
            var loader = implementsAssembly.CreateInterfaceInstance<IServiceLoader<RepositoryLoaderConfig>>();
            loader.Initialize(kernel, config);
        }
        /// <summary>设置对象映射配置</summary>
        public virtual void SetObjectMapperConfig(ObjectMapperLoaderConfig config)
        {
            //自定义配置
        }
        /// <summary>加入对象映射组件</summary>
        private void LoadObjectMapper(IKernel kernel, Assembly implementsAssembly, ObjectMapperLoaderConfig config)
        {
            var loader = implementsAssembly.CreateInterfaceInstance<IServiceLoader<ObjectMapperLoaderConfig>>();
            loader.Initialize(kernel, config);
        }
        /// <summary>设置绑定注入配置</summary>
        public virtual void SetBindingInjectConfig(BindingInjectLoaderConfig config)
        {
            config.EnableWeb = this.EnableWeb;
        }
        /// <summary>加入绑定注入组件</summary>
        private void LoadBindingInject(IKernel kernel, Assembly implementsAssembly, BindingInjectLoaderConfig config)
        {
            var loader = implementsAssembly.CreateInterfaceInstance<IServiceLoader<BindingInjectLoaderConfig>>();
            loader.Initialize(kernel, config);
        }
        /// <summary>获得实例</summary>
        public static object ProviderGet(IContext context, Type providerGenericType)
        {
            var providerType = providerGenericType.MakeGenericType(context.Request.Service);
            var provider = Kernel.Get(providerType);
            return provider.As<IProvider>().Create(context);
        }
        /// <summary>获得实例</summary>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        #region 日志方法
        /// <summary>记录有内容的消息</summary>
        public static void Log(LogLevel level, string message, string content)
        {
            Log(NLog.LogLevel.FromOrdinal((int)level), message, null, null, null, content, null);
        }
        /// <summary>记录详细日志</summary>
        public static void Log(PKS_Log log)
        {
            Log(NLog.LogLevel.FromString(log.LogLevel), log.Message, log.Request, log.Principal, log.ExSource, log.ExContent, log.ExData);
        }
        /// <summary>记录详细日志</summary>
        public static void Log(NLog.LogLevel logLevel, string message, string request, string principal, string source, string content, string data)
        {
            var logger = Get<NLog.ILogger>();
            var theEvent = new NLog.LogEventInfo(logLevel, logger.Name, message);
            if (request != null) theEvent.Properties[nameof(PKS_Log.Request)] = request;
            if (principal != null) theEvent.Properties[nameof(PKS_Log.Principal)] = principal;
            if (source != null) theEvent.Properties[nameof(PKS_Log.ExSource)] = source;
            if (content != null) theEvent.Properties[nameof(PKS_Log.ExContent)] = content;
            if (data != null) theEvent.Properties[nameof(PKS_Log.ExData)] = data;
            logger.Log(theEvent);
        }
        /// <summary>尝试执行忽略错误</summary>
        public static void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Get<Common.Logging.ILog>().Error("TryExecute:", ex);
            }
        }
        /// <summary>记录异常</summary>
        public static void Error(string message, Exception ex)
        {
            Get<Common.Logging.ILog>().Error(message, ex);
        }
        #endregion
    }
}
