using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>搜索完全匹配请求</summary>
    public class SearchRequest : IParameterValidation
    {
        /// <summary>搜索短语</summary>
        [Required(AllowEmptyStrings = false)]
        public string Sentence { get; set; }
        /// <summary>起始索引</summary>
        public int From { get; set; } = 0;
        /// <summary>返回结果数量</summary>
        public int Size { get; set; } = 10;
        /// <summary>查询条件</summary>
        public Dictionary<string, object[]> Filter { get; set; }
        /// <summary>源字段过滤器</summary>
        public SearchSourceFilter Fields { get; set; }
        /// <summary>提升权重集合(键是字段,值是权重提升)</summary>
        public Dictionary<string, int> Boost { get; set; }
        /// <summary>排序(键是字段,值是顺序,0是升序1是降序)</summary>
        public List<PKSKeyValuePair<string, object>> Sort { get; set; }
        /// <summary>分组</summary>
        public SearchGroupRules Group { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
