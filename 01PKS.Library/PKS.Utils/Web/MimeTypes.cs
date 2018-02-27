using System;

namespace PKS.Web
{
    /// <summary>HTTP应答内容类型常量</summary>
    public static class MimeTypes
    {
        /// <summary>流</summary>
        public const string Stream = "application/octet-stream";
        /// <summary>JSON</summary>
        public const string JSON = "application/json";
        /// <summary>Url</summary>
        public const string Url = "application/jurassic-url";
        /// <summary>异常</summary>
        public const string Exception = "application/jurassic-exception+json";
        /// <summary>MIME类型是否流</summary>
        public static bool IsStream(this string mediaType)
        {
            if (mediaType.StartsWith("text/", StringComparison.OrdinalIgnoreCase)) return false;
            if (mediaType.IsJsonMedia()) return false;
            return true;
        }
        /// <summary>MIME类型是否JSON数据</summary>
        public static bool IsJsonMedia(this string mediaType)
        {
            var json = "json";
            if (!mediaType.EndsWith(json, StringComparison.OrdinalIgnoreCase)) return false;
            var diff = mediaType.Length - json.Length;
            if (diff == 0) return true;
            var cvalue = mediaType[mediaType.Length - json.Length - 1];
            if (char.IsLetter(cvalue)) return false;
            return true;
        }
    }
}
