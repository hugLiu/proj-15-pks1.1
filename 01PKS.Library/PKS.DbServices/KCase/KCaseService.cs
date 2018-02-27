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
    public class KCaseService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_KCASE_CATEGORY> _kCaseCategoryRepository;
        private readonly IRepository<PKS_KCASE_THEME> _kCaseThemeRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE_CHART> _kCaseInstanceChartRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE> _kCaseInstanceRepository;
        private readonly IRepository<PKS_KCASE_PARAMETER_CATEGORY> _kCaseParamCategoryRepository;
        private readonly IRepository<PKS_KCASE_THEME_PARAMETER> _kCaseThemeParamRepository;
        private readonly IRepository<PKS_KCASE_INSTANCE_PARAMETER> _kCaseInstanceParamRepository;
        private readonly IRepository<PKS_KCASE_THEME_CHART> _kCaseThemeChartRepository;

        public KCaseService(IRepository<PKS_KCASE_CATEGORY> kCaseCategoryRepository,
                            IRepository<PKS_KCASE_THEME> kCaseThemeRepository,
                            IRepository<PKS_KCASE_INSTANCE_CHART> kCaseInstanceChartRepository,
                            IRepository<PKS_KCASE_INSTANCE> kCaseInstanceRepository,
                            IRepository<PKS_KCASE_PARAMETER_CATEGORY> kCaseParamCategoryRepository,
                            IRepository<PKS_KCASE_THEME_PARAMETER> kCaseThemeParamRepository,
                            IRepository<PKS_KCASE_INSTANCE_PARAMETER> kCaseInstanceParamRepository,
                            IRepository<PKS_KCASE_THEME_CHART> kCaseThemeChartRepository)
        {
            _kCaseCategoryRepository = kCaseCategoryRepository;
            _kCaseThemeRepository = kCaseThemeRepository;
            _kCaseInstanceChartRepository = kCaseInstanceChartRepository;
            _kCaseInstanceRepository = kCaseInstanceRepository;
            _kCaseParamCategoryRepository = kCaseParamCategoryRepository;
            _kCaseThemeParamRepository = kCaseThemeParamRepository;
            _kCaseInstanceParamRepository = kCaseInstanceParamRepository;
            _kCaseThemeChartRepository = kCaseThemeChartRepository;
        }

        public byte[] GetInstanceChart(int instanceId)
        {
            var entity = _kCaseInstanceChartRepository.GetQuery()
                .FirstOrDefault(t => t.KCASEINSTANCEID == instanceId && t.CHART != null);
            if (entity == null) return null;
            return entity.CHART;
        }

        public bool HasChart(int instanceId)
        {
            var entity = _kCaseInstanceChartRepository.GetQuery()
                .FirstOrDefault(t => t.KCASEINSTANCEID == instanceId && t.CHART != null);
            if (entity == null) return false;
            return true;
        }

        public InstanceModel GetInstanceInfo(int instanceId)
        {
            var entity = _kCaseInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.Id == instanceId);
            if (entity == null) return new InstanceModel();
            var result = new InstanceModel
            {
                Id = entity.Id,
                Name = entity.NAME,
                BoDescription = entity.BODESCRIPTION,
                Remark = entity.REMARK,
                Author = entity.AUTHOR,
                Auditor = entity.AUDITOR
            };
            var theme = _kCaseThemeRepository.GetQuery()
                .FirstOrDefault(t => t.Id == entity.KCASETHEMEID);
            if(theme!=null)result.Theme = theme.NAME;
            return result;
        }

        public List<ElementTreeNode> GetCaseTree()
        {
            var result = new List<ElementTreeNode>();
            var categories = _kCaseCategoryRepository.GetQuery()
                .OrderBy(t => t.ORDERNUMBER)
                .Select(t => new ElementTreeNode
                {
                    id = t.Id,
                    pid = t.PARENTID,
                    label = t.NAME,
                    isCase = false
                });
            var themes = _kCaseThemeRepository.GetQuery()
                .Select(t => new ElementTreeNode
                {
                    id = -t.Id,              // Id取负数避免与CategoryId重复
                    pid = t.KCASECATEGORYID,
                    label = t.NAME,
                    isCase = true,
                    caseId = t.Id
                });
            if (categories != null) result.AddRange(categories);
            if (themes != null) result.AddRange(themes);

            return GetElementTree(result);
        }

        

        private List<ElementTreeNode> GetElementTree(List<ElementTreeNode> nodes)
        {
            List<ElementTreeNode> root = nodes.FindAll(n => n.pid == null);
            return BuildTree(nodes, root);
        }        

        private List<ElementTreeNode> BuildTree(List<ElementTreeNode> nodes, List<ElementTreeNode> root)
        {
            for(int i = 0; i < root.Count; i++)
            {
                List<ElementTreeNode> children = nodes.FindAll(n => n.pid == root[i].id);
                BuildTree(nodes, children);
                root[i].children = children;
            }
            return root;
        }

        public List<ParamTreeGridNode> GetParamTreeGrid(int instanceId)
        {
            var result = new List<ParamTreeGridNode>();
            var entity = _kCaseInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.Id == instanceId);
            if (entity == null) return result;
            var categories = _kCaseParamCategoryRepository.GetQuery()
                .Where(t => t.KCASETHEMEID == entity.KCASETHEMEID)
                .Select(t => new ParamTreeGridNode
                {
                    id = t.Id,
                    label = t.NAME,
                    parent_id = t.PARENTID,
                    expanded = true
                })?.ToList();
            if (categories == null || categories.Count() < 1) return null;
            var categoryIds = categories.Select(t => t.id);
            var parameters = _kCaseThemeParamRepository.GetQuery()
                .Where(t => categoryIds.Contains(t.KCASEPARAMETERCATEGORYID))
                .Select(t => new ParamTreeGridNode
                {
                    id = -t.Id,           //参数id取负避免重复
                    label = t.NAME,
                    parent_id = t.KCASEPARAMETERCATEGORYID
                })?.ToList();
            if (parameters != null&&parameters.Count()>0)
            {
                foreach(var item in parameters)
                {
                    var instanceparam = _kCaseInstanceParamRepository.GetQuery()
                        .FirstOrDefault(t => t.KCASETHEMEPARAMETERID == -item.id && t.KCASEINSTANCEID == instanceId);
                    if (instanceparam != null)
                    {
                        item.paramvalue = instanceparam.PARAMETERVALUE;
                        item.sampledata = instanceparam.SAMPLEDATA;
                        item.remark = instanceparam.REMARK;
                    }
                }
                result.AddRange(parameters);
            }
            result.AddRange(categories);
            return GetTreeGrid(result);
        }

        private List<ParamTreeGridNode> GetTreeGrid(List<ParamTreeGridNode> nodes)
        {
            List<ParamTreeGridNode> root = nodes.FindAll(n => n.parent_id == null);
            return BuildTreeGrid(nodes, root, 1);
        }

        private List<ParamTreeGridNode> BuildTreeGrid(List<ParamTreeGridNode> nodes, List<ParamTreeGridNode> root, int depth)
        {
            for (int i = 0; i < root.Count; i++)
            {
                List<ParamTreeGridNode> children = nodes.FindAll(n => n.parent_id == root[i].id);
                if (children != null && children.Count() > 0)
                {
                    for(int c = 0; c < children.Count; c++)
                    {
                        children[c].depth = depth;
                    }
                    BuildTreeGrid(nodes, children, depth + 1);
                    root[i].child_num = children.Count();
                    root[i].children = children;
                }               
            }
            return root;
        }

        public List<ChartModel> GetCharts(int instanceId)
        {
            var result = new List<ChartModel>();
            var instance = _kCaseInstanceRepository.GetQuery()
                .FirstOrDefault(t => t.Id == instanceId);
            if (instance == null) return result;
            var charts = _kCaseThemeChartRepository.GetQuery()
                .Where(t => t.KCASETHEMEID == instance.KCASETHEMEID)
                .Select(t => new ChartModel
                {
                    Id = t.Id,
                    Name = t.NAME,
                    ChartType = t.CHARTTYPE,
                    Parameters = t.PARAMETERS
                });
            if (charts != null) result.AddRange(charts);
            return result;           
        }
    }
}
