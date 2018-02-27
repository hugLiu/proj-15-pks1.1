using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PKS.Core
{
    /// <summary>异常数据</summary>
    public class ExceptionModel
    {
        /// <summary>异常编码</summary>
        public string Code { get; set; }
        /// <summary>异常信息(面向用户)</summary>
        public string Message { get; set; }
        /// <summary>异常明细(面向调用者)</summary>
        public string Details { get; set; }
    }
}