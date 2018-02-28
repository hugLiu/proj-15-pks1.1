using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PKS.Models;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引插入请求</summary>
    public class IndexInsertRequest : IParameterValidation
    {
        /// <summary>元数据集合</summary>
        [CollectionRequired]
        public MetadataCollection Metadatas { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}