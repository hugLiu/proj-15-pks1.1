namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KTEMPLATE_CATEGORY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KTEMPLATE_CATEGORY()
        {
            PKS_KTEMPLATE = new HashSet<PKS_KTEMPLATE>();
            PKS_KTEMPLATE_CATEGORY_PARAMETER = new HashSet<PKS_KTEMPLATE_CATEGORY_PARAMETER>();
        }

        public int Id { get; set; }

        public int SUBSYSTEMID { get; set; }

        [Required]
        [StringLength(100)]
        public string CODE { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        public int KTEMPLATEURLID { get; set; }

        [Required]
        [StringLength(50)]
        public string INSTANCECLASS { get; set; }

        [Required]
        [StringLength(50)]
        public string ROBJECT { get; set; }

        [Required]
        [StringLength(50)]
        public string GROUPNAME { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KTEMPLATE> PKS_KTEMPLATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KTEMPLATE_CATEGORY_PARAMETER> PKS_KTEMPLATE_CATEGORY_PARAMETER { get; set; }

        public virtual PKS_KTEMPLATE_URL PKS_KTEMPLATE_URL { get; set; }
    }
}
