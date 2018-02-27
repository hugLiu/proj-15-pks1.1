using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 定义数据模型和物理模型类型相同时的默认转换
    /// </summary>
    /// <typeparam name="TModel">数据模型</typeparam>
    /// <typeparam name="TEntity">物理模型</typeparam>
    public class SameEntityModelConverter<TModel, TEntity> : IModelEntityConverter<TModel, TEntity>
        where TModel : class
        where TEntity : class
    {
        public virtual Expression<Func<TModel, TEntity>> ModelToEntity
        {
            get { return t => t as TEntity; }
        }

        public virtual Expression<Func<TEntity, TModel>> EntityToModel
        {
            get { return t => t as TModel; }
        }
    }
}
