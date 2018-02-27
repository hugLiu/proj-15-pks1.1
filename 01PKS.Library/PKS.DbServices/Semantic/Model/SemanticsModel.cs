using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    public class SemanticsModel
    {
        public int FTermClassId { get; set; }

        public string SR { get; set; }

        public int LTermClassId { get; set; }

        public string FTerm { get; set; }

        public string LTerm { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }

        public bool IsLeaf { get; set; }
    }
}
