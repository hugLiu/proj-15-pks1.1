using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PKS.Models;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引相关数据保存请求</summary>
    public class IndexDataSaveRequest<T> : IParameterValidation
    {
        /// <summary>数据集合</summary>
        [CollectionRequired]
        public List<T> Values { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}