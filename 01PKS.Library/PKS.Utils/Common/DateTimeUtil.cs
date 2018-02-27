using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace PKS.Utils
{
    /// <summary>日期时间工具</summary>
    public static class DateTimeUtil
    {
        /// <summary>标准时间格式</summary>
        public static readonly string StandardFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>生成标准时间格式串</summary>
        public static string ToStandardString(this DateTime value)
        {
            return value.ToString(StandardFormat);
        }
        /// <summary>生成标准时间格式串</summary>
        public static string ToStandardString(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString(StandardFormat) : null;
        }
        /// <summary>标准日期格式</summary>
        public static readonly string StandardDateFormat = "yyyy-MM-dd";
        /// <summary>生成标准日期格式串</summary>
        public static string ToStandardDateString(this DateTime value)
        {
            return value.ToString(StandardDateFormat);
        }
        /// <summary>标准时间格式</summary>
        private static readonly string[] StandardFormats = new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-dd", "yyyy-M-dd", "yyyy-MM-d", "yyyy-M-d", "yyyyMMdd" };
        /// <summary>生成标准时间格式串</summary>
        public static DateTime ToStandardDateTime(this string value)
        {
            try
            {
                return DateTime.ParseExact(value, StandardFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception ex)
            {
                ex.Data["DateTimeValue"] = value;
                throw;
            }
        }
        /// <summary>生成标准时间格式串</summary>
        public static DateTime? ToNullableStandardDateTime(this string value)
        {
            if (value.IsNullOrEmpty()) return null;
            return ToStandardDateTime(value);
        }
        /// <summary>生成标准时间</summary>
        public static DateTime? TryParseStandardString(this string value)
        {
            DateTime dtValue;
            if (DateTime.TryParseExact(value, StandardFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue)) return dtValue;
            return null;
        }
        /// <summary>ISO日期时间格式</summary>
        public static string ISODateTimeFormat => @"yyyy-MM-dd\THH:mm:ss.fff\Z";
        /// <summary>
        /// 将ISODate字符串转为本地时间的<c>DateTime</c>
        /// </summary>
        /// <param name="isoDateTimeString">ISO Date格式的字符串，如：2012-11-02T07:58:51.718Z</param>
        /// <returns><c>代表本地时间的<c>DateTime</c></c></returns>
        public static DateTime? ToISODate(this string isoDateTimeString)
        {
            if (isoDateTimeString.IsNullOrEmpty()) return null;
            DateTime dt;
            var succeed = DateTime.TryParseExact(isoDateTimeString
                , new string[] { @"yyyy-MM-dd\THH:mm:ss\Z", ISODateTimeFormat, "o" }
                , CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dt);
            return succeed ? new DateTime?(dt.ToLocalTime()) : null;
        }
        /// <summary>生成标准时间</summary>
        public static DateTime ToISODateTime(this string value)
        {
            try
            {
                return DateTime.ParseExact(value, ISODateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception ex)
            {
                ex.Data["ISODateTimeValue"] = value;
                throw;
            }
        }
        /// <summary>
        /// 将本地时间转为ISODate格式的字符串
        /// </summary>
        /// <param name="localTime">代表本地时间的<c>DateTime</c></param>
        /// <returns>ISODate格式的字符串，格式如：2012-11-02T07:58:51.718Z</returns>
        public static string ToISODateString(this DateTime localTime)
        {
            return localTime.ToUniversalTime().ToString(ISODateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}
