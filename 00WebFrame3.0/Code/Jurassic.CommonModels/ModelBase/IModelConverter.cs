using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 定义从物理模型到数据模型的单向转换
    /// </summary>
    /// <typeparam name="TModel">数据模型</typeparam>
    /// <typeparam name="TEntity">物理模型</typeparam>
    public interface IModelConverter<TModel, TEntity>
    {
        /// <summary>
        /// 物理模型转换成数据模型的表达式
        /// </summary>
        Expression<Func<TEntity, TModel>> EntityToModel
        {
            get;
        }
    }
}
