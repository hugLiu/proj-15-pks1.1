namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USERPROFILE")]
    public partial class USERPROFILE
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int USERID { get; set; }

        [Required]
        [StringLength(56)]
        public string USERNAME { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string PHONENUMBER { get; set; }

        public int? AVATARID { get; set; }
    }
}
