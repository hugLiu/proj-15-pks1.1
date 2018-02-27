using PKS.Services;
using System;
using System.Collections.Generic;

namespace PKS.Models
{
    /// <summary>子系统名称</summary>
    public static class PKSSubSystems
    {
        /// <summary>门户站点</summary>
        public static readonly string Portal = "PORTAL";
        /// <summary>搜索站点</summary>
        public static readonly string SOOIL = "SOOIL";
        /// <summary>WebAPI站点</summary>
        public static readonly string WEBAPI = "WEBAPI";
        /// <summary>勘探协同研究环境</summary>
        public static readonly string SZXT = "SZXT";
        /// <summary>勘探研究知识库</summary>
        public static readonly string SZZSK = "SZZSK";
        /// <summary>门户后端管理系统</summary>
        public static readonly string PORTALMGMT = "PORTALMGMT";
        /// <summary>GIS系统</summary>
        public static readonly string GIS = "GIS";
        /// <summary>论坛系统</summary>
        public static readonly string Forum = "Forum";
    }

    /// <summary>子系统信息</summary>
    public interface IPKSSubSystemInfo
    {
        /// <summary>代码</summary>
        string Code { get; }
        /// <summary>名称</summary>
        string Name { get; }
        /// <summary>URL</summary>
        string RootUrl { get; set; }
    }

    /// <summary>子系统配置接口</summary>
    public interface IPKSSubSystemConfig
    {
        /// <summary>当前子系统代码</summary>
        string CurrentCode { get; }
        /// <summary>当前子系统信息</summary>
        IPKSSubSystemInfo CurrentInfo { get; }
        /// <summary>获得某个子系统信息</summary>
        IPKSSubSystemInfo GetInfo(string code);
        /// <summary>子系统URL，键是系统代码，值是URL</summary>
        Dictionary<string, string> Urls { get; }

        /// <summary>获得子系统URL</summary>
        string GetUrl(string code);
    }
}