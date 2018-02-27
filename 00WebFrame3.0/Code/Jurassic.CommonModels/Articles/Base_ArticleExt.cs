namespace Jurassic.CommonModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 文章的扩展属性
    /// </summary>
    [Table("BASE_ARTICLEEXT")]
    public partial class Base_ArticleExt
    {
        /// <summary>
        /// 扩展属性ID
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        [ForeignKey("Article")]
        [Column("ARTICLEID")]
        public int ArticleId { get; set; }

        /// <summary>
        /// 栏目扩展属性的ID
        /// </summary>
        [Column("CATLOGEXTID")]
        public int CatlogExtId { get; set; }

        /// <summary>
        /// 扩展属性的值
        /// </summary>
        [Column("VALUE")]
        public string Value { get; set; }

        /// <summary>
        /// 扩展属性所在的文章
        /// </summary>
        public  Base_Article Article { get; set; }
    }
}
