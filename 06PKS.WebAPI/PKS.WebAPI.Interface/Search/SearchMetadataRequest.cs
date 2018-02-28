using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>按IIID搜索请求</summary>
    public class SearchMetadataRequest : IParameterValidation
    {
        /// <summary>索引ID</summary>
        [Required(AllowEmptyStrings = false)]
        public string IIId { get; set; }
        /// <summary>源字段过滤器</summary>
        public SearchSourceFilter Fields { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
