using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EntityBase;
using Jurassic.CommonModels.ModelBase;
using Jurassic.WebFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 通用的列表界面控制器基类,实体的主键是Int类型
    /// </summary>
    /// <typeparam name="TModel">模型的类型</typeparam>
    /// <typeparam name="TEntity">数据实体的类型</typeparam>
    public class AdvDataController<TModel, TEntity> : BaseDataController<TModel, int>
        where TModel : class, IId<int>
        where TEntity : class, IId<int>
    {
        /// <summary>
        /// 重写，以兼容ModelDataService[Tmoel, TEntity]
        /// </summary>
        protected override IModelDataService<TModel> DataService
        {
            get
            {
                if (_dataService == null)
                {
                    _dataService =
                        SiteManager.Get<ModelDataService<TModel, TEntity>>()
                  ?? SiteManager.Get<IModelDataService<TModel>>();
                }
                return _dataService;
            }
        }

        /// <summary>
        /// 重写以实现ID的默认值为0
        /// </summary>
        /// <param name="id">数据项ID</param>
        /// <returns>编辑页视图</returns>
        public override ActionResult Edit(int id = default(int))
        {
            return base.Edit(id);
        }
    }
}