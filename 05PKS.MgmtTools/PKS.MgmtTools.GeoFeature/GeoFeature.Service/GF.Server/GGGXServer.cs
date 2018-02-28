using Jurassic.PKS.Service.GF;
using GF.Server.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GF.Server
{
    public class GGGXServer : IGGGX
    {
        /// <summary>
        /// 获取对象类型集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetBOTList()
        {
            return ObjectCreate<string>.CreateIGGGX("GGGXBusinesscs").GetBOTList();
        }

        /// <summary>
        /// 获取指定查询条件的取值
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<string> GetDomain(string ft, string parameter)
        {
            return ObjectCreate<string>.CreateIGGGX("GGGXBusinesscs").GetDomain(ft, parameter);
        }
        /// <summary>
        /// 获取FT集合
        /// </summary>
        /// <returns></returns>
        public FTCCollection GetFTCList()
        {
            return ObjectCreate<string>.CreateIGGGX("GGGXBusinesscs").GetFTCList();
        }

        /// <summary>
        /// 根据条件获取3GX数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public XmlDocument GetFeatures(FeatureFilter filter)
        {
            return ObjectCreate<string>.CreateIGGGX("GGGXBusinesscs").GetFeatures(filter);
        }

        /// <summary>
        /// 获取全部对象类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllBOT()
        {
            return ObjectCreate<string>.CreateIGGGX("GGGXBusinesscs").GetAllBOT();
        }
    }
}
