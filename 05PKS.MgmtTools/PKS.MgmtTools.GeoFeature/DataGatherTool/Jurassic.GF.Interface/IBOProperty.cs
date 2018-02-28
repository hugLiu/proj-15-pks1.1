using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IBOProperty
    {
        /// <summary>
        /// 判断对象参数是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool ExistProperty(PropertyModel property);

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool InsertProperty(PropertyModel property);

        /// <summary>
        /// 修改对象参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool UpdateProperty(PropertyModel property);

        /// <summary>
        /// 删除对象参数
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        bool DelProperty(string boid, string ns);

        /// <summary>
        /// 根据对象ID获取对象参数集
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        List<PropertyModel> GetListByID(string boid, string ns = null);
    }
}
