using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Models;
using PKS.Models;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PKS.DbServices
{
    /// <summary>主题服务</summary>
    public class KG_TopicService : AppService, IPerRequestAppService
    {
        /// <summary>构造函数</summary>
        public KG_TopicService() { }
        /// <summary>获得分类ID集合相关的主题集合</summary>
        public List<PKS_KG_Topic> GetPublicListByPage(int[] catalogIds, int pageNumber, int pageSize, out int totalCount)
        {
            return GetService<IRepository<PKS_KG_Topic>>()
                .FindListByPage(e => e.PublicCatalogId != null && catalogIds.Contains(e.PublicCatalogId.Value), s => s, o => o.Id, 1, pageSize, pageNumber, out totalCount)
                .ToList();
        }
        /// <summary>获得分类ID集合相关的主题集合</summary>
        public List<PKS_KG_Topic> GetPrivateListByPage(string user, PageInfo pageInfo, int[] catalogIds)
        {
            Expression<Func<PKS_KG_Topic, bool>> where = null;
            if (catalogIds.IsNullOrEmpty())
            {
                where = e => e.CreatedBy == user;
            }
            else
            {
                where = e => e.CreatedBy == user && catalogIds.Contains(e.PrivateCatalogId.Value);
            }
            int total;
            var result = GetService<IRepository<PKS_KG_Topic>>()
                .FindListByPage(where, o => o.Id, 1, pageInfo.Size, pageInfo.CurrentNumber, out total)
                .ToList();
            pageInfo.Total = total;
            return result;
        }
        /// <summary>保存</summary>
        public PKS_KG_Topic Save(KG_NewTopic model, string user)
        {
            var repo = GetService<IRepository<PKS_KG_Topic>>();
            PKS_KG_Topic entity;
            if (model.Id > 0)
            {
                entity = repo.Find(model.Id);
                model.MapTo(entity);
                entity.LastUpdatedBy = user;
                entity.SetUpdate();
            }
            else
            {
                entity = model.MapTo<PKS_KG_Topic>();
                entity.CreatedBy = user;
                entity.LastUpdatedBy = user;
                entity.SetInsert();
                repo.Add(entity, false);
            }
            repo.Submit();
            return entity;
        }
        /// <summary>删除</summary>
        public void Delete(int id)
        {
            var repo = GetService<IRepository<PKS_KG_Topic>>();
            var entity = repo.Find(id);
            if (entity != null) repo.Delete(entity);
        }

        /// <summary>查询</summary>
        public PKS_KG_Topic GetTopicById(int id)
        {
            var repo = GetService<IRepository<PKS_KG_Topic>>();
            return repo.Find(id);
        }
    }
}
