using PKS.Core;
using PKS.DBModels;
using PKS.Web;

namespace PKS.WebAPI
{
    /// <summary>WEB启动器</summary>
    public class PKSWebBootstrapper : WebBootstrapper
    {
        /// <summary>构造函数</summary>
        public PKSWebBootstrapper()
        {
        }
        /// <summary>设置仓储数据访问配置</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            base.SetRepositoryConfig(config);
            config.MappingConfigs.Add(new SysFrameDbEntityMappingConfiguration());
        }
    }
}