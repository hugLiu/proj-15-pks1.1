using System;
using System.Collections.Specialized;
using System.Configuration;
using Nest;
using Newtonsoft.Json;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.ES;

namespace PKS.WebAPI.Services
{
    /// <summary>ES扩展</summary>
    public static class ElasticExtension
    {
        /// <summary>如果应答不合法则抛出异常</summary>
        public static void ThrowIfIsNotValid(this IResponse response)
        {
            if (response != null && response.IsValid) return;
            if (response == null)
            {
                ApiServiceExceptionCodes.ESServerFailed.ThrowUserFriendly("操作失败！", "ES服务未启动或已中止!");
            }
            response.OriginalException.Throw(ApiServiceExceptionCodes.ESServerFailed, response.ServerError.ToString());
        }
    }
}