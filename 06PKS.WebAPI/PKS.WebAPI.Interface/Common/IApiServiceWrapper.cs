using System;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Services
{
    /// <summary>API服务配置接口</summary>
    public interface IApiServiceConfig : ITokenProvider
    {
        /// <summary>地址</summary>
        string Url { get; }
    }

    /// <summary>API服务配置接口</summary>
    public interface IApiServiceWrapper
    {
        /// <summary>重置服务URL</summary>
        void ResetServiceUrl();
    }
}