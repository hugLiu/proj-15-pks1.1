using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    [Serializable]
    public class ConceptClassItem
    {
        public string CCCode { get; set; }

        public string CC { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public string Remark { get; set; }
    }
}
