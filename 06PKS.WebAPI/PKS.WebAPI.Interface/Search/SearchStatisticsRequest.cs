using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>搜索统计请求</summary>
    public class SearchStatisticsRequest : IParameterValidation
    {
        /// <summary>分组统计字段数组</summary>
        [CollectionRequired]
        public List<string> Groups { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
