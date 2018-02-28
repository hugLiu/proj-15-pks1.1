using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PKS.DbServices;
using PKS.DbServices.KManage.Model;
using PKS.Models;
using PKS.PageEngine;
using PKS.PageEngine.EvenHandlers;
using PKS.PageEngine.Extensions;
using PKS.PageEngine.Param;
using PKS.PageEngine.Query;
using PKS.PageEngine.View;
using PKS.WebAPI.Services;
using PKS.Web;
using TemplateInfo = PKS.DbServices.KManage.Model.TemplateInfo;


namespace PKS.SZZSK.Web.Common
{
    public class BaikePageEngine:PageRenderEngine
    {
        private KManage2Service _kManage2Service;
        private ISearchService _searchService;
        private IAppDataService _appDataService;
        private PageContext _pageContext;
        private TemplateInfo _template;
        private List<FragmentInfo> _fragmentInfos;
        public BaikePageEngine(PageContext pageContext):base(pageContext)
        {
            _pageContext = pageContext;
            InitPageContext();
            _searchService = PKS.Core.Bootstrapper.Get<ISearchService>();
            _kManage2Service = PKS.Core.Bootstrapper.Get<KManage2Service>();
            _appDataService = PKS.Core.Bootstrapper.Get<IAppDataService>();
        }

        public bool Load()
        {
            _template = GetTemplate();
            if (_template == null)
            {
                this.CanLoad = false;
                return false;
            }
            var fragmentModels = GetAllFragmentsFromTemplate(_template);
            if(fragmentModels==null|| fragmentModels.Count()==0)
            {
                this.CanLoad = false;
                return false;
            }
            RebuildPageContext(_template, fragmentModels.Select(item => item.FragmentTypeId).ToList());
            var fragmentInfos = GetFragmentInfos(fragmentModels);
            _fragmentInfos = fragmentInfos;

    
          
            _pageContext.CatalogueItems = GetCatalogueInfos(); //GetCatalogueItems();
            this.LoadView(_template.Template, fragmentInfos);
            return true;
        }

        public override object LoadAllComponentData(List<ComponentQueryInfo> queryInfos)
        {
            if (queryInfos.Count == 0)
                return null;
            IQueryPlanTranslator translator = new EsQueryPlanTranslator();
            StringBuilder queryBuilder=new StringBuilder();
            foreach (var queryInfo in queryInfos)
            {
                queryBuilder.AppendLine("{}");
                var esQueryString = translator.Translate(queryInfo.QueryPlan, queryInfo.OutputParams);
                queryBuilder.AppendLine(esQueryString);
            }
            var queryData = _searchService.ESMSearch(queryBuilder.ToString());
            var jArray = JArray.Parse(Convert.ToString(queryData));
            var arrayObjList=new JArray();
            foreach (var araryItem in jArray)
            {
                arrayObjList.Add(araryItem["results"]);
            }
            return arrayObjList;
        }

        public override void LoadData(object sender, SampleEventArgs e)
        {

            base.LoadData(sender, e);
            var viewComponent = sender as ViewComponent;
            if (viewComponent != null)
            {
                var tagName = viewComponent.FullTagName;
                if (tagName == "pks:baikemulu")
                {
                    //目录组件单独加载数据
                   // e.Data = _kManage2Service.GetCatalogueInfosByTemplateId(_template.Id);

                    e.Data = _pageContext.CatalogueItems;
                }
                   
                if (tagName == "pks:table")
                {
                    var data = GetTableDatas(viewComponent);
                    if(data!=null)
                        e.Data = JArray.Parse(data.FirstOrDefault());
                }
                if ((tagName == "pks:text" && !viewComponent.FragmentInfo.FragmentHasTextTemplate))
                {
                   var data = GetTableDatas(viewComponent);
                    if (data != null)
                        e.Data = data;
                }
            }
        }

        private List<string> GetTableDatas(ViewComponent component)
        {
            List<string> datas = new List<string>();
            var jArray = component.Data as JArray;
            if (jArray == null)
                return null;
            foreach (var item in jArray)
            {
                var dataIdProperty= item.Children().FirstOrDefault(child =>string.Equals("dataid",(child as JProperty).Name,StringComparison.OrdinalIgnoreCase)) as JProperty;
                var dataId = Convert.ToString(dataIdProperty.Value);
                var pageData = (_appDataService.Get(dataId) as PKS.WebAPI.Models.IndexAppData);
                var content = pageData.Content;
                datas.Add(Convert.ToString(content));
            }
            return datas;
        }

        private object GetEsData(string queryString)
        {
            var queryData = _searchService.ESSearchEx(queryString);
            var jResult = (queryData as JObject)["results"];
            //todo 处理请求失败的情况
            var jArray = jResult as JArray; 
            return jArray;
        }

        public List<CatalogueItem> GetCatalogueItems()
        {
            var catalogueInfos = _kManage2Service.GetCatalogueInfosByTemplateId(_template.Id);
            var catalogueItems = catalogueInfos.Select(item => new CatalogueItem
            {
                Code = item.Code,
                Name = item.Name,
                LevelNumber = item.LevelNumber,
                OrderNumber = item.OrderNumber,
                PlaceHolderId = item.PlaceHolderId
            }).ToList();
            return catalogueItems;
        }
        public List<CatalogueItem> GetCatalogueInfos()
        {
            List<CatalogueItem> catalogueInfos = new List<CatalogueItem>();
            var doc= XDocument.Parse(_template.Template);
            var placeholders = doc.Descendants("placeholder");
            Dictionary<int, string> holders = new Dictionary<int, string>();

            for (int i = 0; i < placeholders.Count(); i++)
            {
                var id = placeholders.ElementAt(i).Attribute("id").Value;
                holders.Add(i, id);
            }          

            var headline1s = placeholders.Where(item => item.Attribute("type").Value == "headline1");
            for (int i = 0; i < headline1s.Count(); i++)
            {
                CatalogueItem catalogInfo = new CatalogueItem();
                catalogInfo.Code = Convert.ToString(i + 1);
                //catalogInfo.Name=
                catalogInfo.LevelNumber = 1;
                catalogInfo.OrderNumber = i + 1;
                var id = headline1s.ElementAt(i).Attribute("id").Value;
                catalogInfo.PlaceHolderId = id;
                catalogueInfos.Add(catalogInfo);
             var index = holders.FirstOrDefault(item => item.Value == id).Key;

                if(i+1<= headline1s.Count()-1)
                {
                    var endid= headline1s.ElementAt(i+1).Attribute("id").Value;
                    var endIndex= holders.FirstOrDefault(item => item.Value == endid).Key;

                    var secondIndex = 0;
                    for (int j = index+1; j < endIndex; j++)
                    {
                        var el = placeholders.ElementAt(j);
                        var typeName = el.Attribute("type").Value;
                        var secondId = el.Attribute("id").Value;
                        if (typeName=="headline2")
                        {
                            secondIndex++;
                            CatalogueItem secondCatalogInfo = new CatalogueItem();
                            secondCatalogInfo.Code = catalogInfo.Code+"."+ Convert.ToString(secondIndex);
                            secondCatalogInfo.LevelNumber = 2;
                            secondCatalogInfo.OrderNumber = secondIndex;
                            secondCatalogInfo.PlaceHolderId = secondId;
                            catalogueInfos.Add(secondCatalogInfo);
                        }
                        if(typeName=="headline1")
                        {

                        }
                    }
                }
                else
                {
                    var endIndex = holders.Count()-1;
                    var secondIndex = 0;
                    for (int j = index + 1; j <= endIndex; j++)
                    {
                        var el = placeholders.ElementAt(j);
                        var typeName = el.Attribute("type").Value;
                        var secondId = el.Attribute("id").Value;
                        if (typeName == "headline2")
                        {
                            secondIndex++;
                            CatalogueItem secondCatalogInfo = new CatalogueItem();
                            secondCatalogInfo.Code = catalogInfo.Code + "." + Convert.ToString(secondIndex);
                            secondCatalogInfo.LevelNumber = 2;
                            secondCatalogInfo.OrderNumber = secondIndex;
                            secondCatalogInfo.PlaceHolderId = secondId;
                            catalogueInfos.Add(secondCatalogInfo);
                        }
                        if (typeName == "headline1")
                        {

                        }
                    }
                }
            }

            foreach (var info in catalogueInfos)
            {
                var fragment = _fragmentInfos.FirstOrDefault(item => item.PlaceHolderId == info.PlaceHolderId);
                if(fragment!=null)
                {
                    info.Name = fragment.Title;
                }               
            }
            return catalogueInfos;
        }


        public virtual TemplateInfo GetTemplate()
        {
            string boName = _pageContext.GetContextParamValue<string>("instance");
            var instanceClass = _pageContext.GetContextParamValue<string>("instanceclass");
            int urlid = _pageContext.GetContextParamValue<int>("urlid");

            var templateInfo = _kManage2Service.FindTemplateByBo(urlid, boName, instanceClass);
            if (templateInfo == null)
                return null;
                //throw new Exception("未找到模板");
            if (string.IsNullOrWhiteSpace(templateInfo.Template))
            {
                return null;
                //throw new Exception("模板内容为空");
            }
            return templateInfo;
        }

        private List<FragmentModel> GetAllFragmentsFromTemplate(TemplateInfo templateInfo)
        {
            var rootEle = XElement.Parse(templateInfo.Template);
            var placeHolderIds = rootEle.Descendants("placeholder").Where(item => item.Attribute("id") != null)
                .Select(item => item.Attribute("id").Value).ToList();
            //所有片段
            return _kManage2Service.GetFragmentsByPlaceHolderIds(templateInfo.Id, placeHolderIds.ToArray());
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
                fragmentInfo.QueryPlan = ConvertToQueryPlan(fragmentModel.QueryParameter);
                fragmentInfo.StrComponentParameters = fragmentModel.ComponentParameter;
                fragmentInfo.FragmentTypeId = fragmentModel.FragmentTypeId;
                RebuildFragmentComponentParamsValue(fragmentInfo);
                RebuildFragmentOutputParams(fragmentInfo);
                fragmentInfos.Add(fragmentInfo);
            });
            return fragmentInfos;
        }

        /// <summary>
        /// 构建知识片段组件参数
        /// </summary>
        /// <param name="fragmentInfo"></param>
        private void RebuildFragmentComponentParamsValue(FragmentInfo fragmentInfo)
        {
            var fragmentInfoId = fragmentInfo.FragmentTypeId;
            var mComponentParams = JsonConvert.DeserializeObject<List<ComponentParam>>(fragmentInfo.StrComponentParameters);
            var componentParams = _pageContext.ComponentParams.Where(item => item.Id == fragmentInfoId).ToList();
            var fragmentComponentParams = new List<ComponentParam>();
            foreach (var componentParam in componentParams)
            {
                ComponentParam fragmentComParam=new ComponentParam();
                fragmentComParam.Id = componentParam.Id;
                fragmentComParam.Metadata = componentParam.Metadata;
                fragmentComParam.Code = componentParam.Code;
                fragmentComParam.DataType = componentParam.DataType;
                fragmentComParam.Value = componentParam.Value;
                fragmentComParam.DefaultValue = componentParam.DefaultValue;
                fragmentComParam.Name = componentParam.Name;
                if (mComponentParams != null)
                {
                    var mComponentParam = mComponentParams.FirstOrDefault(item => item.Code == componentParam.Code);
                    if (mComponentParam != null)
                    {
                        fragmentComParam.Value = mComponentParam.Value;
                    }
                }
                fragmentComponentParams.Add(fragmentComParam);
            }
            fragmentInfo.ComponentParameters = fragmentComponentParams;
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
            //所有对应组件参数【获取组件参数详细信息】
            var fragmentTypeParams =
                _kManage2Service.GetComponentParamsByFragmentTypeId(fragmentTypeIds);
            //模板参数【替换组件HtmlText】
            var templateParams = _kManage2Service.GetTemplateParamsByTemplateId(templateInfo.Id);

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

        private QueryPlan ConvertToQueryPlan(string queryParam)
        {
            QueryPlan queryPlan = new QueryPlan();
            if (string.IsNullOrWhiteSpace(queryParam) || string.Equals(queryParam, "null", StringComparison.OrdinalIgnoreCase))
                return queryPlan;
            var root = JObject.Parse(queryParam);
         
            List<QueryField> fields = new List<QueryField>();
            queryPlan.Fields = fields;
            var query = root["query"];
            if (query != null)
            {
                foreach (var child in query.Children())
                {
                    QueryField field = new QueryField();
                    var property = child as JProperty;
                    field.FieldName = property.Name;//pt

                    var valueChildren = property.Value.Children();
                    foreach (var vchild in valueChildren)
                    {
                        var vProperty = vchild as JProperty;
                        field.OperationType = vProperty.Name.ToOperationType();//eq

                        field.Value = Convert.ToString((vProperty.Value as JValue).Value);
                    }
                    field.FieldQueryType = field.Value.StartsWith("@") ? FieldQueryType.Variable : FieldQueryType.Fixed;
                    fields.Add(field);
                }

            }

            var queryOrders = new List<QueryOrder>();
            var sort = root["sort"];
            if (sort != null)
            {
                foreach (var child in sort.Children())
                {
                    QueryOrder queryOrder = new QueryOrder();
                    var property = child as JProperty;
                    queryOrder.FieldName = property.Name;
                    queryOrder.OrderDirection = Convert.ToString((property.Value as JValue).Value);
                    queryOrders.Add(queryOrder);
                }
            }
            queryPlan.QueryOrders = queryOrders;
            var size = root["size"];
            if (size != null)
            {
                var sizeV = (size as JValue).Value;
                if (sizeV == null | sizeV.ToString() == string.Empty)
                    queryPlan.TopCount = 10;
                else
                {
                    queryPlan.TopCount=Convert.ToInt32(sizeV);
                }
            }
            return queryPlan;
        }


        private void InitPageContext()
        {
            //param
            var urlQuery = HttpContext.Current.Request.Url.Query;
            if (urlQuery.Length>1&&urlQuery.IndexOf("?")==0)
            {
                foreach (var pageParam in urlQuery.Substring(1).Split('&'))
                {
                    var paramKey = pageParam.Split('=')[0];
                    var paramValue= pageParam.Split('=').Length>1? pageParam.Split('=')[1]:"";
                    _pageContext.AddContextParam(paramKey, HttpUtility.UrlDecode(paramValue));
                }
            }
            //url
            var urls = GetService<IPKSSubSystemConfig>().Urls;
            foreach (var url in urls)
            {
                _pageContext.AddContextParam(url.Key,url.Value);
            }
            _pageContext.AddContextParam("downloadurl", urls[PKSSubSystems.WEBAPI] + "/api/appdataservice/download");
        }
       
        private static TService GetService<TService>()
        {
            return (TService)DependencyResolver.Current.GetService(typeof(TService));
        }
    }
}