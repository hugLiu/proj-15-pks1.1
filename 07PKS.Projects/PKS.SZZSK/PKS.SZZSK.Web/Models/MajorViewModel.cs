using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.SZZSK.Web.Models
{
    public class MajorViewModel
    {
        public object Data { get; set; }
        public int PageNum { get; set; }
        public int Size { get; set; }
        public long Total { get; set; }
    }
}