using PKS.Core;
using PKS.DBModels;

namespace PKS.PortalMgmt
{
    /// <summary>WEB������</summary>
    public class PKSWebBootstrapper : Bootstrapper
    {
        /// <summary>�Ƿ�֧��WEB</summary>
        protected override bool EnableWeb => true;
        /// <summary>���òִ����ݷ�������</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            base.SetRepositoryConfig(config);
            config.MappingConfigs.Add(new SysFrameDbEntityMappingConfiguration());
            config.PublishChange = true;
        }
    }
}