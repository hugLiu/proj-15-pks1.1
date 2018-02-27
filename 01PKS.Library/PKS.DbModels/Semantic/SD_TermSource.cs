namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_TermSource
    {
        [Key]
        [Column("CCCODE", Order = 0)]
        [StringLength(255)]
        public string CCCode { get; set; }

        [Key]
        [Column("SOURCE", Order = 1)]
        [StringLength(255)]
        public string Source { get; set; }

        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
    }
}
