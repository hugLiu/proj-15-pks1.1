using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引操作结果</summary>
    public class IndexResult : IParameterValidation
    {
        /// <summary>构造函数</summary>
        public IndexResult()
        {
            this.IIIds = new List<string>();
        }
        /// <summary>元数据ID集合</summary>
        [CollectionRequired]
        public List<string> IIIds { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}