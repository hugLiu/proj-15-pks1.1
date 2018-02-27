using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Resources;
using Jurassic.AppCenter.Logs;
using Ninject;
//using Jurassic.CommonModels.Organization;
using Jurassic.CommonModels;
using Jurassic.CommonModels.EFProvider;
using Jurassic.CommonModels.Articles;
using System.IO;
using Jurassic.CommonModels.FileRepository;
using Jurassic.Com.DB;
using System.Diagnostics;
using Jurassic.WebFrame.Models;
using System.Text.RegularExpressions;
using System.Data.Entity;
using Jurassic.CommonModels.ServerAuth;
using Jurassic.CommonModels.Organization;
using Jurassic.CommonModels.Messages;
using Jurassic.CommonModels.Schedule;
using Ninject.Web.Common;
using System.Reflection;

namespace Jurassic.WebFrame
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// MVC应用程序的标准基类，应用程序必须继承此类
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        internal static List<Assembly> Assemblys { get; } = new List<Assembly>();

        static MvcApplication()
        {
            NinjectWebCommon.Start();
        }
        /// <summary>
        /// 应用程序启动事件方法
        /// </summary>
        protected virtual void Application_Start()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var logInfo = new JLogInfo
            {
                LogType = JLogType.Info.ToString(),
                Message = "Application_Start",
                ActionName = "Application_Start",
                ModuleName = "MvcApplication"
            };

            GetAllAssemblys();

            ResHelper.CombinAssemblyResx(Assemblys);
            AddBindings(SiteManager.Kernel);

            AppManager.Instance.UserProvider = SiteManager.Kernel.Get<IDataProvider<AppUser>>();
            AppManager.Instance.RoleProvider = SiteManager.Kernel.Get<IDataProvider<AppRole>>();
            AppManager.Instance.StateProvider = SiteManager.Kernel.Get<IStateProvider>();
            SiteManager.Init();

            SiteManager.Get<UserConfigStorage<UserConfig>>();
            var resFileWriter = new ResFileWriter();
            resFileWriter.WriteResFiles();
            sw.Stop();
            logInfo.Costs = sw.ElapsedMilliseconds;

            LogHelper.Write(logInfo);
            //初始化日程表的基础数据栏目
            SiteManager.Catalog.InitStaticCatalogs(typeof(ScheduleEvent));
            AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            SiteManager.Catalog.InitStaticCatalogs(typeof(MessageRoot));
            //开启数据库更新
            //if (!DbSchemaVersionManager.GetInstance.ValidateVersion())
            //{
            //    DbSchemaVersionManager.GetInstance.UpdateDbSchemaToMaxVersion();
            //}
        }

        private void GetAllAssemblys()
        {
            foreach (var ns in InnerControllerNamespaces)
            {

                Assembly ass = Assembly.Load(ns.EndsWith(".Controllers") ? ns.Remove(ns.LastIndexOf('.')) : ns);
                Assemblys.Add(ass);
            }
        }

        private string _fileRootPath = @"~/WebFrameFile";
        /// <summary>
        /// 使用Ninject用于本系统的自定义接口注入
        /// </summary>
        /// <param name="ninjectKernel"></param>
        protected virtual void AddBindings(IKernel ninjectKernel)
        {
            ninjectKernel.Bind<IAuditDataService<JLogInfo>>().To(
                RefHelper.LoadType("Jurassic.CommonModels.EFProvider.LogProvider"));
            ninjectKernel.Bind<IDataProvider<AppUser>>().To(
                RefHelper.LoadType("Jurassic.CommonModels.EFProvider.MyUserProvider"));
            ninjectKernel.Bind<IDataProvider<AppRole>>().To(
                RefHelper.LoadType("Jurassic.CommonModels.EFProvider.MyRoleProvider"));
            ninjectKernel.Bind<IStateProvider>().To(
                RefHelper.LoadType("Jurassic.CommonModels.EFProvider.MyStateProvider"));
            ninjectKernel.Bind<IDataProvider<Base_Catalog>>().To(
                RefHelper.LoadType("Jurassic.CommonModels.EFProvider.CatalogProvider"));
            ninjectKernel.Bind<CatalogManager>().ToSelf().InSingletonScope();

            ninjectKernel.Bind<IAuditDataService<Base_CatalogArticle>>().To(
                RefHelper.LoadType("Jurassic.CommonModels.EFProvider.ArticleProvider"));

            ninjectKernel.Bind<LogManager>().ToSelf().InSingletonScope();

            ninjectKernel.Bind<ICacheProvider<Stream>>().To(
                 RefHelper.LoadType("Jurassic.AppCenter.Caches.StreamCacheProvider,Jurassic.AppCenter.Core"));

            //string connstrOrKey, string collectionName
            //_ninjectKernel.Bind<ICacheProvider<Stream>>().To(
            //       RefHelper.LoadType("Jurassic.MongoDb.StreamCacheProvider"))
            //        .InSingletonScope()
            //             .WithConstructorArgument("connstrOrKey", "mongoDb")
            //             .WithConstructorArgument("collectionName", "WebTemplateDB");

            ninjectKernel.Bind<IJLogManager>().To(
                   RefHelper.LoadType("Jurassic.Log4.JLogManager")).WithConstructorArgument("logName", "MyLog");

            //ninjectKernel.Bind<ArticleManager>().ToSelf().InThreadScope();

            //ninjectKernel.Bind<LogHelper>().ToSelf().WithConstructorArgument("logName", "MyLog");

            //组织管理注入配置--------------------------------------------------------------
            //统一了ModelContext以后，以下写法已没有必要
            //ninjectKernel.Bind<IAuditDataService<Dep_Department>>().To(
            //RefHelper.LoadType(
            //    "Jurassic.CommonModels.EFProvider.OrgEFAuditDataService`1[[Jurassic.CommonModels.Organization.Dep_Department, Jurassic.CommonModels]],Jurassic.CommonModels.EFProvider"));

            //ninjectKernel.Bind<IAuditDataService<UserProfile>>().To<EFAuditDataService<UserProfile>>();
            //ninjectKernel.Bind<IAuditDataService<Base_ArticleText>>().To<EFAuditDataService<Base_ArticleText>>();
            ninjectKernel.Bind(typeof(IAuditDataService<>)).To(typeof(EFAuditDataService<>));
            //组织结构管理注入配置
            ninjectKernel.Bind<IOrganizationProvider>().To<OrganizationProvider>();

            //服务授权管理注入配置
            ninjectKernel.Bind<IServerAuthProvider>().To<ServerAuthProvider>();
            ninjectKernel.Bind<IDataAuthorizeProvider>().To<DataAuthorizeProvider>();
            ninjectKernel.Bind<IServiceInfoProvider>().To<ServiceInfoProvider>();

            SiteManager.Kernel.Bind<IFileLocator>().To(typeof(FileLocator))
                         .WithConstructorArgument("rootPath", _fileRootPath);

            SiteManager.Kernel.Bind<IFileRepository>().To(typeof(WindowsFileRepository));

            ninjectKernel.Bind<DBHelper>().ToSelf()
                .WithConstructorArgument("connStrOrName", "DefaultConnection");

            ninjectKernel.Bind<DbContext, ModelContext>().To<ModelContext>()
             .InRequestScope() //在一个请求中只生成一个Context, webframe 3.0新增
                               //.InThreadScope()
                               // 以下的一句代码是连接数据库时声明数据库的Schema.
                               // 其中"WEBFRAME"是Oracle库的Schema名称，如果直接运行WebFrame,它默认是连接oracle库,如果运行WebTemplate,它是连sqlserver
                               //.WithPropertyValue("Schema", "PMDB");
                .WithPropertyValue("Schema", "dbo");

            //注册本框架特有的基于内嵌资源的视图引擎
            //ninjectKernel.Bind<ResViewEngine>().ToSelf()
            //    .WithPropertyValue("ResNamespaces", innerControllerSpaces);

            ninjectKernel.Bind<UserConfig>().ToSelf()
            .WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示页，请设置为true
            .WithPropertyValue("Theme", "blue") //系统默认皮肤
            .WithPropertyValue("GridLineStyle", GridLineStyle.Horizental); //默认表格线样式

            ninjectKernel.Bind<UserConfigStorage<UserConfig>>().ToSelf().InSingletonScope();
        }

        /// <summary>
        /// 查找所有插件程序集，并返回带.Controllers后缀的命名空间列表
        /// </summary>
        protected virtual IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

                //为单元测试能通过，加上这一判断
                if (!Directory.Exists(dir))
                {
                    dir = AppDomain.CurrentDomain.BaseDirectory;
                }

                foreach (var asmFile in new DirectoryInfo(dir).GetFiles())
                {
                    if (asmFile.Name.StartsWith("Jurassic.Web", StringComparison.OrdinalIgnoreCase)
                        && asmFile.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return asmFile.Name.Remove(asmFile.Name.LastIndexOf('.'));
                    }
                }
            }
        }

        private string[] _innerControllerspaces;
        /// <summary>
        /// 在子类中重写以声明控制器所在的命名空间
        /// </summary>
        private string[] InnerControllerNamespaces
        {
            get
            {
                if (_innerControllerspaces == null)
                {
                    var framens = typeof(MvcApplication).Namespace;
                    var ns = ControllerNameSpaces.ToList();
                    ns.Remove(framens);
                    ns.Add(framens); // 确保主框架的程序集在末尾，这样，在调用ResFileWriter进行资源还原成文件时，它的优先级最低。
                    _innerControllerspaces = ns.ToArray();
                }
                return _innerControllerspaces;
            }
        }

        /// <summary>
        /// 应用程序启动时的初始化操作，在基类中用于从URL判断当前语种
        /// 如果在子类重写时没有调用基类方法，则会丢失此特性
        /// </summary>
        protected virtual void Application_BeginRequest()
        {
            HttpContextBase contextWrapper = new HttpContextWrapper(HttpContext.Current);

            object culture = null;
            string cultureName = null;
            RouteData routeData = RouteTable.Routes.GetRouteData(contextWrapper);

            //判断URL中有无语言参数
            if (routeData != null && routeData.Values.TryGetValue("culture", out culture))
            {
                cultureName = CommOp.ToStr(culture);
            }
            //没有则从Cookie中取
            else
            {
                cultureName = WebHelper.GetCookie("culture");
            }

            //如果还没有，则从用户浏览器传过来的语言信息判断
            if (cultureName.IsEmpty())
            {
                if (!HttpContext.Current.Request.UserLanguages.IsEmpty())
                {
                    cultureName = HttpContext.Current.Request.UserLanguages.First();
                }
            }

            //如果有明确的语言参数，则切换语言，并保存到Cookie
            if (!cultureName.IsEmpty())
            {
                try
                {
                    ResHelper.CurrentCultureName = cultureName;
                    WebHelper.SetCookie("culture", cultureName, 14);
                }
                catch
                {
                }
            }

            string baseUrl = HttpContext.Current.Request.Url.LocalPath;

            //在浏览没有语言参数的首页URL时，当用户的语言的首选语言不是中文时，自动导向英文
            //if (!cultureName.StartsWith("zh") && baseUrl == "/")
            //{
            //    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri + "en-us");
            //}
        }

        /// <summary>
        /// 用于自定义缓存过期的条件判断
        /// </summary>
        /// <param name="context"></param>
        /// <param name="custom"></param>
        /// <returns></returns>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom == "DataChanged")
            {
                return SiteManager.CacheKey;
            }
            return base.GetVaryByCustomString(context, custom);
        }
        /// <summary>停止</summary>
        protected virtual void Application_End()
        {
            NinjectWebCommon.Stop();
        }
    }
}