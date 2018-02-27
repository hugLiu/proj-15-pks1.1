namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PKS_KFRAGMENT_TYPE_PARAMETER
    {
        public int Id { get; set; }

        public int KFRAGMENTTYPEID { get; set; }

        [Required]
        [StringLength(50)]
        public string CODE { get; set; }

        [Required]
        [StringLength(50)]
        public string NAME { get; set; }

        [Required]
        [StringLength(50)]
        public string DATATYPE { get; set; }

        public string METADATA { get; set; }

        public string DEFAULTVALUE { get; set; }


        [StringLength(50)]
        public string CREATEDBY { get; set; }

        public DateTime? CREATEDDATE { get; set; }

        [StringLength(50)]
        public string LASTUPDATEDBY { get; set; }

        public DateTime? LASTUPDATEDDATE { get; set; }

        public virtual PKS_KFRAGMENT_TYPE PKS_KFRAGMENT_TYPE { get; set; }
    }
}
