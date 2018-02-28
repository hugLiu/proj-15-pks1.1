using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Jurassic.GeoFeature.Model;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IBO : IInterface<BOModel>
    {
        /// <summary>
        /// 根据对象名称和对象类型获取对象列表
        //////////////////////////////////////////////////////////2016.3.10日 陈雯雯补充
        /// </summary>
        /// <param name="boName"></param>
        /// <param name="botName"></param>
        /// <returns></returns>
        List<BOModel> GetBoByBoNameAndBot(string boName, string botName);
        /// <summary>
        /// 导入对象属性数据        
        //////////////////////////////////////////////////////////2016.3.13日 陈雯雯补充
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        bool UploadBoPropertyTran(List<BoExModel> boExModelList);

        /// <summary>
        /// 根据对象名称、类型获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        List<BOModel> GetBoListByName(string name, string bot);

        /// <summary>
        /// 根据别名、应用域获取对象
        /// </summary>
        /// <param name="alisName"></param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        List<BOModel> GetBOByAlisName(string alisName, string appDomain);

        /// <summary>
        /// 根据对象ID查询指定范围内对象
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="distance"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        List<BOModel> GetNearBOByID(string boid, double distance, string bot, string boc);

        /// <summary>
        /// 根据WKT坐标、类型、类别查询指定范围内对象
        /// </summary>
        /// <param name="wkt"></param>
        /// <param name="distance"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        DataSet GetNearBOByLocation(string wkt, double distance, string bot, string boc);

        /// <summary>
        /// 根据WKT坐标、类型、类别查询指定范围内对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot0"></param>
        /// <param name="distance"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        DataSet GetNearBOByName(string name, string bot0, double distance, string bot, string boc);

        /// <summary>
        ///根据对象类型获取对象
        /// </summary>
        /// <param name="bot"></param>
        IList<BOModel> GetBoByBotID(string bot);

        /// <summary>
        /// 获取指定BO的父节点、下级节点、兄弟节点、相邻节点（父、兄弟和下级）和子树
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        List<BOModel> GetBOTree(string boid, string category);

        /// <summary>
        /// 根据业务对象ID查找指定关系对应的对象
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="relation"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        List<BOModel> GetRelatedBO(string boid, string relation, string bot, string boc, string direction);

        /// <summary>
        /// 根据业务类型和名称查找指定关系对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot0"></param>
        /// <param name="relation"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        List<BOModel> GetBOByNameAndRelation(string name, string bot0, string relation, string bot, string boc, string direction);

        List<BOModel> GetBORoot();

        /// <summary>
        /// 事务方式导入3GX数据
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        bool Upload3GXTran(List<BoExModel> boExModelList, string replaceOrLeave = null);

        /// <summary>
        /// 非事务方式（一个对象作为一个事务）导入3GX数据
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        int Upload3GX(List<BoExModel> boExModelList, string replaceOrLeave = null);

        /// <summary>
        /// 获取Bot相关的对象类别
        /// </summary>
        /// <param name="botList"></param>
        /// <returns></returns>
        List<string> GetBoc(List<string> botList);
   
        ///// <summary>
        ///// 根据查询条件返回对象集合
        ///// </summary>
        ///// <param name="filterList">查询条件，请注意参数表名：PROPERTY，参数名：PROTERTYNAME，参数值：PROPERTYVALUE</param>
        ///// <param name="parameter"></param>
        ///// <returns></returns>
        //List<BOModel> GetBoByFilter(List<string> filterList, Dictionary<string, object> parameter, string botid = null, string ns = null);
        //List<BOModel> GetBoByFilter(string filter);

        /// <summary>
        /// 返回对象名称和别名，用于词库分词
        /// </summary>
        /// <param name="isWithAlias"></param>
        /// <returns></returns>
        DataSet GetDictionary(bool isWithAlias);

        /// <summary>
        /// 根据对象类型获取该对象类型下的对象实例
        /// </summary>
        /// <param name="botID"></param>
        /// <returns></returns>
        List<BOModel> GetBoListByBOTID(string botID);

         /// <summary>
        /// 根据对象ID获取该对象信息
        /// </summary>
        /// <param name="BOID"></param>
        /// <returns></returns>
        DataTable GetBoListByID(string BOID);

        DataTable GetALIASNAME(string BOID);

        /// <summary>
        /// 添加对象以及对象参数和对象别名
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        bool InsertBOandPara(List<string> SqlList);
    }
}
