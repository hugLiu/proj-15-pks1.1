using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using PKS.Models;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引数据查询请求</summary>
    public class IndexDataMatchRequest : IPager, IParameterValidation
    {
        /// <summary>返回结果索引</summary>
        [Range(0, int.MaxValue)]
        public int From { get; set; } = 0;
        /// <summary>返回结果数量</summary>
        [Range(0, int.MaxValue)]
        public int Size { get; set; } = 10;
        /// <summary>查询条件</summary>
        public Dictionary<string, object[]> Filter { get; set; }
        /// <summary>排序(键是字段,值是顺序,0是升序1是降序)</summary>
        public List<PKSKeyValuePair<string, int>> Sort { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>索引相关数据查询结果</summary>
    public class IndexDataMatchResult<T>
    {
        /// <summary>总数</summary>
        public int Total { get; set; }
        /// <summary>索引相关数据集合</summary>
        public List<T> Values { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}