namespace Jurassic.CommonModels.Articles
{
    using Jurassic.AppCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 栏目的扩展属性，在根据此栏目创建新对象时，
    /// 对象的扩展属性完全根据它所在的栏目扩展属性集生成
    /// </summary>
    [Table("BASE_CATALOGEXT")]
    public partial class Base_CatalogExt : StateEntity, IIdNameBase<int>
    {
        /// <summary>
        /// 创建一个新的栏目扩展属性
        /// </summary>
        public Base_CatalogExt()
        {
        }

        /// <summary>
        /// 主栏目ID
        /// </summary>
        [Column("CATALOGID")]
        public int CatalogId { get; set; }

        /// <summary>
        /// 扩展属性名称
        /// </summary>
        [Column("NAME")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Column("DATATYPE")]
        public ExtDataType DataType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Column("DEFAULTVALUE")]
        [StringLength(200)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 提供给用户选项的数据源类型
        /// </summary>
        [Column("DATASOURCETYPE")]
        public ExtDataSourceType DataSourceType { get; set; }

        /// <summary>
        /// 用户选项的数据源描述
        /// </summary>
        [Column("DATASOURCE")]
        public string DataSource { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        [Column("MAXLENGTH")]
        public int MaxLength { get; set; }

        ///// <summary>
        ///// 最小长度，默认0
        ///// </summary>
        //public int MinLength { get; set; }

        /// <summary>
        /// 是否允许空值
        /// </summary>
        [Column("ALLOWNULL")]
        public bool AllowNull { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public object MaxValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public object MinValue { get; set; }

        /// <summary>
        /// 排序位
        /// </summary>
        [Column("ORD")]
        public int Ord { get; set; }
    }
}
