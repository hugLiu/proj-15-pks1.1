using System.Collections.Generic;

namespace Jurassic.MongoDb
{
    public class Context
    {
        public Context()
        {
            PRJ = string.Empty;
            BO = new List<BO>();
            BI = new BI();
            PT = new List<string>();
            BF = new List<string>();
            GN = new List<string>();
            DS = new List<string>();
            TL = new List<string>();
        }
        public string PRJ { get; set; }
        public List<BO> BO { get; set; }
        public BI BI { get; set; }
        public List<string> PT { get; set; }
        public List<string> BF { get; set; }
        public List<string> GN { get; set; }
        public List<string> DS { get; set; }
        public List<string> TL { get; set; }
    }
}
