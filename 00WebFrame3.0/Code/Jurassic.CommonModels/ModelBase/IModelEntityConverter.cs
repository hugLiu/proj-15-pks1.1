using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 定义数据模型和物理模型之间的相互转换，用于Linq to Entity的查询场景中
    /// </summary>
    /// <typeparam name="TModel">数据模型</typeparam>
    /// <typeparam name="TEntity">物理模型</typeparam>
    public interface IModelEntityConverter<TModel, TEntity> : IModelConverter<TModel, TEntity>
    {
        /// <summary>
        /// 数据模型转换成物理模型的表达式
        /// </summary>
        Expression<Func<TModel, TEntity>> ModelToEntity
        {
            get;
        }

        /// <summary>
        /// 物理模型转换成数据模型的表达式
        /// </summary>
        Expression<Func<TEntity, TModel>> EntityToModel
        {
            get;
        }
    }
}
