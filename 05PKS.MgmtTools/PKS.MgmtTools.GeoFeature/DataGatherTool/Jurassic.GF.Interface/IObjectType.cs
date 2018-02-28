using Jurassic.GF.Interface.Model;
using System.Collections.Generic;

namespace Jurassic.GF.Interface
{
    public interface IObjectType
    {
        /// <summary>
        /// 获取全部对象类型
        /// </summary>
        /// <returns></returns>
        List<ObjectTypeModel> GetAllObjectType();
    }
}
