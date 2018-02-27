namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WEBPAGES_OAUTHMEMBERSHIP")]
    public partial class WEBPAGES_OAUTHMEMBERSHIP
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string PROVIDER { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string PROVIDERUSERID { get; set; }

        public int USERID { get; set; }
    }
}
