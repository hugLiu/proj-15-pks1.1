namespace PKS.DbModels.OilWiki
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_OILWIKI_ENTRY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_OILWIKI_ENTRY()
        {
            PKS_OILWIKI_ALIASENTRY = new HashSet<PKS_OILWIKI_ALIASENTRY>();
            PKS_OILWIKI_RELATEDENTRY = new HashSet<PKS_OILWIKI_RELATEDENTRY>();
        }

        public int Id { get; set; }

        public int CATALOGID { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(200)]
        public string ENGLISHNAME { get; set; }

        [Required]
        public string CONTENTS { get; set; }

        [StringLength(100)]
        public string AUTHOR { get; set; }

        [StringLength(255)]
        public string SOURCE { get; set; }

        public string IMAGE { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_OILWIKI_ALIASENTRY> PKS_OILWIKI_ALIASENTRY { get; set; }

        public virtual PKS_OILWIKI_CATALOG PKS_OILWIKI_CATALOG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PKS_OILWIKI_RELATEDENTRY> PKS_OILWIKI_RELATEDENTRY { get; set; }

       
    }
}
