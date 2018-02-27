using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using PKS.Utils;
using System.Net.Http.Headers;

namespace PKS.Web
{
    /// <summary>WEB工具</summary>
    public static class WebUtil
    {
        /// <summary>获得URL中的域名URL(类似http://www.temp.com/)</summary>
        public static string GetDomainUrl(this Uri url)
        {
            var builder = new UriBuilder(url.Scheme, url.Host, url.Port);
            return builder.Uri.AbsoluteUri;
        }
        /// <summary>删除查询串(类似http://www.temp.com/a/b)</summary>
        public static Uri RemoveQueryString(this Uri url)
        {
            var builder = new UriBuilder(url.Scheme, url.Host, url.Port, url.LocalPath);
            return builder.Uri;
        }
        /// <summary>删除查询串(类似http://www.temp.com/a/b)</summary>
        public static string RemoveQueryString(this string url)
        {
            return new Uri(url).RemoveQueryString().ToString();
        }
        /// <summary>生成第一个查询参数</summary>
        public static string GetFirstQueryString(this string paramName, string paramValue)
        {
            return $"?{paramName}={paramValue}";
        }
        /// <summary>生成下一个查询参数</summary>
        public static string GetNextQueryString(this string paramName, string paramValue)
        {
            return $"&{paramName}={paramValue}";
        }
        /// <summary>生成查询串参数</summary>
        public static Dictionary<string, object> BuildQueryParams(string key, object value)
        {
            var queryParams = new Dictionary<string, object>();
            queryParams.Add(key, value);
            return queryParams;
        }
        /// <summary>构造带查询串的URL(类似http://www.temp.com/Index?a=***&amp;b=***)</summary>
        public static string GetQueryUrl(this string url, Dictionary<string, object> queryParams)
        {
            if (queryParams.IsNullOrEmpty()) return url;
            var url2 = new StringBuilder(url);
            url2.Append("?");
            foreach (var pair in queryParams)
            {
                var values = pair.Value.As<IEnumerable>();
                if (values == null)
                {
                    var value = HttpUtility.UrlEncode(pair.Value.ToString());
                    url2.Append(pair.Key)
                        .Append("=")
                        .Append(value)
                        .Append("&");
                    continue;
                }
                foreach (var value in values)
                {
                    url2.Append(pair.Key)
                        .Append("=")
                        .Append(HttpUtility.UrlEncode(value.ToString()))
                        .Append("&");
                }
            }
            url2.Length -= 1;
            return url2.ToString();
        }
        /// <summary>规范化URL</summary>
        public static string NormalizeUrl(this string url)
        {
            var url2 = url.Trim();
            if (!url2.EndsWith("/")) url2 += "/";
            return url2;
        }
        /// <summary>生成媒体类型头串</summary>
        public static string BuildMediaType(this string mediaType, string charSet)
        {
            if (charSet.IsNullOrEmpty()) return mediaType;
            var header = new MediaTypeHeaderValue(mediaType);
            header.CharSet = charSet;
            return header.ToString();
        }
        /// <summary>URL编码</summary>
        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }
        /// <summary>URL解码</summary>
        public static string UrlDecode(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }
    }
}
