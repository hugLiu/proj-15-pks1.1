namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KCASE_INSTANCE_PARAMETER
    {
        public int Id { get; set; }

        public int KCASETHEMEPARAMETERID { get; set; }

        [Required]
        [StringLength(1000)]
        public string PARAMETERVALUE { get; set; }

        [StringLength(1000)]
        public string SAMPLEDATA { get; set; }

        [StringLength(1000)]
        public string REMARK { get; set; }

        public int KCASEINSTANCEID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KCASE_INSTANCE PKS_KCASE_INSTANCE { get; set; }

        public virtual PKS_KCASE_THEME_PARAMETER PKS_KCASE_THEME_PARAMETER { get; set; }
    }
}
