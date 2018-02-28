using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nest;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>排序规则</summary>
    public class SearchSortRule
    {
        /// <summary>字段</summary>
        public string Field { get; set; }
        /// <summary>排序方向</summary>
        public SortOrder Order { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
