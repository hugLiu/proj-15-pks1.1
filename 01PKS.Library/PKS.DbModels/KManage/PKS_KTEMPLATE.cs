namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KTEMPLATE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_KTEMPLATE()
        {
            PKS_KTEMPLATE_CATALOGUE = new HashSet<PKS_KTEMPLATE_CATALOGUE>();
            PKS_KTEMPLATE_INSTANCE = new HashSet<PKS_KTEMPLATE_INSTANCE>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CODE { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        public bool ISDEFAULT { get; set; }

        public int KTEMPLATECATEGORYID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        /// <summary>
        /// 编辑工具生成的字符串
        /// </summary>
        public string TEMPLATE { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KTEMPLATE_CATEGORY PKS_KTEMPLATE_CATEGORY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KTEMPLATE_CATALOGUE> PKS_KTEMPLATE_CATALOGUE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_KTEMPLATE_INSTANCE> PKS_KTEMPLATE_INSTANCE { get; set; }
    }
}
