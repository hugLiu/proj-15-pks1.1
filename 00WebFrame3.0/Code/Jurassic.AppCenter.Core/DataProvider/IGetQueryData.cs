using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 支持IQuerable的对象查询类
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IGetQueryData<T>
    {
        /// <summary>
        /// 获取可查询对象，在此基础上可以附加查询条件
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetQuery();

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns>T实体</returns>
        T GetByKey(object key);
    }
}
