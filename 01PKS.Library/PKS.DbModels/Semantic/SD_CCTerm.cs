namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_CCTerm
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public SD_CCTerm()
        //{
        //    SD_Semantics = new HashSet<SD_Semantics>();
        //    SD_Semantics1 = new HashSet<SD_Semantics>();
        //    SD_TermKeyword = new HashSet<SD_TermKeyword>();
        //    SD_TermTranslation = new HashSet<SD_TermTranslation>();
        //}

        [Key]
        [Column("TERMCLASSID")]
        public int TermClassID { get; set; }

        [StringLength(255)]
        [Column("CCCODE")]
        public string CCCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("TERM")]
        public string Term { get; set; }

        [StringLength(10)]
        [Column("LANGCODE")]
        public string LangCode { get; set; }

        [StringLength(1000)]
        [Column("PATHTERM")]
        public string PathTerm { get; set; }

        [StringLength(255)]
        [Column("SOURCE")]
        public string Source { get; set; }

        [StringLength(500)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("ORDERINDEX")]
        public int? OrderIndex { get; set; }

        [Column("CREATEDDATE")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        [Column("CREATEDBY")]
        public string CreatedBy { get; set; }

        [Column("LASTUPDATEDDATE")]
        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        [Column("LASTUPDATEDBY")]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        [Column("REMARK")]
        public string Remark { get; set; }

        public virtual SD_ConceptClass SD_ConceptClass { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_Semantics> SD_Semantics { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_Semantics> SD_Semantics1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_TermKeyword> SD_TermKeyword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_TermTranslation> SD_TermTranslation { get; set; }
    }
}
