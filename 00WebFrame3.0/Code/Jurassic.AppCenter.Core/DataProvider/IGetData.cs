using System;
using System.Collections.Generic;
namespace Jurassic.AppCenter
{
    /// <summary>
    /// 定义可以从有限数据集合中返回全部数据的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGetData<T>
    {
        /// <summary>
        /// 获取全部T的列表
        /// </summary>
        /// <returns>全部T的集合</returns>
        IEnumerable<T> GetData();
    }
}
