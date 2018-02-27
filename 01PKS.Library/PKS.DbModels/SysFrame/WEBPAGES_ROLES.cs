namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WEBPAGES_ROLES")]
    public partial class WEBPAGES_ROLES
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ROLEID { get; set; }

        [Required]
        [StringLength(256)]
        public string ROLENAME { get; set; }

        [StringLength(50)]
        public string DESCRIPTION { get; set; }
    }
}
