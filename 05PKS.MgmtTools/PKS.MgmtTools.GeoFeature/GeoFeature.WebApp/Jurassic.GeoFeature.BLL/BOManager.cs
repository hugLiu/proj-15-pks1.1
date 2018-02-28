using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Jurassic.GeoFeature.BLL
{
    public class BOManager
    {
        /// <summary>
        /// 根据对象名称和对象类型获取对象列表
        //////////////////////////////////////////////////////////2016.3.10日 陈雯雯补充
        /// </summary>
        /// <param name="boName"></param>
        /// <param name="botName"></param>
        /// <returns></returns>
        public List<BOModel> GetBoByBoNameAndBot(string boName, string botName)
        {
            return PrivateObjectCreate<string>.CreateIBO("BOBusiness").GetBoByBoNameAndBot(boName, botName);
        }
        /// <summary>
        /// 导入对象属性数据        
        //////////////////////////////////////////////////////////2016.3.13日 陈雯雯补充
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        public bool UploadBoPropertyTran(List<BoExModel> boExModelList)
        {
            return PrivateObjectCreate<string>.CreateIBO("BOBusiness").UploadBoPropertyTran(boExModelList);
        }

        //public bool Exist(BOModel model)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").Exist(model);
        //}

        //public int Insert(BOModel model)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").Insert(model);
        //}

        //public int Update(BOModel model)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").Update(model);
        //}

        //public int Delete(BOModel model)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").Delete(model);
        //}

        //public IList<BOModel> GetListByID(string ID)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetListByID(ID);
        //}

        //public IList<BOModel> GetListByName(string Name)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetListByName(Name);
        //}

        //public IList<BOModel> GetList()
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetList();
        //}

        ///// <summary>
        ///// 根据对象名称和对象类型ID获取对象列表
        ///// </summary>
        ///// <param name="name">对象名称</param>
        ///// <param name="botId">对象类型ID</param>
        ///// <returns></returns>
        //public List<BOModel> GetBoListByName(string name, string botId)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetBoListByName(name, botId);
        //}

        ///// <summary>
        ///// 根据别名获取对象
        ///// </summary>
        ///// <param name="alisName"></param>
        ///// <param name="appDomain"></param>
        ///// <returns></returns>
        //public List<BOModel> GetBOByAlisName(string alisName, string appDomain)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetBOByAlisName(alisName, appDomain);
        //}

        ///// <summary>
        ///// 根据对象ID查询指定范围内对象,可选对象类型和对象类别
        ///// </summary>
        ///// <param name="boid"></param>
        ///// <param name="distance"></param>
        ///// <param name="bot"></param>
        ///// <param name="boc"></param>
        ///// <returns></returns>
        //public List<BOModel> GetNearBOByID(string boid, double distance, string bot = null, string boc = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetNearBOByID(boid, distance, bot, boc);
        //}

        ///// <summary>
        ///// 根据WKT坐标、类型、类别查询指定范围内对象
        ///// </summary>
        ///// <param name="wkt"></param>
        ///// <param name="distance"></param>
        ///// <param name="bot"></param>
        ///// <param name="boc"></param>
        ///// <returns></returns>
        //public DataSet GetNearBOByLocation(string wkt, double distance, string bot = null, string boc = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetNearBOByLocation(wkt, distance, bot, boc);
        //}

        ///// <summary>
        ///// 根据对象名称、类型、类别查询指定范围内对象
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="bot0"></param>
        ///// <param name="distance"></param>
        ///// <param name="bot"></param>
        ///// <param name="boc"></param>
        ///// <returns></returns>
        //public DataSet GetNearBOByName(string name, string bot0, double distance, string bot = null, string boc = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetNearBOByName(name, bot0, distance, bot, boc);
        //}

        ///// <summary>
        ///// 根据类型获取对象
        ///// </summary>
        ///// <param name="bot"></param>
        ///// <returns></returns>
        //public IList<BOModel> GetBoByBotID(string botId)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetBoByBotID(botId);
        //}

        ///// <summary>
        ///// SubTree：返回指定BO的子树节点；
        ///// Around：返回指定BO的相邻节点，包括父节点、兄弟节点和下级节点；
        ///// Parent：返回指定BO的父节点；
        ///// Brother：返回指定BO的兄弟节点；
        ///// Child：返回指定BO的下级节点；
        ///// </summary>
        ///// <param name="boid"></param>
        ///// <param name="category"></param>
        ///// <returns></returns>
        //public List<BOModel> GetBOTree(string boid, string category)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetBOTree(boid, category);
        //}

        ///// <summary>
        ///// 根据业务对象ID查找指定关系对应的对象
        ///// </summary>
        ///// <param name="boid"></param>
        ///// <param name="relation"></param>
        ///// <param name="direction">关联方向，包括正向（Forward），反向（Backward）</param>
        ///// <param name="bot"></param>
        ///// <param name="boc"></param>
        ///// <returns></returns>
        //public List<BOModel> GetRelatedBO(string boid, string relation, string direction, string bot = null, string boc = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetRelatedBO(boid, relation, direction, bot, boc);
        //}

        ///// <summary>
        ///// 根据业务类型和名称查找指定关系对象
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="bot0"></param>
        ///// <param name="relation"></param>
        ///// <param name="bot"></param>
        ///// <param name="boc"></param>
        ///// <param name="direction"></param>
        ///// <returns></returns>
        //public List<BOModel> GetBOByNameAndRelation(string name, string bot0, string relation, string direction, string bot = null, string boc = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetBOByNameAndRelation(name, bot0, relation, direction, bot, boc);
        //}

        //public List<BOModel> GetBORoot()
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").GetBORoot();
        //}

        ///// <summary>
        ///// 事务方式导入3GX数据
        ///// </summary>
        ///// <param name="boExModelList"></param>
        ///// <param name="replaceOrLeave"></param>
        ///// <returns></returns>
        //public bool Upload3GXTran(List<BoExModel> boExModelList, string replaceOrLeave = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").Upload3GXTran(boExModelList, replaceOrLeave);
        //}

        ///// <summary>
        ///// 非事务方式（一个对象作为一个事务）导入3GX数据
        ///// </summary>
        ///// <param name="boExModelList"></param>
        ///// <param name="replaceOrLeave"></param>
        ///// <returns></returns>
        //public int Upload3GX(List<BoExModel> boExModelList, string replaceOrLeave = null)
        //{
        //    return ObjectCreate<BOModel>.CreateIBO("BOServer").Upload3GX(boExModelList, replaceOrLeave);
        //}

        ///// <summary>
        ///// 获取Bot相关的对象类别
        ///// </summary>
        ///// <param name="botList"></param>
        ///// <returns></returns>
        //public List<string> GetBoc(List<string> botList)
        //{
        //    return ObjectCreate<string>.CreateIBO("BOServer").GetBoc(botList);
        //}

        ///// <summary>
        ///// 根据查询条件返回对象集合
        ///// </summary>
        ///// <param name="filterList">查询条件，请注意参数表名：PROPERTY，参数名：PROTERTYNAME，参数值：PROPERTYVALUE</param>
        ///// <param name="parameter"></param>
        ///// <returns></returns>
        //public List<BOModel> GetBoByFilter(List<string> filterList, Dictionary<string, object> parameter, string botid = null, string ns = null)
        //{
        //    return ObjectCreate<string>.CreateIBO("BOServer").GetBoByFilter(filterList, parameter, botid, ns);
        //}
        //public List<BOModel> GetBoByFilter(string filter)
        //{
        //    return ObjectCreate<string>.CreateIBO("BOServer").GetBoByFilter(filter);
        //}
        ///// <summary>
        ///// 返回对象名称和别名，用于词库分词
        ///// </summary>
        ///// <param name="isWithAlias"></param>
        ///// <returns></returns>
        //public DataSet GetDictionary(bool isWithAlias)
        //{
        //    return ObjectCreate<string>.CreateIBO("BOServer").GetDictionary(isWithAlias);
        //}

        ///// <summary>
        ///// 获取对象某个参数的数据类型，作为filter中字段的数据类型
        ///// </summary>
        ///// <param name="boid"></param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //public string GetPropertyType(string propertyName)
        //{
        //    return ObjectCreate<string>.CreateIBO("BOServer").GetPropertyType(propertyName);
        //}

        public List<BOModel> GetBoListByBOTID(string botID)
        {
            return PrivateObjectCreate<string>.CreateIBO("BOBusiness").GetBoListByBOTID(botID);
        }

        public DataTable GetBoListByID(string BOID)
        {
            return PrivateObjectCreate<string>.CreateIBO("BOBusiness").GetBoListByID(BOID);
        }

        public IList<BOModel> GetListByID(string ID)
        {
            return PrivateObjectCreate<BOModel>.CreateIBO("BOBusiness").GetListByID(ID);
        }

        public DataTable GetALIASNAME(string BOID)
        {
            return PrivateObjectCreate<string>.CreateIBO("BOBusiness").GetALIASNAME(BOID);
        }
        public int Delete(BOModel model)
        {
            return PrivateObjectCreate<BOModel>.CreateIBO("BOBusiness").Delete(model);
        }
        /// <summary>
        /// 根据对象名称和对象类型ID获取对象列表
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="botId">对象类型ID</param>
        /// <returns></returns>
        public List<BOModel> GetBoListByName(string name, string botId)
        {
            return PrivateObjectCreate<BOModel>.CreateIBO("BOBusiness").GetBoListByName(name, botId);
        }

        public int Update(BOModel model)
        {
            return PrivateObjectCreate<BOModel>.CreateIBO("BOBusiness").Update(model);
        }
        public bool InsertBOandPara(List<string> SqlList)
        {
            return PrivateObjectCreate<string>.CreateIBO("BOBusiness").InsertBOandPara(SqlList);
        }

    }
}
