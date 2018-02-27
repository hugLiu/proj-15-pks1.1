using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.KCase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices
{
    public class KCaseInputService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_KCASE_INSTANCE> _kCaseInstanceRepository;
        private readonly IRepository<PKS_KCASE_PARAMETER_CATEGORY> _kCaseParamCategoryRepository;
        private readonly IRepository<PKS_KCASE_THEME_PARAMETER> _kCaseThemeParamRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE_PARAMETER> _kCaseInstanceParamRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE_CHART> _kCaseInstanceChartRepository;
        private readonly IRepository<PKS_KCASE_THEME> _kCaseThemeRepository;
        private readonly IRepository<PKS_KCASE_CATEGORY> _kCaseCategoryRepository;

        public KCaseInputService(IRepository<PKS_KCASE_INSTANCE> kCaseInstanceRepository,
                                 IRepository<PKS_KCASE_PARAMETER_CATEGORY> kCaseParamCategoryRepository,
                                 IRepository<PKS_KCASE_THEME_PARAMETER> kCaseThemeParamRepository,
                                 IRepository<PKS_KCASE_INSTANCE_PARAMETER> kCaseInstanceParamRepository,
                                 IRepository<PKS_KCASE_INSTANCE_CHART> kCaseInstanceChartRepository,
                                 IRepository<PKS_KCASE_THEME> kCaseThemeRepository,
                                 IRepository<PKS_KCASE_CATEGORY> kCaseCategoryRepository)
        {
            _kCaseInstanceRepository = kCaseInstanceRepository;
            _kCaseParamCategoryRepository = kCaseParamCategoryRepository;
            _kCaseThemeParamRepository = kCaseThemeParamRepository;
            _kCaseInstanceParamRepository = kCaseInstanceParamRepository;
            _kCaseInstanceChartRepository = kCaseInstanceChartRepository;
            _kCaseThemeRepository = kCaseThemeRepository;
            _kCaseCategoryRepository = kCaseCategoryRepository;
        }

        public InstanceModel GetInstance(int id)
        {
            var query = _kCaseInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.Id == id);
            if (query == null) return null;
            InstanceModel result = new InstanceModel
            {
                Id = query.Id,
                Name = query.NAME,
                BoDescription = query.BODESCRIPTION,
                Remark = query.REMARK,
                Author = query.AUTHOR,
                Auditor = query.AUDITOR,
                KCaseThemeId = query.KCASETHEMEID
            };
            return result;
        }

        

        public int UpdateInstance(InstanceModel model, string name)
        {
            var entity = _kCaseInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.Id == model.Id);
            if (entity == null) return -1;
            entity.NAME = model.Name;
            entity.BODESCRIPTION = model.BoDescription;
            entity.REMARK = model.Remark;
            entity.AUTHOR = model.Author;
            entity.AUDITOR = model.Auditor;
            entity.LASTUPDATEDBY = name;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _kCaseInstanceRepository.Update(entity);
            return entity.Id;
        }

        

        public int AddInstance(InstanceModel model, string name)
        {
            var entity = new PKS_KCASE_INSTANCE
            {
                NAME = model.Name,
                BODESCRIPTION = model.BoDescription,
                REMARK = model.Remark,
                AUTHOR = model.Author,
                AUDITOR = model.Auditor,
                KCASETHEMEID = model.KCaseThemeId,
                CREATEDBY = name,
                CREATEDDATE = DateTime.Now,
                LASTUPDATEDBY = name,
                LASTUPDATEDDATE = DateTime.Now
            };
            _kCaseInstanceRepository.Add(entity);
            return entity.Id;
        }

       

        public void DeleteInstance(int id)
        {
            _kCaseInstanceChartRepository.DeleteList(t => t.KCASEINSTANCEID == id);
            _kCaseInstanceParamRepository.DeleteList(t => t.KCASEINSTANCEID == id);
            _kCaseInstanceRepository.DeleteList(t => t.Id == id);
        }

        public List<InstanceModel> GetInstancesByThemeId(int themeId)
        {
            var result = new List<InstanceModel>();
            var query = _kCaseInstanceRepository.GetQuery()
                .Where(t => t.KCASETHEMEID == themeId)
                .Select(t => new InstanceModel
                {
                    Id = t.Id,
                    Name = t.NAME,
                    BoDescription = t.BODESCRIPTION,
                    Remark = t.REMARK,
                    Author = t.AUDITOR,
                    Auditor = t.AUDITOR,
                    KCaseThemeId = t.KCASETHEMEID
                });
            if (query != null) result.AddRange(query);
            return result;
        }

        public List<ParamTreeRow> GetParamTreeGrid(int themeId, int instanceId)
        {
            var result = new List<ParamTreeRow>();
            var categories = _kCaseParamCategoryRepository.GetQuery()
                .Where(t => t.KCASETHEMEID == themeId)
                .OrderBy(t => t.ORDERNUMBER)
                .Select(t => new ParamTreeRow
                {
                    Id = t.Id,
                    Pid = t.PARENTID,
                    Name = t.NAME,
                    IsParam = false
                });
            if (categories != null) result.AddRange(categories);
            var query = from param in _kCaseThemeParamRepository.GetQuery()
                        join category in _kCaseParamCategoryRepository.GetQuery() on param.KCASEPARAMETERCATEGORYID equals category.Id
                        
                        where category.KCASETHEMEID == themeId
                        select new ParamTreeRow
                        {
                            Id = -param.Id,          // Id取负数避免与CategoryId重复
                            Pid = param.KCASEPARAMETERCATEGORYID,
                            Name = param.NAME,
                            IsParam = true,
                            ParamId = param.Id,
                            ParamType = param.PARAMETERTYPE,
                            Options = param.OPTIONS,
                            Range = param.RANGE,
                            Unit = param.UNIT
                        };
            if (query != null)
            {
                var list = query.ToList();
                foreach (var item in list)
                {
                    var instanceParam = _kCaseInstanceParamRepository.GetQuery()
                        .FirstOrDefault(t => t.KCASETHEMEPARAMETERID == item.ParamId.Value && t.KCASEINSTANCEID == instanceId);
                    if (instanceParam != null)
                    {
                        item.ParamValue = instanceParam.PARAMETERVALUE;
                        item.SampleData = instanceParam.SAMPLEDATA;
                        item.Remark = instanceParam.REMARK;
                    }
                    // TODO: 获取图版/公式数量
                }
                result.AddRange(list);
            }
                      
            return result;
        }

        public void UpdateInstanceParam(int instanceId, List<ParamTreeRow> models, string name)
        {
            foreach(var model in models)
            {
                if (String.IsNullOrEmpty(model.ParamValue)) continue;
                var entity = _kCaseInstanceParamRepository.GetQuery()
                    .FirstOrDefault(t => t.KCASEINSTANCEID == instanceId && t.KCASETHEMEPARAMETERID == model.ParamId);
                if(entity == null)
                {
                    var newEntity = new PKS_KCASE_INSTANCE_PARAMETER
                    {
                        KCASETHEMEPARAMETERID = model.ParamId.Value,
                        PARAMETERVALUE = model.ParamValue,
                        SAMPLEDATA = model.SampleData,
                        REMARK = model.Remark,
                        KCASEINSTANCEID = instanceId,
                        CREATEDBY = name,
                        CREATEDDATE = DateTime.Now,
                        LASTUPDATEDBY = name,
                        LASTUPDATEDDATE = DateTime.Now
                    };
                    _kCaseInstanceParamRepository.Add(newEntity);
                }
                else
                {
                    entity.PARAMETERVALUE = model.ParamValue;
                    entity.SAMPLEDATA = model.SampleData;
                    entity.REMARK = model.Remark;
                    entity.LASTUPDATEDBY = name;
                    entity.LASTUPDATEDDATE = DateTime.Now;
                    _kCaseInstanceParamRepository.Update(entity);
                }
            }
        }

        public byte[] GetInstanceChart(int instanceId, int chartId)
        {
            var entity = _kCaseInstanceChartRepository.GetQuery()
                .FirstOrDefault(t => t.KCASEINSTANCEID == instanceId && t.KCASETHEMECHARTID == chartId);
            if (entity == null) return null;
            return entity.CHART;
        }

        public void DeleteInstanceChart(int instanceId, int chartId)
        {
            var entity = _kCaseInstanceChartRepository.GetQuery()
                .FirstOrDefault(t => t.KCASEINSTANCEID == instanceId && t.KCASETHEMECHARTID == chartId);
            if (entity == null) _kCaseInstanceChartRepository.Delete(entity);
        }

        public void UpdateInstanceChart(int instanceId, int chartId, byte[] bytes, string name)
        {
            
            var entity = _kCaseInstanceChartRepository.GetQuery()
                .FirstOrDefault(t => t.KCASEINSTANCEID == instanceId && t.KCASETHEMECHARTID == chartId);
            if(entity == null)
            {
                var newEntity = new PKS_KCASE_INSTANCE_CHART
                {
                    KCASETHEMECHARTID = chartId,
                    CHART = bytes,
                    KCASEINSTANCEID = instanceId,
                    CREATEDBY = name,
                    CREATEDDATE = DateTime.Now,
                    LASTUPDATEDBY = name,
                    LASTUPDATEDDATE = DateTime.Now
                };
                _kCaseInstanceChartRepository.Add(newEntity);
            }
            else
            {
                entity.CHART = bytes;
                _kCaseInstanceChartRepository.Update(entity);
            }
        }

        public InstanceIndexModel GetInstanceIndexModel(int instanceId)
        {
            var entity = _kCaseInstanceRepository.GetQuery().FirstOrDefault(t => t.Id == instanceId);
            if (entity == null) return null;
            InstanceIndexModel model = new InstanceIndexModel();
            model.Id = entity.Id;
            model.Name = entity.NAME;
            model.Bo = entity.BODESCRIPTION;
            model.Remark = entity.REMARK;
            model.Author = entity.AUTHOR;
            model.Auditor = entity.AUDITOR;
            model.CreateDate = entity.CREATEDDATE;
            var theme = _kCaseThemeRepository.GetQuery().FirstOrDefault(t => t.Id == entity.KCASETHEMEID);
            model.KCaseTheme = theme.NAME;
            model.Description = theme.DESCRIPTION;
            var category = _kCaseCategoryRepository.GetQuery().FirstOrDefault(t => t.Id == theme.KCASECATEGORYID);
            model.KCaseCategory = category.NAME;
            var chart = _kCaseInstanceChartRepository.GetQuery().FirstOrDefault(t => t.KCASEINSTANCEID == entity.Id && t.CHART != null);
            if (chart != null)
            {
                model.Chart = chart.CHART;
            }
            return model;
        }
    }
}
