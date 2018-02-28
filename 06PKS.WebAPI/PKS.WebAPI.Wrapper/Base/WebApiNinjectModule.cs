using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Ninject.Modules;
using PKS.Services;

namespace PKS.WebAPI.Services.Wrapper
{
    /// <summary>注入模块</summary>
    public class WebApiNinjectModule : NinjectModule
    {
        /// <summary>加载注入</summary>
        public override void Load()
        {
            Bind<IFileFormatService>().To<AppDataServiceWrapper>().InSingletonScope();
        }
    }
}
