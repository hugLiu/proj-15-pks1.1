using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IHisGeometry
    {
        /// <summary>
        /// 判断历史空间坐标
        /// </summary>
        /// <param name="HisGeometry"></param>
        /// <returns></returns>
        bool ExistHisGeometry(HisGeometryModel HisGeometry);

        /// <summary>
        /// 添加历史坐标
        /// </summary>
        /// <param name="HisGeometry"></param>
        /// <returns></returns>
        bool InsertHisGeometry(HisGeometryModel HisGeometry);

        /// <summary>
        /// 修改历史坐标
        /// </summary>
        /// <param name="HisGeometry"></param>
        /// <returns></returns>
        bool UpdateHisGeometry(HisGeometryModel HisGeometry);

        /// <summary>
        /// 删除历史坐标
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        bool DelHisGeometry(string boid);

        /// <summary>
        /// 根据对象ID获取对象历史坐标
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        List<HisGeometryModel> GetHisGeometryByID(string boid, string gatherid);
    }
}
