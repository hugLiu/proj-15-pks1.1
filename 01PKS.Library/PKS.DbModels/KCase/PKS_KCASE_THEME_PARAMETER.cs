namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KCASE_THEME_PARAMETER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KCASE_THEME_PARAMETER()
        {
            PKS_KCASE_INSTANCE_PARAMETER = new HashSet<PKS_KCASE_INSTANCE_PARAMETER>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(255)]
        public string DESCRIPTION { get; set; }

        public int PARAMETERTYPE { get; set; }

        [StringLength(4000)]
        public string OPTIONS { get; set; }

        [StringLength(50)]
        public string RANGE { get; set; }

        [StringLength(50)]
        public string UNIT { get; set; }

        public int KCASEPARAMETERCATEGORYID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KCASE_INSTANCE_PARAMETER> PKS_KCASE_INSTANCE_PARAMETER { get; set; }

        public virtual PKS_KCASE_PARAMETER_CATEGORY PKS_KCASE_PARAMETER_CATEGORY { get; set; }
    }
}
