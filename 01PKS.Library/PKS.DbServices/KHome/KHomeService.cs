using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.KHome.Model;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KHome
{
    public class KHomeService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_KHOME_MODULE_CATEGORY> _kHomeModuleCategoryRepository;
        private readonly IRepository<PKS_KHOME_MODULE> _kHomeModuleRepository;
        private readonly IRepository<PKS_KHOME_MODULE_QUERY> _kHomeModuleQueryRepository;
        private readonly IRepository<PKS_KHOME_POST_MODULE> _kHomePostModuleRepository;
        private readonly IRepository<PKS_KHOME_POST_MODULE_FILTER> _kHomePostModuleFilterRepository;
        private readonly IRepository<WEBPAGES_ROLES> _webpagesRolesRepository;

        public KHomeService(IRepository<PKS_KHOME_MODULE_CATEGORY> kHomeModuleCategoryRepository,
                            IRepository<PKS_KHOME_MODULE> kHomeModuleRepository,
                            IRepository<PKS_KHOME_MODULE_QUERY> kHomeModuleQueryRepository,
                            IRepository<PKS_KHOME_POST_MODULE> kHomePostModuleRepository,
                            IRepository<PKS_KHOME_POST_MODULE_FILTER> kHomePostModuleFilterRepository,
                            IRepository<WEBPAGES_ROLES> webpagesRolesRepository)
        {
            _kHomeModuleCategoryRepository = kHomeModuleCategoryRepository;
            _kHomeModuleRepository = kHomeModuleRepository;
            _kHomeModuleQueryRepository = kHomeModuleQueryRepository;
            _kHomePostModuleRepository = kHomePostModuleRepository;
            _kHomePostModuleFilterRepository = kHomePostModuleFilterRepository;
            _webpagesRolesRepository = webpagesRolesRepository;
            
        }

        #region 首页模块树维护

        public List<ModuleTreeNode> GetModuleTree()
        {
            var result = new List<ModuleTreeNode>();
            var categories = _kHomeModuleCategoryRepository.GetQuery()
                .Select(t => new ModuleTreeNode
                {
                    Id = t.Id,   
                    Name = t.NAME,
                    Pid = null,
                    IsModule = false,
                    ComponentType = null
                });
            if (categories == null) return result;
            result.AddRange(categories);
            var modules = _kHomeModuleRepository.GetQuery()
                .Select(t => new ModuleTreeNode
                {
                    Id = -t.Id,// 模块ID取负
                    Name = t.NAME,
                    Pid = t.KHOMEMODULECATEGORYID,
                    IsModule = true,
                    ModuleId = t.Id,
                    Description = t.DESCRIPTION,
                    ComponentType = t.COMPONENTTYPE
                });
            if (modules != null) result.AddRange(modules);
            return result;
        }

        public int AddModuleCategory(ModuleTreeNode node, string user)
        {
            var entity = new PKS_KHOME_MODULE_CATEGORY
            {
                NAME = node.Name,
                CREATEDBY = user,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = user,
                LASTUPDATEDDATE = DateTime.Now
            };
            _kHomeModuleCategoryRepository.Add(entity);
            return entity.Id;
        }

        public void UpdateModuleCategory(ModuleTreeNode node, string user)
        {
            var entity = _kHomeModuleCategoryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == node.Id);
            if (entity == null) return;
            entity.NAME = node.Name;
            entity.LASTUPDATEDBY = user;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _kHomeModuleCategoryRepository.Update(entity);
        }

        public void DeleteModuleCategory(int id)
        {
            var entity = _kHomeModuleCategoryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (entity == null) return;
            _kHomeModuleCategoryRepository.Delete(entity);
        }

        public int AddModule(ModuleTreeNode node, string user)
        {
            var entity = new PKS_KHOME_MODULE
            {
                NAME = node.Name,
                DESCRIPTION = node.Description,
                COMPONENTTYPE = node.ComponentType.Value,
                KHOMEMODULECATEGORYID = node.Pid.Value,
                CREATEDBY = user,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = user,
                LASTUPDATEDDATE = DateTime.Now
            };
            _kHomeModuleRepository.Add(entity);
            return entity.Id;
        }

        public void UpdateModule(ModuleTreeNode node, string user)
        {
            var entity = _kHomeModuleRepository.GetQuery()
                .FirstOrDefault(t => t.Id == node.ModuleId);
            if (entity == null) return;
            entity.NAME = node.Name;
            entity.DESCRIPTION = node.Description;
            entity.COMPONENTTYPE = node.ComponentType.Value;
            entity.LASTUPDATEDBY = user;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _kHomeModuleRepository.Update(entity);
        }

        

        public void DeleteModule(int id)
        {
            var entity = _kHomeModuleRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (entity == null) return;
            _kHomeModuleRepository.Delete(entity);
        }

        #endregion

        #region 首页模块查询条件

        public List<ModuleQueryInfo> GetModuleQueries(int moduleId)
        {
            var result = new List<ModuleQueryInfo>();
            var query = _kHomeModuleQueryRepository.GetQuery()
                .Where(t => t.KHOMEMODULEID == moduleId)
                .Select(t => new ModuleQueryInfo
                {
                    Id = t.Id,
                    Name = t.NAME,
                    QueryParameter = t.QUERYPARAMETER,
                    ModuleId = t.KHOMEMODULEID
                });
            if (query != null) result.AddRange(query);
            return result;
                
        }

        public int AddQuery(ModuleQueryInfo model, string name)
        {
            var entity = new PKS_KHOME_MODULE_QUERY
            {
                NAME = model.Name,
                QUERYPARAMETER = model.QueryParameter,
                KHOMEMODULEID = model.ModuleId,
                CREATEDBY = name,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = name,
                LASTUPDATEDDATE = DateTime.Now
            };
            _kHomeModuleQueryRepository.Add(entity);
            return entity.Id;
        }

        public void UpdateQuery(ModuleQueryInfo model, string name)
        {
            var entity = _kHomeModuleQueryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == model.Id);
            if (entity == null) return;
            entity.NAME = model.Name;
            entity.LASTUPDATEDBY = name;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _kHomeModuleQueryRepository.Update(entity);
        }

        public void DeleteQuery(int queryId)
        {
            var entity = _kHomeModuleQueryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == queryId);
            if (entity == null) return;
            _kHomeModuleQueryRepository.Delete(entity);
        }

        public List<QueryParamItem> GetModuleQueryParams(int queryId)
        {
            var result = new List<QueryParamItem>();
            var entity = _kHomeModuleQueryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == queryId);
            if (entity == null) return result;
            if (String.IsNullOrEmpty(entity.QUERYPARAMETER)) return result;
            var query = entity.QUERYPARAMETER.JsonTo<List<QueryParamItem>>();
            if (query != null) result.AddRange(query);
            return result;
        }

        public void UpdateQueryParams(int queryId, List<QueryParamItem> model, string name)
        {
            var entity = _kHomeModuleQueryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == queryId);
            if (entity == null) return;
            entity.QUERYPARAMETER = model.ToJson();
            _kHomeModuleQueryRepository.Update(entity);
        }
        #endregion

        #region 岗位模块

        public List<PostItem> GetPostList()
        {
            var result = new List<PostItem>();
            var query = _webpagesRolesRepository.GetQuery()
                .Select(t => new PostItem
                {
                    id = t.ROLEID,
                    text = t.ROLENAME
                });
            if (query != null) result.AddRange(query);
            return result;
        }

        public List<PostModule> GetPostModuleList(int postid)
        {
            var result = new List<PostModule>();
            var query = _kHomePostModuleRepository.GetQuery()
                .Where(t => t.ROLEID == postid)
                .OrderBy(t => t.ORDER)
                .Select(t => new PostModule
                {
                    Id = t.Id,
                    Name = t.NAME,
                    Order = t.ORDER,
                    ModuleId = t.KHOMEMODULEID,
                    RoleId = t.ROLEID
                });
            if (query != null) result.AddRange(query);
            return result;
        }

        public int AddPostModule(PostModule model, string user)
        {
            int order = 1;
            //并发问题
            var query = _kHomePostModuleRepository.GetQuery()
                .Where(t => t.ROLEID == model.RoleId)
                .Select(t => t.ORDER)
                .ToList();
            if (query != null && query.Count > 0)
            {
                order = query.Max();
                order += 1;
            }

            var entity = new PKS_KHOME_POST_MODULE
            {
                NAME = model.Name,
                ORDER = order,
                ROLEID = model.RoleId,
                KHOMEMODULEID = model.ModuleId,
                CREATEDBY = user,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = user,
                LASTUPDATEDDATE = DateTime.Now
            };
            _kHomePostModuleRepository.Add(entity);
            return entity.Id;
        }

        public void RemovePostModule(int id)
        {
            var entity = _kHomePostModuleRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (entity == null) return;
            _kHomePostModuleRepository.Delete(entity);
        }

        public void MovePostModule(int postModuleId1, int postModuleId2)
        {
            var entity1 = _kHomePostModuleRepository.GetQuery()
                .FirstOrDefault(t => t.Id == postModuleId1);
            if (entity1 == null) throw new ArgumentNullException();
            var entity2 = _kHomePostModuleRepository.GetQuery()
                .FirstOrDefault(t => t.Id == postModuleId2);
            if (entity2 == null) throw new ArgumentNullException();
            int tempOrder = entity1.ORDER;
            entity1.ORDER = entity2.ORDER;
            _kHomePostModuleRepository.Update(entity1);
            entity2.ORDER = tempOrder;
            _kHomePostModuleRepository.Update(entity2);
        }

        #endregion

        #region 岗位参数

        public List<QueryParamItem> GetFilterParams(int queryId, int postModuleId)
        {
            var result = new List<QueryParamItem>();
            var query = _kHomePostModuleFilterRepository.GetQuery()
                .FirstOrDefault(t => t.KHOMEMODULEQUERYID == queryId && t.KHOMEPOSTMODULEID == postModuleId);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.FILTERPARAMETER))
                {
                    result.AddRange(query.FILTERPARAMETER.JsonTo<List<QueryParamItem>>());
                }
            }
            return result;
        }

        public void UpdateFilterParams(int queryId, int postModuleId, List<QueryParamItem> model, string user)
        {
            var entity = _kHomePostModuleFilterRepository.GetQuery()
                .FirstOrDefault(t => t.KHOMEMODULEQUERYID == queryId && t.KHOMEPOSTMODULEID == postModuleId);
            if (entity == null) {
                var newEntity = new PKS_KHOME_POST_MODULE_FILTER
                {
                    FILTERPARAMETER = model.ToJson(),
                    KHOMEMODULEQUERYID = queryId,
                    KHOMEPOSTMODULEID = postModuleId,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                _kHomePostModuleFilterRepository.Add(newEntity);
            }
            else
            {
                entity.FILTERPARAMETER = model.ToJson();
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kHomePostModuleFilterRepository.Update(entity);
            }        
        }

        public List<QueryParamItem> GetSortParams(int queryId, int postModuleId)
        {
            var result = new List<QueryParamItem>();
            var query = _kHomePostModuleFilterRepository.GetQuery()
                .FirstOrDefault(t => t.KHOMEMODULEQUERYID == queryId && t.KHOMEPOSTMODULEID == postModuleId);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.SORTPARAMETER))
                {
                    result.AddRange(query.SORTPARAMETER.JsonTo<List<QueryParamItem>>());
                }
            }
            return result;
        }

        public void UpdateSortParams(int queryId, int postModuleId, List<QueryParamItem> model, string user)
        {
            var entity = _kHomePostModuleFilterRepository.GetQuery()
                .FirstOrDefault(t => t.KHOMEMODULEQUERYID == queryId && t.KHOMEPOSTMODULEID == postModuleId);
            if (entity == null)
            {
                var newEntity = new PKS_KHOME_POST_MODULE_FILTER
                {
                    SORTPARAMETER = model.ToJson(),
                    KHOMEMODULEQUERYID = queryId,
                    KHOMEPOSTMODULEID = postModuleId,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                _kHomePostModuleFilterRepository.Add(newEntity);
            }
            else
            {
                entity.SORTPARAMETER = model.ToJson();
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kHomePostModuleFilterRepository.Update(entity);
            }
        }

        public int? GetReturnCount(int queryId, int postModuleId)
        {
            int? result = null;
            var query = _kHomePostModuleFilterRepository.GetQuery()
                .FirstOrDefault(t => t.KHOMEMODULEQUERYID == queryId && t.KHOMEPOSTMODULEID == postModuleId);
            if (query != null)
            {
                result = query.RETURNCOUNT;
            }
            return result;
        }

        public void UpdateReturnCount(int queryId, int postModuleId, int count, string user)
        {
            var entity = _kHomePostModuleFilterRepository.GetQuery()
                .FirstOrDefault(t => t.KHOMEMODULEQUERYID == queryId && t.KHOMEPOSTMODULEID == postModuleId);
            if (entity == null)
            {
                var newEntity = new PKS_KHOME_POST_MODULE_FILTER
                {
                    RETURNCOUNT = count,
                    KHOMEMODULEQUERYID = queryId,
                    KHOMEPOSTMODULEID = postModuleId,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                _kHomePostModuleFilterRepository.Add(newEntity);
            }
            else
            {
                entity.RETURNCOUNT = count;
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kHomePostModuleFilterRepository.Update(entity);
            }
        }
        #endregion
    }
}
