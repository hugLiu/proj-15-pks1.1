using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 支持数据批量增删改的数据接口
    /// </summary>
    /// <typeparam name="T">数据实体类型</typeparam>
    public interface IDataBatchCUD<T> : IDataCUD<T>
        where T : class
    {

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        int Add(IEnumerable<T> ts);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        int Change(IEnumerable<T> ts);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        int DeleteByKeys(IEnumerable keys);
    }
}
