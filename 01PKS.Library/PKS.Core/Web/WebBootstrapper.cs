using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Bootstrapper = PKS.Core.Bootstrapper;

namespace PKS.Web
{
    /// <summary>WEB启动器</summary>
    public class WebBootstrapper : Bootstrapper
    {
        /// <summary>NInject内置启动器</summary>
        private readonly Ninject.Web.Common.Bootstrapper _bootstrapper = new Ninject.Web.Common.Bootstrapper();

        /// <summary>构造函数</summary>
        public WebBootstrapper()
        {
            Start();
        }

        /// <summary>是否支持WEB</summary>
        protected override bool EnableWeb => true;

        /// <summary>
        /// Starts the application
        /// </summary>
        public void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            _bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private IKernel CreateKernel()
        {
            Kernel = new StandardKernel();
            var kernel = Kernel;
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            return kernel;
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public void Stop()
        {
            _bootstrapper.ShutDown();
        }
    }
}