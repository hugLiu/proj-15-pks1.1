using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>源字段过滤条件</summary>
    public class SearchSourceFilter
    {
        /// <summary>包含字段数组</summary>
        public List<string> Includes { get; set; }
        /// <summary>排除字段数组</summary>
        public List<string> Excludes { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
