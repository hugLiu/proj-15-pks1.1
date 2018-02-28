using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface.Model
{
    public class DataGatherModel
    {
        public string GATHERID { get; set; }
        public string ENVENT { get; set; }
        public DateTime ENVENTDATA { get; set; }
        public string GATHERER { get; set; }
        public DateTime UPLOADDATE { get; set; }
        public string NOTE { get; set; }
        public string BOTID { get; set; }
    }
}
