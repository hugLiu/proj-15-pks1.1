using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.Factory;

namespace Jurassic.GF.Server
{
    class HisBOPropertyServer : IHisBOProperty
    {
        public bool DelHisProperty(string boid, string ns)
        {
            return ObjectCreate<HisPropertyModel>.CreateIHisBOProperty("BOPropertyBusiness").DelHisProperty(boid, ns);
        }

        public bool ExistHisProperty(HisPropertyModel HisProperty)
        {
            return ObjectCreate<HisPropertyModel>.CreateIHisBOProperty("BOPropertyBusiness").ExistHisProperty(HisProperty);
        }

        public List<HisPropertyModel> GetHisPropertyByID(string boid, string ns, string gatherid)
        {
            return ObjectCreate<HisPropertyModel>.CreateIHisBOProperty("BOPropertyBusiness").GetHisPropertyByID(boid, ns, gatherid);
        }

        public bool InsertHisProperty(HisPropertyModel HisProperty)
        {
            return ObjectCreate<GeometryModel>.CreateIHisBOProperty("BOPropertyBusiness").InsertHisProperty(HisProperty);
        }

        public bool UpdateHisProperty(HisPropertyModel HisProperty)
        {
            return ObjectCreate<HisPropertyModel>.CreateIHisBOProperty("BOPropertyBusiness").UpdateHisProperty(HisProperty);
        }
    }
}
