using System;
using System.Collections.Generic;
using PKS.PageEngine.Data;
using PKS.PageEngine.View;

namespace PKS.PageEngine
{
    /// <summary>
    /// 页面组件渲染引擎
    /// </summary>
    public class PageRenderEngine
    {
        private ViewManager _viewManager;
        private PageContext _pageContext;

        public bool CanLoad { get; set; }

        public PageRenderEngine(PageContext pageContext)
        {
            CanLoad = true;
            _pageContext = pageContext;
            _viewManager = new ViewManager(pageContext);
            _viewManager.LoadAllComponentDataEventHandler += _viewManager_LoadAllComponentDataEventHandler;
            _viewManager.BeforeLoadComponentDataEventHandler += BeforeLoadDataEventHandler;
            _viewManager.LoadComponentDataEventHandler += LoadDataEventHandler;
            _viewManager.AfterLoadComponentDataEventHandler += AfterLoadDataEventHandler;
        }

      

        public ViewManager ViewManager
        {
            get { return _viewManager; }
        }
        public PageContext PageContext
        {
            get { return _pageContext; }
        }

        public string GetHtml()
        {
            return _viewManager.GetHtml();
        }

        public JsDataModel GetData()
        {
            if (CanLoad)
            {
                _viewManager.LoadData();
            }
            
            return _viewManager.JsModel;
        }

        public virtual void LoadView(string strPageElements, List<FragmentInfo> fragmentInfos)
        {
            _pageContext.RebuildParams();
            _viewManager.LoadComponents(strPageElements, fragmentInfos);
        }

        public virtual object LoadAllComponentData(List<ComponentQueryInfo> queryInfos)
        {
            return null;
        }

        public virtual void LoadData(object sender, EvenHandlers.SampleEventArgs e)
        {
            
        }
        public virtual void BeforeLoadData(object sender, EventArgs e)
        {

        }
        public virtual void AfterLoadData(object sender, EvenHandlers.SampleEventArgs e)
        {

        }
        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadDataEventHandler(object sender, EvenHandlers.SampleEventArgs e)
        {
            LoadData(sender, e);
        }
    
        public void BeforeLoadDataEventHandler(object sender, EventArgs e)
        {
            BeforeLoadData(sender, e);
        }

      
        public void AfterLoadDataEventHandler(object sender, EvenHandlers.SampleEventArgs e)
        {
            AfterLoadData(sender, e);
        }

        private void _viewManager_LoadAllComponentDataEventHandler(object sender, EvenHandlers.ComponentsEventArg e)
        {
            e.Data= LoadAllComponentData( e.QueryInfos);
        }
    }
}
