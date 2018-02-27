namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dict_CommonView
    {
        [Key]
        [StringLength(255)]
        [Column("TERM")]
        public string term { get; set; }
    }
}
