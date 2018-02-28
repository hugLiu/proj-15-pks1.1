using Newtonsoft.Json;
using PKS.Models;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace PKS.WebAPI
{
    /// <summary>BOT场景类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BOTScenarioType
    {
        #region 类型
        /// <summary>数据</summary>
        Data,
        /// <summary>筛选</summary>
        Filter,
        /// <summary>都支持</summary>
        Both,
        #endregion
    }

    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class BOTPropertyDefinition
    {
        /// <summary>属性名</summary>
        public string Name { get; set; }
        /// <summary>属性显示名</summary>
        public string DisplayName { get; set; }
        /// <summary>属性值类型</summary>
        public MetadataTagType Type { get; set; }
        /// <summary>可选值列表</summary>
        public List<string> Options { get; set; }
        /// <summary>顺序</summary>
        public int Sequence { get; set; }
        /// <summary>应用场景类型</summary>
        public BOTScenarioType Scenario { get; set; }
    }
}
