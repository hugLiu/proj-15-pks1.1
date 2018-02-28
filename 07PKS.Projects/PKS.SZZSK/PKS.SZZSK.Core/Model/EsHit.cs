using PKS.SZZSK.Core.DTO;
using System.Collections.Generic;

namespace PKS.SZZSK.Core.Model
{
    public class EsHit
    {
        public double? total { get; set; }
        public double? max_score { get; set; }
        public List<EsItem> hits { get; set; }
    }
}
