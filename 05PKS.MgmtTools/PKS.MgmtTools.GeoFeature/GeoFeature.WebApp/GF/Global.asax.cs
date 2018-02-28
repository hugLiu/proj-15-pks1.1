using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;
using Jurassic.Com.Tools;
using System.Globalization;
using Jurassic.AppCenter.Resources;
using Jurassic.AppCenter.Logs;
using Ninject;
using Jurassic.CommonModels.Organization;
using Jurassic.CommonModels;
using Jurassic.CommonModels.EFProvider;
using Jurassic.WebFrame;

namespace GeoFeature
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class Application1 : MvcApplication
    {
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                //var list = base.ControllerNameSpaces.ToList();
                //list.Add(typeof(Application1).Namespace + ".Controllers");
                //return list;

                return base.ControllerNameSpaces;
            }
        }

        protected override void Application_Start()
        {
            base.Application_Start();

            //todo: 额外的初始化代码
            // RedefineViewLocator();
        }

        /// <summary>
        /// 追加或重新定义视图搜索路径
        /// </summary>
        protected void RedefineViewLocator()
        {
            var viewEngine = (ViewEngines.Engines[0] as RazorViewEngine);
            if (viewEngine == null) return;
            viewEngine.ViewLocationFormats =
                new string[] { 
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Demo/{1}/{0}.cshtml"};
        }

        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);

            //要支持Oralce数据库，请在""中填写Oralce库的Schema名称
            //ninjectKernel.Rebind<ModelContext>().ToSelf()
            //  .WithPropertyValue("Schema", "");

            ninjectKernel.Rebind<ModelContext>().ToSelf()
            .WithPropertyValue("Schema", "SYSFRAME");

            //如果要修改上传根目录，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<IFileLocator>().To(typeof(FileLocator))
            //       .WithConstructorArgument("rootPath", "D:\\Temp"); 

            //如果要开启多标签，或修改默认皮肤，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<UserConfig>().ToSelf()
            //.WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示页，请设置为true
            //.WithPropertyValue("Theme", "blue");  //系统默认皮肤

            //todo: 额外的注入代码
        }
    }
}