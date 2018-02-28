using PKS.Core;
using PKS.DBModels;

namespace PKS.PortalMgmt
{
    /// <summary>WEB启动器</summary>
    public class PKSWebBootstrapper : Bootstrapper
    {
        /// <summary>是否支持WEB</summary>
        protected override bool EnableWeb => true;
        /// <summary>设置仓储数据访问配置</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            base.SetRepositoryConfig(config);
            config.MappingConfigs.Add(new SysFrameDbEntityMappingConfiguration());
            config.PublishChange = true;
        }
    }
}