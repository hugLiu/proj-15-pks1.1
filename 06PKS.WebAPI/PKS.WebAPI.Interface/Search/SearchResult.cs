using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PKS.Models;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>搜索结果</summary>
    public class SearchResult : MatchResult
    {
        /// <summary>聚合结果</summary>
        public Dictionary<string, Dictionary<string, long?>> Groups { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}