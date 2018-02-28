using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>前端脚本错误</summary>
    public class JavaScriptError : IParameterValidation
    {
        /// <summary>信息</summary>
        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }
        /// <summary>内容</summary>
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}