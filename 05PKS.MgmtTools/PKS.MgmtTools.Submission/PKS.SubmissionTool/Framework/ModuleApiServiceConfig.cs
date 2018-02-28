using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using PKS.Core;
using PKS.Models;
using PKS.SubmissionTool.Index;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.SubmissionTool
{
    /// <summary>API服务配置</summary>
    public class ModuleApiServiceConfig : IApiServiceConfig, ISingletonAppService
    {
        /// <summary>API服务配置</summary>
        public XmlApiServiceConfig Config { get; set; }
        /// <summary>服务地址</summary>
        public string Url
        {
            get
            {
                if (this.Config == null) return string.Empty;
                if (this.Config.Url.IsNullOrEmpty()) return string.Empty;
                return this.Config.Url.NormalizeUrl() + "api/";
            }
        }
        /// <summary>登录结果</summary>
        public LoginResult LoginResult { get; set; }
        /// <summary>从登录结果中获得令牌</summary>
        public string GetToken(object context)
        {
            return this.LoginResult?.Token;
        }
    }
}