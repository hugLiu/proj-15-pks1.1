namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KFRAGMENT_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KFRAGMENT_TYPE()
        {
            PKS_KFRAGMENT = new HashSet<PKS_KFRAGMENT>();
            PKS_KFRAGMENT_TYPE_PARAMETER = new HashSet<PKS_KFRAGMENT_TYPE_PARAMETER>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CODE { get; set; }

        [Required]
        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string VUETAG { get; set; }

        public bool HASTEXTTEMPLATE { get; set; }
        public int ORDERNUMBER { get; set; }
        public string IMAGEURL { get; set; }
        public int? Category { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KFRAGMENT> PKS_KFRAGMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KFRAGMENT_TYPE_PARAMETER> PKS_KFRAGMENT_TYPE_PARAMETER { get; set; }
    }
}
