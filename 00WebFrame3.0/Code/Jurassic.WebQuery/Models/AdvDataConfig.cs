using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery.Models
{
    /// <summary>
    /// 某个业务模型类在界面上显示或录入时的的显示配置项
    /// </summary>
    public class AdvDataConfig
    {
        /// <summary>
        /// ctor：业务模型类在界面上显示或录入时的的显示配置项
        /// </summary>
        public AdvDataConfig()
        {
            Items = new List<AdvDataConfigItem>();
        }

        /// <summary>
        /// 业务模型的类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///  该配置项的子项集合
        /// </summary>
        public List<AdvDataConfigItem> Items { get; set; }
    }

    /// <summary>
    /// 业务模型类显示配置项的子项
    /// </summary>
    public class AdvDataConfigItem
    {
        /// <summary>
        /// ctor: 业务模型类显示配置项的子项
        /// </summary>
        public AdvDataConfigItem()
        {
            Rows = 1;
            Cols = 1;
            Fixed = false;
            Browsable = true;
        }

        /// <summary>
        /// 是否覆盖CatalogExtAttribute的属性
        /// </summary>
        public bool OverWrite { get; set; }
        public string DataType { get; set; }

        public string DefaultValue { get; set; }

        public string DataSourceType { get; set; }

        public string DataSource { get; set; }

        public int MaxLength { get; set; }

        public int MinLength { get; set; }

        public bool AllowNull { get; set; }

        public string MaxValue { get; set; }

        public string MinValue { get; set; }

        public string RegExpr { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 表单中的显示顺序
        /// </summary>
        public int FormOrder { get; set; }

        /// <summary>
        /// DataGrid中的显示顺序
        /// </summary>
        public int GridOrder { get; set; }

        /// <summary>
        /// 该属性录入控件在表单中占几行的高度
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 该属性录入控件在表单中占几个单元格的宽度
        /// </summary>
        public int Cols { get; set; }

        /// <summary>
        /// 在DataGrid中的列宽
        /// </summary>
        public int ColumnWidth { get; set; }

        /// <summary>
        /// 是否冻结该列
        /// </summary>
        public bool Fixed { get; set; }

        /// <summary>
        /// 该列的统计类型，sum/avg/count/max/min
        /// </summary>
        public string SummaryType { get; set; }

        public string InputFormat { get; set; }

        public string DisplayFormat { get; set; }

        public bool Browsable { get; set; }
    }
}