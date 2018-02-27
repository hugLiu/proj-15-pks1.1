namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KTEMPLATE_URL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KTEMPLATE_URL()
        {
            PKS_KTEMPLATE_CATEGORY = new HashSet<PKS_KTEMPLATE_CATEGORY>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TITLE { get; set; }

        [Required]
        [StringLength(255)]
        public string URL { get; set; }

        [StringLength(50)]
        public string PAGEID { get; set; }
        
        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KTEMPLATE_CATEGORY> PKS_KTEMPLATE_CATEGORY { get; set; }
    }
}
