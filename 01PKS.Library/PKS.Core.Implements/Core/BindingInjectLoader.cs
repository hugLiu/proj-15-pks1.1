using System;
using System.Diagnostics;
using System.Reflection;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;
//using Ninject.Extensions.Interception;
using Ninject.Web.Common;
using PKS.Utils;

namespace PKS.Core
{
    /// <summary>绑定注入加载器</summary>
    public class BindingInjectLoader : IServiceLoader<BindingInjectLoaderConfig>
    {
        /// <summary>初始化</summary>
        public void Initialize(IKernel kernel, BindingInjectLoaderConfig config)
        {
            var assemblies = ReflectUtil.GetAssemblies(config.AutoLoad, config.SectionName);
            foreach (var assembly in assemblies)
            {
                BindByConvention(kernel, assembly, config);
            }
            kernel.Bind<IADIdentityService>().To<ADIdentityService>().InSingletonScope();
        }

        /// <summary>根据约定绑定程序集</summary>
        public void BindByConvention(IKernel kernel, Assembly assembly, BindingInjectLoaderConfig config)
        {
            kernel.Bind(syntax => BindByConvention<ISingletonAppService>(assembly, syntax, e => true,
                e => e.InSingletonScope()));
            var singleton = typeof(ISingletonAppService);
            kernel.Bind(syntax => BindByConvention<IPerThreadAppService>(assembly, syntax,
                e => !singleton.IsAssignableFrom(e), e => e.InThreadScope()));
            var thread = typeof(IPerThreadAppService);
            if (config.EnableWeb)
            {
                kernel.Bind(syntax => BindByConvention<IPerRequestAppService>(assembly, syntax,
                    e => !singleton.IsAssignableFrom(e) && !thread.IsAssignableFrom(e), e => e.InRequestScope()));
            }
            else
            {
                kernel.Bind(syntax => BindByConvention<IPerRequestAppService>(assembly, syntax,
                    e => !singleton.IsAssignableFrom(e) && !thread.IsAssignableFrom(e), e => e.InThreadScope()));
            }
            var request = typeof(IPerRequestAppService);
            kernel.Bind(syntax => BindByConvention<IAppService>(assembly, syntax,
                e => !singleton.IsAssignableFrom(e) && !thread.IsAssignableFrom(e) && !request.IsAssignableFrom(e),
                e => e.InTransientScope()));
            kernel.Load(assembly);
        }

        /// <summary>根据约定绑定</summary>
        private void BindByConvention<T>(Assembly assembly, IFromSyntax fromSyntax, Func<Type, bool> filter,
            ConfigurationAction configuration)
        {
            fromSyntax.From(assembly)
                .IncludingNonePublicTypes()
                .SelectAllClasses()
                .InheritedFrom<T>()
                .Where(filter)
                .BindDefaultInterfaces()
                .Configure(configuration);
        }

        /// <summary>加载间接DLL</summary>
        private void LoadIndirectAssemblies()
        {
            //Debug.WriteLine(typeof(IInterceptor));
            //Debug.WriteLine(typeof(DynamicProxyModule));
            //Debug.WriteLine(typeof(IServiceProviderEx));
        }
    }
}