using System;
using System.Collections.Generic;
using PKS.MgmtServices.Converters;
using PKS.Models;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>上传文件上下文</summary>
    public class UploadFileContext
    {
        /// <summary>上传文档</summary>
        public MongoUploadFile UploadFileDoc{ get; set; }
        /// <summary>上传文档格式</summary>
        public FileFormat UploadFileFormat { get; set; }
        /// <summary>规范化的上传文档</summary>
        public MongoUploadFile NormalizedFileDoc { get; set; }
        /// <summary>规范化的上传文档格式</summary>
        public FileFormat NormalizedFileFormat { get; set; }
    }

    /// <summary>文件格式转换器</summary>
    public class FileFormatConverters
    {
        /// <summary>PDF转换器</summary>
        public IPdfConverter PdfConverter { get; set; }
        /// <summary>图片转换器</summary>
        public IImageConverter ImageConverter { get; set; }
        /// <summary>缩略图转换器</summary>
        public IThumbnailConverter ThumbnailConverter { get; set; }
        /// <summary>全文转换器</summary>
        public IFulltextConverter FulltextConverter { get; set; }
        /// <summary>图片转换器</summary>
        public IHtmlConverter HtmlConverter { get; set; }
    }
}