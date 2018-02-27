using System;
using System.Diagnostics;
using Ninject.Modules;

namespace PKS.Core.Template
{
    /// <summary>自定义注入模块</summary>
    /// <remarks>
    /// 使用注入模块解决按约定不能注入的问题
    /// 约定注入会自动注入
    /// </remarks>
    public class CustomNinjectModule : NinjectModule
    {
        /// <summary>加载注入</summary>
        public override void Load()
        {
            //示例:绑定
            //Bind<ILog>().ToConstant(commonLogger);
            //示例:用自己的绑定覆盖已有绑定
            //Rebind<IJLogManager>().ToConstant(jLogManager);
        }
    }
}
