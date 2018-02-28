using PKS.SZXT.Core.DTO;
using System.Collections.Generic;

namespace PKS.SZXT.Core.Model.EsRawResult
{
    public class EsHit
    {
        public double? total { get; set; }
        public double? max_score { get; set; }
        public List<EsItem> hits { get; set; }
    }
}
