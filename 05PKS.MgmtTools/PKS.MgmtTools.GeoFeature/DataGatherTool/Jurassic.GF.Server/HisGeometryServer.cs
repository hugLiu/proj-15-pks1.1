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
    class HisGeometryServer : IHisGeometry
    {
        public bool DelHisGeometry(string boid)
        {
            return ObjectCreate<HisGeometryModel>.CreateIHisGeometry("HisGeometryBusiness").DelHisGeometry(boid);
        }

        public bool ExistHisGeometry(HisGeometryModel HisGeometry)
        {
            return ObjectCreate<HisGeometryModel>.CreateIHisGeometry("HisGeometryBusiness").ExistHisGeometry(HisGeometry);
        }

        /// <summary>
        /// 获取历史版本空间数据
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="gatherid"></param>
        /// <returns></returns>
        public List<HisGeometryModel> GetHisGeometryByID(string boid, string gatherid)
        {
            return ObjectCreate<HisGeometryModel>.CreateIHisGeometry("HisGeometryBusiness").GetHisGeometryByID(boid, gatherid);
        }

        public bool InsertHisGeometry(HisGeometryModel HisGeometry)
        {
            return ObjectCreate<HisGeometryModel>.CreateIHisGeometry("HisGeometryBusiness").InsertHisGeometry(HisGeometry);
        }

        public bool UpdateHisGeometry(HisGeometryModel HisGeometry)
        {
            return ObjectCreate<HisGeometryModel>.CreateIHisGeometry("HisGeometryBusiness").UpdateHisGeometry(HisGeometry);
        }
    }
}
