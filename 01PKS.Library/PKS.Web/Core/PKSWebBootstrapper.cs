using System.Web.Mvc;
using Ninject.Web.Mvc;
using PKS.Core;
using PKS.DBModels;
using PKS.Web.MVC;

namespace PKS.Web
{
    /// <summary>WEB启动器</summary>
    public class PKSWebBootstrapper : WebBootstrapper
    {
        /// <summary>构造函数</summary>
        public PKSWebBootstrapper()
        {
        }

        /// <summary>初始化</summary>
        public override void Initialize()
        {
            base.Initialize();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(Kernel));
        }
        /// <summary>设置仓储数据访问配置</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            base.SetRepositoryConfig(config);
            config.MappingConfigs.Add(new SysFrameDbEntityMappingConfiguration());
        }
    }
}