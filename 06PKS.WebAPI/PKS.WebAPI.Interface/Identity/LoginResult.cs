using PKS.Models;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>登录结果</summary>
    public class LoginResult : OperationResult
    {
        /// <summary>认证令牌</summary>
        public string Token { get; set; }
        /// <summary>用户身份</summary>
        public IPKSPrincipal Principal { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}