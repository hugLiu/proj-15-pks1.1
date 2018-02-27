using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using PKS.PageEngine.Data;
using PKS.PageEngine.EvenHandlers;

namespace PKS.PageEngine.View
{
    public  class ViewManager
    {
        public event EventHandler<SampleEventArgs> LoadComponentDataEventHandler;
        public event EventHandler<EventArgs> BeforeLoadComponentDataEventHandler;
        public event EventHandler<SampleEventArgs> AfterLoadComponentDataEventHandler;

        public event EventHandler<ComponentsEventArg> LoadAllComponentDataEventHandler;
        private ViewDataManager _viewDataManager;
        private PageContext _pageContext;
        private DataLoadType _dataLoadType;
        public ViewManager(PageContext pageContext,DataLoadType dataLoadType=DataLoadType.AllAtOnce)
        {
            _pageContext = pageContext;
            _dataLoadType = dataLoadType;
            _viewDataManager = new ViewDataManager(_pageContext);
        }
        /// <summary>
        /// JsModel
        /// </summary>
        public JsDataModel JsModel { get; set; }
        /// <summary>
        /// 组件对应片段信息
        /// </summary>
        public List<FragmentInfo> FragmentInfos { get; set; }
        /// <summary>
        /// 根组件
        /// </summary>
        public ViewComponentContainer Root { get; set; }
        /// <summary>
        /// 所有的具体组件【如PKS】
        /// </summary>
        public Dictionary<string,ViewComponent> ViewComponents { get; set; }

        public void LoadComponents(string strPageElements, List<FragmentInfo> fragmentInfos)
        {
            FragmentInfos = fragmentInfos;
            var pageXmlRoot = XElement.Parse(strPageElements);
            Root =new ViewComponentContainer();
            Root.OrginalEditConfig = strPageElements;
            AssignmentXmlAttributeToComponent(pageXmlRoot, Root); 
            foreach (var xElement in pageXmlRoot.Elements())
            {
                Resolve(xElement, Root);
            }
        }

        public void LoadData()
        {
            //todo 重新整理
            if(_dataLoadType==DataLoadType.OneByOne)
                JsModel = _viewDataManager.GenerateComponentDataModel(this.ViewComponents);
            if (_dataLoadType == DataLoadType.AllAtOnce)
            {
                if (LoadAllComponentDataEventHandler != null)
                {
                    var queryInfos = this.ViewComponents.Select(item => new ComponentQueryInfo()
                    {
                        ComponentId = item.Key,
                        QueryPlan = item.Value.FragmentInfo.QueryPlan,
                        OutputParams = item.Value.FragmentInfo.QueryOutputParams
                    }).Where(item=>item.QueryPlan!=null&&item.QueryPlan.Fields.Count>0).ToList();
                    queryInfos.ForEach(item => _viewDataManager.ReplaceQueryVariableParam(item.QueryPlan));
                    var eventArg = new ComponentsEventArg() {QueryInfos = queryInfos};
                    LoadAllComponentDataEventHandler(this, eventArg);
                    var data = (eventArg.Data as JArray);
                    if(data!=null)
                    {
                        for (int i = 0; i < queryInfos.Count; i++)
                        {
                            this.ViewComponents[queryInfos[i].ComponentId].Data = data[i];
                        }
                    }
                }
              
                JsModel = _viewDataManager.GenerateComponentDataModel(this.ViewComponents);
            }
        }

        public string GetHtml()
        {
            return Root.ToHtml();
        }

        public List<string> GetAllComponentTagNames()
        {
            return ViewComponents.Select(item => item.Value.FullTagName).ToList();
        }

        private void Resolve(XElement xElement,ViewComponentBase parentComponent)
        {
            var name = xElement.Name;
            if (string.Equals(name.LocalName, "placeholder", StringComparison.OrdinalIgnoreCase))
            {
                var placeHolderId = xElement.Attribute("id").Value;
                var fragmentInfo=FragmentInfos.FirstOrDefault(
                    item => string.Equals(item.PlaceHolderId, placeHolderId, StringComparison.OrdinalIgnoreCase));
                if (fragmentInfo == null)
                    throw new Exception("缺少placeHolderId为" + placeHolderId + "的知识片段数据");
                ViewComponent component=new ViewComponent(fragmentInfo);
                component.OrginalEditConfig = xElement.ToString();
                component.BeforeLoadDataEventHandler += Component_BeforeLoadDataEventHandler;
                component.LoadDataEventHandler += Component_LoadDataEventHandler;
                component.AfterLoadDataEventHandler += Component_AfterLoadDataEventHandler;
                AssignmentXmlAttributeToComponent(xElement,component);
                parentComponent.AddChild(component);

                if(ViewComponents==null)
                    ViewComponents=new Dictionary<string, ViewComponent>(StringComparer.OrdinalIgnoreCase);
                component.Id = "v" + (ViewComponents.Count + 1).ToString();
                if (ViewComponents.ContainsKey(component.Id))
                {
                    throw new Exception("存在同名的组件");
                }
                ViewComponents.Add(component.Id,component);
            }
            else
            {
                var parentCom=new ViewComponentContainer();
                AssignmentXmlAttributeToComponent(xElement, parentCom);
                parentCom.OrginalEditConfig = xElement.ToString();
                parentComponent.AddChild(parentCom);
                //如果子孙节点存在placeholder，继续
                if (xElement.Descendants("placeholder").Any())
                {
                    foreach (var childElement in xElement.Elements())
                    {
                        Resolve(childElement, parentCom);
                    }
                }
            }
        }

        private void Component_BeforeLoadDataEventHandler(object sender, EventArgs e)
        {
            if (BeforeLoadComponentDataEventHandler != null)
                BeforeLoadComponentDataEventHandler(sender, e);
        }

        private void Component_LoadDataEventHandler(object sender, SampleEventArgs e)
        {
            if (LoadComponentDataEventHandler != null)
                LoadComponentDataEventHandler(sender, e);
        }
        private void Component_AfterLoadDataEventHandler(object sender, SampleEventArgs e)
        {
            if (AfterLoadComponentDataEventHandler != null)
                AfterLoadComponentDataEventHandler(sender, e);
        }

        private void AssignmentXmlAttributeToComponent(XElement element, ViewComponentBase component)
        {
            var attributes = element.Attributes();
            component.HtmlAttributes = attributes.ToDictionary(item => item.Name.LocalName, item => item.Value,StringComparer.OrdinalIgnoreCase);
            //component.Id = component.HtmlAttributes.ContainsKey("id") ? component.HtmlAttributes["id"]: Guid.NewGuid().ToString();
            if (component.HtmlAttributes.ContainsKey("Name"))
                component.Name = component.HtmlAttributes["Name"];
        }
    }
}