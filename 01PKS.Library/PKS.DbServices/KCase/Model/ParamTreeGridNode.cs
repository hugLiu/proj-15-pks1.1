using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class ParamTreeGridNode
    {
        public int? id { get; set; }
        public string label { get; set; }
        public int? parent_id { get; set; }
        public string url { get; set; }
        public int depth { get; set; }
        public int child_num { get; set; }
        public bool expanded { get; set; }
        public string paramvalue { get; set; }
        public string sampledata { get; set; }
        public string remark { get; set; }
        public List<ParamTreeGridNode> children { get; set; }
    }
}
