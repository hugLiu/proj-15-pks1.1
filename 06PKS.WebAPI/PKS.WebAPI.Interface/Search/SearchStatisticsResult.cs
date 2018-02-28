using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>搜索分组结果</summary>
    public class SearchGroupResult
    {
        /// <summary>分组统计字段</summary>
        public string Field { get; set; }
        /// <summary>分组统计字段值数量</summary>
        public int Count { get; set; }
        /// <summary>子分组统计</summary>
        public List<SearchGroupResult> Children { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>搜索统计结果</summary>
    public class SearchStatisticsResult
    {
        /// <summary>分组统计结果</summary>
        public Dictionary<string, Dictionary<string, long?>> Groups { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
