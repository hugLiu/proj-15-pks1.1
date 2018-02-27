using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Jurassic.WebUpload
{
    /// <summary>
    /// 上传结果返回给前端的Model
    /// </summary>
    public class UploadResult
    {
        public int Id { get; set; }
        public string FileKey { get; set; }
        public string UserId { get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
        public int CatalogId { get; set; }

    }

    /// <summary>
    /// 分块上传时记录的块信息
    /// </summary>
    public class ChunkInfo
    {
        public long Size { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
    }
}