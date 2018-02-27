namespace Jurassic.CommonModels.Articles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BASE_ARTICLERELATION")]
    public partial class Base_ArticleRelation
    {
        [Column("ID")]
        [Key]
        public int Id
        {
            get;
            set;
        }
        
        [ForeignKey("Source")]
         
        [Column("SOURCEID")]
        public int SourceId { get; set; }
        
        [ForeignKey("Target")]
        [Column("TARGETID")]
        public int TargetId { get; set; }

        [Column("RELATIONTYPE")]
        public int RelationType { get; set; }
        [Column("ORD")]
        public int Ord { get; set; }


        public virtual Base_Article Source { get; set; }

        public virtual Base_Article Target { get; set; }
    }
}
