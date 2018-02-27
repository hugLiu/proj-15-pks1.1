using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CacheManager.Core;
using EventBus;
using Ninject;
using PKS.Core;
using PKS.Data;
using PKS.DBModels;
using PKS.Models;
using PKS.Utils;
using PKS.Services;

namespace PKS.DbServices
{
    /// <summary>子系统配置</summary>
    public class PKSSubSystemConfig : AppService, IInitializable, IPKSSubSystemConfig
    {
        /// <summary>构造函数</summary>
        public PKSSubSystemConfig()
        {
            CurrentCode = ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_SubSystem];
            BracketPatternUtil.Add(new CustomBracketPatternProvider(this));
        }
        /// <summary>初始化</summary>
        void IInitializable.Initialize()
        {
            this.EventBus.Register(this);
        }
        /// <summary>当前子系统代码</summary>
        public string CurrentCode { get; }

        /// <summary>当前子系统信息</summary>
        public IPKSSubSystemInfo CurrentInfo => GetInfo(CurrentCode);

        /// <summary>获得某个子系统信息</summary>
        public IPKSSubSystemInfo GetInfo(string code)
        {
            return GetService<IRepository<PKS_SUBSYSTEM>>().GetQuery().FirstOrDefault(p => p.Code == code);
        }

        /// <summary>子系统URL，键是系统代码，值是URL</summary>
        public Dictionary<string, string> Urls
        {
            get { return this.MemcachedCacher.TryGetOrAddValue<Dictionary<string, string>>(CacheConst.SubSystemUrlsKey, CacheConst.SubSystemRegion, GetCacheItem_Urls); }
        }
        /// <summary>子系统URL，键是系统代码，值是URL</summary>
        private CacheItem<object> GetCacheItem_Urls(string key, string region)
        {
            var value = GetService<IRepository<PKS_SUBSYSTEM>>().GetQuery().ToDictionary(e => e.Code, e => GetRootUrl(e));
            return new CacheItem<object>(key, region, value, ExpirationMode.None, TimeSpan.Zero);
        }
        /// <summary>获得子系统URL</summary>
        internal static string GetRootUrl(PKS_SUBSYSTEM subSystem)
        {
            // TODO : 支持本机调试(服务器[192.168.1.236]名称为WIN-VT6LM6PK4JS)
            var rootUrl = subSystem.RootUrl;
            if (rootUrl.IndexOf("192.168.1.236") > 0)
            {
                if (Environment.MachineName.ToUpperInvariant() != "WIN-VT6LM6PK4JS")
                {
                    rootUrl = rootUrl.Replace("192.168.1.236", "localhost");
                }
            }
            else if (rootUrl.IndexOf("10.138.99.231") > 0)
            {
                if (!Environment.MachineName.ToUpperInvariant().StartsWith("Z440SV08CONNJUR"))
                {
                    rootUrl = rootUrl.Replace("10.138.99.231", "127.0.0.1");
                }
            }
            return rootUrl;
        }
        /// <summary>获得子系统URL</summary>
        public string GetUrl(string code)
        {
            return this.Urls[code];
        }
        /// <summary>处理变化事件</summary>
        [EventSubscriber]
        public void OnChanged(EntityChangedEventArgs<PKS_SUBSYSTEM> e)
        {
            this.MemcachedCacher.TryRemove(CacheConst.SubSystemUrlsKey, CacheConst.SubSystemRegion);
        }

        #region 子系统方括号模式提供者
        /// <summary>子系统方括号模式提供者</summary>
        private sealed class CustomBracketPatternProvider : BracketPatternProvider
        {
            /// <summary>构造函数</summary>
            public CustomBracketPatternProvider(PKSSubSystemConfig host) : base(BracketPatternUtil.System)
            {
                this.Host = host;
            }
            /// <summary>宿主</summary>
            public PKSSubSystemConfig Host { get; }
            /// <summary>获得标签值</summary>
            public override string GetTagValue()
            {
                return this.Host.CurrentCode;
            }
            /// <summary>解析标签值</summary>
            public override string Resolve(string tagValue)
            {
                return this.Host.GetUrl(tagValue).TrimEnd('/');
            }
        }
        #endregion
    }
}