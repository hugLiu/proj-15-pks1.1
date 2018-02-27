namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KTEMPLATE_CATEGORY_PARAMETER
    {
        public int Id { get; set; }

        public int KTEMPLATECATEGORYID { get; set; }

        public int KTEMPLATEPARAMETERID { get; set; }

        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KTEMPLATE_CATEGORY PKS_KTEMPLATE_CATEGORY { get; set; }

        public virtual PKS_KTEMPLATE_PARAMETER PKS_KTEMPLATE_PARAMETER { get; set; }
    }
}
