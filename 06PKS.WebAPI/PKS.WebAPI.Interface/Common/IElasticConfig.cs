using System;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.WebAPI.Services
{
    /// <summary>Elastic库配置接口</summary>
    public interface IElasticConfig
    {
        /// <summary>客户端</summary>
        object Client { get; }
        /// <summary>元数据</summary>
        object MetadataType { get; }
        /// <summary>用户行为日志</summary>
        object UserBehaviorType { get; }
    }
}