using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IHisBOProperty
    {
        /// <summary>
        /// 判断历史属性是否
        /// </summary>
        /// <param name="HisProperty"></param>
        /// <returns></returns>
        bool ExistHisProperty(HisPropertyModel HisProperty);

        /// <summary>
        /// 添加历史属性
        /// </summary>
        /// <param name="HisProperty"></param>
        /// <returns></returns>
        bool InsertHisProperty(HisPropertyModel HisProperty);

        /// <summary>
        /// 修改历史属性
        /// </summary>
        /// <param name="HisProperty"></param>
        /// <returns></returns>
        bool UpdateHisProperty(HisPropertyModel HisProperty);

        /// <summary>
        /// 删除历史属性
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        bool DelHisProperty(string boid, string ns);

        /// <summary>
        /// 根据对象ID获取对象历史属性
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        List<HisPropertyModel> GetHisPropertyByID(string boid, string ns, string gatherid);
    }
}
