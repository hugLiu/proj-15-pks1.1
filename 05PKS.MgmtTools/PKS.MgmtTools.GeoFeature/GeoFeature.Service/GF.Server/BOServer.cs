using Jurassic.PKS.Service.GF;
using Jurassic.PKS.Service.Interfaces.GF;
using GF.Server.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GF.Server
{
    public class BOServer : IBO
    {
        /// <summary>
        /// 根据对象ID、G|P|B查询对象3GX数据，3GX数据中可包含坐标信息或参数信息或两者都包含
        /// </summary>
        /// <param name="boid">对象ID</param>
        /// <param name="category">枚举值[G|P|B]</param>
        /// <returns></returns>
        public XmlDocument Get3GXById(string boid, GGGXDataCategory category)
        {
            return ObjectCreate<XmlDocument>.CreateIBO("BOBusiness").Get3GXById(boid, category);
        }

        /// <summary>
        /// 根据业务对象ID和业务域查询业务对象别名
        /// </summary>
        /// <param name="boid">业务对象ID</param>
        /// <param name="appdomains">业务域</param>
        /// <returns></returns>
        public AliasCollection GetBOAliasByID(string boid, params string[] appdomains)
        {
            return ObjectCreate<AliasCollection>.CreateIBO("BOBusiness").GetBOAliasByID(boid, appdomains);
        }

        /// <summary>
        /// 根据对象别名、应用域查询业务对象。
        /// </summary>
        /// <param name="alias">对象别名</param>
        /// <param name="appdomain">应用域</param>
        /// <returns></returns>
        public BO GetBOByAlias(string alias, string appdomain)
        {
            return ObjectCreate<BO>.CreateIBO("BOBusiness").GetBOByAlias(alias, appdomain);
        }

        /// <summary>
        /// 根据对象ID查询业务对象
        /// </summary>
        /// <param name="boid">对象ID</param>
        /// <returns></returns>
        public BO GetBOById(string boid)
        {
            return ObjectCreate<BO>.CreateIBO("BOBusiness").GetBOById(boid);
        }

        /// <summary>
        /// 根据业务对象名称、对象类型查询对象ID
        /// </summary>
        /// <param name="name">业务对象名称</param>
        /// <param name="bot">对象类型</param>
        /// <returns></returns>
        public BO GetBOByName(string name, string bot)
        {
            return ObjectCreate<BO>.CreateIBO("BOBusiness").GetBOByName(name, bot);
        }

        /// <summary>
        /// 根据应用场景和过滤条件查询业务对象。通过对象的参数集进行过滤，返回符合条件的对象列表
        /// </summary>
        /// <param name="bot">业务对象类型</param>
        /// <param name="wktBBox">空间范围</param>
        /// <param name="filte">过滤条件</param>
        /// <returns></returns>
        public BOCollection GetBOListByFilter(string bot, string wktBBox, string filte)
        {
            return ObjectCreate<BOCollection>.CreateIBO("BOBusiness").GetBOListByFilter(bot, wktBBox, filte);
        }

        /// <summary>
        /// 根据业务对象类型和过滤条件获取对象列表
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public BOCollection GetBOListByType(string bot, string filter)
        {
            return ObjectCreate<BOCollection>.CreateIBO("BOBusiness").GetBOListByType(bot, filter);
        }

        /// <summary>
        /// 根据业务对象ID获取指定BO的父节点、下级节点、兄弟节点、相邻节点（父节点、兄弟节点和下级节点）和子树。返回的节点中不包括自己
        /// </summary>
        /// <param name="template">树模板</param>
        /// <returns></returns>
        public TreeBOCollection GetBOTree(BOTreeTemplate template)
        {
            return ObjectCreate<BOCollection>.CreateIBO("BOBusiness").GetBOTree(template); ;
        }

        /// <summary>
        /// 获取BO的叙词分类。主要用于短语分词的时候识别业务对象是什么类型
        /// </summary>
        /// <returns></returns>
        public TermBOCollection GetCCTermOfBO()
        {
            return ObjectCreate<TermBOCollection>.CreateIBO("BOBusiness").GetCCTermOfBO();
        }

        /// <summary>
        /// 根据业务对象ID查找在指定距离范围内的业务对象。
        /// 根据BO的空间坐标信息，计算出该对象指定范围内的要求的业务对象类型和业务对象类别的临近对象，
        /// 并按照与指定对象的距离进行排序，距离近的排在前面。返回对象包括它自己
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="distince"></param>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public NearBOCollection GetNearBOById(string boid, decimal distince, string bot, string filter)
        {
            return ObjectCreate<NearBOCollection>.CreateIBO("BOBusiness").GetNearBOById(boid, distince, bot, filter);
        }

        /// <summary>
        /// 根据WKT格式坐标、对象类型、对象类别查询在该坐标指定距离范围内的业务对象，
        /// 对象类型、对象类别为可选条件，为空则返回所有符合位置关系的对象。返回对象包括它自己
        /// </summary>
        /// <param name="pointWKT"></param>
        /// <param name="distince"></param>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public NearBOCollection GetNearBOByLocation(string pointWKT, decimal distince, string bot, string filter)
        {
            return ObjectCreate<NearBOCollection>.CreateIBO("BOBusiness").GetNearBOByLocation(pointWKT, distince, bot, filter);
        }

        /// <summary>
        /// 根据对象名称、对象类型查询在该对象指定距离范围内对象，
        /// 对象类型、对象类别为可选条件，为空则返回所有符合位置关系的对象。返回对象包括它自己
        /// </summary>
        /// <param name="boName"></param>
        /// <param name="boType"></param>
        /// <param name="distince"></param>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public NearBOCollection GetNearBOByName(string boName, string boType, decimal distince, string bot, string filter)
        {
            return ObjectCreate<NearBOCollection>.CreateIBO("BOBusiness").GetNearBOByName(boName, boType, distince, bot, filter);
        }

        /// <summary>
        /// 根据业务对象类型和应用域获取属性定义信息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="appDomain"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public List<PropertySchema> GetPropertySchema(string bot, string appDomain, List<PropertyName> names)
        {
            return ObjectCreate<NearBOCollection>.CreateIBO("BOBusiness").GetPropertySchema(bot, appDomain, names);
        }

        /// <summary>
        /// 获取DBINFO基本信息
        /// </summary>
        /// <returns></returns>
        public DbInfo GetDbInfo()
        {
            return ObjectCreate<NearBOCollection>.CreateIBO("BOBusiness").GetDbInfo();
        }

        /// <summary>
        /// 根据过滤条件获取3GX数据
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="bos"></param>
        /// <param name="filter"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public XmlDocument Get3GXByFilter(string bot, List<string> bos, string filter, GGGXDataCategory category)
        {
            return ObjectCreate<XmlDocument>.CreateIBO("BOBusiness").Get3GXByFilter(bot, bos, filter, category);
        }

        /// <summary>
        /// 获取应用场景已经参数
        /// </summary>
        /// <returns></returns>
        public AppDomainCollection GetAppDomains()
        {
            return ObjectCreate<AppDomainCollection>.CreateIBO("BOBusiness").GetAppDomains();
        }
    }
}
