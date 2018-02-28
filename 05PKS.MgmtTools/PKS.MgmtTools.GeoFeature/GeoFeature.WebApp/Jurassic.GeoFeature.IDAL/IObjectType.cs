using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GeoFeature.Model;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IObjectType : IInterface<ObjectTypeModel>
    {
        IList<ObjectTypeModel> GetListByTypeClass(string ClassId);

    }
}
