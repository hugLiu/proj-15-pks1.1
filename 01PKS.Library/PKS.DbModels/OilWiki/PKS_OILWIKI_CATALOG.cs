namespace PKS.DbModels.OilWiki
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_OILWIKI_CATALOG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PKS_OILWIKI_CATALOG()
        {
            //PKS_OILWIKI_CATALOG1 = new HashSet<PKS_OILWIKI_CATALOG>();
            PKS_OILWIKI_ENTRY = new HashSet<PKS_OILWIKI_ENTRY>();
        }
    
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(255)]
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// ¸Ã×Ö¶Î´æ´¢Ä¿Â¼Í¼±êbase64±àÂëµÄ×Ö·û´®
        /// </summary>
        public string IMAGEURL { get; set; }

        public int LEVELNUMBER { get; set; }

        public int ORDERNUMBER { get; set; }

        [StringLength(1000)]
        public string KMD { get; set; }

        public int? PARENTID { get; set; }

        public int DOMAINID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<PKS_OILWIKI_CATALOG> PKS_OILWIKI_CATALOG1 { get; set; }

        //public virtual PKS_OILWIKI_CATALOG PKS_OILWIKI_CATALOG2 { get; set; }

        [JsonIgnore]
        public virtual PKS_OILWIKI_DOMAIN PKS_OILWIKI_DOMAIN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<PKS_OILWIKI_ENTRY> PKS_OILWIKI_ENTRY { get; set; }
    }
}
