namespace PKS.DbModels.OilWiki
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_OILWIKI_DOMAIN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_OILWIKI_DOMAIN()
        {
            PKS_OILWIKI_CATALOG = new HashSet<PKS_OILWIKI_CATALOG>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [Required]
        [StringLength(255)]
        public string DESCRIPTION { get; set; }

        [StringLength(255)]
        public string IMAGEURL { get; set; }

        [StringLength(1000)]
        public string KMD { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_OILWIKI_CATALOG> PKS_OILWIKI_CATALOG { get; set; }
    }
}
