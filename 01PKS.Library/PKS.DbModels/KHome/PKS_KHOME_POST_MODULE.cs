namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KHOME_POST_MODULE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KHOME_POST_MODULE()
        {
            PKS_KHOME_POST_MODULE_FILTER = new HashSet<PKS_KHOME_POST_MODULE_FILTER>();
            PKS_KHOME_USER_MODULE = new HashSet<PKS_KHOME_USER_MODULE>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        public int ORDER { get; set; }

        public int ROLEID { get; set; }

        public int KHOMEMODULEID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KHOME_MODULE PKS_KHOME_MODULE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KHOME_POST_MODULE_FILTER> PKS_KHOME_POST_MODULE_FILTER { get; set; }

        public virtual WEBPAGES_ROLES WEBPAGES_ROLES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KHOME_USER_MODULE> PKS_KHOME_USER_MODULE { get; set; }
    }
}
