using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class ParamModel
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public int ParamType { get; set; }

        public String Options { get; set; }

        public String Range { get; set; }

        public String Unit { get; set; }
    }
}
