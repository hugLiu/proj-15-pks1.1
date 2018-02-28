using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.Model;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IAlisaName:IInterface<AliasNameModel>
    {
        IList<AliasNameModel> GetAlisaNameByIDAndAppDomain(string boId, string appDomain);
    }
}
