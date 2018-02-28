using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>字段准确匹配查询</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SearchTermQueryOperator
    {
        /// <summary>相等</summary>
        Equal,
        /// <summary>在数组内</summary>
        In,
        /// <summary>在数值范围内</summary>
        Range,
        /// <summary>是否存在</summary>
        Exits,
        /// <summary>前缀</summary>
        Prefix,
        /// <summary>通配符</summary>
        Wildcard,
        /// <summary>正则</summary>
        Regex,
        /// <summary>相似度</summary>
        Fuzzy,
    }

    /// <summary>字段准确匹配查询</summary>
    public class SearchTermQuery
    {
        /// <summary>取反</summary>
        public bool Not { get; set; }
        /// <summary>字段名称</summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        /// <summary>包含字段数组</summary>
        public SearchTermQueryOperator Operator { get; set; }
        /// <summary>字段值</summary>
        public string Value { get; set; }
        /// <summary>用于需要数组的操作</summary>
        public List<string> Values { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
