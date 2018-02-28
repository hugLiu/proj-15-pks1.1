using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Core.DTO
{
    public class EsBuckets : List<BucketItem>
    {
    }

    public class BucketItem
    {
        public string key { get; set; }
        public int doc_count { get; set; }
    }
}
