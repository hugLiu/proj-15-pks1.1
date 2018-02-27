namespace PKS.DbModels.OilWiki
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_OILWIKI_ALIASENTRY
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        public int ENTRYID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public PKS_OILWIKI_ENTRY PKS_OILWIKI_ENTRY { get; set; }
    }
}
