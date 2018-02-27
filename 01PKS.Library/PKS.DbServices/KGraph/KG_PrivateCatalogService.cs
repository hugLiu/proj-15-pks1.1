using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Models;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKS.DbServices
{
    /// <summary>私有图谱服务</summary>
    public class KG_PrivateCatalogService : AppService, IPerRequestAppService
    {
        /// <summary>构造函数</summary>
        public KG_PrivateCatalogService() { }
        /// <summary>获得全部</summary>
        public List<KG_CatalogNode> GetAll(string user)
        {
            return GetService<IRepository<PKS_KG_PrivateCatalog>>()
                .GetQuery()
                .Where(e => e.CreatedBy == user)
                .OrderBy(e => e.LevelNumber)
                .ThenBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
        }
        /// <summary>保存</summary>
        public KG_CatalogNode Save(KG_CatalogNode model)
        {
            var repo = GetService<IRepository<PKS_KG_PrivateCatalog>>();
            PKS_KG_PrivateCatalog entity = null;
            if (model.Id > 0)
            {
                entity = repo.Find(model.Id);
                model.MapTo(entity);
                entity.SetUpdate();
            }
            else
            {
                entity = model.MapTo<PKS_KG_PrivateCatalog>();
                entity.SetInsert();
                repo.Add(entity, false);
            }
            repo.Submit();
            entity.MapTo(model);
            return model;
        }
        /// <summary>删除</summary>
        public void Delete(int id, string user)
        {
            var repo = GetService<IRepository<PKS_KG_PrivateCatalog>>();
            var entity = repo.Find(id);
            var children = entity.Topics;
            foreach (var child in children)
            {
                child.PrivateCatalogId = entity.ParentId;
                child.LastUpdatedBy = user;
                child.LastUpdatedDate = DateTime.Now;
            }
            repo.Delete(entity, false);
            repo.Submit();
        }
        /// <summary>排序</summary>
        public void Sort(KG_CatalogNode[] models)
        {
            var repo = GetService<IRepository<PKS_KG_PrivateCatalog>>();
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
        /// <summary>获得某个分类及其完整树</summary>
        public List<KG_CatalogNode> GetChildren(string user, params int[] ids)
        {
            var repo = GetService<IRepository<PKS_KG_PrivateCatalog>>();
            var where = $"{repo.GetStoreFieldName(nameof(PKS_KG_PrivateCatalog.CreatedBy))}='{user}'";
            var models = repo.GetChildren(ids.Cast<object>().ToArray(), nameof(PKS_KG_PrivateCatalog.ParentId), where)
                .OrderBy(e => e.LevelNumber).ThenBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
            return models;
        }
        /// <summary>获得某个分类及其完整树</summary>
        public List<KG_CatalogNode> GetParents(string user, params int[] ids)
        {
            var repo = GetService<IRepository<PKS_KG_PrivateCatalog>>();
            var where = $"{repo.GetStoreFieldName(nameof(PKS_KG_PrivateCatalog.CreatedBy))}='{user}'";
            var models = repo.GetParents(ids.Cast<object>().ToArray(), nameof(PKS_KG_PublicCatalog.ParentId), where)
                .OrderBy(e => e.LevelNumber).ThenBy(e => e.OrderNumber)
                .MapTo<KG_CatalogNode>()
                .ToList();
            return models;
        }
    }
}
