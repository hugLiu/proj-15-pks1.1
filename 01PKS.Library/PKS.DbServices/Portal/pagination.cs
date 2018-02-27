using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PKS.DbServices.Portal
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class Pagination<T> where T:class,new()
    {
        /// <summary>
        /// 数据
        /// </summary>
        [JsonIgnore]
        public List<T> data{get;set;}
        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonIgnore]
        public int total{get;set;}     
    }
}
