namespace Jurassic.CommonModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ���µ���չ����
    /// </summary>
    [Table("BASE_ARTICLEEXT")]
    public partial class Base_ArticleExt
    {
        /// <summary>
        /// ��չ����ID
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        [ForeignKey("Article")]
        [Column("ARTICLEID")]
        public int ArticleId { get; set; }

        /// <summary>
        /// ��Ŀ��չ���Ե�ID
        /// </summary>
        [Column("CATLOGEXTID")]
        public int CatlogExtId { get; set; }

        /// <summary>
        /// ��չ���Ե�ֵ
        /// </summary>
        [Column("VALUE")]
        public string Value { get; set; }

        /// <summary>
        /// ��չ�������ڵ�����
        /// </summary>
        public  Base_Article Article { get; set; }
    }
}
