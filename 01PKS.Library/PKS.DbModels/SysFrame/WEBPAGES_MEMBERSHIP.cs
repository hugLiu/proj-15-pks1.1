namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WEBPAGES_MEMBERSHIP")]
    public partial class WEBPAGES_MEMBERSHIP
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int USERID { get; set; }

        public DateTime? CREATEDATE { get; set; }

        [StringLength(128)]
        public string CONFIRMATIONTOKEN { get; set; }

        public bool ISCONFIRMED { get; set; }

        public DateTime? LASTPASSWORDFAILUREDATE { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        [Required]
        [StringLength(128)]
        public string PASSWORD { get; set; }

        public DateTime? PASSWORDCHANGEDDATE { get; set; }

        [Required]
        [StringLength(128)]
        public string PASSWORDSALT { get; set; }

        [StringLength(128)]
        public string PASSWORDVERIFICATIONTOKEN { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
    }
}
