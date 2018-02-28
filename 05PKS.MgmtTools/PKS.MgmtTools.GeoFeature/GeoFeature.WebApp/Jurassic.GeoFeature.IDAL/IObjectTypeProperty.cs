using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IObjectTypeProperty : IInterface<ObjTypePropertyModel>
    {
        List<ObjTypePropertyModel> GetBOTProp(string BOTID, string Name);
    }

    
}
