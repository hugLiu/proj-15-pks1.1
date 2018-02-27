using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.KCase.Model;
using PKS.DbServices.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices
{
    public class KCaseThemeService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_KCASE_CATEGORY> _kCaseCategoryRepository;
        private readonly IRepository<PKS_KCASE_THEME> _kCaseThemeRepository;
        private readonly IRepository<PKS_KCASE_PARAMETER_CATEGORY> _kCaseParamCategoryRepository;
        private readonly IRepository<PKS_KCASE_THEME_PARAMETER> _kCaseThemeParamRepository;
        private readonly IRepository<PKS_KCASE_THEME_CHART> _kCaseThemeChartRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE> _kCaseInstanceRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE_CHART> _kCaseInstanceChartRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE_PARAMETER> _kCaseInstanceParamRepository;

        public KCaseThemeService(IRepository<PKS_KCASE_CATEGORY> kCaseCategoryRepository,
                                 IRepository<PKS_KCASE_THEME> kCaseThemeRepository,
                                 IRepository<PKS_KCASE_PARAMETER_CATEGORY> kCaseParamCategoryRepository,
                                 IRepository<PKS_KCASE_THEME_PARAMETER> kCaseThemeParamRepository,
                                 IRepository<PKS_KCASE_THEME_CHART> kCaseThemeChartRepository,
                                 IRepository<PKS_KCASE_INSTANCE> kCaseInstanceRepository,
                                 IRepository<PKS_KCASE_INSTANCE_CHART> kCaseInstanceChartRepository,
                                 IRepository<PKS_KCASE_INSTANCE_PARAMETER> kCaseInstanceParamRepository)
        {
            _kCaseCategoryRepository = kCaseCategoryRepository;
            _kCaseThemeRepository = kCaseThemeRepository;
            _kCaseParamCategoryRepository = kCaseParamCategoryRepository;
            _kCaseThemeParamRepository = kCaseThemeParamRepository;
            _kCaseThemeChartRepository = kCaseThemeChartRepository;
            _kCaseInstanceRepository = kCaseInstanceRepository;
            _kCaseInstanceChartRepository = kCaseInstanceChartRepository;
            _kCaseInstanceParamRepository = kCaseInstanceParamRepository;
        }

        #region 主题树维护

        public List<CaseTreeNode> GetCaseTree()
        {
            var result = new List<CaseTreeNode>();
            var categories = _kCaseCategoryRepository.GetQuery()
                .OrderBy(t => t.ORDERNUMBER)
                .Select(t => new CaseTreeNode
                {
                    Id = t.Id,
                    Pid = t.PARENTID,
                    Name = t.NAME,
                    IsCase = false
                });
            var themes = _kCaseThemeRepository.GetQuery()
                .Select(t => new CaseTreeNode
                {
                    Id = -t.Id,              // Id取负数避免与CategoryId重复
                    Pid = t.KCASECATEGORYID,
                    Name = t.NAME,
                    IsCase = true,
                    ThemeId = t.Id
                });
            if (categories != null) result.AddRange(categories);
            if (themes != null) result.AddRange(themes);
            return result;
        }

        public void UpdateThemeDesc(int themeId, string description, string user)
        {
            var entity = _kCaseThemeRepository.GetQuery()
                .FirstOrDefault(t => t.Id == themeId);
            if (entity == null) return;
            entity.DESCRIPTION = description;
            entity.LASTUPDATEDBY = user;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _kCaseThemeRepository.Update(entity);
        }

        public string GetThemeDesc(int themeId)
        {
            return _kCaseThemeRepository.GetQuery()
                .FirstOrDefault(t => t.Id == themeId)?.DESCRIPTION;
        }

        public void DeleteCaseNode(int id, bool isCase)
        {
            if(isCase)
            {
                /**
                 * 删除主题
                 * 删除主题图版 -> 删除实例图版
                 * 删除参数类型 -> 删除主题参数 -> 删除实例参数
                 * 删除实例
                 * */
                //var charts = _kCaseThemeChartRepository.GetQuery().Where(t => t.KCASETHEMEID == -id);
                //if(charts != null)
                //{
                //    foreach(var chart in charts)
                //    {
                //        _kCaseInstanceChartRepository.DeleteList(t => t.KCASETHEMECHARTID == chart.Id);
                //    }
                //    _kCaseThemeChartRepository.DeleteList(t => t.KCASETHEMEID == -id);
                //}

                //var categories = _kCaseParamCategoryRepository.GetQuery()
                //    .Where(t => t.KCASETHEMEID == -id);
                //if (categories != null)
                //{
                //    foreach (var category in categories)
                //    {
                //        var parameters = _kCaseThemeParamRepository.GetQuery()
                //            .Where(t => t.KCASEPARAMETERCATEGORYID == category.Id);
                //        if(parameters != null)
                //        {
                //            foreach(var parameter in parameters)
                //            {
                //                _kCaseInstanceParamRepository.DeleteList(t => t.KCASETHEMEPARAMETERID == parameter.Id);
                //            }
                //            _kCaseThemeParamRepository.DeleteList(t => t.KCASEPARAMETERCATEGORYID == category.Id);
                //        }
                //    }
                //    _kCaseParamCategoryRepository.DeleteList(t => t.KCASETHEMEID == -id);
                //}
                //_kCaseInstanceRepository.DeleteList(t => t.KCASETHEMEID == -id);
                _kCaseThemeRepository.DeleteList(t => t.Id == -id);
            } else
            {
                _kCaseCategoryRepository.DeleteList(t => t.Id == id);
            }
        }

        public int AddNewCaseNode(CaseTreeNode node, string user)
        {
            if(node.IsCase)
            {
                var entity = new PKS_KCASE_THEME
                {
                    NAME = node.Name,
                    KCASECATEGORYID = node.Pid.Value,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                _kCaseThemeRepository.Add(entity);
                return entity.Id;
            }
            else
            {
                var entity = new PKS_KCASE_CATEGORY
                {
                    NAME = node.Name,
                    PARENTID = node.Pid,
                    LEVELNUMBER = 1,
                    ORDERNUMBER = 1,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                if (node.Pid != null)
                {
                    entity.LEVELNUMBER = _kCaseCategoryRepository.GetQuery()
                        .FirstOrDefault(t => t.Id == node.Pid).LEVELNUMBER + 1;
                }
                var query = _kCaseCategoryRepository.GetQuery().Where(t => t.LEVELNUMBER == entity.LEVELNUMBER);
                if (query != null && query.Count() > 0)
                {
                    entity.ORDERNUMBER = query.Max(t => t.ORDERNUMBER) + 1;
                } 
                _kCaseCategoryRepository.Add(entity);
                return entity.Id;
            }
        }


        public void UpdateCaseNode(CaseTreeNode node, string user)
        {
            if (node.IsCase)
            {
                var entity = _kCaseThemeRepository.GetQuery()
                    .FirstOrDefault(t => t.Id == node.ThemeId);
                entity.NAME = node.Name;
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kCaseThemeRepository.Update(entity);
            } else
            {
                var entity = _kCaseCategoryRepository.GetQuery()
                    .FirstOrDefault(t => t.Id == node.Id);
                entity.NAME = node.Name;
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kCaseCategoryRepository.Update(entity);
            }
        }

        #endregion

        #region 维护参数树

        public List<ParamTreeNode> GetParamTreeByThemeId(int themeId)
        {
            var result = new List<ParamTreeNode>();
            var categories = _kCaseParamCategoryRepository.GetQuery()
                .Where(t => t.KCASETHEMEID == themeId)
                .OrderBy(t => t.ORDERNUMBER)
                .Select(t => new ParamTreeNode
                {
                    Id = t.Id,
                    Pid = t.PARENTID,
                    Name = t.NAME,
                    IsParam = false
                });
            
            var query = from param in _kCaseThemeParamRepository.GetQuery()
                        join category in _kCaseParamCategoryRepository.GetQuery()
                        on param.KCASEPARAMETERCATEGORYID equals category.Id
                        where category.KCASETHEMEID == themeId
                        select new ParamTreeNode
                        {
                            Id = -param.Id,          // Id取负数避免与CategoryId重复
                            Pid = param.KCASEPARAMETERCATEGORYID,                        
                            Name = param.NAME,
                            IsParam = true,
                            ParamId = param.Id
                        };
            if (categories != null) result.AddRange(categories);
            if (query != null) result.AddRange(query);
            return result;
        }

        public ParamModel GetParamInfoById(int id)
        {
            var result = new ParamModel();
            var entity = _kCaseThemeParamRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (entity == null) return result;
            result.Id = entity.Id;
            result.Name = entity.NAME;
            result.Description = entity.DESCRIPTION;
            result.ParamType = entity.PARAMETERTYPE;
            result.Options = entity.OPTIONS;
            result.Range = entity.RANGE;
            result.Unit = entity.UNIT;
            return result;
        }

        public void SaveParamInfo(ParamModel model, string user)
        {
            var entity = _kCaseThemeParamRepository.GetQuery()
                .FirstOrDefault(t => t.Id == model.Id);
            if (entity == null) return;
            entity.NAME = model.Name;
            entity.DESCRIPTION = model.Description;
            entity.PARAMETERTYPE = model.ParamType;
            entity.OPTIONS = model.Options;
            entity.RANGE = model.Range;
            entity.UNIT = model.Unit;
            entity.LASTUPDATEDBY = user;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _kCaseThemeParamRepository.Update(entity);
        }

        public int AddNewParamNode(int themeId, ParamTreeNode node, string user)
        {
            if (node.IsParam)
            {
                var entity = new PKS_KCASE_THEME_PARAMETER
                {
                    NAME = node.Name,
                    PARAMETERTYPE = 1,
                    KCASEPARAMETERCATEGORYID = node.Pid.Value,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                _kCaseThemeParamRepository.Add(entity);
                return entity.Id;
            }
            else
            {
                var entity = new PKS_KCASE_PARAMETER_CATEGORY
                {
                    NAME = node.Name,
                    LEVELNUMBER = 1,
                    ORDERNUMBER = 1,
                    PARENTID = null,
                    KCASETHEMEID = themeId,
                    CREATEDBY = user,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = user,
                    LASTUPDATEDDATE = DateTime.Now
                };
                var query = _kCaseParamCategoryRepository.GetQuery().Where(t => t.LEVELNUMBER == entity.LEVELNUMBER);
                if (query != null && query.Count() > 0)
                {
                    entity.ORDERNUMBER = query.Max(t => t.ORDERNUMBER) + 1;
                }
                _kCaseParamCategoryRepository.Add(entity);
                return entity.Id;
            }
        }

        public void UpdateParamNode(ParamTreeNode node, string user)
        {
            if (node.IsParam)
            {
                var entity = _kCaseThemeParamRepository.GetQuery()
                    .FirstOrDefault(t => t.Id == node.ParamId);
                entity.NAME = node.Name;
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kCaseThemeParamRepository.Update(entity);
            }
            else
            {
                var entity = _kCaseParamCategoryRepository.GetQuery()
                    .FirstOrDefault(t => t.Id == node.Id);
                entity.NAME = node.Name;
                entity.LASTUPDATEDBY = user;
                entity.LASTUPDATEDDATE = DateTime.Now;
                _kCaseParamCategoryRepository.Update(entity);
            }
        }

        public void DeleteParamNode(int id, bool isParam)
        {
            if (isParam)
            {
                _kCaseThemeParamRepository.DeleteList(t => t.Id == -id);
            }
            else
            {
                _kCaseParamCategoryRepository.DeleteList(t => t.Id == id);
            }
        }

        #endregion

        #region 图版/公式

        public List<ChartModel> GetChartsByThemeId(int themeId)
        {
            var result = new List<ChartModel>();
            var query = _kCaseThemeChartRepository.GetQuery()
                .Where(t => t.KCASETHEMEID == themeId)
                .Select(t => new ChartModel
                {
                    Id = t.Id,
                    Name = t.NAME,
                    ChartType = t.CHARTTYPE,
                    Parameters = t.PARAMETERS,
                    ThemeId = t.KCASETHEMEID
                });
            if (query != null) result.AddRange(query);
            return result;
        }

        public void UpdateChart(int themeId, List<ChartModel> models, string name)
        {
            foreach(var model in models)
            {
                if(model._state == EnumNodeState.added.ToString())
                {
                    var entity = new PKS_KCASE_THEME_CHART
                    {
                        NAME = model.Name,
                        CHARTTYPE = model.ChartType,
                        PARAMETERS = model.Parameters,
                        KCASETHEMEID = themeId,
                        CREATEDBY = name,
                        CREATEDDATE = DateTime.Now,
                        LASTUPDATEDBY = name,
                        LASTUPDATEDDATE = DateTime.Now
                    };
                    _kCaseThemeChartRepository.Add(entity);
                }
                if(model._state == EnumNodeState.modified.ToString())
                {
                    var entity = _kCaseThemeChartRepository.GetQuery()
                        .FirstOrDefault(t => t.Id == model.Id);
                    if (entity == null) continue;
                    entity.NAME = model.Name;
                    entity.CHARTTYPE = model.ChartType;
                    entity.PARAMETERS = model.Parameters;
                    entity.LASTUPDATEDBY = name;
                    entity.LASTUPDATEDDATE = DateTime.Now;
                    _kCaseThemeChartRepository.Update(entity);
                }
                if(model._state == EnumNodeState.removed.ToString())
                {
                    var entity = _kCaseThemeChartRepository.GetQuery()
                        .FirstOrDefault(t => t.Id == model.Id);
                    if (entity == null) continue;
                    _kCaseThemeChartRepository.Delete(entity);
                }
            }
        }

        #endregion
    }
}
