namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_TermKeyword
    {
        [Key]
        [Column("TERMCLASSID", Order = 0)]
        public int TermClassID { get; set; }

        [Key]
        [StringLength(100)]
        [Column("KEYWORD", Order = 1)]
        public string Keyword { get; set; }

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

        public virtual SD_CCTerm SD_CCTerm { get; set; }
    }
}
