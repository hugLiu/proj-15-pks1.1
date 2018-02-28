using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.Factory;
using System;
using System.Collections.Generic;

namespace Jurassic.GF.Server
{
    public class ObjectTypeServer
    {
        public List<ObjectTypeModel> GetAllObjectType()
        {
            return ObjectCreate<ObjectTypeModel>.CreateIObjectType("ObjectTypeBusiness").GetAllObjectType();
        }
    }
}
