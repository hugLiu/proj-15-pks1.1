using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KHome.Model
{
    public class ModuleTreeNode
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int? Pid { get; set; }
        public bool IsModule { get; set; }
        public int? ModuleId { get; set; }
        public String Description { get; set; }
        public int? ComponentType { get; set; }
    }
}
