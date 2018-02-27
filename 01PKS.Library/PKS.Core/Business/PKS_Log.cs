using System;
using PKS.Web;

namespace PKS.Models
{
    /// <summary>日志</summary>
    public class PKS_Log : IParameterValidation
    {
        /// <summary>主键</summary>
        public int Id { get; set; }
        /// <summary>系统</summary>
        public string System { get; set; }
        /// <summary>日志级别</summary>
        public string LogLevel { get; set; }
        /// <summary>创建时间</summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>日志消息</summary>
        public string Message { get; set; }
        /// <summary>请求信息</summary>
        public string Request { get; set; }
        /// <summary>用户信息</summary>
        public string Principal { get; set; }
        /// <summary>异常来源</summary>
        public string ExSource { get; set; }
        /// <summary>异常内容</summary>
        public string ExContent { get; set; }
        /// <summary>异常数据</summary>
        public string ExData { get; set; }
    }
}
