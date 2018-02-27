namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KHOME_MODULE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KHOME_MODULE()
        {
            PKS_KHOME_MODULE_QUERY = new HashSet<PKS_KHOME_MODULE_QUERY>();
            PKS_KHOME_POST_MODULE = new HashSet<PKS_KHOME_POST_MODULE>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(2000)]
        public string DESCRIPTION { get; set; }

        public int? COMPONENTTYPE { get; set; }

        public int KHOMEMODULECATEGORYID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KHOME_MODULE_CATEGORY PKS_KHOME_MODULE_CATEGORY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KHOME_MODULE_QUERY> PKS_KHOME_MODULE_QUERY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KHOME_POST_MODULE> PKS_KHOME_POST_MODULE { get; set; }
    }
}
