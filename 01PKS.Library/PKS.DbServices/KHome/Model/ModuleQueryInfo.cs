using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KHome.Model
{
    public class ModuleQueryInfo
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String QueryParameter { get; set; }
        public int ModuleId { get; set; }
    }
}
