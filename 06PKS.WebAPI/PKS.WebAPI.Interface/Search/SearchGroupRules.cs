using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>分组规则</summary>
    public class SearchGroupRules
    {
        /// <summary>最多的分组数量</summary>
        public int Top { get; set; }
        /// <summary>分组字段数组</summary>
        public List<string> Fields { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
