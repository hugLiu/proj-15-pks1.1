using System;
using System.Collections.Generic;
using System.Net;

namespace PKS.Core
{
    /// <summary>WEB异常数据</summary>
    public class WebExceptionModel
    {
        /// <summary>状态代码</summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>原因短语，允许为null，为null使用状态代码的默认值</summary>
        public string ReasonPhrase { get; set; }
        /// <summary>BadRequest</summary>
        public static WebExceptionModel BadRequest { get; } = new WebExceptionModel() { StatusCode = HttpStatusCode.BadRequest };
        /// <summary>Unauthorized</summary>
        public static WebExceptionModel Unauthorized { get; } = new WebExceptionModel() { StatusCode = HttpStatusCode.Unauthorized };
        /// <summary>服务器处理失败</summary>
        public static WebExceptionModel ServerProcessingFailed { get; } = new WebExceptionModel() { StatusCode = HttpStatusCode.Forbidden, ReasonPhrase = "Server processing failed" };
    }
}
