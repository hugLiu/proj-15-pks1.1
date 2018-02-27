using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>表列</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class HtmlTableColumn
    {
        /// <summary>字段</summary>
        public string Field { get; set; } = string.Empty;
        /// <summary>标题</summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>数据类型</summary>
        public JsonDataType Type { get; set; }
        /// <summary>格式</summary>
        public string Format { get; set; }
        ///// <summary>列宽度</summary>
        //public double Width { get; set; }
        ///// <summary>水平对齐</summary>
        //public HtmlHAlign Align { get; set; }
        ///// <summary>垂直对齐</summary>
        //public HtmlVAlign VAlign { get; set; }
        /// <summary>顺序</summary>
        public int Order { get; set; }
        ///// <summary>是否支持排序</summary>
        //public bool Sortable { get; set; }
        ///// <summary>是否可见</summary>
        //public bool Visible { get; set; } = true;
        /// <summary>行跨度</summary>
        public int RowSpan { get; set; } = 1;
        /// <summary>列跨度</summary>
        public int ColSpan { get; set; } = 1;
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
