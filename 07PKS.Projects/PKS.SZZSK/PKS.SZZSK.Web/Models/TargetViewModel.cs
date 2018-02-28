using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.Service.TargetBaiKe.Model
{
   public class TargetViewModel
    {
        public object Data { get; set; }
        public int PageNum { get; set; }
        public int Size { get; set; }
        public long Total { get; set; }
    }
}
