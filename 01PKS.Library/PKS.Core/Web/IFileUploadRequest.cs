using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.Web
{
    /// <summary>文件上传请求</summary>
    public interface IFileUploadRequest
    {
        /// <summary>文件名称</summary>
        string FileName { get; set; }
        /// <summary>内容类型</summary>
        string ContentType { get; set; }
        /// <summary>服务端接收文件</summary>
        string ServerFile { get; set; }
    }
}