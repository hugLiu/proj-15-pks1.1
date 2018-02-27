using System;
using System.Collections.Generic;
using System.IO;
using PKS.Core;
using PKS.Models;
using PKS.Utils;

namespace PKS.Services
{
    /// <summary>文件格式服务</summary>
    public interface IFileFormatService
    {
        /// <summary>获得全部文件格式</summary>
        List<FileFormat> GetAll();
    }

    /// <summary>文件格式扩展</summary>
    public static class FileFormatExtension
    {
        /// <summary>文件格式集合</summary>
        public static List<FileFormat> Values { get; private set; }
        /// <summary>文件格式映射</summary>
        private static Dictionary<string, FileFormat> Mapper { get; } = new Dictionary<string, FileFormat>(StringComparer.OrdinalIgnoreCase);
        /// <summary>Mime前缀</summary>
        private static readonly string MimePrefix = "MIME_";
        /// <summary>扩展名前缀</summary>
        private static readonly string ExtPrefix = "EXT_";
        /// <summary>初始化</summary>
        public static void Init()
        {
            if (Values != null) return;
            lock (Mapper)
            {
                if (Values != null) return;
                var values = Bootstrapper.Get<IFileFormatService>().GetAll();
                foreach (var value in values)
                {
                    Mapper[MimePrefix + value.MediaType] = value;
                    foreach (var ext in value.Ext)
                    {
                        Mapper[ExtPrefix + ext] = value;
                    }
                }
                Mapper[MimePrefix] = Mapper[ExtPrefix];
                Values = values;
            }
        }
        /// <summary>重载</summary>
        public static void Reload()
        {
            Values = null;
            Init();
        }
        /// <summary>获得文件扩展名</summary>
        public static string GetExtension(this string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
        }
        /// <summary>获得文件格式</summary>
        public static FileFormat GetFileFormat(this string ext)
        {
            ext = ext.TrimStart('.');
            var result = Mapper.GetValueBy(ExtPrefix + ext);
            if (result == null) result = Mapper.GetValueBy(ExtPrefix);
            return result;
        }
        /// <summary>根据MIME类型获得文件格式</summary>
        public static FileFormat GetFileFormatFromMime(this string mediaType)
        {
            var result = Mapper.GetValueBy(MimePrefix + mediaType);
            if (result == null) result = Mapper.GetValueBy(MimePrefix);
            return result;
        }
        /// <summary>根据文件扩展名获得数据格式的MIME类型</summary>
        public static string GetMediaType(this string ext)
        {
            return ext.GetFileFormat().MediaType;
        }
        /// <summary>根据文件名获得数据格式的MIME类型</summary>
        public static string GetMediaTypeFromFile(this string fileName)
        {
            if (fileName == null) return null;
            return fileName.GetExtension().GetFileFormat().MediaType;
        }
    }
}
