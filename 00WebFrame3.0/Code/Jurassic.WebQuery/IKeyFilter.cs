using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 用于高级查询中自定义查询条件的接口
    /// </summary>
    /// <typeparam name="T">用于查询的数据实体类型</typeparam>
    public interface IKeyFilter<T> where T : class
    {
        /// <summary>
        /// 返回自定义的查询条件表达式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Expression<Func<T, bool>> GetKeyFilter(string key);
    }
}
