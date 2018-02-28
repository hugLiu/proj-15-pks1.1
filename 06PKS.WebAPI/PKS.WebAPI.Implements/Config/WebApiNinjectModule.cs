using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MongoDB.Driver;
using Ninject.Modules;
using PKS.Core;
using PKS.Services;

namespace PKS.WebAPI.Services
{
    /// <summary>注入模块</summary>
    public class WebApiNinjectModule : NinjectModule
    {
        /// <summary>加载注入</summary>
        public override void Load()
        {
            Bind(typeof(IMongoCollection<>)).ToMethod(context => Bootstrapper.ProviderGet(context, typeof(MongoColletionProvider<>))).InSingletonScope();
        }
    }
}
