using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 在QueryDataProvider的基础上，再增加分页功能的查询的数据提供类，
    /// </summary>
    /// <typeparam name="T">列表元素的数据类型</typeparam>
    public interface ILinqDataProvider<T> : IDataCUD<T>, IGetQueryData<T>
    {
        /// <summary>
        /// 带条件的分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="page">分页号</param>
        /// <param name="pageSize">页的大小</param>
        /// <param name="sortExpression">排序字段和排序方向</param>
        /// <returns>分页后带分页信息的数据</returns>
        Pager<T> PageQuery(Expression<Func<T, bool>> whereExpression, int page, int pageSize, string sortExpression = null);

        /// <summary>
        /// 带字符串表示的条件的分页查询
        /// </summary>
        /// <param name="page">分页号</param>
        /// <param name="pageSize">页的大小</param>
        /// <param name="whereExpression">条件的字符串表达式</param>
        /// <param name="sortExpression">排序的字符串表达式</param>
        /// <returns>分页后带分页信息的数据</returns>
        Pager<T> PageQuery(int page, int pageSize, string whereExpression = null, string sortExpression = null);
    }
}
