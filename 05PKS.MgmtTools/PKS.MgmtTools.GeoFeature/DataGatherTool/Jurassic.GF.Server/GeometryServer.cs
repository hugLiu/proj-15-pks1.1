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
    public class GeometryServer : IGeometry
    {
        public bool DelGeometry(string boid)
        {
            return ObjectCreate<GeometryModel>.CreateIGeometry("GeometryBusiness").DelGeometry(boid);
        }

        public bool ExistGeometry(GeometryModel Geometry)
        {
            return ObjectCreate<GeometryModel>.CreateIGeometry("GeometryBusiness").ExistGeometry(Geometry);
        }

        public List<GeometryModel> GetGeometryByID(string boid)
        {
            return ObjectCreate<GeometryModel>.CreateIGeometry("GeometryBusiness").GetGeometryByID(boid);
        }

        public bool InsertGeometry(GeometryModel Geometry)
        {
            return ObjectCreate<GeometryModel>.CreateIGeometry("GeometryBusiness").InsertGeometry(Geometry);
        }

        public bool UpdateGeometry(GeometryModel Geometry)
        {
            return ObjectCreate<GeometryModel>.CreateIGeometry("GeometryBusiness").UpdateGeometry(Geometry);
        }
    }
}
