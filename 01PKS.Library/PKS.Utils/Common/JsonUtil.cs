using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PKS.Utils
{
    /// <summary>成员名称小写命名策略</summary>
    public class LowerCaseNamingStrategy : NamingStrategy
    {
        /// <summary>
        /// Resolves the specified property name.
        /// </summary>
        /// <param name="name">The property name to resolve.</param>
        /// <returns>The resolved property name.</returns>
        protected override string ResolvePropertyName(string name)
        {
            return name.ToLowerInvariant();
        }
    }
    /// <summary>JSON工具</summary>
    public static class JsonUtil
    {
        #region 序列化参数方法
        /// <summary>静态构造函数</summary>
        static JsonUtil()
        {
            CamelCaseJsonSerializerSettings = CreateSettings(new CamelCaseNamingStrategy());
            //LowerCaseJsonSerializerSettings = CreateSettings(new LowerCaseNamingStrategy());
        }
        /// <summary>静态构造函数</summary>
        private static JsonSerializerSettings CreateSettings(NamingStrategy namingStrategy)
        {
            var contractResolver = new DefaultContractResolver();
            contractResolver.NamingStrategy = namingStrategy;
            contractResolver.IgnoreSerializableAttribute = true;
            //contractResolver.IgnoreSerializableInterface = true;
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = contractResolver;
            return settings;
        }
        /// <summary>支持成员名称CamelCase命名策略的序列化参数</summary>
        public static JsonSerializerSettings CamelCaseJsonSerializerSettings { get; private set; }

        /// <summary>创建默认参数</summary>
        public static JsonSerializerSettings CreateDefault()
        {
            return CamelCaseJsonSerializerSettings;
        }
        /// <summary>支持成员名称小写命名策略的序列化参数</summary>
        private static JsonSerializerSettings LowerCaseJsonSerializerSettings { get; set; }
        /// <summary>设置默认为支持成员名称CamelCase命名策略的序列化参数</summary>
        public static void DefaultUseCamelCaseNamingStrategy()
        {
            var settigns = CreateDefault();
            settigns.NullValueHandling = NullValueHandling.Ignore;
            JsonConvert.DefaultSettings = CreateDefault;
        }
        /// <summary>设置默认为支持成员名称CamelCase命名策略的序列化参数</summary>
        public static void UseCamelCaseNamingStrategy(this JsonSerializerSettings settigns)
        {
            settigns.NullValueHandling = NullValueHandling.Ignore;
            var contractResolver = settigns.ContractResolver.As<DefaultContractResolver>();
            contractResolver.NamingStrategy = CamelCaseJsonSerializerSettings.ContractResolver.As<DefaultContractResolver>().NamingStrategy;
            contractResolver.IgnoreSerializableAttribute = true;
        }
        #endregion

        #region 序列化方法
        /// <summary>生成指定格式的JSON串</summary>
        public static string ToJson(this object value, Formatting formatting = Formatting.Indented)
        {
            var settings = new JsonSerializerSettings() { Formatting = formatting };
            return JsonConvert.SerializeObject(value, settings);
        }
        /// <summary>根据指定参数生成JSON串</summary>
        public static string ToJson(this object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
        /// <summary>根据JSON串生成对象</summary>
        public static object JsonTo(this string value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject(value, settings);
        }
        /// <summary>根据JSON串生成对象</summary>
        public static T JsonTo<T>(this string value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
        #endregion

        #region 转换为对象方法
        /// <summary>转换为对象</summary>
        public static object ToObject(object value)
        {
            var jToken = value.As<JToken>();
            return jToken == null ? value : ToObject(jToken);
        }
        /// <summary>转换为对象</summary>
        public static object ToObject(JToken jToken)
        {
            object result = null;
            switch (jToken.Type)
            {
                case JTokenType.Array:
                    result = ToObjectArray(((JArray)jToken));
                    break;
                case JTokenType.Object:
                    result = ToObjectDictionary(((JObject)jToken));
                    break;
                case JTokenType.Property:
                    result = ToObject(((JProperty)jToken).Value);
                    break;
                case JTokenType.Comment:
                case JTokenType.Constructor:
                case JTokenType.None:
                case JTokenType.Null:
                case JTokenType.Undefined:
                    break;
                default:
                    result = ((JValue)jToken).Value;
                    break;
            }
            return result;
        }
        /// <summary>转换为对象数组</summary>
        public static object[] ToObjectArray(JArray jArray)
        {
            return jArray.Select(ToObject).ToArray();
        }
        /// <summary>转换为对象字典</summary>
        public static Dictionary<string, object> ToObjectDictionary(JObject jObject)
        {
            return ((IDictionary<string, JToken>)jObject).ToDictionary(pair => pair.Key, pair => ToObject(pair.Value));
        }
        #endregion
    }
}
