namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KCASE_CATEGORY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KCASE_CATEGORY()
        {
            PKS_KCASE_CATEGORY1 = new HashSet<PKS_KCASE_CATEGORY>();
            PKS_KCASE_THEME = new HashSet<PKS_KCASE_THEME>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        public int LEVELNUMBER { get; set; }

        public int ORDERNUMBER { get; set; }

        public int? PARENTID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KCASE_CATEGORY> PKS_KCASE_CATEGORY1 { get; set; }

        public virtual PKS_KCASE_CATEGORY PKS_KCASE_CATEGORY2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KCASE_THEME> PKS_KCASE_THEME { get; set; }
    }
}
