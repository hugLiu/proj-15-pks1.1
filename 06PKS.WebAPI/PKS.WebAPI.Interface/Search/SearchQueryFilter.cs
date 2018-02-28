using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>查询条件</summary>
    public class SearchQueryFilter
    {
        /// <summary>AND条件</summary>
        public List<SearchTermQuery> And { get; set; }
        /// <summary>OR数组条件</summary>
        public List<List<SearchTermQuery>> Ors { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
