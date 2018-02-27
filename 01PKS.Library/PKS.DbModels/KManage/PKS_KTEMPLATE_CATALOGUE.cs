namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KTEMPLATE_CATALOGUE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KTEMPLATE_CATALOGUE()
        {
            PKS_KFRAGMENT = new HashSet<PKS_KFRAGMENT>();
            Children = new HashSet<PKS_KTEMPLATE_CATALOGUE>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CODE { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        public int LEVELNUMBER { get; set; }

        public int ORDERNUMBER { get; set; }

        public int? PARENTID { get; set; }

        public int KTEMPLATEID { get; set; }

        public string PLACEHOLDERID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KFRAGMENT> PKS_KFRAGMENT { get; set; }

        public virtual PKS_KTEMPLATE PKS_KTEMPLATE { get; set; }

        [ForeignKey("PARENTID")]
        public virtual ICollection<PKS_KTEMPLATE_CATALOGUE> Children { get; set; }
    }
}
