namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KCASE_INSTANCE_CHART
    {
        public int Id { get; set; }

        public int KCASETHEMECHARTID { get; set; }

        //[Column(TypeName = "image")]
        [Required]
        public byte[] CHART { get; set; }

        public int KCASEINSTANCEID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KCASE_INSTANCE PKS_KCASE_INSTANCE { get; set; }

        public virtual PKS_KCASE_THEME_CHART PKS_KCASE_THEME_CHART { get; set; }
    }
}
