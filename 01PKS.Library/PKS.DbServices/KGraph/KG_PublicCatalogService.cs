using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Models;
using PKS.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace PKS.DbServices
{
    /// <summary>公共图谱服务</summary>
    public class KG_PublicCatalogService : AppService, IPerRequestAppService
    {
        /// <summary>构造函数</summary>
        public KG_PublicCatalogService() { }
        /// <summary>获得全部</summary>
        public List<KG_CatalogNode> GetAll()
        {
            return GetService<IRepository<PKS_KG_PublicCatalog>>()
                .GetQuery()
                .OrderBy(e => e.LevelNumber)
                .ThenBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
        }
        /// <summary>获得公共分类第一级</summary>
        public List<KG_CatalogNode> GetFirstLevel()
        {
            return GetService<IRepository<PKS_KG_PublicCatalog>>()
                .GetQuery()
                .Where(e => e.LevelNumber == 0)
                .OrderBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
        }
        /// <summary>获得某个分类及其完整树</summary>
        public List<KG_CatalogNode> GetChildren(params int[] ids)
        {
            var models = GetService<IRepository<PKS_KG_PublicCatalog>>()
                .GetChildren(ids.Cast<object>().ToArray(), nameof(PKS_KG_PublicCatalog.ParentId))
                .OrderBy(e => e.LevelNumber)
                .ThenBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
            return models;
        }
        /// <summary>获得某个分类及其完整树</summary>
        public List<KG_CatalogNode> GetParents(params int[] ids)
        {
            var models = GetService<IRepository<PKS_KG_PublicCatalog>>()
                .GetParents(ids.Cast<object>().ToArray(), nameof(PKS_KG_PublicCatalog.ParentId))
                .OrderBy(e => e.LevelNumber)
                .ThenBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
            return models;
        }
        /// <summary>插入</summary>
        public KG_CatalogNode Insert(KG_CatalogNode model)
        {
            var entity = model.MapTo<PKS_KG_PublicCatalog>();
            GetService<IRepository<PKS_KG_PublicCatalog>>().Add(entity);
            return entity.MapTo<KG_CatalogNode>();
        }
        /// <summary>保存</summary>
        public KG_CatalogNode Save(KG_CatalogNode model)
        {
            var repo = GetService<IRepository<PKS_KG_PublicCatalog>>();
            PKS_KG_PublicCatalog entity = null;
            if (model.Id > 0)
            {
                entity = repo.Find(model.Id);
                model.MapTo(entity);
                entity.SetUpdate();
            }
            else
            {
                entity = model.MapTo<PKS_KG_PublicCatalog>();
                entity.SetInsert();
                repo.Add(entity, false);
            }
            repo.Submit();
            entity.MapTo(model);
            return model;
        }
        /// <summary>删除</summary>
        public void Delete(KG_CatalogNode model)
        {
            var repo = GetService<IRepository<PKS_KG_PublicCatalog>>();
            var entity = repo.Find(model.Id);
            var children = entity.Topics;
            foreach (var child in children)
            {
                child.PublicCatalogId = entity.ParentId;
                child.LastUpdatedBy = model.LastUpdatedBy;
                child.SetUpdate();
            }
            repo.Delete(entity, false);
            repo.Submit();
        }
        /// <summary>排序</summary>
        public void Sort(KG_CatalogNode[] models)
        {
            var repo = GetService<IRepository<PKS_KG_PublicCatalog>>();
            foreach (var model in models)
            {
                if (model == null) continue;
                var entity = repo.Find(model.Id);
                entity.OrderNumber = model.Order;
                if (!repo.IsModified(entity)) continue;
                entity.LastUpdatedBy = model.LastUpdatedBy;
                entity.SetUpdate();
                entity.MapTo(model);
            }
            repo.Submit();
        }
    }
}
