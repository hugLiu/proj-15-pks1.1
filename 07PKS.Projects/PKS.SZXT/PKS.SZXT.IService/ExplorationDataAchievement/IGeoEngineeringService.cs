using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDataAchievement
{
    public interface IGeoEngineeringService : IViewService
    {
        //获取地震工区查询属性
        object GetSWAProperties();

        //根据名称获取地震工区列表
        object GetSWAListByName(string name, int from, int size);

        //根据属性获取地震工区列表
        object GetSWAListByProperties(Dictionary<string, List<string>> properties, int? from, int? size);

        /// <summary>
        /// 根据SWA获取地震工区详情
        /// </summary>
        /// <param name="swa"></param>
        /// <returns>Jobject对象</returns>
        object GetSWAPTByName(string swa);
    }
}
