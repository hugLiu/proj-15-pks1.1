using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PKS.WITSML
{
    public static class Common
    {
        public static T GetAttribute<T>(XElement element, string attributeName, T defaultValue)
        {
            var attr = element.Attribute(attributeName);
            if (attr == null || attr.Value == null) { return defaultValue; }
            return Convert<T>(attr.Value, defaultValue);
        }

        public static T GetElement<T>(XElement xElement, string elementName, T defaultValue)
        {
            var nameSpace = xElement.Name.Namespace;
            var element = xElement.Element(nameSpace + elementName);
            if (element == null || element.Value == null) { return defaultValue; }

            return Convert<T>(element.Value, defaultValue);
        }

        //空值解析默认值
        const double NULL_DEFAULT_DOUBLE = double.NaN;
        static readonly DateTime NULL_DEFAULT_DATETIME = DateTime.MinValue;
        static readonly string NULL_DEFAULT_STRING = string.Empty;

        public static object ConvertToType(Type type, string valueStr, string nullValue)
        {
            if (type == null) { type = typeof(string); }
            var equalNullValue = valueStr == nullValue;
            if (type == typeof(double))
            {
                if (equalNullValue) { return NULL_DEFAULT_DOUBLE; }
                return Convert<double>(valueStr, double.NaN);
            }
            else if (type == typeof(DateTime))
            {
                if (equalNullValue) { return NULL_DEFAULT_DATETIME; }
                return Convert<DateTime>(valueStr, NULL_DEFAULT_DATETIME);
            }
            else if (type == typeof(string))
            {
                if (equalNullValue) { return NULL_DEFAULT_STRING; }
                return Convert<string>(valueStr, NULL_DEFAULT_STRING);
            }
            else
            {
                throw new NotSupportedException("类型" + type + "不支持");
            }
        }

        private static T Convert<T>(string str, T defaultValue)
        {
            if (typeof(T) == typeof(string)) { return (T)(object)str; }

            if (typeof(T) == typeof(double))
            {
                double num;
                if (!double.TryParse(str, out num))
                {
                    return defaultValue;
                }
                return (T)(object)num;
            }
            if (typeof(T) == typeof(DateTime))
            {
                DateTime t;
                if (!DateTime.TryParse(str, out t))
                {
                    return defaultValue;
                }
                return (T)(object)t;
            }

            throw new NotSupportedException();

        }
    }
}
