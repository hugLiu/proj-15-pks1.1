using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.KManage.Model;
using PKS.DbServices.XEditor.Model;
using PKS.DBModels;
using PKS.Utils.NPinyin;

namespace PKS.DbServices.XEditor
{
    public partial class XEditorService : AppService, IPerRequestAppService
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

        public XEditorService(IRepository<PKS_KFRAGMENT> kFragmentRepository,
            IRepository<PKS_KTEMPLATE> kTemplateRepository,
                               IRepository<PKS_KTEMPLATE_CATEGORY> kTCategoryRepository,
                               IRepository<PKS_KTEMPLATE_CATALOGUE> kTCatalogueRepository,
                               IRepository<PKS_KTEMPLATE_PARAMETER> kTParameterRepository,
                               IRepository<PKS_KTEMPLATE_CATEGORY_PARAMETER> kCategoryParamsRepository,
                               IRepository<PKS_KTEMPLATE_URL> kTUrlRepository,
                               IRepository<PKS_KTEMPLATE_INSTANCE> kTInstanceRepository,
                               IRepository<PKS_SUBSYSTEM> subSystemRepository,
                               IRepository<PKS_Code> codeRepository,
                               IRepository<PKS_CodeType> codeTypeRepository)
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

        }
     
        public List<TemplateTree> GetTemplateTree()
        {
            var result = new List<TemplateTree>();
            var categories = _kTCategoryRepository.GetQuery()
                .Select(t => new TemplateTree
                {
                    Id = t.Id,
                    SubSystemId = t.SUBSYSTEMID,
                    Code = t.CODE,
                    Name = t.NAME,
                    TemplateUrlId = t.KTEMPLATEURLID,
                    InstanceClass = t.INSTANCECLASS,
                    IsTemplate = false,
                    NodeId = "cat_"+t.Id
                }).ToList();

            result.AddRange(categories);

            foreach (var item in categories)
            {
                var templates = _kTemplateRepository.GetQuery()
                    .Where(t => t.KTEMPLATECATEGORYID == item.Id)
                    .Select(t => new TemplateTree
                    {
                        Id = t.Id,
                        SubSystemId = item.SubSystemId,
                        TemplateUrlId = item.TemplateUrlId,
                        InstanceClass = item.InstanceClass,
                        Code = t.CODE,
                        Name = t.NAME,
                        IsTemplate=true,
                        //Template = t.TEMPLATE,
                        IsDefault = t.ISDEFAULT,
                        TemplateCategoryId = t.KTEMPLATECATEGORYID,
                        NodeId = "tem_"+t.Id,
                        ParentNodeId = item.NodeId
                    }).ToList();
                result.AddRange(templates);
            }
            return result;
        }

        public List<FragmentModel> GetFragmentsByTemplateId(int templateId)
        {
            var fragmentTypeRepository = Bootstrapper.Get<IRepository<PKS_KFRAGMENT_TYPE>>();
            var fragmentQuery = from fragment in _kFragmentRepository.GetQuery()
                                join fragmentType in fragmentTypeRepository.GetQuery()
                                on fragment.KFRAGMENTTYPEID equals fragmentType.Id
                                where fragment.KTEMPLATEID == templateId
                                select (new FragmentModel()
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

        /// <summary>
        /// 根据模板获取模板参数
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
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
                                 Id = templateParam.Id,
                                 Code = templateParam.CODE,
                                 DataType = templateParam.DATATYPE,
                                 Name = templateParam.NAME,
                                 ParentId = templateParam.PARENTID
                             };
            return paramQuery.ToList();
        }

        public bool UpdateTemplateName(int templateId ,string templateName)
        {
            var templateInfo=_kTemplateRepository.Find(templateId);
            if (templateInfo != null)
            {
                templateInfo.NAME = templateName;
                templateInfo.CODE = Pinyin.GetPinyin(templateName);
                _kTemplateRepository.Update(templateInfo);
            }
            return true;
        }
        public bool AddTemplates(List<TemplateTree> templateTrees)
        {
            if(templateTrees!=null)
            {
                List<PKS_KTEMPLATE> templates = new List<PKS_KTEMPLATE>();
                foreach(var tree in templateTrees)
                {
                    PKS_KTEMPLATE template = new PKS_KTEMPLATE();
                    template.CODE = tree.Code;
                    template.ISDEFAULT =false ;
                    template.KTEMPLATECATEGORYID = tree.TemplateCategoryId;
                    template.NAME = tree.Name;
                    templates.Add(template);
                }
                _kTemplateRepository.AddRange(templates);
            }
            return true;
        }
        public TemplateTree AddTemplate(int pid, string name)
        {
            PKS_KTEMPLATE template = new PKS_KTEMPLATE();
            template.CODE = Pinyin.GetPinyin(name);
            //template.ISDEFAULT = templateInfo.IsDefault;
            template.KTEMPLATECATEGORYID = pid;
            template.NAME = name;
            template.CREATEDDATE = DateTime.Now;
            // template.TEMPLATE = templateInfo.Template;
            _kTemplateRepository.Add(template);
            _kTemplateRepository.Submit();
            var tempalteId = template.Id;
            var templateTrees =from tem in _kTemplateRepository.GetQuery()
                            join cat in _kTCategoryRepository.GetQuery() 
                            on tem.KTEMPLATECATEGORYID equals cat.Id
                            where cat.Id==pid&&tem.Id== tempalteId
                            select new TemplateTree
               {
                                Id = tem.Id,
                                SubSystemId = cat.SUBSYSTEMID,
                                TemplateUrlId = cat.KTEMPLATEURLID,
                                InstanceClass = cat.INSTANCECLASS,
                                Code = tem.CODE,
                                Name = tem.NAME,
                                Template = tem.TEMPLATE,
                                IsDefault = tem.ISDEFAULT,
                                TemplateCategoryId = tem.KTEMPLATECATEGORYID,
                                NodeId = "tem_" + tem.Id,
                                ParentNodeId = "cat_"+cat.Id
                            };
            return templateTrees.FirstOrDefault();
        }
        public TemplateTree GetTemplateById(int templateId)
        {
            var templateTrees = from tem in _kTemplateRepository.GetQuery()
                                join cat in _kTCategoryRepository.GetQuery()
                                on tem.KTEMPLATECATEGORYID equals cat.Id
                                where tem.Id== templateId
                                select new TemplateTree
                                {
                                    Id = tem.Id,
                                    SubSystemId = cat.SUBSYSTEMID,
                                    TemplateUrlId = cat.KTEMPLATEURLID,
                                    InstanceClass = cat.INSTANCECLASS,
                                    Code = tem.CODE,
                                    Name = tem.NAME,
                                    Template = tem.TEMPLATE,
                                    IsDefault = tem.ISDEFAULT,
                                    TemplateCategoryId = tem.KTEMPLATECATEGORYID,
                                    NodeId = "tem_" + tem.Id,
                                    ParentNodeId = "cat_" + cat.Id
                                };
            return templateTrees.FirstOrDefault();
        }

        public bool DeleteTemplate(int templateId)
        {
            _kTemplateRepository.BeginTrans();
            //模板实例
            var toRemoveInstanceRepository = _kTInstanceRepository.GetQuery()
               .Where(item => item.KTEMPLATEID == templateId).ToList();
            toRemoveInstanceRepository.ForEach(item =>
            {
                _kTInstanceRepository.Delete(item, false);
            });
            //目錄
            var toRemoveCatalogues = _kTCatalogueRepository.GetQuery()
               .Where(item => item.KTEMPLATEID == templateId).ToList();
            toRemoveCatalogues.ForEach(item =>
            {
                _kTCatalogueRepository.Delete(item, false);
            });
            //知识片段
            var toRemoveFragments =
          _kFragmentRepository.GetQuery()
              .Where(item => item.KTEMPLATEID == templateId)
              .ToList();
            if (toRemoveFragments.Any())
            {
                toRemoveFragments.ForEach(item =>
                {
                    _kFragmentRepository.Delete(item, false);
                });
            }
            _kTemplateRepository.DeleteByKey(new PKS_KTEMPLATE() {Id = templateId}, false);
            _kTemplateRepository.Submit();
            _kTemplateRepository.EndTrans();
            return true;
        }

     

        public bool AddTemplate(TemplateTree templateInfo)
        {
            if (templateInfo.IsTemplate)
            {
                PKS_KTEMPLATE template = new PKS_KTEMPLATE();
                template.CODE = templateInfo.Code;
                template.ISDEFAULT = templateInfo.IsDefault;
                template.KTEMPLATECATEGORYID = templateInfo.TemplateCategoryId;
                template.NAME = templateInfo.Name;
                template.TEMPLATE = templateInfo.Template;
                if (templateInfo.Id > 0)
                {
                    template.Id = templateInfo.Id;
                    template.LASTUPDATEDDATE = DateTime.Now;
                    _kTemplateRepository.Update(template);
                }
                else
                {
                    template.CREATEDDATE = DateTime.Now;
                    _kTemplateRepository.Add(template);
                }
            }
            else
            {
                PKS_KTEMPLATE_CATEGORY category = new PKS_KTEMPLATE_CATEGORY();
                category.CODE = templateInfo.Code;
                category.SUBSYSTEMID = templateInfo.SubSystemId;
                category.NAME = templateInfo.Name;
                category.KTEMPLATEURLID = templateInfo.TemplateUrlId;
                category.INSTANCECLASS = templateInfo.InstanceClass;
                if (templateInfo.Id > 0)
                {
                    category.Id = templateInfo.Id;
                    _kTCategoryRepository.Update(category);
                }
                else
                {
                    _kTCategoryRepository.Add(category);
                }
            }
            return true;
        }

        public bool SaveTemplate(TemplateTree templateTree ,List<FragmentModel> fragmentModels ,List<CatalogueInfo> catalogueInfos )
        {
            //模板
            int templateId = templateTree.Id;
            var kTemplate =
               _kTemplateRepository.GetQuery().FirstOrDefault(item => item.Id == templateId);
            if (kTemplate == null)
            {
                throw new Exception("模板不存在");
            }

            _kTemplateRepository.BeginTrans();

            kTemplate.TEMPLATE = templateTree.Template;
            kTemplate.LASTUPDATEDDATE = DateTime.Now;
            _kTemplateRepository.Update(kTemplate,false);
            //知识片段
            var db_fragments =
              _kFragmentRepository.GetQuery()
                .Where(item => item.KTEMPLATEID == templateId)
                .ToList();
            foreach (var item in db_fragments)
            {
                _kFragmentRepository.Delete(item, false);
            }
            List<PKS_KFRAGMENT> toAddFragments = new List<PKS_KFRAGMENT>();
            foreach (var fragmentModel in fragmentModels)
            {
                PKS_KFRAGMENT fragment = new PKS_KFRAGMENT();
                fragment.KTEMPLATEID = templateId;
                fragment.TITLE = fragmentModel.Title;
                fragment.QUERYPARAMETER = fragmentModel.QueryParameter;
                fragment.COMPONENTPARAMETER = fragmentModel.ComponentParameter;
                fragment.HTMLTEXT = fragmentModel.Htmltext;
                fragment.KFRAGMENTTYPEID = fragmentModel.FragmentTypeId;
                fragment.PLACEHOLDERID = fragmentModel.PlaceholderId;
                fragment.CREATEDDATE = DateTime.Now;
                toAddFragments.Add(fragment);
            }
            if(toAddFragments.Any())
                _kFragmentRepository.AddRange(toAddFragments,false);
            //目录
            var db_catalogues =
             _kTCatalogueRepository.GetQuery()
               .Where(item => item.KTEMPLATEID == templateId)
               .ToList();
            db_catalogues.ForEach(item =>
            {
                _kTCatalogueRepository.Delete(item, false);
            });
            List<PKS_KTEMPLATE_CATALOGUE> rootCatalogues = new List<PKS_KTEMPLATE_CATALOGUE>();
            var rootCatalogueInfos =
                catalogueInfos.Where(item => string.IsNullOrWhiteSpace(item.ParentNodeId) || item.ParentNodeId == "0");
            foreach (var catalogueInfo in rootCatalogueInfos)
            {
                PKS_KTEMPLATE_CATALOGUE catalogue = new PKS_KTEMPLATE_CATALOGUE();
                catalogue.Id = 0;
                catalogue.CODE = catalogueInfo.Code;
                catalogue.NAME = catalogueInfo.Name;
                catalogue.LEVELNUMBER = catalogueInfo.LevelNumber;
                catalogue.ORDERNUMBER = catalogueInfo.OrderNumber;
                catalogue.KTEMPLATEID = catalogueInfo.TemplateId;
                catalogue.PLACEHOLDERID = catalogueInfo.PlaceHolderId;
                catalogue.CREATEDDATE = DateTime.Now;
                catalogueInfo.Catalogue = catalogue;
                BuildChildCatalogue(catalogue, catalogueInfo, catalogueInfos);
                rootCatalogues.Add(catalogue);
            }
            if (rootCatalogues.Any())
                _kTCatalogueRepository.AddRange(rootCatalogues, false);
            //SaveCatalogues2(templateId, catalogueInfos);
            _kTemplateRepository.Submit();
            _kTemplateRepository.EndTrans();

            //更新知识片段的CatalogueId
            foreach (var fragment in toAddFragments)
            {
                var placeHolderId = fragment.PLACEHOLDERID;
                var catalogueInfo=catalogueInfos.Where(item => item.Catalogue != null && item.PlaceHolderId == placeHolderId).FirstOrDefault();
                if (catalogueInfo != null)
                {
                    fragment.KTEMPLATECATALOGUEID = catalogueInfo.Catalogue.Id;
                    fragment.MPLACEHOLDERID = placeHolderId;
                    _kFragmentRepository.Update(fragment);
                }
                else
                {
                    var fragmentModel= fragmentModels.FirstOrDefault(item => item.PlaceholderId == placeHolderId);
                    if(fragmentModel!=null)
                    {
                        var catalogNodeId = fragmentModel.CatalogueNodeId;
                        if(!string.IsNullOrWhiteSpace(catalogNodeId))
                        {
                            var cat = catalogueInfos.FirstOrDefault(item => string.Equals(item.PlaceHolderId, catalogNodeId, StringComparison.OrdinalIgnoreCase));
                            fragment.KTEMPLATECATALOGUEID = cat.Catalogue.Id;
                            fragment.MPLACEHOLDERID = catalogNodeId;
                            _kFragmentRepository.Update(fragment);
                        }
                    }
                }                
            }
            return true;
        }

        public bool SaveFragments(int templateId, List<FragmentModel> fragmentModels)
        {
            //知识片段
            var db_fragmentIds =
                _kFragmentRepository.GetQuery()
                    .Where(item => item.KTEMPLATEID == templateId)
                    .Select(item => item.Id)
                    .ToList();
            var toRemoveFragmentIds =
                fragmentModels.Where(item => item.Id > 0 && !db_fragmentIds.Contains(item.Id))
                    .Select(item => item.Id)
                    .ToList();

            if (toRemoveFragmentIds.Any())
            {
                var toRemoveFragments=_kFragmentRepository.GetQuery()
                    .Where(item => item.KTEMPLATEID == templateId && toRemoveFragmentIds.Contains(item.Id)).ToList();
                toRemoveFragments.ForEach(item =>
                {
                    _kFragmentRepository.Delete(item,false);
                });
            }
            foreach (var fragmentModel in fragmentModels)
            {
                PKS_KFRAGMENT fragment = new PKS_KFRAGMENT();
                fragment.KTEMPLATEID = templateId;
               // fragment.KTEMPLATECATALOGUEID = fragmentModel.TemplateCatalogueId;
                fragment.TITLE = fragmentModel.Title;
                fragment.QUERYPARAMETER = fragmentModel.QueryParameter;
                fragment.COMPONENTPARAMETER = fragmentModel.ComponentParameter;
                fragment.HTMLTEXT = fragmentModel.Htmltext;
                fragment.KFRAGMENTTYPEID = fragmentModel.FragmentTypeId;
                fragment.PLACEHOLDERID = fragmentModel.PlaceholderId;
                if (fragmentModel.Id > 0)
                {
                    fragment.Id = fragmentModel.Id;
                    _kFragmentRepository.Update(fragment,false);
                }
                else
                {
                    _kFragmentRepository.Add(fragment,false);
                }
            }
            return true;
        }

        public bool SaveCatalogues2(int templateId, List<CatalogueInfo> catalogueInfos)
        {
            //刪除重复创建

            var toRemoveCatalogues = _kTCatalogueRepository.GetQuery()
                .Where(item => item.KTEMPLATEID == templateId ).ToList();
            toRemoveCatalogues.ForEach(item =>
            {
                _kTCatalogueRepository.Delete(item,false);
            });

            List<PKS_KTEMPLATE_CATALOGUE> rootCatalogues = new List<PKS_KTEMPLATE_CATALOGUE>();
            var rootCatalogueInfos =
                catalogueInfos.Where(item => string.IsNullOrWhiteSpace(item.ParentNodeId) || item.ParentNodeId == "0");
            foreach (var catalogueInfo in rootCatalogueInfos)
            {
                PKS_KTEMPLATE_CATALOGUE catalogue = new PKS_KTEMPLATE_CATALOGUE();
                catalogue.Id = 0;
                catalogue.CODE = catalogueInfo.Code;
                catalogue.NAME = catalogueInfo.Name;
                catalogue.LEVELNUMBER = catalogueInfo.LevelNumber;
                catalogue.ORDERNUMBER = catalogueInfo.OrderNumber;
                catalogue.KTEMPLATEID = catalogueInfo.TemplateId;
                BuildChildCatalogue(catalogue, catalogueInfo, catalogueInfos);
                rootCatalogues.Add(catalogue);
            }
            if(rootCatalogues.Any())
                _kTCatalogueRepository.AddRange(rootCatalogues,false);
            return true;
        }

        public void BuildChildCatalogue(PKS_KTEMPLATE_CATALOGUE parentCatalogue, CatalogueInfo parentCatalogueInfo, List<CatalogueInfo> catalogueInfos)
        {
            var childrenCatalogueInfos = catalogueInfos.Where(item => string.Equals(item.ParentNodeId,parentCatalogueInfo.NodeId,StringComparison.OrdinalIgnoreCase));
            if (childrenCatalogueInfos.Any())
            {
                foreach (var childrenCatalogueInfo in childrenCatalogueInfos)
                {
                    PKS_KTEMPLATE_CATALOGUE catalogue = new PKS_KTEMPLATE_CATALOGUE();
                    catalogue.Id = 0;
                    catalogue.CODE = childrenCatalogueInfo.Code;
                    catalogue.NAME = childrenCatalogueInfo.Name;
                    catalogue.LEVELNUMBER = childrenCatalogueInfo.LevelNumber;
                    catalogue.ORDERNUMBER = childrenCatalogueInfo.OrderNumber;
                    catalogue.KTEMPLATEID = childrenCatalogueInfo.TemplateId;
                    catalogue.PLACEHOLDERID = childrenCatalogueInfo.PlaceHolderId;
                    childrenCatalogueInfo.Catalogue = catalogue;
                    parentCatalogue.Children.Add(catalogue);

                    BuildChildCatalogue(catalogue, childrenCatalogueInfo, catalogueInfos);
                }
            }
        }


        public bool SaveCatalogues(int templateId, List<CatalogueInfo> catalogueInfos)
        {
            var db_catalogueIds = _kTCatalogueRepository.GetQuery().Where(item => item.KTEMPLATEID == templateId).Select(item => item.Id).ToList();
            var toRemoveCatalogueIds = catalogueInfos.Where(item => item.Id > 0 && !db_catalogueIds.Contains(item.Id)).Select(item => item.Id).ToList();
            if (toRemoveCatalogueIds.Any())
            {
                List<int> toRemoveIds = new List<int>(toRemoveCatalogueIds);
                var toRemoveCatalogueInfos =
                    _kTCatalogueRepository.GetQuery()
                        .Where(item => item.KTEMPLATEID == templateId && toRemoveCatalogueIds.Contains(item.Id))
                        .Select(item => new { item.Id, item.CODE }).ToList();
                foreach (var removeCatalogue in toRemoveCatalogueInfos)
                {
                    var ids = _kTCatalogueRepository.GetQuery().Where(item => item.KTEMPLATEID == templateId
                                                                              && item.CODE.IndexOf(removeCatalogue.CODE + ".") == 0)
                        .Select(item => item.Id)
                        .ToList();
                    if (ids.Any())
                        toRemoveIds.AddRange(ids);
                }
                toRemoveIds = toRemoveIds.Distinct().ToList();

                if (toRemoveIds.Any())
                {
                    var toRemoveFragments = _kTCatalogueRepository.GetQuery()
                        .Where(item => item.KTEMPLATEID == templateId && toRemoveIds.Contains(item.Id)).ToList();
                    toRemoveFragments.ForEach(item =>
                    {
                        _kTCatalogueRepository.Delete(item);
                    });
                }
            }


            var topCatalogues =
                catalogueInfos.Where(item => string.IsNullOrWhiteSpace(item.ParentNodeId) || item.ParentNodeId == "0")
                    .ToList();
            topCatalogues.ForEach(catalogueInfo =>
            {
                PKS_KTEMPLATE_CATALOGUE catalogue = new PKS_KTEMPLATE_CATALOGUE();
                catalogue.CODE = catalogueInfo.Code;
                catalogue.NAME = catalogueInfo.Name;
                catalogue.LEVELNUMBER = catalogueInfo.LevelNumber;
                catalogue.ORDERNUMBER = catalogueInfo.OrderNumber;
                catalogue.PARENTID = catalogueInfo.ParentId;
                catalogue.KTEMPLATEID = catalogueInfo.TemplateId;
                if (catalogueInfo.Id > 0)
                {
                    catalogue.Id = catalogueInfo.Id;
                    _kTCatalogueRepository.Update(catalogue);
                }
                else
                {
                    _kTCatalogueRepository.Add(catalogue);
                    catalogueInfo.Id = catalogue.Id;
                }

                SaveChildCatalogues(catalogueInfo, catalogueInfos);
            });
           
            return true;
        }

        private void SaveChildCatalogues(CatalogueInfo parentCatalogueInfo, List<CatalogueInfo> allCatalogueInfos)
        {
            var childCatalogueInfos =
                allCatalogueInfos.Where(item => item.ParentNodeId == parentCatalogueInfo.NodeId).ToList();
            var parentId = parentCatalogueInfo.Id;
            foreach (var catalogueInfo in childCatalogueInfos)
            {
                PKS_KTEMPLATE_CATALOGUE catalogue = new PKS_KTEMPLATE_CATALOGUE();
                catalogue.CODE = catalogueInfo.Code;
                catalogue.NAME = catalogueInfo.Name;
                catalogue.LEVELNUMBER = catalogueInfo.LevelNumber;
                catalogue.ORDERNUMBER = catalogueInfo.OrderNumber;
                catalogue.PARENTID = parentId;
                catalogue.KTEMPLATEID = catalogueInfo.TemplateId;
                if (catalogueInfo.Id > 0)
                {
                    catalogue.Id = catalogueInfo.Id;
                    _kTCatalogueRepository.Update(catalogue);
                }
                else
                {
                    _kTCatalogueRepository.Add(catalogue);
                    catalogueInfo.Id = catalogue.Id;
                }
                SaveChildCatalogues(catalogueInfo, allCatalogueInfos);
            }
        }
    }
}
