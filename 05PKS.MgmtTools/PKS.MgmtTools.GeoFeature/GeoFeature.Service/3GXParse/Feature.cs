using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGGXParse
{
    public class GeoFeature
    {
        public string BOID { get; set; }
        public string NAME { get; set; }
        public string BOT { get; set; }
        public string CLASS { get; set; }
        public string FT { get; set; }
        public List<AliasName> AliasNameList { get; set; }
        public List<Geometry> GeometryList { get; set; }
        public List<Property> PropertyList { get; set; }
        public bool UNCHANGOROVERRIDE { get; set; }
    }
}
