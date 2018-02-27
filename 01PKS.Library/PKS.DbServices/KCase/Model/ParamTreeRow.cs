using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class ParamTreeRow
    {
        public int Id { get; set; }

        public int? Pid { get; set; }

        public string Name { get; set; }

        public bool IsParam { get; set; }

        public int? ParamId { get; set; }

        public String ParamValue { get; set; }

        public String SampleData { get; set; }

        public String Remark { get; set; }

        public String ChartNumber { get; set; }

        public String FormulaNumber { get; set; }

        public int ParamType { get; set; }

        public string Options { get; set; }

        public string Range { get; set; }

        public string Unit { get; set; }

        public String _state { get; set; }
    }
}
