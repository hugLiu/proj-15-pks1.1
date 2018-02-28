using PKS.WebAPI.Models;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.SZXT.Infrastructure
{
    public static class StringExtension
    {
        public static T To<T>(this string str)
        {
            return DeserializeObject<T>(str);
        }
        public static string ToNormal(this string str)
        {
            return str.To<string>();
        }
        public static NearRequest ToNearRequest(this string str, params object[] args)
        {
            var patten = GetPatten("@");
            var jStr = str.FormatCommon(patten, args);
            return jStr.To<NearRequest>();
        }
        public static string ToEsQuery(this string str, params object[] args)
        {
            var patten =GetPatten("@");
            return str.FormatCommon(patten, args);
        }
        private static string ToEsQuery(this string str, string flag,params object[] args)
        {
            var patten = GetPatten(flag);
            return str.FormatCommon(patten, args);
        }
        public static string FormatCommon(this string str, string patten, object[] args)
        {
            var res = str;
            var reg = new Regex(patten);
            var arg = string.Empty;
            for (int i = 0, j = args.Length; i < j; i++)
            {
                arg = TryResolveFormatString(args[i]);
                res = reg.Replace(res, arg, 1);
            }
            return res;
        }
        private static string GetPatten(string flag)
        {
            return $"\"{flag}\\w+\"";
        }
        private static string TryResolveFormatString(object obj)
        {
            if (obj is string)
                return $"\"{obj}\"";
            if (IsNumber(obj))
                return obj.ToString();
            if (obj is List<string>)
                return SerializeListToEsString((List<string>)obj);
            if (obj is List<int>)
                return SerializeListToEsArray((List<int>)obj);
            return string.Empty;
        }
        private static bool IsNumber(object obj)
        {
            if (obj is int)
                return true;
            if (obj is long)
                return true;
            if (obj is float)
                return true;
            if (obj is double)
                return true;
            if (obj is decimal)
                return true;
            return false;
        }
        private static string SerializeListToEsString(List<string> lst)
        {
            var sb = new StringBuilder();
            for (int i = 0, j = lst.Count; i < j; i++)
            {
                sb.Append($"\"{lst[i]}\"");
                if (i < j - 1)
                    sb.Append(",");
            }
            return sb.ToString();
        }
        private static string SerializeListToEsArray(List<int> lst)
        {
            var sb = new StringBuilder();
            for (int i = 0, j = lst.Count; i < j; i++)
            {
                sb.Append($"\"{lst[i]}\"");
                if (i < j - 1)
                    sb.Append(",");
            }
            return sb.ToString();
        }
    }
}
