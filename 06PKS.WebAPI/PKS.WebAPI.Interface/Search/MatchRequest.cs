using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nest;
using Newtonsoft.Json;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>准确查询请求</summary>
    public class MatchRequest : IParameterValidation
    {
        /// <summary>返回结果数量</summary>
        public int Top { get; set; } = 1;
        /// <summary>查询条件</summary>
        public Dictionary<string, object[]> Filter { get; set; }
        /// <summary>源字段过滤器</summary>
        public SearchSourceFilter Fields { get; set; }
        /// <summary>排序(键是字段,值是顺序,0是升序1是降序)</summary>
        public List<PKSKeyValuePair<string, object>> Sort { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
