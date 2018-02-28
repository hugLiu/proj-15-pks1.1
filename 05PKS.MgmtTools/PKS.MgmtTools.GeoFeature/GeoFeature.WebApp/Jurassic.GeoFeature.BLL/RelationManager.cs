using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;

namespace Jurassic.GeoFeature.BLL
{
    public class RelationManager
    {
        //public bool Exist(RelationModel model)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").Exist(model);
        //}

        //public int Add(RelationModel model)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").Insert(model);
        //}

        //public int Update(RelationModel model)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").Update(model);
        //}

        //public int Delete(RelationModel model)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").Delete(model);
        //}

        //public IList<RelationModel> GetListByID(string id)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").GetListByID(id);
        //}
        //public IList<RelationModel> GetList()
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").GetList();
        //}

        ///// <summary>
        ///// 根据对象ID查询对象关系
        ///// </summary>
        ///// <param name="boid"></param>
        ///// <param name="direction">查询方向，包括正向（Forward），反向（Backward），为空则两个方向都查</param>
        ///// <returns></returns>
        //public IList<RelationModel> GetListByID(string boid, string direction = null)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").GetListByID(boid, direction);
        //}

        ///// <summary>
        ///// 根据类型关系ID获取对象关系的实例
        ///// </summary>
        ///// <param name="rtid"></param>
        ///// <returns></returns>
        //public IList<RelationModel> GetBoRelTypeListByRTID(string rtid)
        //{
        //    return ObjectCreate<RelationModel>.CreatIRelation("RelationServer").GetBoRelTypeListByRTID(rtid);
        //}

        public List<string> GetBObyName(string BOName1, string BOName2)
        {
            return PrivateObjectCreate<RelationModel>.CreatIRelation("RelationBusiness").GetBObyName(BOName1, BOName2);
        }

        public int Add(RelationModel model)
        {
            return PrivateObjectCreate<RelationModel>.CreatIRelation("RelationBusiness").Insert(model);
        }
    }
}
