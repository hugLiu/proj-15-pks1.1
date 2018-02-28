using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Core.DTO
{
    public class EsItem
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public string _score { get; set; }
        public Dictionary<string, object> _source { get; set; }
    }
}
