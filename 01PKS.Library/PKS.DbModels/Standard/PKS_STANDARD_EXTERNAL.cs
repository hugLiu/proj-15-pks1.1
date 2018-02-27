namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_STANDARD_EXTERNAL
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string NAME { get; set; }

        [Required]
        [StringLength(100)]
        public string TYPE { get; set; }

        [Required]
        public string URL { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }
    }
}
