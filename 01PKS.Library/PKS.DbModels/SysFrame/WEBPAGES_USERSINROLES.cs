namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WEBPAGES_USERSINROLES")]
    public partial class WEBPAGES_USERSINROLES
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column(Order = 0)]
        public int USERID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ROLEID { get; set; }
    }
}
