using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>JSON数据类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum JsonDataType
    {
        /// <summary>字符串</summary>
        [JsonProperty("string")]
        String,
        /// <summary>bool</summary>
        [JsonProperty("boolean")]
        Boolean,
        /// <summary>数字</summary>
        [JsonProperty("number")]
        Number,
        /// <summary>日期</summary>
        [JsonProperty("datetime")]
        Date,
    }

    /// <summary>水平对齐</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HtmlHAlign
    {
        /// <summary>默认</summary>
        [JsonProperty("")]
        Default,
        /// <summary>左对齐</summary>
        [JsonProperty("left")]
        Left,
        /// <summary>右对齐</summary>
        [JsonProperty("booleanright")]
        Right,
        /// <summary>居中对齐</summary>
        [JsonProperty("center")]
        Center,
        /// <summary>两端对齐</summary>
        [JsonProperty("justify")]
        Justify,
    }

    /// <summary>垂直对齐</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HtmlVAlign
    {
        /// <summary>默认</summary>
        [JsonProperty("")]
        Default,
        /// <summary>顶对齐</summary>
        [JsonProperty("top")]
        Top,
        /// <summary>底对齐</summary>
        [JsonProperty("bottom")]
        Bottom,
        /// <summary>居中对齐</summary>
        [JsonProperty("middle")]
        Middle,
    }
}
