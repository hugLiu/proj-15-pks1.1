
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>按IIID数组搜索请求</summary>
    public class SearchMetadatasRequest : IParameterValidation
    {
        /// <summary>索引ID集合</summary>
        [CollectionRequired]
        public List<string> IIIds { get; private set; } = new List<string>();
        /// <summary>源字段过滤器</summary>
        public SearchSourceFilter Fields { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
