using System;
using System.Collections.Generic;
using System.Net;

namespace PKS.Core.Template
{
    /// <summary>自定义WEB异常映射</summary>
    public class CustomWebExceptionMap : WebExceptionMap
    {
        /// <summary>服务名称</summary>
        public override string ServiceName { get; } = "Custom";
        /// <summary>加入配置</summary>
        protected override void Add()
        {
            //Add(ExceptionCodes.ParameterParsingFailed.ToString(),
            //    new WebExceptionModel()
            //    {
            //        StatusCode = HttpStatusCode.Forbidden,
            //        ReasonPhrase = "Adapter Not Exist"
            //    });
        }
    }
}
