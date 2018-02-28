using PKS.Core;
using PKS.DBModels;
using PKS.Web;

namespace PKS.WebAPI
{
    /// <summary>WEB������</summary>
    public class PKSWebBootstrapper : WebBootstrapper
    {
        /// <summary>���캯��</summary>
        public PKSWebBootstrapper()
        {
        }
        /// <summary>���òִ����ݷ�������</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            base.SetRepositoryConfig(config);
            config.MappingConfigs.Add(new SysFrameDbEntityMappingConfiguration());
        }
    }
}