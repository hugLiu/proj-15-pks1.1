using System.Web.Mvc;
using Ninject.Web.Mvc;
using PKS.Core;
using PKS.DBModels;
using PKS.Web.MVC;

namespace PKS.Web
{
    /// <summary>WEB������</summary>
    public class PKSWebBootstrapper : WebBootstrapper
    {
        /// <summary>���캯��</summary>
        public PKSWebBootstrapper()
        {
        }

        /// <summary>��ʼ��</summary>
        public override void Initialize()
        {
            base.Initialize();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(Kernel));
        }
        /// <summary>���òִ����ݷ�������</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            base.SetRepositoryConfig(config);
            config.MappingConfigs.Add(new SysFrameDbEntityMappingConfiguration());
        }
    }
}