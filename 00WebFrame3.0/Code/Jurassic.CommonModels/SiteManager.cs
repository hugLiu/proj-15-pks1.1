using Jurassic.AppCenter;
using Jurassic.AppCenter.Caches;
using Jurassic.AppCenter.Logs;
using Jurassic.Com.DB;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.Messages;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Jurassic.CommonModels
{
    /// <summary>
    /// 应用程序管理类，用此类简化对一些常规业务逻辑的访问
    /// </summary>
    public static class SiteManager
    {
        /// <summary>
        /// OutputCache的Key, 修改这个属性控制缓存整体失效
        /// </summary>
        public static string CacheKey { get; set; }

        static IKernel kernel = new StandardKernel();

        /// <summary>
        /// Ninject在本项目中的容器实例
        /// </summary>
        public static IKernel Kernel
        {
            get { return kernel; }
        }

        /// <summary>
        /// 获取Ninject容器中的指定类型对象。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>指定类型的对象</returns>
        public static T Get<T>()
        {
            return Kernel.TryGet<T>();
        }

        /// <summary>
        /// 获取Ninject容器中的指定类型对象的非泛型版本。
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>指定类型的对象</returns>
        public static object Get(Type type)
        {
            return Kernel.Get(type);
        }

        private static CatalogManager mCatalogManager;

        /// <summary>
        /// 初始化日志对象和栏目文章对象
        /// </summary>
        public static void Init()
        {
            LogHelper.Init(kernel.Get<IJLogManager>(), "MyLog");

            if (Catalog == null || StreamCache == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static DBHelper _dbHelper;
        /// <summary>
        /// 系统默认的DBHelper
        /// </summary>
        public static DBHelper DefaultDBHelper
        {
            get
            {
                if (_dbHelper == null)
                {
                    _dbHelper = kernel.Get<DBHelper>();
                }
                return _dbHelper;
            }
        }

        /// <summary>
        /// 设置用户必须在第一次登录时修改密码
        /// </summary>
        public static bool MustChangePasswordFirst { get; set; }

        /// <summary>
        /// 栏目的管理对象
        /// </summary>
        public static CatalogManager Catalog
        {
            get
            {
                if (mCatalogManager == null)
                {
                    mCatalogManager = kernel.Get<CatalogManager>();
                }
                return mCatalogManager;
            }
        }

        private static LogManager mLog;
        /// <summary>
        /// 系统的日志对象
        /// </summary>
        public static LogManager Log
        {
            get
            {
                if (mLog == null)
                {
                    mLog = kernel.Get<LogManager>();
                }
                return mLog;
            }
        }

        private static Lazy<MessageManager> mMessage = new Lazy<MessageManager>(() => kernel.Get<MessageManager>());

        /// <summary>
        /// 通用消息管理对象
        /// </summary>
        public static MessageManager Message
        {
            get
            {
                return mMessage.Value;
            }
        }

        private static ICacheProvider<Stream> mStreamCache;
        /// <summary>
        /// 基于文件流的字典对象
        /// </summary>
        public static ICacheProvider<Stream> StreamCache
        {
            get
            {
                if (mStreamCache == null)
                {
                    mStreamCache = //new L2Cache<Stream>(new StreamCacheProvider(),
                                    kernel.Get<ICacheProvider<Stream>>();
                }
                return mStreamCache;
            }
        }
    }
}