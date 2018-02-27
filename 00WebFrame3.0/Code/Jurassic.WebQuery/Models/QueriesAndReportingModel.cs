using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.WebQuery.Models
{
    //自定义的查询
    public class QueriesAndReportingModel
    {
        //用户选择的表，包括表中选择的显示字段。
        public IList<SettingTableModel> Tables { get; set; }
        public IList<SettingFieldModel> Fields { get; set; }
        public IList<JoinSettingModel> JoinSettings { get; set; }
        public IList<WhereSettingModel> WhereSettings { get; set; }
        public IList<OrderSettingModel> OrderSettings { get; set; }

        public IList<ComboSettingModel> ComboSettings { get; set; }
    }
    //用户选择的表
    public class SettingTableModel
    {
        public string Id { get; set; }
        public string ENName { get; set; }
    }
    //用户选择的显示字段
    public class SettingFieldModel
    {
        public string Id { get; set; }
        public string TableId { get; set; }
        public string TableENName { get; set; }
        public string ENName { get; set; }
    }

    //join连接的用户设置模型
    public class JoinSettingModel
    {
        public string LeftTableENName { get; set; }
        public string LeftAttributeENName { get; set; }
        public string Connectors { get; set; }
        public string RightTableENName { get; set; }
        public string RightAttributeENName { get; set; }
    }
    //where的用户设置的模型
    public class WhereSettingModel
    {
        public string LeftTableENName { get; set; }
        public string LeftAttributeENName { get; set; }
        /// <summary>
        /// 和左边条件的连接
        /// </summary>
        public string Connectors { get; set; }
        /// <summary>
        /// 选择的表，用户自定义输入文本的时候为null
        /// </summary>
        public string RightTableENName { get; set; }
        /// <summary>
        /// 选择的字段，或者用户自定义输入。判断条件为RightTableENName是否为null
        /// </summary>
        public string RightAttributeENName { get; set; }
        /// <summary>
        /// 和上一条Where语句的连接，第一条Where这个参数为null
        /// </summary>
        public string Operator { get; set; }
    }

    public class ComboSettingModel
    {
        /// <summary>
        /// 选择的表，用户自定义输入文本的时候为null
        /// </summary>
        public string RightTableENName { get; set; }
        /// <summary>
        /// 选择的字段，或者用户自定义输入。判断条件为RightTableENName是否为null
        /// </summary>
        public string RightAttributeENName { get; set; }
    }


    //Order排序的用户设置模型
    public class OrderSettingModel
    {
        public string TableENName { get; set; }
        public string AttributeENName { get; set; }
        public string SortBy { get; set; }
    }
}
