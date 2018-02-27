using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PKS.Models
{
    /// <summary>认证方式</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthenticationType
    {
        /// <summary>表单认证方式</summary>
        Forms,
        /// <summary>Windows认证方式</summary>
        Windows,
        /// <summary>混合认证方式</summary>
        Mixed,
    }
}