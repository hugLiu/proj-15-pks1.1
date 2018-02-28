using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.Factory;

namespace Jurassic.GF.Server
{
    class BOPropertyServer : IBOProperty
    {
        /// <summary>
        /// 删除对象参数
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public bool DelProperty(string boid, string ns)
        {
            return ObjectCreate<PropertyModel>.CreatIBOProperty("BOPropertyBusiness").DelProperty(boid, ns);
        }

        /// <summary>
        /// 判断对象参数是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool ExistProperty(PropertyModel property)
        {
            return ObjectCreate<PropertyModel>.CreatIBOProperty("BOPropertyBusiness").ExistProperty(property);
        }

        /// <summary>
        /// 根据对象ID获取对象参数
        /// </summary>
        /// <param name="boid"></param>
        public List<PropertyModel> GetListByID(string boid, string ns = null)
        {
            return ObjectCreate<PropertyModel>.CreatIBOProperty("BOPropertyBusiness").GetListByID(boid, ns);
        }

        /// <summary>
        /// 添加对象参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool InsertProperty(PropertyModel property)
        {
            return ObjectCreate<PropertyModel>.CreatIBOProperty("BOPropertyBusiness").InsertProperty(property);
        }

        /// <summary>
        /// 修改对象参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool UpdateProperty(PropertyModel property)
        {
            return ObjectCreate<PropertyModel>.CreatIBOProperty("BOPropertyBusiness").UpdateProperty(property);
        }
    }
}
