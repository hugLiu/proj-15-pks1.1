using System;
using System.Collections.Generic;
using System.Net;
using PKS.Core;

namespace PKS.WebAPI.Services
{
    /// <summary>服务异常代码</summary>
    public enum ApiServiceExceptionCodes
    {
        /// <summary>应用数据ID不存在</summary>
        DataIdNotExists,
        /// <summary>页面数据ID不存在</summary>
        PageIdNotExists,
        /// <summary>文件ID不存在</summary>
        FileIdNotExists,
        /// <summary>文件已经存在</summary>
        FileExists,
        /// <summary>MD5校验失败</summary>
        MD5VerifyFailed,
        /// <summary>缺少元数据标签</summary>
        MetadataTagMissing,
        /// <summary>ES服务器应答失败</summary>
        ESServerFailed,
        /// <summary>资源键不存在</summary>
        ResourceKeyNotExists,
    }
}
