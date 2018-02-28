using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GeoFeature.BLL
{
    public class ObjTypePropertyManager
    {
        ///// <summary>
        ///// 根据类型名称和对象类型ID判断对象类型参数是否存在
        ///// </summary>
        ///// <param name="Name"></param>
        ///// <returns></returns>
        //public bool Exist(ObjTypePropertyModel model)
        //{
        //    return ObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyServer").Exist(model);
        //}

        ///// <summary>
        /////添加对象类型参数
        ///// </summary>
        ///// <param name="objectTypeModel"></param>
        //public int AddObjTypeProperty(ObjTypePropertyModel objectTypeModel)
        //{
        //    return ObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyServer").Insert(objectTypeModel);
        //}
        ///// <summary>
        ///// 修改对象类型参数
        ///// </summary>
        ///// <param name="objectTypeModel"></param>
        //public int UpdateObjTypeProperty(ObjTypePropertyModel objectTypeModel)
        //{
        //    return ObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyServer").Update(objectTypeModel);
        //}
        ///// <summary>
        ///// 删除对象类型参数
        ///// </summary>
        ///// <param name="objectTypeModel"></param>
        ///// <returns></returns>
        //public int DelObjTypeProperty(ObjTypePropertyModel objectTypeModel)
        //{
        //    return ObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyServer").Delete(objectTypeModel);
        //}
        ///// <summary>
        ///// 根据对象类型名称获取对象类型参数
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public IList<ObjTypePropertyModel> GetObjTypePropertyByName(string name)
        //{
        //    return ObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyServer").GetListByName(name);
        //}
        ///// <summary>
        ///// 获取全部对象类型参数
        ///// </summary>
        ///// <returns></returns>
        //public IList<ObjTypePropertyModel> GetAllObjTypeProperty()
        //{
        //    return ObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyServer").GetList();
        //}
        /// <summary>
        /// 根据对象类型ID获取对象类型参数
        /// </summary>
        /// <param name="botId"></param>
        /// <returns></returns>
        public IList<ObjTypePropertyModel> GetObjTypePropertyBoid(string botId)
        {
            return PrivateObjectCreate<ObjTypePropertyModel>.CreateIInterface("ObjTypePropertyBusiness").GetListByID(botId);
        }
        public List<ObjTypePropertyModel> GetOBJTypePropByName(string BOTID, string Name)
        {
            return PrivateObjectCreate<ObjTypePropertyModel>.CreatIObjectTypeProperty("ObjTypePropertyBusiness").GetBOTProp(BOTID, Name);

        }


    }
}
