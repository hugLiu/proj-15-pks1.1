using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KCase.Model
{
    public class ChartModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int ChartType { get; set; }
        public String Parameters { get; set; }
        public int ThemeId { get; set; }
        public String _state { get; set; }
    }
}
