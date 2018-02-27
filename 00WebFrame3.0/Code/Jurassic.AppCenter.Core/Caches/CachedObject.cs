using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Globalization;
using System.IO;
using System.Threading;
using Jurassic.Com.Tools;
using System.Text;

namespace Jurassic.AppCenter.Caches
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用于存储并用json序列化和反序列化某个对象的缓存类
    /// 可以在缓存依赖项更改时，立即刷新缓存，在IIS进程回收时，也可以及时回写内存数据到文件
    /// </summary>
    public class CachedObject<T> : ICanSave
        where T : class,new()
    {
        T mItem;
        Cache mCache;
        protected string mBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

        private string _cfgFileName;

        /// <summary>
        /// 持久化的json数据文件
        /// </summary>
        protected virtual string CfgFileName
        {
            get
            {
                if (_cfgFileName == null)
                    _cfgFileName = Path.Combine(mBasePath, typeof(T).Name + ".json");
                return _cfgFileName;
            }
            set
            {
                _cfgFileName = value;
            }
        }

        /// <summary>
        /// 存储T的List集合
        /// </summary>
        protected T Item
        {
            get
            {
                if (mItem == null)
                {
                    ReadObject();
                    AddCache();
                }
                return mItem;
            }
            set
            {
                mItem = value;
            }
        }

        /// <summary>
        /// 指定要持久化的文件名来创建CachedObject
        /// </summary>
        /// <param name="cfgFileName">文件名,该文件名只需提供相对于App_Data文件夹的相对路径</param>
        public CachedObject(string cfgFileName = null)
        {
            if (!cfgFileName.IsEmpty())
            {
                if (cfgFileName.Contains(':'))
                {
                    CfgFileName = cfgFileName;
                }
                else
                {
                    CfgFileName = Path.Combine(mBasePath, cfgFileName);
                }
            }

            if (HttpContext.Current != null)
            {
                mCache = HttpContext.Current.Cache;
            }
            if (mCache == null)
            {
                mCache = HttpRuntime.Cache;
            }
            if (!File.Exists(CfgFileName))
            {
                mItem = new T();
                //Save();
            }
            AddCache();
        }

        /// <summary>
        /// 将列表保存到默认的文件中
        /// </summary>
        public void Save()
        {
            if (!Directory.Exists(mBasePath))
            {
                Directory.CreateDirectory(mBasePath);
            }
            inWriteBack = true;
            lock (synObj2)
            {
                File.WriteAllText(CfgFileName, JsonHelper.ToJson(mItem), Encoding.UTF8);
            }
            inWriteBack = false;
        }

        /// <summary>
        /// 将列表另存到其他文件
        /// </summary>
        /// <param name="filePath">另存到的文件的完全路径名</param>
        public void SaveAs(string filePath)
        {
            File.WriteAllText(filePath, JsonHelper.ToJson(Item));
        }

        static object synObj2 = new object();

        /// <summary>
        /// 从js文件读取List[T]清单
        /// </summary>
        void ReadObject()
        {
            lock (synObj2)
            {
                string s = File.ReadAllText(CfgFileName);
                mItem = JsonHelper.FromJson<T>(s) ?? new T();
            }
        }

        /// <summary>
        /// 标志T.js是否在回写，如果是在回写则在下面的方法中跳出
        /// 避免重复处理
        /// </summary>
        bool inWriteBack = false;

        /// <summary>
        /// 在缓存过期，重新从文件读取对象以后，与原有对象的合并操作
        /// </summary>
        public event EventHandler<CachedObjectOnMergeEventArgs<T>> OnMerge;

        /// <summary>
        /// 当缓存过期时的委托
        /// </summary>
        /// <param name="key">缓存的Key</param>
        /// <param name="value">缓存的值</param>
        /// <param name="reason">移除原因</param>
        void WriteBack(string key, object value, CacheItemRemovedReason reason)
        {
            if (destoryed) return;
            //如果是因为外部修改了文件
            if (reason == CacheItemRemovedReason.DependencyChanged && !inWriteBack)
            {
                //则将当前内存中的对象与磁盘上的对象合并
                var oldObj = mItem;
                ReadObject();
                OnMerge?.Invoke(this, new CachedObjectOnMergeEventArgs<T>(oldObj, mItem));
                //  LogHelper.SysLog("缓存文件被覆盖", "json缓存文件：更新").ModuleName = typeof(T).Name;
            }
            else
            {
                //Save(); 
            }

            AddCache();
        }

        string cacheKey = Guid.NewGuid().ToString();
        void AddCache()
        {
            mCache.Add(cacheKey, 1, new CacheDependency(CfgFileName),
                Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, new CacheItemRemovedCallback(WriteBack));
        }

        bool destoryed = false;
        public void Dispose()
        {
            if (!destoryed)
            {
                destoryed = true;
                //Save();
                mCache.Remove(cacheKey);
                Item = null;
                mCache = null;
            }
        }

        ~CachedObject()
        {
            Dispose();
        }
    }
}