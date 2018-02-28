using GGGXParse;
using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IDataGather
    {
        /// <summary>
        /// 判断版本是否存在
        /// </summary>
        /// <param name="DataGather"></param>
        /// <returns></returns>
        bool ExistDataGather(DataGatherModel DataGather);

        /// <summary>
        /// 添加版本
        /// </summary>
        /// <param name="DataGather"></param>
        /// <returns></returns>
        bool InsertDataGather(DataGatherModel DataGather);

        /// <summary>
        /// 修改版本
        /// </summary>
        /// <param name="DataGather"></param>
        /// <returns></returns>
        bool UpdateDataGather(DataGatherModel DataGather);

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        bool DelDataGather(string boid);

        /// <summary>
        /// 根据对象类型ID获取版本列表
        /// </summary>
        /// <returns></returns>
        List<DataGatherModel> GetDataGatherByBOTID(string BOTID);

        /// <summary>
        /// 数据采集
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        SubmissionResult Submit(DataGatherModel dataGather, List<GeoFeature> SaveData);

    }
}
