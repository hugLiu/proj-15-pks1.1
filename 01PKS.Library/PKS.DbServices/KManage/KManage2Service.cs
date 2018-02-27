using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DBModels;
using PKS.DbServices.KManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.DbServices.KManage;
using PKS.Utils;

namespace PKS.DbServices
{
    public class KManage2Service : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_KFRAGMENT> _kFragmentRepository;
        private readonly IRepository<PKS_KTEMPLATE> _kTemplateRepository;
        private readonly IRepository<PKS_KTEMPLATE_CATEGORY> _kTCategoryRepository;
        private readonly IRepository<PKS_KTEMPLATE_CATALOGUE> _kTCatalogueRepository;
        private readonly IRepository<PKS_KTEMPLATE_PARAMETER> _kTParameterRepository;
        private readonly IRepository<PKS_KTEMPLATE_CATEGORY_PARAMETER> _kCategoryParamsRepository;
        private readonly IRepository<PKS_KTEMPLATE_URL> _kTUrlRepository;
        private readonly IRepository<PKS_KTEMPLATE_INSTANCE> _kTInstanceRepository;
        private readonly IRepository<PKS_SUBSYSTEM> _subSystemRepository;
        private readonly IRepository<PKS_Code> _codeRepository;
        private readonly IRepository<PKS_CodeType> _codeTypeRepository;
        private readonly IRepository<PKS_KTEMPLATE_CATALOGUE> _kTemplateCatalogueRepository;

        public KManage2Service(IRepository<PKS_KFRAGMENT> kFragmentRepository,
            IRepository<PKS_KTEMPLATE> kTemplateRepository,
                               IRepository<PKS_KTEMPLATE_CATEGORY> kTCategoryRepository,
                               IRepository<PKS_KTEMPLATE_CATALOGUE> kTCatalogueRepository,
                               IRepository<PKS_KTEMPLATE_PARAMETER> kTParameterRepository,
                               IRepository<PKS_KTEMPLATE_CATEGORY_PARAMETER> kCategoryParamsRepository,
                               IRepository<PKS_KTEMPLATE_URL> kTUrlRepository,
                               IRepository<PKS_KTEMPLATE_INSTANCE> kTInstanceRepository,
                               IRepository<PKS_SUBSYSTEM> subSystemRepository,
                               IRepository<PKS_Code> codeRepository,
                               IRepository<PKS_CodeType> codeTypeRepository,
                               IRepository<PKS_KTEMPLATE_CATALOGUE> kTemplateCatalogueRepository)
        {
            _kFragmentRepository = kFragmentRepository;
            _kTemplateRepository = kTemplateRepository;
            _kTCategoryRepository = kTCategoryRepository;
            _kTCatalogueRepository = kTCatalogueRepository;
            _kTParameterRepository = kTParameterRepository;
            _kCategoryParamsRepository = kCategoryParamsRepository;
            _kTUrlRepository = kTUrlRepository;
            _kTInstanceRepository = kTInstanceRepository;
            _subSystemRepository = subSystemRepository;
            _codeRepository = codeRepository;
            _codeTypeRepository = codeTypeRepository;
            _kTemplateCatalogueRepository = kTemplateCatalogueRepository;
        }

        #region PageManage

        // 获取模板树
        public object GetTemplateTree()
        {
            var result = new List<KTemplateTreeItem>();
            var categories = _kTCategoryRepository.GetQuery()
                .Select(t => new KTemplateTreeItem
                {
                    id = t.Id,
                    text = t.NAME,
                    IsCategory = true
                }).OrderBy(t => t.id).ToList();

            foreach(var item in categories)
            {
                var templates = _kTemplateRepository.GetQuery()
                    .Where(t => t.KTEMPLATECATEGORYID == item.id)
                    .Select(t => new KTemplateTreeItem
                    {
                        id = t.Id,
                        text = t.ISDEFAULT ? "*" + t.NAME : t.NAME,
                        IsCategory = false
                    }).ToList();
                if (templates != null)
                {
                    item.children = new List<KTemplateTreeItem>();
                    item.children.AddRange(templates);
                }
                    
            }

            if (categories != null) result.AddRange(categories);
            return result;
        }

        public List<TreeNode> GetParameterTree()
        {
            return _kTParameterRepository.GetQuery()
                .Select(t => new TreeNode
                {
                    Id = t.Id.ToString(),
                    Text = t.NAME,
                    Pid = t.PARENTID.ToString()
                }).ToList();
        }

        public List<ComboItem> GetInstanceClass()
        {
            var results = new List<ComboItem>();
            var codeType = _codeTypeRepository.GetQuery()
                .FirstOrDefault(t => t.Code == "InstanceClass");
            if (codeType != null)
            {
                var codes = _codeRepository.GetQuery()
                    .Where(t => t.CodeTypeId == codeType.Id)
                    .Select(t => new ComboItem
                    {
                        id = t.Id,
                        text = t.Title
                    }).ToList();
                results.AddRange(codes);
            }
            return results;
        }

        public List<ComboItem> GetSubSystems()
        {
            return _subSystemRepository.GetQuery()
                .Select(t => new ComboItem
                {
                    id = t.Id,
                    text = t.Name
                }).ToList();
        }

        public List<ComboItem> GetUrls()
        {
            return  _kTUrlRepository.GetQuery()
                .Select(t => new ComboItem
                {
                    id = t.Id,
                    text = t.TITLE,
                    code = t.PAGEID
                }).ToList();
        }

        public List<ComboItem> GetTemplates(int id)
        {
            return _kTemplateRepository.GetQuery()
                .Where(t => t.KTEMPLATECATEGORYID == id)
                .Select(t => new ComboItem
                {
                    id = t.Id,
                    text = t.NAME
                }).ToList();
        }

        public PageManageModel GetPageManageData(int id)
        {
            var result = new PageManageModel();
            var item = _kTCategoryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                result.SubSystemId = item.SUBSYSTEMID;
                result.UrlId = item.KTEMPLATEURLID;
                result.InstanceClass = item.INSTANCECLASS;
                result.GroupName = item.GROUPNAME;
                result.RObject = item.ROBJECT;
            }

            var defaultTemplate = _kTemplateRepository.GetQuery()
                .FirstOrDefault(t => t.KTEMPLATECATEGORYID == id && t.ISDEFAULT == true);
            if (defaultTemplate != null) result.DefaultTempId = defaultTemplate.Id;

            var categoryParams = from c in _kCategoryParamsRepository.GetQuery()
                                 join p in _kTParameterRepository.GetQuery() on c.KTEMPLATEPARAMETERID equals p.Id
                                 where c.KTEMPLATECATEGORYID == id
                                 select new ComboItem
                                 {
                                     id = p.Id,
                                     text = p.NAME
                                 };
            if (categoryParams != null) result.Params = categoryParams.ToList();
            return result;
        }

        public void SavePageManage(int id, PageManageModel model)
        {
            var item = _kTCategoryRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.SUBSYSTEMID = model.SubSystemId;
                item.KTEMPLATEURLID = model.UrlId;
                item.INSTANCECLASS = model.InstanceClass;
                item.GROUPNAME = model.GroupName;
                item.ROBJECT = model.RObject;
            }

            var templates = _kTemplateRepository.GetQuery()
                .Where(t => t.KTEMPLATECATEGORYID == id);
            foreach(var template in templates)
            {
                template.ISDEFAULT = false;
            }
            var defaultTemplate = _kTemplateRepository.GetQuery()
                .FirstOrDefault(t => t.Id == model.DefaultTempId);
            if (defaultTemplate != null) defaultTemplate.ISDEFAULT = true;

            _kCategoryParamsRepository.DeleteList(t => t.KTEMPLATECATEGORYID == id);
            foreach (var para in model.Params)
            {
                _kCategoryParamsRepository.Add(new PKS_KTEMPLATE_CATEGORY_PARAMETER
                {
                    KTEMPLATECATEGORYID = id,
                    KTEMPLATEPARAMETERID = para.id
                });
            }

            _kTCategoryRepository.Submit();
            _kTemplateRepository.Submit();
            _kCategoryParamsRepository.Submit();
        }

        #endregion

        #region Template
        public string GetDefaultTemplate(int categoryId)
        {
            var query = _kTemplateRepository.GetQuery()
                .FirstOrDefault(t => t.KTEMPLATECATEGORYID == categoryId && t.ISDEFAULT == true);
            if (query != null) return query.NAME;
            return null;
        }

        public string GetInstanceTemplate(string instance, string instanceClass)
        {
            var query = _kTInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.INSTANCE == instance && t.INSTANCECLASS == instanceClass);
            if (query != null)
            {
                var result = _kTemplateRepository.GetQuery()
                    .First(t => t.Id == query.KTEMPLATEID);
                return result?.NAME;
            }
            return null;
        }

        public string GetCatalogueStr(string template)
        {
            var query = _kTemplateRepository.GetQuery()
                .FirstOrDefault(t => t.NAME == template);
            if (query == null) return "";
            var list = _kTemplateCatalogueRepository.GetQuery()
                    .Where(t => t.KTEMPLATEID == query.Id)
                    .OrderBy(t => t.CODE)
                    .Select(t => t.NAME);
            if (list == null) return "";
            var sb = new StringBuilder();
            foreach(var item in list)
            {
                sb.Append(" ").Append(item);
            }
            return sb.ToString().TrimEnd();
        }

        public TemplateInfo GetTemplateInfo(int tid)
        {
            var query = from t in _kTemplateRepository.GetQuery()
                        join c in _kTCategoryRepository.GetQuery() on t.KTEMPLATECATEGORYID equals c.Id
                        where t.Id == tid
                        select new TemplateInfo
                        {
                            Id = t.Id,
                            Name = t.NAME,
                            IsDefault = t.ISDEFAULT,
                            InstanceClass = c.INSTANCECLASS
                        };
            if (query != null)
                return query.First();
            return null;
        }

        public TemplateInfo GetTemplateInfoWithTemplateContent(int tid)
        {
            var query = from t in _kTemplateRepository.GetQuery()
                        join c in _kTCategoryRepository.GetQuery() on t.KTEMPLATECATEGORYID equals c.Id
                        where t.Id == tid
                        select new TemplateInfo
                        {
                            Id = t.Id,
                            Name = t.NAME,
                            IsDefault = t.ISDEFAULT,
                            Template=t.TEMPLATE,
                            InstanceClass = c.INSTANCECLASS
                        };
            if (query != null)
                return query.First();
            return null;
        }

        public List<KInstance> GetInstances(int tid)
        {
            return _kTInstanceRepository.GetQuery()
                .Where(t => t.KTEMPLATEID == tid)
                .Select(t => new KInstance
                {
                    Id = t.Id,
                    KTemplateId = t.KTEMPLATEID,
                    Instance = t.INSTANCE,
                    StaticUrl = t.STATICURL,
                    StaticDate = t.STATICDATE
                }).ToList();
        }

        public List<string> GetInstancesByClass(string bot)
        {
            return _kTInstanceRepository.GetQuery()
                .Where(t => t.INSTANCECLASS == bot)
                .Select(t => t.INSTANCE)
                .ToList();
        }

        public List<string> FilterInstances(List<string> bos)
        {
            var instances = _kTInstanceRepository.GetQuery().Select(t => t.INSTANCE).ToList();
            if (instances.Count > 0)
            {
                return bos.Where(t => !instances.Contains(t)).ToList();
            }
            return bos;
        }

        public bool HasTemplate(int tid, string instance, string instanceClass)
        {
            var template = _kTemplateRepository.GetQuery()
                .FirstOrDefault(t => t.Id == tid);
            var templateIds = _kTemplateRepository.GetQuery()
                .Where(t => t.KTEMPLATECATEGORYID == template.KTEMPLATECATEGORYID)
                .Select(t => t.Id);
            var instanceTemplateIds = _kTInstanceRepository.GetQuery()
                .Where(t => t.INSTANCE == instance && t.INSTANCECLASS == instanceClass)
                .Select(t => t.KTEMPLATEID);
            if (instanceTemplateIds == null || instanceTemplateIds.Count() < 1) return false;
            return templateIds.Intersect(instanceTemplateIds).Count() > 0;
            //return _kTInstanceRepository.GetQuery().Any(t => t.INSTANCE == instance && t.INSTANCECLASS == instanceClass && t.KTEMPLATEID == tid);
        }

        public void SaveInstances(int tid, string instanceclass, List<string> pushlist, string user)
        {
            var template = _kTemplateRepository.GetQuery()
                .FirstOrDefault(t => t.Id == tid);
            if (template == null) return;
            var templateIds = _kTemplateRepository.GetQuery()
                .Where(t => t.KTEMPLATECATEGORYID == template.KTEMPLATECATEGORYID)
                .Select(t => t.Id);

            foreach (var item in pushlist)
            {
                if (HasTemplate(tid, item, instanceclass))
                {
                    var instanceTemplateIds = _kTInstanceRepository.GetQuery()
                        .Where(t => t.INSTANCE == item && t.INSTANCECLASS == instanceclass)
                        .Select(t => t.KTEMPLATEID);
                    var list = templateIds.Intersect(instanceTemplateIds);
                    _kTInstanceRepository.DeleteList(t => list.Contains(t.KTEMPLATEID));


                }
                _kTInstanceRepository.Add(new PKS_KTEMPLATE_INSTANCE
                {
                    KTEMPLATEID = tid,
                    INSTANCE = item,
                    INSTANCECLASS = instanceclass,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                });
            }
        }

        public void DeleteInstance(int id)
        {
            var item = _kTInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (item != null) _kTInstanceRepository.Delete(item);
        }
        #endregion

        /// <summary>
        /// 根据Bo查找对应的所有知识片段
        /// </summary>
        /// <param name="boName"></param>
        /// <param name="boType"></param>
        /// <returns></returns>
        public TemplateInfo FindTemplateByBo(int urlId, string boName, string instanceClass)
        {
            var categoryEntity = _kTCategoryRepository.GetQuery()
                .FirstOrDefault(t => t.KTEMPLATEURLID == urlId);
            if (categoryEntity == null) return null;

            int categoryId = categoryEntity.Id;
            var list = _kTemplateRepository.GetQuery()
                .Where(t => t.KTEMPLATECATEGORYID == urlId)
                .Select(t => t.Id).ToList();
            if (list == null || list.Count < 1) return null;


            var query = from instance in _kTInstanceRepository.GetQuery()
                        join template in _kTemplateRepository.GetQuery()
                        on instance.KTEMPLATEID equals template.Id
                        where instance.INSTANCE == boName && instance.INSTANCECLASS == instanceClass && list.Contains(instance.KTEMPLATEID)
                select new TemplateInfo
                {
                    Id = template.Id,
                    IsDefault = template.ISDEFAULT,
                    Template = template.TEMPLATE
                };

            if (query.Count() == 0)
            {
                //若未找到模板，则查找默认模板
                var query2 = from template in _kTemplateRepository.GetQuery()
                    join category in _kTCategoryRepository.GetQuery()
                    on template.KTEMPLATECATEGORYID equals category.Id
                    where category.INSTANCECLASS == instanceClass && category.KTEMPLATEURLID == urlId && template.ISDEFAULT 
                    select new TemplateInfo
                    {
                        Id = template.Id,
                        IsDefault = template.ISDEFAULT,
                        Template = template.TEMPLATE
                    };
                return query2.ToList().FirstOrDefault();
            }

            var defaultTemplates = query.Where(item => item.IsDefault).ToList();
            if (defaultTemplates.Count == 0)
                return query.FirstOrDefault();
            return defaultTemplates.FirstOrDefault();
        }

        public List<FragmentModel> GetFragmentsByPlaceHolderIds(int templateId,string[] holderIds)
        {
            if (holderIds.Count() == 0)
                return null;
            var fragmentTypeRepository = Bootstrapper.Get<IRepository<PKS_KFRAGMENT_TYPE>>();
            var fragmentQuery =from fragment in _kFragmentRepository.GetQuery()
                               join fragmentType in fragmentTypeRepository.GetQuery()
                               on fragment.KFRAGMENTTYPEID equals  fragmentType.Id
                               where holderIds.Contains(fragment.PLACEHOLDERID)&&fragment.KTEMPLATEID==templateId
                               select ( new FragmentModel()
                               {
                                   Id = fragment.Id,
                                   TemplateCatalogueId = fragment.KTEMPLATECATALOGUEID,
                                   Title = fragment.TITLE,
                                   QueryParameter = fragment.QUERYPARAMETER,
                                   ComponentParameter = fragment.COMPONENTPARAMETER,
                                   Htmltext = fragment.HTMLTEXT,
                                   FragmentTypeId = fragment.KFRAGMENTTYPEID,
                                   PlaceholderId = fragment.PLACEHOLDERID,
                                   FragmentVueTag = fragmentType.VUETAG,
                                   FragmentHasTextTemplate = fragmentType.HASTEXTTEMPLATE,
                                   FragmentTypeCode = fragmentType.CODE,
                                   FragmentTypeName = fragmentType.NAME,
                                   TemplateId = fragment.KTEMPLATEID
                               });
            return fragmentQuery.ToList();
        }

        public List<TemplateParam> GetTemplateParamsByTemplateId(int templateId)
        {
            var paramQuery = from template in _kTemplateRepository.GetQuery()
                join templateCategoryParameter in _kCategoryParamsRepository.GetQuery()
                on template.KTEMPLATECATEGORYID equals templateCategoryParameter.KTEMPLATECATEGORYID
                join templateParam in _kTParameterRepository.GetQuery()
                on templateCategoryParameter.KTEMPLATEPARAMETERID equals templateParam.Id
                where template.Id == templateId
                select new TemplateParam()
                {
                    Id =templateParam.Id,
                    Code = templateParam.CODE,
                    DataType = templateParam.DATATYPE,
                    Name=templateParam.NAME,
                    ParentId=templateParam.PARENTID
                };
            return paramQuery.ToList();
        }

        /// <summary>
        /// 根据组件类型获取组件参数
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public List<FragmentTypeParam> GetComponentParamsByFragmentTypeId(List<int> fragmentTypeIds)
        {
            var fragmentTypeParamRepository = Bootstrapper.Get<IRepository<PKS_KFRAGMENT_TYPE_PARAMETER>>();
            var paramQuery =
                fragmentTypeParamRepository.GetQuery().Where(item => fragmentTypeIds.Contains(item.KFRAGMENTTYPEID))
                    .Select(item => new FragmentTypeParam
                    {
                        Id = item.Id,
                        Code = item.CODE,
                        DataType = item.DATATYPE,
                        Name = item.NAME,
                        FragmentTypeId = item.KFRAGMENTTYPEID,
                        Metadata = item.METADATA,
                        DefaultValue = item.DEFAULTVALUE
                    }).ToList();
            return paramQuery;
        }

        /// <summary>
        /// 根据模板获取所有目录
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public List<CatalogueInfo> GetCatalogueInfosByTemplateId(int templateId)
        {
            var paramQuery =
                _kTCatalogueRepository.GetQuery().Where(item => item.KTEMPLATEID == templateId)
                    .Select(item => new CatalogueInfo()
                    {
                        Id = item.Id,
                        Code = item.CODE,
                        Name = item.NAME,
                        LevelNumber = item.LEVELNUMBER,
                        OrderNumber = item.ORDERNUMBER,
                        ParentId = item.PARENTID,
                        TemplateId = item.KTEMPLATEID
                    }).ToList();
            return paramQuery.OrderBy(item=>item.Code).ToList();
        }

        #region InstanceManage
        #endregion
    }
}
