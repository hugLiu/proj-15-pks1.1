using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Semantic.Model
{
    public class TermTreeModel
    {
        public int TermClassId { get; set; }

        public string Term { get; set; }

        public int? PId { get; set; }

        public int lvl { get; set; }

        public string Source { get; set; }

        public int? OrderIndex { get; set; }

        public int IsLeaf { get; set; }

        public bool isChecked { get; set; }

        public string PathTerm { get; set; }
    }
}
