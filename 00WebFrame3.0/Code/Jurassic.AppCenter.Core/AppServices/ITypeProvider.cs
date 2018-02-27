using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.AppServices
{
    /// <summary>
    /// 为WCF服务提供AppUser、AppRole、AppFunction等的已知派生类型
    /// </summary>
    public interface ITypeProvider
    {
        /// <summary>
        /// 获取类型列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetTypes();
    }
}
