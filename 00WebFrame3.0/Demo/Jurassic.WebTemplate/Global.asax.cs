using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
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
using Jurassic.WebFrame.Models;
using Jurassic.CommonModels.FileRepository;
using Jurassic.CommonModels.Schedule;

namespace Jurassic.WebTemplate
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class Application1 : MvcApplication
    {
        /// <summary>
        // 加载控制器命名空间, 在此，在基类的基础上加载了AddinDemo这个示例插件模块的命名空间
        /// </summary>
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                var list = base.ControllerNameSpaces.ToList();
                //如果需要集成AddinDemo这个模块，则取消下面的注释，并确保AddinDemo.dll放在bin目录中
                list.Insert(0, "AddinDemo"); //这里写不带.DLL后缀的模块名称
                return list;
            }
        }

        protected override void Application_Start()
        {
            base.Application_Start();

            //todo: 额外的初始化代码
            // RedefineViewLocator();
            SiteManager.Message.ToString();
        }

        /// <summary>
        /// 追加或重新定义视图搜索路径
        /// </summary>
        protected void RedefineViewLocator()
        {
            var viewEngine = ViewEngines.Engines.FirstOrDefault(eng=>eng is RazorViewEngine) as RazorViewEngine;
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
            //如果业务系统自身的表在同一库中，则需要继承ModelContext创建新的Context, 并如下写法：
            //ninjectKernel.Rebind<ModelContext,DbContext>().To<YourContext>()
            //.WithPropertyValue("Schema", "");
            ninjectKernel.Rebind<ModelContext>().ToSelf()
              .WithPropertyValue("Schema", "");

            //如果要修改上传根目录，请恢复以下代码,并修改第二个参数
            //既支持物理路径(如 D:\UploadFiles)，也支持虚拟路径 如"~/UploadFiles"
            //ninjectKernel.Rebind<IFileLocator>().To(typeof(FileLocator))
            //       .WithConstructorArgument("rootPath", "~/UploadFiles"); 

            //如果要开启多标签，或修改默认皮肤，请恢复以下代码,并修改第二个参数
            ninjectKernel.Rebind<UserConfig>().ToSelf()
            .WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示 页，请设置为true
            .WithPropertyValue("Theme", "blue")  //系统默认皮肤
            .WithPropertyValue("GridLineStyle", GridLineStyle.Horizental); //表格线设置


            //todo: 额外的注入代码
            ninjectKernel.Bind<IMenuExtInfoService>().To<TempMenuExtInfoService>();
        }
    }
}