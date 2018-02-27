namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SMT_PTContextView
    {
        [Key]
        [Column(Order = 0)]
        public Guid TermClassId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string PT { get; set; }

        public string BD { get; set; }

        public string UBD { get; set; }

        public string BT { get; set; }

        public string UBT { get; set; }

        public string BP { get; set; }

        public string UBP { get; set; }

        public string BA { get; set; }

        public string UBA { get; set; }

        public string DS { get; set; }

        public string UDS { get; set; }

        public string UPT { get; set; }

        public string TL { get; set; }

        public string GN { get; set; }

        public string BF { get; set; }

        public string BS { get; set; }

        public string BOT { get; set; }

        public string BDId { get; set; }

        public string UBDId { get; set; }

        public string BTId { get; set; }

        public string UBTId { get; set; }

        public string BPId { get; set; }

        public string UBPId { get; set; }

        public string BAId { get; set; }

        public string UBAId { get; set; }

        public string DSId { get; set; }

        public string UDSId { get; set; }

        public string UPTId { get; set; }

        public string TLId { get; set; }

        public string GNId { get; set; }

        public string BFId { get; set; }

        public string BSId { get; set; }

        public string BOTId { get; set; }
    }
}
