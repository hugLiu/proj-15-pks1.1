using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class ParamTreeNode
    {
        public int Id { get; set; }

        public int? Pid { get; set; }

        public string Name { get; set; }

        public bool IsParam { get; set; }

        public int? ParamId { get; set; }
    }
}
