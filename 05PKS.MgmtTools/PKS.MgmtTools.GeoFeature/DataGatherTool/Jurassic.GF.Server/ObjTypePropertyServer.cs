using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Server
{
    public class ObjTypePropertyServer
    {
        public List<ObjTypePropertyModel> GetObjPropertyByBOTID(string BOTID)
        {
            return ObjectCreate<ObjTypePropertyModel>.CreateIObjTypeProperty("ObjTypePropertyBusiness").GetObjPropertyByBOTID(BOTID);
        }
    }
}
