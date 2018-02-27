using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.EntityBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 基于数据模型到物理模型的数据访问提供程序
    /// </summary>
    /// <typeparam name="TModel">数据模型</typeparam>
    /// <typeparam name="TEntity">物理模型</typeparam>
    public class ModelDataService<TModel, TEntity> : IModelDataService<TModel>
        where TModel : class, IId<int>
        where TEntity : class ,IId<int>
    {
        EFAuditDataService<TEntity> _innerDataProvider;
        ModelRule _entityRule;
        ModelRule _modelRule;
        LangDataMapper<TModel, TEntity> _dataMapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="innerDataProvider"></param>
        public ModelDataService(EFAuditDataService<TEntity> innerDataProvider)
        {
            _innerDataProvider = innerDataProvider;
            _dataMapper = new LangDataMapper<TModel, TEntity>();
            _entityRule = ModelRule.Get<TEntity>();
            _modelRule = ModelRule.Get<TModel>();
        }

        /// <summary>
        /// 根据指定主键获取对应的业务模型对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TModel GetByKey(object key)
        {
            int id = CommOp.ToInt(key);
            var t = GetQuery().FirstOrDefault(e => e.Id == id);
            GetAttachmentsCollection(t);
            GetCollections(t);
            return t;
        }

        /// <summary>
        /// 获取强主外键关联的子表对象集合
        /// </summary>
        /// <typeparam name="TItemModel">子表的业务模型类</typeparam>
        /// <typeparam name="TItemEntity">子表的数据模型类</typeparam>
        /// <param name="modelId">主表ID</param>
        /// <returns>子表集合</returns>
        protected List<TItemModel> GetCollection<TItemModel, TItemEntity>(int modelId)
            where TItemModel : class, IId<int>
            where TItemEntity : class, IDetailEntity
        {
            using (var service = SiteManager.Get<EFAuditDataService<TItemEntity>>())
            {

                var list = service.GetQuery().Where(item => item.MasterId == modelId);
                if (typeof(ICanLogicalDeleteEntity).IsAssignableFrom(typeof(TItemEntity)))
                {
                    list = list.Where("IsDeleted=False");
                }
                var langMapper = new LangDataMapper<TItemModel, TItemEntity>();
                return list.ProjectTo<TItemModel>().ToList();
            }
        }

        /// <summary>
        /// 获取非强主外键关联的子表对象集合
        /// </summary>
        /// <typeparam name="TItemModel">非主外键关联的子表业务实体类型</typeparam>
        /// <typeparam name="TItemEntity">非主外键关联的子表数据实体类型</typeparam>
        /// <param name="modelId"></param>
        /// <returns></returns>
        protected List<TItemModel> GetAttachmentCollection<TItemModel, TItemEntity>(int modelId)
            where TItemModel : class, IId<int>
            where TItemEntity : class, IAttachmentEntity
        {
            using (var service = SiteManager.Get<EFAuditDataService<TItemEntity>>())
            {
                string moduleCode = typeof(TEntity).Name;
                var list = service.GetQuery().Where(item => item.BillId == modelId && item.ModuleCode == moduleCode);

                if (typeof(ICanLogicalDeleteEntity).IsAssignableFrom(typeof(TItemEntity)))
                {
                    list = list.Where("IsDeleted=False");
                }
                var langMapper = new LangDataMapper<TItemModel, TItemEntity>();
                return list.ProjectTo<TItemModel>().ToList();
            }
        }

        /// <summary>
        /// 获取非强主外键关联的子表对象集合
        /// </summary>
        /// <param name="model"></param>
        private void GetAttachmentsCollection(TModel model)
        {
            MethodInfo mi = this.GetType().GetMethod("GetAttachmentCollection", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            foreach (var coll in _modelRule.CollectionRules.Where(r => typeof(IAttachmentEntity).IsAssignableFrom(r.Attr.EntityType)))
            {
                MethodInfo gm = mi.MakeGenericMethod(coll.ModelType, coll.Attr.EntityType);
                var list = gm.Invoke(this, new object[] { model.Id });
                coll.Attr.Property.SetValue(model, list, null);
            }
        }

        /// <summary>
        /// 获取强主外键关联的子表对象集合
        /// </summary>
        /// <param name="model"></param>
        private void GetCollections(TModel model)
        {
            MethodInfo mi = this.GetType().GetMethod("GetCollection", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            foreach (var coll in _modelRule.CollectionRules.Where(r => typeof(IDetailEntity).IsAssignableFrom(r.Attr.EntityType)))
            {
                var oldList = coll.Attr.Property.GetValue(model, null) as IList;
                if (oldList != null && oldList.Count > 0)
                {
                    continue;
                }
                MethodInfo gm = mi.MakeGenericMethod(coll.ModelType, coll.Attr.EntityType);
                var list = gm.Invoke(this, new object[] { model.Id });
                coll.Attr.Property.SetValue(model, list, null);
            }
        }

        /// <summary>
        /// 获取数据模型对象中的对应集合数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="itemEntityType"></param>
        /// <returns></returns>
        private IList GetCollectionInEntity(TEntity data, Type itemEntityType)
        {
            var rule = _entityRule.CollectionRules.FirstOrDefault(r => r.ModelType == itemEntityType);
            if (rule != null)
            {
                return RefHelper.GetValue(data, rule.Name) as IList;
            }
            return null;
        }

        protected virtual void MarkState(object t, EntityState state)
        {
            _innerDataProvider.MarkState(t, state);
        }

        /// <summary>
        /// 批量处理数据对象中的强关联集合数据， 此方法被反射调用
        /// </summary>
        /// <typeparam name="TItemModel">业务对象中的集合元素类型</typeparam>
        /// <typeparam name="TItemEntity">数据对象中的集合元素类型</typeparam>
        /// <param name="model">业务对象</param>
        /// <param name="data">数据对象</param>
        private void ProcessCollection<TItemModel, TItemEntity>(TModel model, TEntity data)
            where TItemModel : class, IId<int>
            where TItemEntity : class, IDetailEntity
        {
            ICollection<TItemEntity> dataItemCollection = _entityRule.GetCollectionValue<TItemEntity>(data) as ICollection<TItemEntity>;
            ICollection<TItemModel> modelItemList = _modelRule.GetCollectionValue<TItemModel>(model) as ICollection<TItemModel>;

            //重新从数据库中加载原有集合以和现有集合进行比对
            var oldModelItemList = GetCollection<TItemModel, TItemEntity>(model.Id);
            var itemMapper = new LangDataMapper<TItemModel, TItemEntity>();
            //将新集合与已有集合进行比对，凡不在新集合中的原有集合ID所指对象代表已经被删除
            var deletedItemIds = oldModelItemList.Select(m => m.Id).Except(modelItemList.Select(m => m.Id));
            var deletedItems = oldModelItemList.Where(m => deletedItemIds.Contains(m.Id));
            foreach (var modelItem in deletedItems)
            {
                var entityItem = itemMapper.ToEntity(modelItem);
                var logicalDeleteItem = entityItem as ICanLogicalDeleteEntity;
                if (logicalDeleteItem != null)
                {
                    logicalDeleteItem.IsDeleted = true;
                    MarkState(entityItem, EntityState.Modified);
                }
                else
                {
                    MarkState(entityItem, EntityState.Deleted);
                }
            }
            //判断业务模型的子表哪些数据是新增/修改或删除的，转换成数据实体子项后，对其加上对应的EF标记
            foreach (var modelItem in modelItemList)
            {
                var entityItem = itemMapper.ToEntity(modelItem);

                if (entityItem is IDetailEntity<TEntity>)
                {
                    ((IDetailEntity<TEntity>)entityItem).Master = data;
                }
                entityItem.MasterId = data.Id;

                if (modelItem.Id == 0)
                {
                    dataItemCollection.Add(entityItem);
                    MarkState(entityItem, EntityState.Added);
                }
                else
                {
                    var oldModelItem = oldModelItemList.FirstOrDefault(item => item.Id == modelItem.Id);
                    // 与原有ID相同的对象比较，如果内容不同说明已经被修改
                    if (CompareChanged(modelItem, oldModelItem))
                    {
                        MarkState(entityItem, EntityState.Modified);
                    }
                }
            }
        }

        /// <summary>
        /// 批量处理数据对象中的非强关联集合数据， 此方法被反射调用
        /// </summary>
        /// <typeparam name="TItemModel">业务对象中的集合元素类型</typeparam>
        /// <typeparam name="TItemEntity">数据对象中的集合元素类型</typeparam>
        /// <param name="model">业务对象</param>
        /// <param name="data">数据对象</param>
        protected void ProcessAttachmentCollection<TItemModel, TItemEntity>(TModel model, TEntity data)
            where TItemModel : class, IId<int>
            where TItemEntity : class, IAttachmentEntity
        {
            ICollection<TItemEntity> dataItemCollection = _entityRule.GetCollectionValue<TItemEntity>(data) as ICollection<TItemEntity>;
            ICollection<TItemModel> modelItemList = _modelRule.GetCollectionValue<TItemModel>(model) as ICollection<TItemModel>;

            //重新从数据库中加载原有集合以和现有集合进行比对

            var oldModelItemList = GetAttachmentCollection<TItemModel, TItemEntity>(model.Id);
            var itemMapper = new LangDataMapper<TItemModel, TItemEntity>();

            //将新集合与已有集合进行比对，凡不在新集合中的原有集合ID所指对象代表已经被删除
            var deletedItemIds = oldModelItemList.Select(m => m.Id).Except(modelItemList.Select(m => m.Id));
            var deletedItems = oldModelItemList.Where(m => deletedItemIds.Contains(m.Id));
            foreach (var modelItem in deletedItems)
            {
                var entityItem = itemMapper.ToEntity(modelItem);
                var logicalDeleteItem = entityItem as ICanLogicalDeleteEntity;
                if (logicalDeleteItem != null)
                {
                    logicalDeleteItem.IsDeleted = true;
                    MarkState(entityItem, EntityState.Modified);
                }
                else
                {
                    MarkState(entityItem, EntityState.Deleted);
                }
            }

            //判断业务模型的子表哪些数据是新增/修改或删除的，转换成数据实体子项后，对其加上对应的EF标记
            foreach (var modelItem in modelItemList)
            {
                var entityItem = itemMapper.ToEntity(modelItem);

                entityItem.BillId = data.Id;
                entityItem.ModuleCode = typeof(TEntity).Name;

                if (entityItem.Id == 0)
                {
                    dataItemCollection.Add(entityItem);
                    MarkState(entityItem, EntityState.Added);
                }
                else
                {
                    var oldModelItem = oldModelItemList.FirstOrDefault(item => item.Id == modelItem.Id);
                    // 与原有ID相同的对象比较，如果内容不同说明已经被修改
                    if (CompareChanged(modelItem, oldModelItem))
                    {
                        MarkState(entityItem, EntityState.Modified);
                    }
                }
            }
        }

        private bool CompareChanged<TItemModel>(TItemModel model1, TItemModel model2)
        {
            return !JsonHelper.ToJson(model1).Equals(JsonHelper.ToJson(model2));
        }

        /// <summary>
        /// 批量处理业务对象中明细表对象集
        /// </summary>
        /// <param name="model">业务对象</param>
        /// <param name="data">数据对象</param>
        protected void ProcessCollections(TModel model, TEntity data)
        {
            MethodInfo mi = this.GetType().GetMethod("ProcessCollection", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            foreach (var rule in _modelRule.CollectionRules.Where(r => !typeof(IAttachmentEntity).IsAssignableFrom(r.Attr.EntityType)))
            {
                var gmi = mi.MakeGenericMethod(rule.ModelType, rule.Attr.EntityType);
                gmi.Invoke(this, new object[] { model, data });
            }
        }

        private void ProcessAttachmentCollections(TModel model, TEntity data)
        {
            MethodInfo mi = this.GetType().GetMethod("ProcessAttachmentCollection", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            foreach (var rule in _modelRule.CollectionRules.Where(r => typeof(IAttachmentEntity).IsAssignableFrom(r.Attr.EntityType)))
            {
                var gmi = mi.MakeGenericMethod(rule.ModelType, rule.Attr.EntityType);
                gmi.Invoke(this, new object[] { model, data });
            }
        }

        /// <summary>
        /// 将新增的业务模型对象保存到数据库表，并返回新增后的Id
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Add(TModel t)
        {
            TEntity data = _dataMapper.ToEntity(t);
            _innerDataProvider.BeginTrans();
            ProcessCollections(t, data);
            try
            {
                var r = _innerDataProvider.Add(data);
                ProcessAttachmentCollections(t, data);
                _dataMapper.SaveLanguages(_innerDataProvider, data, t);
                _innerDataProvider.EndTrans();
                t.Id = data.Id;
                return data.Id;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errMsg = "Validation Error:" + String.Join(";", ex.EntityValidationErrors.Select(val => String.Join(",", val.ValidationErrors.Select(v => v.ErrorMessage))));
                _innerDataProvider.RollbackTrans();
                throw new JException(errMsg);
            }
            catch
            {
                _innerDataProvider.RollbackTrans();
                throw;
            }
        }

        /// <summary>
        /// 将修改后的业务模型保存到数据库表，并返回业务模型的ID
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Change(TModel t)
        {
            TEntity data = _dataMapper.ToEntity(t);
            _innerDataProvider.BeginTrans();
            ProcessCollections(t, data);
            try
            {
                var r = _innerDataProvider.Change(data);
                ProcessAttachmentCollections(t, data);
                _dataMapper.SaveLanguages(_innerDataProvider, data, t);
                _innerDataProvider.EndTrans();
                return data.Id;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errMsg = String.Join(";", ex.EntityValidationErrors.Select(val => String.Join(",", val.ValidationErrors.Select(v => v.ErrorMessage))));
                _innerDataProvider.RollbackTrans();
                throw new JException(errMsg);
            }
            catch
            {
                _innerDataProvider.RollbackTrans();
                throw;
            }
        }

        /// <summary>
        /// 在数据库表中删除指定的业务模型对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Delete(TModel t)
        {
            TEntity data = _dataMapper.ToEntity(t);
            if (data is ICanLogicalDeleteEntity)
            {
                (data as ICanLogicalDeleteEntity).IsDeleted = true;
                return _innerDataProvider.Change(data);
            }
            else
            {
                return _innerDataProvider.Delete(data);
            }
        }

        /// <summary>
        /// 返回业务模型对象全部的结果查询表达式
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TModel> GetQuery()
        {
            IQueryable<TEntity> query = _innerDataProvider.GetQuery();
            if (typeof(MultiLanguage).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Include("LangTexts");
            }

            //如果实现逻辑删除接口，并且没有改写EFAuditDataSerice,则在此加条件
            if (typeof(ICanLogicalDeleteEntity).IsAssignableFrom(typeof(TEntity)) && _innerDataProvider.GetType() == typeof(EFAuditDataService<TEntity>))
            {
                query = query.Where("IsDeleted=false");
            }

            if (typeof(IDataRule).IsAssignableFrom(typeof(TEntity)))
            {
                var context = _innerDataProvider.GetContext();
                var userId = AppManager.Instance.GetCurrentUserId().ToInt();
                query = query.Where(t => context.Set<Sys_DataRule>().Any(dr => dr.BillId == t.Id && dr.ObjectId == userId));
            }
            var langMapper = new LangDataMapper<TModel, TEntity>();
            var duQuery = new DeptUserAuthQuery<TModel>();
            return duQuery.GetQuery(query.ProjectTo<TModel>());
        }


        /// <summary>
        /// 根据一串ID在数据库中删除对应的全部数据对象
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual int DeleteByKeys(IEnumerable keys)
        {
            if (typeof(ICanLogicalDeleteEntity).IsAssignableFrom(typeof(TEntity)))
            {
                var ids = keys.Each(key => CommOp.ToInt(key)).ToArray();
                var logicalDeletedList = _innerDataProvider.GetQuery().Where(t => ids.Contains(t.Id))
                      .ToArray()
                      .Each(t => { (t as ICanLogicalDeleteEntity).IsDeleted = true; });
                return _innerDataProvider.Change(logicalDeletedList);
            }
            else
            {
                return _innerDataProvider.DeleteByKeys(keys);
            }
        }

        /// <summary>
        /// 根据一个新增对象集合在数据库中新增对应对象
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public virtual int Add(IEnumerable<TModel> ts)
        {
            if (ts.IsEmpty())
            {
                return 0;
            }
            var dataList = new List<TEntity>();
            foreach (var t in ts)
            {
                dataList.Add(_dataMapper.ToEntity(t));
            };

            return _innerDataProvider.Add(dataList);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public virtual int Change(IEnumerable<TModel> ts)
        {
            if (ts.IsEmpty())
            {
                return 0;
            }
            var dataList = new List<TEntity>();
            foreach (var t in ts)
            {
                dataList.Add(_dataMapper.ToEntity(t));
            };

            return _innerDataProvider.Change(dataList);
        }

        public void Dispose()
        {
            if (_innerDataProvider != null)
            {
                _innerDataProvider.Dispose();
                _innerDataProvider = null;
            }
        }
    }
}