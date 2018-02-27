namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_Semantics
    {
        [Key]
        [Column("FTERMCLASSID", Order = 0)]
        public int FTermClassId { get; set; }

        [Key]
        [StringLength(100)]
        [Column("SR", Order = 1)]
        public string SR { get; set; }

        [Key]
        [Column("LTERMCLASSID", Order = 2)]
        public int LTermClassId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("FTERM")]
        public string FTerm { get; set; }

        [Required]
        [StringLength(255)]
        [Column("LTERM")]
        public string LTerm { get; set; }

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

        public virtual SD_CCTerm SD_CCTerm1 { get; set; }

        public virtual SD_SemanticsType SD_SemanticsType { get; set; }
    }
}
