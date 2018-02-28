using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>文件上传请求</summary>
    public class UploadFileRequest
    {
        /// <summary>文件名称</summary>
        public string FileName { get; set; }
        /// <summary>是否返回模式URL</summary>
        public bool EnablePattern { get; set; }
        /// <summary>服务端接收文件</summary>
        public string ServerFile { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>文件上传结果</summary>
    public class UploadFileResult
    {
        /// <summary>文件相对路径</summary>
        public string RelativePath { get; set; }
        /// <summary>文件绝对URL</summary>
        public string FileUrl { get; set; }
        /// <summary>文件模式URL</summary>
        public string PatternUrl { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}