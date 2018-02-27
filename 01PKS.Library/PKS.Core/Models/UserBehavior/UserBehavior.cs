using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;
using System.ComponentModel.DataAnnotations;

namespace PKS.Models
{
    /// <summary></summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class UserBehavior: IParameterValidation
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public UserBehavior() { }
        #endregion

        #region IUserBehavior接口
        /// <summary>索引ID,元数据唯一标识</summary>
        [Required(AllowEmptyStrings = false)]
        public string IIId { get; set; }
        /// <summary>日志时间</summary>
        public DateTime LogDate { get; set; }
        /// <summary>日志ID</summary>
        public string LogId { get; set; }
        /// <summary>操作类型</summary>
        public string Type { get; set; }
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>展示页路径</summary>
        public string Url { get; set; }
        /// <summary>引用页路径</summary>
        public string Referer { get; set; }
        /// <summary>系统</summary>
        public string System { get; set; }
        /// <summary>用户</summary>
        public string User { get; set; }
        /// <summary>角色</summary>
        public string Role { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
        #endregion
    }
}
