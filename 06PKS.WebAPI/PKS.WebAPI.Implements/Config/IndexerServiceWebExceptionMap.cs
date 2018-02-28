using System;
using System.Collections.Generic;
using System.Net;
using PKS.Core;

namespace PKS.WebAPI.Services
{
    /// <summary>索引服务WEB异常映射</summary>
    public class IndexerServiceWebExceptionMap : WebExceptionMap
    {
        /// <summary>服务名称</summary>
        public override string ServiceName { get; } = "IndexerService";
        /// <summary>加入配置</summary>
        protected override void Add()
        {
            Add(ApiServiceExceptionCodes.MetadataTagMissing.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "MetadataTag Missing"
                });
        }
    }
}
