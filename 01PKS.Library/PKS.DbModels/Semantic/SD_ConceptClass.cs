namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_ConceptClass
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public SD_ConceptClass()
        //{
        //    SD_CCTerm = new HashSet<SD_CCTerm>();
        //}

        [Key]
        [StringLength(255)]
        [Column("CCCODE")]
        public string CCCode { get; set; }

        [StringLength(255)]
        [Column("CC")]
        public string CC { get; set; }

        [StringLength(20)]
        [Column("TAG")]
        public string Tag { get; set; }

        [StringLength(50)]
        [Column("TYPE")]
        public string Type { get; set; }

        [StringLength(255)]
        [Column("SOURCE")]
        public string Source { get; set; }

        [StringLength(200)]
        [Column("REMARK")]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_CCTerm> SD_CCTerm { get; set; }
    }
}
