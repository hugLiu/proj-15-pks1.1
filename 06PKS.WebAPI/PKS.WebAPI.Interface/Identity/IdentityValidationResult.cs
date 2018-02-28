using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>身份验证结果</summary>
    public class IdentityValidationResult
    {
        /// <summary>是否合法</summary>
       // [JsonIgnore]
        public bool Valid { get; set; }
        /// <summary>新认证令牌</summary>
        public string Token { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}