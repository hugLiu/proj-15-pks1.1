namespace Jurassic.CommonModels.Articles
{
    using Jurassic.AppCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Table("BASE_ARTICLE")]
    public partial class Base_Article : StateEntity, IIdNameBase<int>
    {
        public Base_Article()
        {
            Exts = new HashSet<Base_ArticleExt>();
            Targets = new HashSet<Base_ArticleRelation>();
            TargetArticles = new List<Base_Article>();
        }

        [Column("TITLE"),
        StringLength(200)]
        public string Title { get; set; }
        [Column("KEYWORDS")]
        [StringLength(200)]
        public string Keywords { get; set; }
        [Column("ABSTRACT")]
        [StringLength(200)]
        public string Abstract { get; set; }

        [ForeignKey("Article")]
        public virtual Base_ArticleText ArticleText { get; set; }
        
        [NotMapped]
        public string Text
        {
            get
            {
                if (ArticleText == null)
                {
                    return null;
                }
                return ArticleText.Text;
            }
            set
            {
                if (ArticleText == null)
                {
                    ArticleText = new Base_ArticleText();
                }
                ArticleText.Text = value;
            }
        }

        [Column("URLTITLE")]
        //[StringLength(50)]
        public string UrlTitle { get; set; }
        [Column("AUTHOR")]
        //[StringLength(50)]
        public string Author { get; set; }
        [Column("EDITORID")]
        public int EditorId { get; set; }
        [Column("CLICKS")]
        public int Clicks { get; set; }

        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }

        [Column("EDITTIME")]
        public DateTime EditTime { get; set; }

        //[ForeignKey("EditorId")]
        //public virtual UserProfile Editor { get; set; }
        
        [ForeignKey("ArticleId")]
        public ICollection<Base_ArticleExt> Exts { get; set; }
        
        /// <summary>
        /// 文章的关联文章集合
        /// </summary>
        /// <example>
        /// 典型的例子如：
        /// 一个论坛的贴子和下面的若干回复之间的关系。
        /// </example>
        [ForeignKey("SourceId")]
        public ICollection<Base_ArticleRelation> Targets { get; set; }

        /// <summary>
        /// 该属性用于获取从界面传过来的相关文章集合, 而这些相关文章需要
        /// 在数据库中进行Update操作。
        /// </summary>
        /// <remarks>
        /// Targets是一个总的集合，里面有各种各样的关系。
        /// 出于性能考虑，Targets中的文章有些是不需要再次update的。
        /// 而另外一类相关文章是需要从界面输入一些确切值，特别是文章的特殊属性集
        /// 而它们在实际持久化时，需要Update。
        /// 所以应该先放到此集合中，确保它们能实际更新。
        /// <example>
        /// 典型的例子如: 一份购物单，主体购物单对象下面的若干明细。
        /// 它们在界面中由用户录入，在提交时需要保存明细的详细信息。
        /// </example>
        /// </remarks>
        [NotMapped]
        public ICollection<Base_Article> TargetArticles { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }
    }
}
