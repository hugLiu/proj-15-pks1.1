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
    public class UploadRequest
    {
        /// <summary>多个上传文件的MD5</summary>
        public string[] Md5 { get; set; }
        /// <summary>文件分片上传时的唯一标识</summary>
        public string Guid { get; set; }

        /// <summary>分片索引</summary>
        public int Chunk { get; set; } = -1;
        /// <summary>分片数量</summary>
        public int Chunks { get; set; } = -1;
        /// <summary>文件名称</summary>
        public string FileName { get; set; }
        /// <summary>字符集</summary>
        public string CharSet { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>服务端文件上传请求</summary>
    public class ServerUploadRequest : UploadRequest
    {
        /// <summary>文件上传请求</summary>
        public ServerUploadRequest() { }
        /// <summary>文件上传请求</summary>
        public ServerUploadRequest(UploadRequest request)
        {
            if (request == null) return;
            this.Md5 = request.Md5;
            this.Guid = request.Guid;
            this.Chunk = request.Chunk;
            this.Chunks = request.Chunks;
            this.FileName = request.FileName;
            this.CharSet = request.CharSet;
        }

        /// <summary>服务端接收文件</summary>
        public string ServerFile { get; set; }
        /// <summary>离线上传文件路径</summary>
        public string RelativePath { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>上传结果</summary>
    public class UploadResult
    {
        /// <summary>临时文件ID集合</summary>
        public string[] TempFileIds { get; set; }
        /// <summary>第一个文件ID</summary>
        public string FirstFileId
        {
            get
            {
                if (this.TempFileIds.IsNullOrEmpty()) return null;
                return this.TempFileIds[0];
            }
        }
        /// <summary>分片索引</summary>
        public int Chunk { get; set; } = -1;
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}