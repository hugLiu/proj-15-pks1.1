using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using PKS.Core;
using PKS.DbServices.SysFrame;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Services
{
    /// <summary>API服务配置</summary>
    public class ApiServiceConfig : IApiServiceConfig, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public ApiServiceConfig(IPKSSubSystemConfig config)
        {
            this.Url = config.GetUrl(PKSSubSystems.WEBAPI).NormalizeUrl() + "api/";
        }
        /// <summary>服务地址</summary>
        public string Url { get; set; }
        /// <summary>从当前会话中获得令牌</summary>
        public string GetToken(object context)
        {
            return context.As<HttpContext>()?.Session[PKSWebConsts.Session_Authentication].As<string>();
        }
    }
}