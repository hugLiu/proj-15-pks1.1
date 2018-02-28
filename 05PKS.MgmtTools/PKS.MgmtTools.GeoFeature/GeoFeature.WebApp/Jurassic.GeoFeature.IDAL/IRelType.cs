using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.Model;
using System.Data;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IRelType : IInterface<RelTypeModel>
    {
        IList<RelTypeModel> GetListByBot(string bot, string direction);

        bool Exist(RelTypeModel model, ref string RTID);
        /// <summary>
        /// 添加对象关系类型
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int AddRelationModel(List<RelTypeModel> list);

        /// <summary>
        /// 根据关系类型ID获取类型关系列表
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        IList<ObjTypeRelationModel> GetRelationListByrt(string rt);

        /// <summary>
        /// 获取全部类型关系
        /// </summary>
        /// <returns></returns>
        IList<RelTypeModel> GetAllRelationRT();

        /// <summary>
        ///根据rtid删除对象类型关系实例
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        int DeleteRelTypeByRtID(string rtid);

        /// <summary>
        /// 修改对象关系类型
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oldRT"></param>
        /// <returns></returns>
        int UpdateRelTypeByRt(List<RelTypeModel> list, string oldRT);

        /// <summary>
        /// 获取全部对象类型关系
        /// </summary>
        /// <returns></returns>
        DataSet GetAllRelation();
        
        /// <summary>
        /// 获取所有关系类型树，或者根据节点取得子树
        /// </summary>
        /// <param name="rootList"></param>
        /// <returns></returns>
        IList<RelTypeModel> GetRelTree(List<string> rootList = null);

        IList<RelTypeModel> GetRelTreeRoot();
        IList<RelTypeModel> GetRelTreeSubNode(string botid1);


        /// <summary>
        /// 通过两个对象名称获取两个对象类型的ID
        /// </summary>
        /// <param name="BOTName1"></param>
        /// <param name="BOTName2"></param>
        /// <returns></returns>
        List<string> GetBOTbyName(string BOTName1, string BOTName2);

        List<RelTypeModel> GetRelTypeNameByID(string BOTID);
        /// <summary>
        /// 获取两个对象是否都具有空间坐标属性
        /// </summary>
        /// <param name="BOTName1"></param>
        /// <param name="BOTName2"></param>
        /// <returns></returns>
        List<string> GetBOTRelByName(string BOTName1, string BOTName2);

        /// <summary>
        /// 根据对象关系类型ID获取当前的对象类型及实例关系
        /// </summary>
        /// <param name="RTID"></param>
        /// <returns></returns>
        DataTable GetRelTable(string RTID);

        bool DelBOTRel(List<string> RTID, string BOTorBO);

    }
}
