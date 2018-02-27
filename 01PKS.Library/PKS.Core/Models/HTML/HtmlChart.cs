using System.Collections.Generic;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>HTML图</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class HtmlChart
    {
        /// <summary>构造函数</summary>
        public HtmlChart() { }
        /// <summary>参数</summary>
        public HtmlChartSettings Setting { get; set; }
        /// <summary>列头集合</summary>
        public List<HtmlTableColumn> Columns { get; set; }
        /// <summary>数据行集合</summary>
        public List<List<object>> Rows { get; set; }
        /// <summary>初始化</summary>
        public void Init()
        {
            this.Setting = new HtmlChartSettings();
            this.Columns = new List<HtmlTableColumn>();
            this.Rows = new List<List<object>>();
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>HTML图参数</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class HtmlChartSettings
    {
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>是否平滑</summary>
        public bool Smooth { get; set; }
        /// <summary>默认图表类型</summary>
        public string DefautChart { get; set; }
        /// <summary>图表类型集合</summary>
        public List<string> Chart { get; set; }
        /// <summary>图例集合</summary>
        public List<string> Legend { get; set; }
        /// <summary>X轴字段</summary>
        public string XAxisField { get; set; }
        /// <summary>X轴标题</summary>
        public string XAxisCaption { get; set; }
        /// <summary>Y轴标题</summary>
        public string YAxisCaption { get; set; }
        /// <summary>初始化</summary>
        public void Init()
        {
            this.Chart = new List<string>();
            this.Legend = new List<string>();
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}