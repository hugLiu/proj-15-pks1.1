using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.Model;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IRelation : IInterface<RelationModel>
    {
        IList<RelationModel> GetListByID(string boid, string direction = null);

        /// <summary>
        /// 根据类型关系ID获取对象关系的实例
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        IList<RelationModel> GetBoRelTypeListByRTID(string rtid);

        /// <summary>
        /// 通过两个对象名称获取两个对象的ID
        /// </summary>
        /// <param name="BOName1"></param>
        /// <param name="BOName2"></param>
        /// <returns></returns>
        List<string> GetBObyName(string BOName1, string BOName2);
    }
}
