using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PKS.Models;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引保存请求</summary>
    public class IndexSaveRequest : IndexInsertRequest, IParameterValidation
    {
        /// <summary>是否替换</summary>
        public bool Replace { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}