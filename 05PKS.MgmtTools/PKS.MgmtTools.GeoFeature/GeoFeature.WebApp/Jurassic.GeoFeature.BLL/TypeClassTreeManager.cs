using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GeoFeature.Model;
using Jurassic.GeoFeature.Factory;

namespace Jurassic.GeoFeature.BLL
{
    public class TypeClassTreeManager
    {
        public IList<TypeClassTree> GetList()
        {
            return PrivateObjectCreate<TypeClassTree>.CreatITypeClass("TypeClassTreeBusiness").GetList();
        }
        public bool Save(List<TypeClassTree> tcl)
        {
            return PrivateObjectCreate<TypeClassTree>.CreatITypeClass("TypeClassTreeBusiness").Save(tcl);
        }

    }
}
