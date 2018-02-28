using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationResearchAchievement
{
    public interface ITargetEvaluationService : IViewService
    {

        /// <summary>
        ///  获取目标区和圈闭的查询过滤属性
        /// </summary>
        /// <returns></returns>
        object GetTrapProperties();

        /// <summary>
        ///  根据名称获取圈闭列表
        /// </summary>
        /// <returns></returns>
        object GetTrapListByName(string name, int? from, int? size);

        /// <summary>
        /// 根据属性获取圈闭列表
        /// </summary>
        /// <returns></returns>
        object GetTrapListByProperties(Dictionary<string, List<string>> properties, int? from, int? size);

        /// <summary>
        /// 根据圈闭名称获取圈闭详情
        /// </summary>
        /// <param name="Trap"></param>
        /// <returns>Jobject对象</returns>
        object GetTrapPTByName(string Trap);
        /// <summary>
        /// 根据圈闭名称和grid获取圈闭详情
        /// </summary>
        /// <param name="Trap"></param>
        /// <param name="grid">json文件中的“G1_1”等 grid序号</param>
        /// <returns></returns>
        object GetTrapPTByName(string Trap,string grid);
    }
}
