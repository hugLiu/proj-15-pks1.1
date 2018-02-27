using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    public class TermSourceModel
    {
        public string CCCode { get; set; }

        public string Source { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
