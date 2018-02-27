using System;
namespace Jurassic.AppCenter
{
    /// <summary>
    /// 定义可以增删改的数据接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataCUD<T>
    {
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="t">要新增的T对象</param>
        /// <returns>成功返回大于零的数，不成功返回0</returns>
        int Add(T t);

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="t">要修改的对象T</param>
        /// <returns>成功返回大于零的数，不成功返回0</returns>
        int Change(T t);

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="t">要删除的T对象</param>
        /// <returns>成功返回大于零的数，不成功返回0</returns>
        int Delete(T t);
    }
}
