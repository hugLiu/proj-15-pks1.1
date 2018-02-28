using Jurassic.PKS.Service;
using System;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>服务支持能力数据</summary>
    public class ServiceCapabilities
    {
        /// <summary>服务信息</summary>
        public ServiceInfo Service { get; set; }
        /// <summary>支持请求集合</summary>
        public string[] Requests { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}