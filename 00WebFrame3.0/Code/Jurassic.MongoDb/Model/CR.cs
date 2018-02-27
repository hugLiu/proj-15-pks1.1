using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class CR
    {
        public CR()
        { }
        public CR(string rIIid,string rt)
        {
            this.RIIId = rIIid;
            this.RT = rt;
        }
        public string RIIId { get; set; }
        public string RT { get; set; }
    }
}
