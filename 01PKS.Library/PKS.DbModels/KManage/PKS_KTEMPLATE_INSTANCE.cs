namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KTEMPLATE_INSTANCE
    {
        public int Id { get; set; }

        public int KTEMPLATEID { get; set; }

        [Required]
        [StringLength(100)]
        public string INSTANCE { get; set; }

        [Required]
        [StringLength(50)]
        public string INSTANCECLASS { get; set; }

        [StringLength(255)]
        public string STATICURL { get; set; }

        public DateTime? STATICDATE { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KTEMPLATE PKS_KTEMPLATE { get; set; }
    }
}
