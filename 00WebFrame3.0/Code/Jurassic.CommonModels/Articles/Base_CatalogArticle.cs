using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Articles
{
    [Table("BASE_CATALOGARTICLE")]
    public class Base_CatalogArticle 
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("CATALOGID")]
        public int CatalogId { get; set; }

        [Column("ARTICLEID")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }

        public virtual Base_Article Article { get; set; }

        [Column("ORD")]
        public int Ord { get; set; }

    }
}
