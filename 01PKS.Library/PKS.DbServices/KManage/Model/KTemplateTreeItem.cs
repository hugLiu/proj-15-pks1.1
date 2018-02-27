using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KManage.Model
{
    public class KTemplateTreeItem
    {
        public int id { get; set; }

        public string text { get; set; }

        public bool IsCategory { get; set; }

        public List<KTemplateTreeItem> children { get; set; }
    }
}
