namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VI_USERINFO")]
    public partial class VI_USERINFO
    {
        [Key]
        [Column(Order = 0)]
        public int USERID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(56)]
        public string USERNAME { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string PHONENUMBER { get; set; }

        public int? AVATARID { get; set; }

        public DateTime? CREATEDATE { get; set; }

        [StringLength(128)]
        public string CONFIRMATIONTOKEN { get; set; }

        public bool ISCONFIRMED { get; set; }

        public DateTime? LASTPASSWORDFAILUREDATE { get; set; }

        public int PASSWORDFAILURESSINCELASTSUCCESS { get; set; }

        [StringLength(128)]
        public string PASSWORD { get; set; }

        public DateTime? PASSWORDCHANGEDDATE { get; set; }

        [StringLength(128)]
        public string PASSWORDSALT { get; set; }

        [StringLength(128)]
        public string PASSWORDVERIFICATIONTOKEN { get; set; }

        public DateTime? PASSWORDVERIFICATIONTOKENEXPIRATIONDATE { get; set; }
        public int? ROLEID { get; set; }

        [StringLength(256)]
        public string ROLENAME { get; set; }

        [StringLength(50)]
        public string ROLEDESC { get; set; }
    }
}
