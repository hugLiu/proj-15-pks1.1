using System;
using System.Collections.Generic;
using System.Net;
using PKS.Core;

namespace PKS.WebAPI.Services
{
    /// <summary>页面数据服务WEB异常映射</summary>
    public class PageDataServiceWebExceptionMap : WebExceptionMap
    {
        /// <summary>服务名称</summary>
        public override string ServiceName { get; } = "PageService";
        /// <summary>加入配置</summary>
        protected override void Add()
        {
            Add(ApiServiceExceptionCodes.PageIdNotExists.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = "PageId Not Found"
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
