namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KCASE_THEME_CHART
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KCASE_THEME_CHART()
        {
            PKS_KCASE_INSTANCE_CHART = new HashSet<PKS_KCASE_INSTANCE_CHART>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        public int CHARTTYPE { get; set; }

        [StringLength(1000)]
        public string PARAMETERS { get; set; }

        public int KCASETHEMEID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KCASE_INSTANCE_CHART> PKS_KCASE_INSTANCE_CHART { get; set; }

        public virtual PKS_KCASE_THEME PKS_KCASE_THEME { get; set; }
    }
}
