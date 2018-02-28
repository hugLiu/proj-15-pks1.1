using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jurassic.CommonModels;
using Jurassic.CommonModels.EFProvider;
using Ninject;
using PKS.Web.MVC;
using System.Data.Common;
using System.Configuration;
using System.Web;
using System;
using System.IO;
using System.Net;
using Common.Logging;
using PKS.Web;
using PKS.Data;
using Jurassic.AppCenter;

namespace PKS.PortalMgmt
{
    public class Global : Jurassic.WebFrame.MvcApplication
    {
        /// <summary>WEB启动器</summary>
        protected static PKSWebBootstrapper s_Bootstrapper { get; set; } = new PKSWebBootstrapper();
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                var list = base.ControllerNameSpaces.ToList();
                //声明自身Controller所在的命名空间
                list.Insert(0, typeof(Jurassic.WebFrame.MvcApplication).Namespace + ".Controllers");
                return list;
            }
        }

        protected override void Application_Start()
        {
            base.Application_Start();
            PKSWebBootstrapper.Kernel = SiteManager.Kernel;
            s_Bootstrapper.Initialize();
            PKSMvcConfig.RegisterGlobalFiltersForPortalMgmt(GlobalFilters.Filters);
            PKS.Core.Bootstrapper.Get<ILog>().Info(nameof(Application_Start));
        }

        /// <summary>
        /// 加入注入服务绑定
        /// </summary>
        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);
            var settings = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (settings.IsOracleClient())
            {
                var builder = new DbConnectionStringBuilder();
                builder.ConnectionString = settings.ConnectionString;
                var schema = builder["USER ID"].ToString();
                //要支持Oralce数据库，请在""中填写Oralce库的Schema名称
                ninjectKernel.Rebind<ModelContext>().ToSelf()
                .WithPropertyValue("Schema", schema);
            }

            //如果要修改上传根目录，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<IFileLocator>().To(typeof(FileLocator))
            //       .WithConstructorArgument("rootPath", "D:\\Temp"); 

            //如果要开启多标签，或修改默认皮肤，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<UserConfig>().ToSelf()
            //.WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示页，请设置为true
            //.WithPropertyValue("Theme", "blue");  //系统默认皮肤

            //额外的注入代码
            ninjectKernel.Rebind<IStateProvider>().ToConstant(new PKSStateProvider());
        }
        /// <summary>处理应用程序错误</summary>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
            this.HandleApplicationError();
        }
        /// <summary>停止</summary>
        protected override void Application_End()
        {
            this.LogApplicationEnd();
            base.Application_End();
        }
    }

}