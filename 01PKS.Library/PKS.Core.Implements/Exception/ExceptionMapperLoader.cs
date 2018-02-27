using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Ninject;
using PKS.Utils;

namespace PKS.Core
{
    /// <summary>异常组件加载器</summary>
    public class ExceptionMapperLoader : IServiceLoader<ExceptionLoaderConfig>
    {
        /// <summary>初始化</summary>
        public void Initialize(IKernel kernel, ExceptionLoaderConfig config)
        {
            var assemblies = ReflectUtil.GetAssemblies(config.AutoLoad, config.SectionName);
            var handler = new WebExceptionHandler();
            handler.Initialize();
            handler.LoadConfig(assemblies);
            kernel.Bind<IWebExceptionHandler>().ToConstant(handler);
        }
    }
}