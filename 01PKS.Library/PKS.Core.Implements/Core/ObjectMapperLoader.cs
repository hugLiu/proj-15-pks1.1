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
    /// <summary>对象映射加载器</summary>
    public class ObjectMapperLoader : IServiceLoader<ObjectMapperLoaderConfig>
    {
        /// <summary>初始化</summary>
        public void Initialize(IKernel kernel, ObjectMapperLoaderConfig config)
        {
            var assemblies = ReflectUtil.GetAssemblies(config.AutoLoad, config.SectionName);
            Mapper.Initialize(cfg => cfg.AddProfiles(assemblies));
            Mapper.AssertConfigurationIsValid();
        }
    }
}