using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.BLL
{
    public class ObjectTypeManager
    {
        public IList<ObjectTypeModel> GetListByTypeClass(string ClassId)
        {
            return PrivateObjectCreate<ObjectTypeModel>.CreateIObjectType("ObjectTypeBusiness").GetListByTypeClass(ClassId);
        }
        ///// <summary>
        ///// 根据类型名称判断对象类型是否存在
        ///// </summary>
        ///// <param name="Name"></param>
        ///// <returns></returns>
        //public bool Exist(ObjectTypeModel model)
        //{
        //    return ObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeServer").Exist(model);
        //}

        ///// <summary>
        /////添加对象类型
        ///// </summary>
        ///// <param name="objectTypeModel"></param>
        //public int AddObjectType(ObjectTypeModel objectTypeModel)
        //{
        //    return ObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeServer").Insert(objectTypeModel);
        //}
        ///// <summary>
        ///// 修改对象类型
        ///// </summary>
        ///// <param name="objectTypeModel"></param>
        //public int UpdateObjectType(ObjectTypeModel objectTypeModel)
        //{
        //    return ObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeServer").Update(objectTypeModel);
        //}
        ///// <summary>
        ///// 删除对象类型
        ///// </summary>
        ///// <param name="objectTypeModel"></param>
        ///// <returns></returns>
        //public int DelObjectType(ObjectTypeModel objectTypeModel)
        //{
        //    return ObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeServer").Delete(objectTypeModel);
        //}

        /// <summary>
        /// 获取全部对象类型
        /// </summary>
        /// <returns></returns>
        public IList<ObjectTypeModel> GetAllObjType()
        {
            return PrivateObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeBusiness").GetList();
        }
        /// <summary>
        /// 根据对象类型名称获取对象类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<ObjectTypeModel> GetObjectTypeByFName(string name)
        {
            return PrivateObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeBusiness").GetListByName(name);
        }

        /// <summary>
        /// 根据BotID获取对象类型
        /// </summary>
        /// <param name="BotId"></param>
        /// <returns></returns>
        public IList<ObjectTypeModel> GetObjectTypeByBotId(string BotId)
        {
            return PrivateObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeBusiness").GetListByID(BotId);
        }

        ///// <summary>
        ///// 获取有对象实例的对象类型
        ///// </summary>
        ///// <returns></returns>
        //public List<ObjectTypeModel> GetObjectTypeWithBoList()
        //{
        //    return ObjectCreate<ObjectTypeModel>.CreateIObjectType("ObjectTypeServer").GetObjectTypeWithBoList();
        //}
        ///// <summary>
        ///// 根据对象类型ID获取对象类型
        ///// </summary>
        ///// <param name="botId"></param>
        ///// <returns></returns>
        //public IList<ObjectTypeModel> GetFeatureTypeByFTId(string botId)
        //{
        //    return ObjectCreate<ObjectTypeModel>.CreateIInterface("ObjectTypeServer").GetListByID(botId);
        //}
    }
}
