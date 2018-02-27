using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace PKS.Utils
{
    /// <summary>字符串工具</summary>
    public static class StringUtil
    {
        #region 逻辑方法
        /// <summary>是否NULL或Empty</summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        #endregion

        #region 大小写转换方法
        /// <summary>转换为驼峰写法(JSON方式)</summary>
        public static string JsonToCamelCase(this string s)
        {
            if (s.IsNullOrEmpty() || !char.IsUpper(s[0]))
            {
                return s;
            }
            char[] array = s.ToCharArray();
            int num = 0;
            while (num < array.Length && (num != 1 || char.IsUpper(array[num])))
            {
                bool flag = num + 1 < array.Length;
                if ((num > 0 & flag) && !char.IsUpper(array[num + 1]))
                {
                    break;
                }
                char c = char.ToLower(array[num], CultureInfo.InvariantCulture);
                array[num] = c;
                num++;
            }
            return new string(array);
        }
        /// <summary>
        /// 转化字符串为首字母大写，同时会将首字符之后的转化为小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(value);
        }

        /// <summary>
        /// 字符串首字母转化大写，其他字符不变
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUpperInitial(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }
        #endregion

        #region 操作方法
        /// <summary>重复指定字符串指定次数</summary>
        public static string Repeat(this string value, int count)
        {
            var sBuilder = new StringBuilder(value.Length * count);
            for (int i = 0; i < count; i++)
            {
                sBuilder.Append(value);
            }
            return sBuilder.ToString();
        }
        #endregion

        #region 字节转换
        /// <summary>生成十六进制字符串</summary>
        public static string ToHex2(this byte value)
        {
            return value.ToString("X2");
        }
        /// <summary>生成字节数组的十六进制字符串</summary>
        public static string ToHexString(this byte[] values)
        {
            var builder = new StringBuilder(values.Length * 2);
            for (int i = 0; i < values.Length; i++)
            {
                builder.Append(values[i].ToHex2());
            }
            return builder.ToString();
        }
        #endregion

        #region 整数转换
        /// <summary>转成整数</summary>
        public static int ToInt32(this string value)
        {
            return int.Parse(value);
        }
        #endregion

        #region 枚举转换
        /// <summary>枚举转换</summary>
        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = false)
            where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse<TEnum>(value, ignoreCase, out result)) return result;
            throw new ArgumentException($"{value}不是有效的{typeof(TEnum).FullName}枚举值！");
        }
        #endregion

        #region 布尔转换
        /// <summary>布尔转换</summary>
        public static bool ToBool(this string value)
        {
            return bool.Parse(value);
        }
        #endregion

        #region Guid转换
        /// <summary>转换为GUID</summary>
        public static Guid ToGuid(this string value)
        {
            if (value.IsNullOrEmpty()) throw new ArgumentNullException(value, "GUID串不允许为空！");
            Guid guid;
            var isValid = Guid.TryParse(value, out guid);
            if (!isValid) throw new FormatException("GUID串格式无效！");
            return guid;
        }
        #endregion

        #region Base64转换
        /// <summary>Base64编码</summary>
        public static string Base64Encode(this string value, Encoding encoding)
        {
            var buffer = encoding.GetBytes(value);
            return Convert.ToBase64String(buffer);
        }
        /// <summary>Base64(UTF8)编码</summary>
        public static string Base64Utf8Encode(this string value)
        {
            return Base64Encode(value, Encoding.UTF8);
        }
        /// <summary>Base64解码</summary>
        public static string Base64Decode(this string value, Encoding encoding)
        {
            var buffer = Convert.FromBase64String(value);
            return encoding.GetString(buffer);
        }
        /// <summary>Base64(UTF8)解码</summary>
        public static string Base64Utf8Decode(this string value)
        {
            return Base64Decode(value, Encoding.UTF8);
        }
        /// <summary>查询串编码字符数组</summary>
        private static readonly char[] QueryStringChars = { '=', '&', '%' };
        /// <summary>查询串编码值数组</summary>
        private static readonly string[] QueryStringEncodeStrings = { "%61", "%38", "%37" };
        /// <summary>查询串编码</summary>
        public static string QueryStringEncode(this string value)
        {
            if (value.IsNullOrEmpty()) return value;
            if (value.IndexOfAny(QueryStringChars) < 0) return value;
            var pattern = string.Join("|", QueryStringChars);
            return Regex.Replace(value, pattern, match =>
                QueryStringEncodeStrings[Array.IndexOf(QueryStringChars, match.Value[0])]
            );
        }
        /// <summary>查询串解码</summary>
        public static string QueryStringDecode(this string value)
        {
            if (value.IsNullOrEmpty()) return value;
            if (value.IndexOf('%') < 0) return value;
            var pattern = string.Join("|", QueryStringEncodeStrings);
            return Regex.Replace(value, pattern, match =>
                QueryStringChars[Array.IndexOf(QueryStringEncodeStrings, match.Value)].ToString()
            );
        }
        #endregion

        #region 路径方法
        /// <summary>脱壳,默认为引号</summary>
        public static string Divest(this string file, char symbol = '"')
        {
            if (file.IsNullOrEmpty()) return file;
            return file.Trim().Trim(symbol);
        }
        /// <summary>规范化路径</summary>
        public static string NormalizePath(this string path)
        {
            return path.Trim().Replace('/', Path.DirectorySeparatorChar);
        }
        /// <summary>规范化相对路径</summary>
        public static string NormalizeRelativePath(this string path)
        {
            var path2 = path.NormalizePath();
            if (!path2.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path2 = Path.DirectorySeparatorChar + path2;
            }
            return path2;
        }
        #endregion
    }
}
