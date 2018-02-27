namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KHOME_USER_MODULE
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        public int? ORDER { get; set; }

        public int? USERID { get; set; }

        public int? KHOMEPOSTMODULEID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KHOME_POST_MODULE PKS_KHOME_POST_MODULE { get; set; }

        public virtual USERPROFILE USERPROFILE { get; set; }
    }
}
