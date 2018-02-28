using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;
using System.Data;

namespace Jurassic.GeoFeature.BLL
{
    public class RelTypeManager
    {
        public void Add(RelTypeModel model)
        {
            PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").Insert(model);
        }

        public IList<RelTypeModel> GetList()
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").GetList();
        }

        public List<string> GetBOTbyName(string BOTName1, string BOTName2)
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").GetBOTbyName(BOTName1, BOTName2);
        }
        public List<RelTypeModel> GetRelTypeNameByID(string BOTID)
        {
            return PrivateObjectCreate<string>.CreatIRelType("RelTypeBusiness").GetRelTypeNameByID(BOTID);
        }

        public List<string> GetBOTRelByName(string BOTName1, string BOTName2)
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").GetBOTRelByName(BOTName1, BOTName2);
        }  //
            

        public bool DelBOTRel(List<string> RTID, string BOTorBO)
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").DelBOTRel(RTID, BOTorBO);
        }

        public DataTable GetRelTable(string RTID)
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").GetRelTable(RTID);
        }


        public bool Exist(RelTypeModel model, ref string RTID)
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").Exist(model, ref RTID);
        }

        /// <summary>
        /// 添加对象关系类型
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddRelationModel(List<RelTypeModel> list)
        {
            return PrivateObjectCreate<RelTypeModel>.CreatIRelType("RelTypeBusiness").AddRelationModel(list);
        }

        //public void Add(RelTypeModel model)
        //{
        //    ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").Insert(model);
        //}

        //public void Update(RelTypeModel model)
        //{
        //    ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").Update(model);
        //}

        //public int Delete(RelTypeModel model)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").Delete(model);
        //}

        //public IList<RelTypeModel> GetListByID(string id)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetListByID(id);
        //}

        //public IList<RelTypeModel> GetListByName(string name)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetListByName(name);
        //}
        //public IList<RelTypeModel> GetList()
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetList();
        //}

        ///// <summary>
        ///// 根据对象类型获取对象关系类型
        ///// </summary>
        ///// <param name="bot"></param>
        ///// <param name="direction">关联方向，包括正向（Forward），反向（Backward），如果为空则两个方向都查询</param>
        ///// <returns></returns>
        //public IList<RelTypeModel> GetListByBot(string bot, string direction = null)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetListByBot(bot, direction);
        //}

        ///// <summary>
        ///// 添加对象关系类型
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public int AddRelationModel(List<RelTypeModel> list)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").AddRelationModel(list);
        //}

        ///// <summary>
        ///// 根据关系类型对象类型关系列表
        ///// </summary>
        ///// <param name="rtID"></param>
        ///// <returns></returns>
        //public IList<ObjTypeRelationModel> GetRelationListByrt(string rt)
        //{
        //    return ObjectCreate<ObjTypeRelationModel>.CreatIRelType("RelTypeServer").GetRelationListByrt(rt);
        //}
        ///// <summary>
        ///// 获取全部关系类型
        ///// </summary>
        ///// <returns></returns>
        //public IList<RelTypeModel> GetAllRelationRT()
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetAllRelationRT();
        //}

        ///// <summary>
        ///// 根据对象关系实例ID删除对象关系
        ///// </summary>
        ///// <param name="rtid"></param>
        ///// <returns></returns>
        //public int DeleteRelTypeByRtID(string rtid)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").DeleteRelTypeByRtID(rtid);
        //}

        ///// <summary>
        ///// 修改对象关系类型
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="oldRT"></param>
        ///// <returns></returns>
        //public int UpdateRelTypeByRt(List<RelTypeModel> list, string oldRT)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").UpdateRelTypeByRt(list, oldRT);
        //}
        ///// <summary>
        ///// 获取全部对象类型关系
        ///// </summary>
        ///// <returns></returns>
        //public DataSet GetAllRelation()
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetAllRelation();
        //}

        ///// <summary>
        ///// 获取所有关系类型树，或者根据节点取得子树
        ///// </summary>
        ///// <param name="rootList"></param>
        ///// <returns></returns>
        //public IList<RelTypeModel> GetRelTree(List<string> rootList = null)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetRelTree(rootList);
        //}

        //public IList<RelTypeModel> GetRelTreeRoot()
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetRelTreeRoot();
        //}

        //public IList<RelTypeModel> GetRelTreeSubNode(string botid1)
        //{
        //    return ObjectCreate<RelTypeModel>.CreatIRelType("RelTypeServer").GetRelTreeSubNode(botid1);
        //}

    }
}
