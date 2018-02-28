using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>ES原生搜索请求</summary>
    public class ESSearchRequest : IParameterValidation
    {
        /// <summary>搜索短语</summary>
        [Required(AllowEmptyStrings = false)]
        public string Query { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
