using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class ElementTreeNode
    {
        public int id { get; set; }

        public int? pid { get; set; }

        public String label { get; set; }

        public List<ElementTreeNode> children { get; set; }

        public bool isCase { get; set; }

        public int? caseId { get; set; }

    }
}
