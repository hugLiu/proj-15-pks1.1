using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.Factory;
using GGGXParse;

namespace Jurassic.GF.Server
{
    public class DataGatherServer : IDataGather
    {
        public bool DelDataGather(string boid)
        {
            throw new NotImplementedException();
        }

        public bool ExistDataGather(DataGatherModel DataGather)
        {
            return ObjectCreate<DataGatherModel>.CreateIDataGather("DataGatherBusiness").ExistDataGather(DataGather);
        }

        /// <summary>
        /// 根据对象类型ID获取版本列表
        /// </summary>
        /// <param name="BOTID"></param>
        /// <returns></returns>
        public List<DataGatherModel> GetDataGatherByBOTID(string BOTID)
        {
            return ObjectCreate<DataGatherModel>.CreateIDataGather("DataGatherBusiness").GetDataGatherByBOTID(BOTID);
        }

        public bool InsertDataGather(DataGatherModel DataGather)
        {
            return ObjectCreate<DataGatherModel>.CreateIDataGather("DataGatherBusiness").InsertDataGather(DataGather);
        }

        /// <summary>
        /// 数据采集
        /// </summary>
        /// <param name="dataGather"></param>
        /// <param name="SaveData"></param>
        /// <returns></returns>
        public SubmissionResult Submit(DataGatherModel dataGather, List<GeoFeature> SaveData)
        {
            return ObjectCreate<DataGatherModel>.CreateIDataGather("DataGatherBusiness").Submit(dataGather, SaveData);
        }

        public bool UpdateDataGather(DataGatherModel DataGather)
        {
            return ObjectCreate<DataGatherModel>.CreateIDataGather("DataGatherBusiness").UpdateDataGather(DataGather);
        }
    }
}
