using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.BLL
{
    public class PropertyManager
    {
        //public void AddProterty(PropertyModel model)
        //{
        //    ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").Insert(model);
        //}

        //public IList<PropertyModel> GetPropertyByFId(string fId)
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").GetListByID(fId);
        //}

        //public bool Exist(PropertyModel model)
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").Exist(model);
        //}
        //public int Insert(PropertyModel model)
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").Insert(model);
        //}
        //public int Update(PropertyModel model)
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").Update(model);
        //}
        //public int Delete(PropertyModel model)
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").Delete(model);
        //}
        //public IList<PropertyModel> GetListByID(string boid)
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").GetListByID(boid);
        //}
        //public IList<PropertyModel> GetList()
        //{
        //    return ObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").GetList();
        //}

        public IList<PropertyModel> GetListByID(string boid)
        {
            return PrivateObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").GetListByID(boid);
        }

        public int Update(PropertyModel model)
        {
            return PrivateObjectCreate<PropertyModel>.CreateIInterface("PropertyServer").Update(model);
        }
    }
}
