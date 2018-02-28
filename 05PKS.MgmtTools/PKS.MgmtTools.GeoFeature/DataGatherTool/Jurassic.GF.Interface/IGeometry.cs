using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IGeometry
    {
        /// <summary>
        /// 判断空间坐标
        /// </summary>
        /// <param name="Geometry"></param>
        /// <returns></returns>
        bool ExistGeometry(GeometryModel Geometry);

        /// <summary>
        /// 添加坐标
        /// </summary>
        /// <param name="Geometry"></param>
        /// <returns></returns>
        bool InsertGeometry(GeometryModel Geometry);

        /// <summary>
        /// 修改坐标
        /// </summary>
        /// <param name="Geometry"></param>
        /// <returns></returns>
        bool UpdateGeometry(GeometryModel Geometry);

        /// <summary>
        /// 删除坐标
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        bool DelGeometry(string boid);

        /// <summary>
        /// 根据对象ID获取对象坐标
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        List<GeometryModel> GetGeometryByID(string boid);
    }
}
