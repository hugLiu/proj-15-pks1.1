using System.Collections.Generic;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>HTML表</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class HtmlTable
    {
        /// <summary>构造函数</summary>
        public HtmlTable()        {        }
        /// <summary>表名</summary>
        public string TableName { get; set; }
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>长度单位</summary>
        public string Unit { get; set; }
        /// <summary>表头集合</summary>
        public List<List<HtmlTableColumn>> Headers { get; set; }
        /// <summary>表头行高度集合</summary>
        public List<double> HeaderRowHeights { get; set; }
        /// <summary>列头集合</summary>
        public List<HtmlTableColumn> Columns { get; set; }
        /// <summary>列行高度</summary>
        public double ColumnRowHeight { get; set; }
        /// <summary>数据行集合</summary>
        public List<List<object>> Rows { get; set; }
        /// <summary>数据行高度集合</summary>
        public List<double> RowHeights { get; set; }
        /// <summary>初始化</summary>
        public void Init()
        {
            this.Headers = new List<List<HtmlTableColumn>>();
            this.HeaderRowHeights = new List<double>();
            this.Columns = new List<HtmlTableColumn>();
            this.Rows = new List<List<object>>();
            this.RowHeights = new List<double>();
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
