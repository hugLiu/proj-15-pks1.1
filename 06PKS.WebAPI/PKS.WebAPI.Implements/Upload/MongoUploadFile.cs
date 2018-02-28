using System;
using System.Collections.Generic;
using PKS.Models;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>文件转换类型</summary>
    public enum FileConvertType
    {
        /// <summary>未知</summary>
        Pdf,
        /// <summary>未知</summary>
        Image,
        /// <summary>缩略图</summary>
        Thumbnail,
        /// <summary>全文</summary>
        FullText,
        /// <summary>Html</summary>
        Html,
    }

    /// <summary>Mongo上传文件</summary>
    public class MongoUploadFile
    {
        /// <summary>文件ID</summary>
        public string FileId { get; set; }
        /// <summary>文件名称</summary>
        public string FileName { get; set; }
        /// <summary>相对上传路径</summary>
        public string RelativePath { get; set; }
        /// <summary>Http媒体类型头值</summary>
        public string ContentType { get; set; }
        /// <summary>文件MD5</summary>
        public string Md5 { get; set; }
        /// <summary>创建时间</summary>
        public DateTime CreateDate { get; set; }
        /// <summary>UTF8编码的文件ID</summary>
        public string Utf8FileId { get; set; }
        /// <summary>PDF文件ID</summary>
        public string PdfFileId { get; set; }
        /// <summary>图片文件ID</summary>
        public string ImageFileId { get; set; }
        /// <summary>缩略图文件ID</summary>
        public string ThumbnailFileId { get; set; }
        /// <summary>全文文件ID</summary>
        public string FullTextFileId { get; set; }
        /// <summary>HTML文件ID</summary>
        public string HtmlFileId { get; set; }
        /// <summary>生成JSON串</summary>createdate
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}