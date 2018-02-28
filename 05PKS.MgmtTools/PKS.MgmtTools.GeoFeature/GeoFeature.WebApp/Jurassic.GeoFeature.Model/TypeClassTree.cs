using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GeoFeature.Model
{
    public class TypeClassTree
    {
        public string Id { get; set; }
        public string PId { get; set; }
        public string Name { get; set; }
        public string EName { get; set; }
        public string FT { get; set; }
        public string IsUserDefine{get;set;}
        public string Shape { get; set; }
        public string UseGeometry{get;set;}
        public string Type { get; set; }
        public List<ObjTypePropertyModel> OPL { get; set; }
    }
}
