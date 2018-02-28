using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.SZZSK.Web.Models
{
    public class CaseItem
    {
        public int Id { get; set; }
        public string IIId { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public byte[] Image { get; set; }
        public bool HasChart { get; set; }
    }
}