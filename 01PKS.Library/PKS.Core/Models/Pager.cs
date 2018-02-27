using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PKS.Utils;
using System.Collections.Generic;

namespace PKS.Models
{
    /// <summary>分页参数</summary>
    public interface IPager
    {
        /// <summary>起始索引</summary>
        int From { get; }
        /// <summary>每页数量</summary>
        int Size { get; }
    }

    /// <summary>分页参数</summary>
    public class Pager : IPager
    {
        /// <summary>构造函数</summary>
        public Pager() { }
        /// <summary>构造函数</summary>
        public Pager(int from, int size)
        {
            this.From = from;
            this.Size = size;
        }
        /// <summary>起始索引</summary>
        public int From { get; set; }
        /// <summary>每页数量</summary>
        public int Size { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>每页信息</summary>
    public class PageInfo
    {
        /// <summary>总数</summary>
        public int Total { get; set; }
        /// <summary>当前页数</summary>
        public int CurrentNumber { get; set; }
        /// <summary>每页数量</summary>
        public int Size { get; set; }
        /// <summary>每页数据</summary>
        public IEnumerable<object> Data { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>页扩展</summary>
    public static class PageExtension
    {
    }
}
