using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PKS.Models
{
    /// <summary>元数据标签类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MetadataTagType
    {
        #region 标签类型
        /// <summary>字符串</summary>
        String,
        /// <summary>
        /// 字符串数组
        /// </summary>
        StringArray,
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// ISO date
        /// </summary>
        ISODate
        #endregion
    }

    /// <summary>元数据标签格式</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MetadataTagFormat
    {
        #region 标签格式
        /// <summary>字符串</summary>
        String,
        /// <summary>
        /// ISO日期
        /// </summary>
        ISODate,
        /// <summary>
        /// 字符串 ，表示Base64流的字符串格式
        /// </summary>
        Base64String,
        /// <summary>
        /// 字符串 ，表示空间数据的WKT(well known text)格式
        /// </summary>
        WKTString,
        /// <summary>
        /// 字符串 ， 表示一个字符串数组 string[]
        /// </summary>
        Enum,
        #endregion
    }

    /// <summary>
    /// 元数据标签前台展示类型
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MetadataUiType
    {
        #region 标签前台展示类型
        /// <summary>
        /// 标签
        /// </summary>
        Label,
        /// <summary>
        /// 文本框
        /// </summary>
        TextBox,
        /// <summary>
        /// 文本域
        /// </summary>
        TextArea,
        /// <summary>
        /// 列表
        /// </summary>
        List,
        /// <summary>
        /// 下拉框
        /// </summary>
        DropdownList,
        /// <summary>
        /// 多文本框
        /// </summary>
        TagEditor,
        /// <summary>
        /// 日期
        /// </summary>
        Datetime,
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        #endregion
    }

    /// <summary>元数据标签枚举值项</summary>
    [Table("PKS_METADATAITEM")]
    public class MetadataValueItem
    {
        #region 标签枚举值项
        /// <summary>Id</summary>
        public int Id { get; set; }
        /// <summary>MetaId</summary>
        [JsonIgnore]
        public int MetaId { get; set; }
        /// <summary>文本</summary>
        public string Text { get; set; }
        /// <summary>值</summary>
        public string Value { get; set; }
        /// <summary>是否默认选择</summary>
        public bool Selected { get; set; }
        /// <summary>Meta</summary>
        [JsonIgnore]
        public virtual MetadataDefinition Meta { get; set; }
        #endregion
    }

    /// <summary>元数据标签分组代码</summary>
    public static class MetadataGroupCode
    {
        #region 分组代码
        /// <summary>内部标签</summary>
        public const string Inner = "kmd";
        /// <summary>源</summary>
        public const string Source = "kmd.source";
        /// <summary>DC</summary>
        public const string DC = "kmd.dc";
        /// <summary>EP</summary>
        public const string EP = "kmd.ep";
        #endregion
    }

    /// <summary>元数据定义</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    [Table("PKS_METADATADEFINITION")]
    public class MetadataDefinition
    {
        /// <summary>ID</summary>
        //[JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>描述</summary>
        public string Description { get; set; }
        /// <summary>是否必须的</summary>
        public bool Required { get; set; }
        /// <summary>标签类型</summary>
        public string Type { get; set; }
        /// <summary>格式</summary>
        public string Format { get; set; }
        /// <summary>是否内置标签</summary>
        public bool InnerTag { get; set; }
        /// <summary>前台展示类型</summary>
        public string UiType { get; set; }
        /// <summary>分组代码</summary>
        public string GroupCode { get; set; }
        /// <summary>分组名称</summary>
        public string GroupName { get; set; }
        /// <summary>分组顺序</summary>
        public int? GroupOrder { get; set; }
        /// <summary>标签顺序</summary>
        public int? ItemOrder { get; set; }
        /// <summary>父ID</summary>
        public int? PId { get; set; }
        /// <summary>数据源</summary>
        public string DataSource { get; set; }
        /// <summary>数据源</summary>
        public bool? CanSearch { get; set; }

        /// <summary>数据源</summary>
        public double? SearchWeight { get; set; }
        /// <summary>标签值集合</summary>
        public virtual List<MetadataValueItem> Items { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
