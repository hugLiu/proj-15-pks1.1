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
        /// ���µĹ������¼���
        /// </summary>
        /// <example>
        /// ���͵������磺
        /// һ����̳�����Ӻ���������ɻظ�֮��Ĺ�ϵ��
        /// </example>
        [ForeignKey("SourceId")]
        public ICollection<Base_ArticleRelation> Targets { get; set; }

        /// <summary>
        /// ���������ڻ�ȡ�ӽ��洫������������¼���, ����Щ���������Ҫ
        /// �����ݿ��н���Update������
        /// </summary>
        /// <remarks>
        /// Targets��һ���ܵļ��ϣ������и��ָ����Ĺ�ϵ��
        /// �������ܿ��ǣ�Targets�е�������Щ�ǲ���Ҫ�ٴ�update�ġ�
        /// ������һ�������������Ҫ�ӽ�������һЩȷ��ֵ���ر������µ��������Լ�
        /// ��������ʵ�ʳ־û�ʱ����ҪUpdate��
        /// ����Ӧ���ȷŵ��˼����У�ȷ��������ʵ�ʸ��¡�
        /// <example>
        /// ���͵�������: һ�ݹ��ﵥ�����幺�ﵥ���������������ϸ��
        /// �����ڽ��������û�¼�룬���ύʱ��Ҫ������ϸ����ϸ��Ϣ��
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
