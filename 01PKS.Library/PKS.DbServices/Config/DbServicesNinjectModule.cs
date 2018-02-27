using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using PKS.Models;
using PKS.DbServices.SysFrame;

namespace PKS.DbServices
{
    /// <summary>注入模块</summary>
    public class DbServicesNinjectModule : NinjectModule
    {
        /// <summary>加载注入</summary>
        public override void Load()
        {
            Bind<IPKSSubSystemConfig>().To<PKSSubSystemConfig>().InSingletonScope();
            Kernel.Get<IPKSSubSystemConfig>();
            Bind<RolePermissionsService>().ToSelf().InSingletonScope();
            Kernel.Get<RolePermissionsService>();

            Bind<RoleMetadataPermissionService>().ToSelf().InSingletonScope();
            Kernel.Get<RoleMetadataPermissionService>();
        }
    }
}
