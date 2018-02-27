using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    public class TermTranslationModel
    {
        public int TermClassID { get; set; }

        public string LangCode { get; set; }

        public string Translation { get; set; }

        public int? IsMain { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }
    }
}
