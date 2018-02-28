using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PKS.DbServices;
using PKS.DbServices.KManage;
using PKS.DbServices.KManage.Model;
using PKS.PageEngine;
using PKS.PageEngine.Data;
using PKS.PageEngine.EvenHandlers;
using PKS.PageEngine.Extensions;
using PKS.PageEngine.Param;
using PKS.PageEngine.Query;
using PKS.PageEngine.View;
using PKS.Web;
using PKS.WebAPI.Services;

namespace PKS.SZXT.Web.Common
{
    public class BaikePageEngine:PageRenderEngine
    {
        private KManage2Service _kManage2Service;
        private ISearchService _searchService;
        private PageContext _pageContext;

        public BaikePageEngine(PageContext pageContext):base(pageContext)
        {
            _pageContext = pageContext;
            _searchService = PKS.Core.Bootstrapper.Get<ISearchService>();
            _kManage2Service = PKS.Core.Bootstrapper.Get<KManage2Service>();
        }

        public void Load()
        {
            TemplateInfo templateInfo = GetTemplate();
            var fragmentModels = GetAllFragmentsFromTemplate(templateInfo);
            RebuildPageContext(templateInfo, fragmentModels.Select(item => item.FragmentTypeId).ToList());
            var fragmentInfos = GetFragmentInfos(fragmentModels);
            this.LoadView(templateInfo.Template, fragmentInfos);
        }

        public override void LoadData(object sender, SampleEventArgs e)
        {
            base.LoadData(sender, e);
            ViewComponent viewComponent=sender as ViewComponent;
            //e.Data 调用Es服务，填充e.Data
            IQueryPlanTranslator translator=new EsQueryPlanTranslator();
            var esQueryString=translator.Translate(viewComponent.FragmentInfo.QueryPlan, viewComponent.FragmentInfo.QueryOutputParams);
            e.Data = GetEsData(esQueryString);
        }

        private object GetEsData(string queryString)
        {
            var queryData = _searchService.ESSearchEx(queryString);
            var jResult = (queryData as JObject)["results"];
            //todo 处理请求失败的情况
            var jArray = jResult as JArray; 
            return jArray;
        }

        private TemplateInfo GetTemplate()
        {
            string boName = _pageContext.GetContextParamValue<string>("boname");
            var instanceClass = _pageContext.GetContextParamValue<string>("instanceclass");

            var templateInfo = _kManage2Service.FindTemplateByBo(0,boName, instanceClass);
            if (templateInfo == null)
                throw new Exception("未找到模板");
            if (string.IsNullOrWhiteSpace(templateInfo.Template))
            {
                throw new Exception("模板内容为空");
            }
            return templateInfo;
        }

        private List<FragmentModel> GetAllFragmentsFromTemplate(TemplateInfo templateInfo)
        {
            var rootEle = XElement.Parse(templateInfo.Template);
            var placeHolderIds = rootEle.Descendants("placeholder").Where(item => item.Attribute("id") != null)
                .Select(item => item.Attribute("id").Value).ToList();
            //所有片段
            return _kManage2Service.GetFragmentsByPlaceHolderIds(templateInfo.Id ,placeHolderIds.ToArray());
        }
        private List<FragmentInfo> GetFragmentInfos(List<FragmentModel> fragmentModels)
        {
            var fragmentInfos = new List<FragmentInfo>();
            fragmentModels.ForEach(fragmentModel =>
            {
                FragmentInfo fragmentInfo = new FragmentInfo();
                fragmentInfo.Id = fragmentModel.Id.ToString();
                fragmentInfo.FragmentHasTextTemplate = fragmentModel.FragmentHasTextTemplate;
                fragmentInfo.FragmentTypeCode = fragmentModel.FragmentTypeCode;
                fragmentInfo.FragmentTypeName = fragmentModel.FragmentTypeName;
                fragmentInfo.FragmentVueTag = fragmentModel.FragmentVueTag;
                fragmentInfo.Htmltext = fragmentModel.Htmltext;
                fragmentInfo.PlaceHolderId = fragmentModel.PlaceholderId;
                fragmentInfo.Title = fragmentModel.Title;
                //输入参数
                fragmentInfo.QueryPlan = JsonConvert.DeserializeObject<QueryPlan>(fragmentModel.QueryParameter);
                fragmentInfo.StrComponentParameters = fragmentModel.ComponentParameter;
                fragmentInfo.FragmentTypeId = fragmentModel.FragmentTypeId;
                RebuildComponentParamsValue(fragmentInfo);
                RebuildFragmentOutputParams(fragmentInfo);
                fragmentInfos.Add(fragmentInfo);
            });
            return fragmentInfos;
        }

        /// <summary>
        /// 组件参数值
        /// </summary>
        /// <param name="fragmentInfo"></param>
        private void RebuildComponentParamsValue(FragmentInfo fragmentInfo)
        {
            var fragmentInfoId = fragmentInfo.FragmentTypeId;
            var mComponentParams = JsonConvert.DeserializeObject<List<ComponentParam>>(fragmentInfo.StrComponentParameters);
            var componentParams = _pageContext.ComponentParams.Where(item => item.Id == fragmentInfoId).ToList();
            foreach (var componentParam in componentParams)
            {
                if (mComponentParams != null)
                {
                    var mComponentParam = mComponentParams.FirstOrDefault(item => item.Code == componentParam.Code);
                    if (mComponentParam != null)
                    {
                        componentParam.Value = mComponentParam.Value;
                    }
                }
            }
            fragmentInfo.ComponentParameters = componentParams;
        }

        /// <summary>
        /// 组件查询输出参数
        /// </summary>
        private void RebuildFragmentOutputParams(FragmentInfo fragmentInfo)
        {
            if (fragmentInfo.ComponentParameters != null)
            {
                fragmentInfo.QueryOutputParams =
                    fragmentInfo.ComponentParameters.Where(item => !string.IsNullOrWhiteSpace(item.Metadata))
                        .Select(item => new QueryOutputParam()
                        {
                            Code = item.Code,
                            DataType = item.DataType,
                            DefaultValue = item.DefaultValue,
                            Name = item.Name,
                            Value = item.Value,
                            Metadata = item.Metadata
                        }).ToList();
                //文本模板变量也作为输出参数
                if (fragmentInfo.FragmentHasTextTemplate)
                {
                    foreach (var templateParam in _pageContext.TextTemplateParams)
                    {
                        var queryOutputParam = fragmentInfo.QueryOutputParams.FirstOrDefault(item=>item.Code== templateParam.Code);
                        if (queryOutputParam == null)
                        {
                            fragmentInfo.QueryOutputParams.Add(new QueryOutputParam
                            {
                                Code= templateParam.Code,
                                DataType= templateParam.DataType,
                                DefaultValue=templateParam.DefaultValue,
                                Metadata=templateParam.Code,
                                Name=templateParam.Name
                            });
                        }
                    }
                }
            }
        }

     
        private void RebuildPageContext(TemplateInfo templateInfo, List<int> fragmentTypeIds)
        {
            var kManage2Service = PKS.Core.Bootstrapper.Get<KManage2Service>();
            //所有对应组件参数【获取组件参数详细信息】
            var fragmentTypeParams =
                kManage2Service.GetComponentParamsByFragmentTypeId(fragmentTypeIds);
            //模板参数【替换组件HtmlText】
            var templateParams = kManage2Service.GetTemplateParamsByTemplateId(templateInfo.Id);

            var componentParams = fragmentTypeParams.Select(item => new ComponentParam()
            {
                Id = item.FragmentTypeId,
                Metadata = item.Metadata,
                Code = item.Code,
                Name = item.Name,
                DataType = item.DataType,
                DefaultValue = item.DefaultValue
            });
            _pageContext.AddComponentParams(componentParams);

            var textTemplateParams = templateParams.Select(item => new TextTemplateParam()
            {
                Id = item.Id,
                ParentId = item.ParentId,
                Code = item.Code,
                Name = item.Name,
                DataType = item.DataType
            });
            _pageContext.AddTextParams(textTemplateParams);
        }
    }
}