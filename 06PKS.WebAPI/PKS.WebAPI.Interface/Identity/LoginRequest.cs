using System.ComponentModel.DataAnnotations;
using PKS.Models;
using PKS.Utils;
using System;

namespace PKS.WebAPI.Models
{
    /// <summary>登录请求</summary>
    public class LoginRequest
    {
        /// <summary>
        /// 当前系统代码
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>用户名称</summary>
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }
        /// <summary>用户密码</summary>
        public string Password { get; set; }
        /// <summary>认证方式</summary>
        public AuthenticationType AuthenticationType { get; set; }
        /// <summary>
        /// 客户端Host
        /// </summary>
        public string UserHostAddress { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}