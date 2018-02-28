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
    public class BOServer : IBO
    {
        public bool DelBO(string boid)
        {
            return ObjectCreate<PropertyModel>.CreateIBO("BOBusiness").DelBO(boid);
        }

        public bool ExistBO(string name, string bot)
        {
            return ObjectCreate<PropertyModel>.CreateIBO("BOBusiness").ExistBO(name, bot);
        }

        public BoMode GetBoListByName(string name, string bot)
        {
            return ObjectCreate<PropertyModel>.CreateIBO("BOBusiness").GetBoListByName(name, bot);
        }

        public bool InsertBO(BoMode model)
        {
            return ObjectCreate<PropertyModel>.CreateIBO("BOBusiness").InsertBO(model);
        }

        public bool UpdateBO(BoMode model)
        {
            return ObjectCreate<PropertyModel>.CreateIBO("BOBusiness").UpdateBO(model);
        }
    }
}
