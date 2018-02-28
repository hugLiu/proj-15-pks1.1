using System;
using System.Collections.Generic;
using System.Net;
using PKS.Core;

namespace PKS.WebAPI.Services
{
    /// <summary>应用数据服务WEB异常映射</summary>
    public class AppDataServiceWebExceptionMap : WebExceptionMap
    {
        /// <summary>服务名称</summary>
        public override string ServiceName { get; } = "AppDataService";
        /// <summary>加入配置</summary>
        protected override void Add()
        {
            Add(ApiServiceExceptionCodes.DataIdNotExists.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "DataId Not Found"
                });
            Add(ApiServiceExceptionCodes.FileIdNotExists.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "FileId Not Found"
                });
            Add(ApiServiceExceptionCodes.MD5VerifyFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "MD5 Verify Failed"
                });
        }
    }
}
