using PKS.Core;
using PKS.SZXT.IService.Common;
using PKS.SZXT.Web.Config.PageSearchService;
using System.Collections.Generic;

namespace PKS.SZXT.Service.Common
{
    public class DefaultPageSearchConfigGrabber : IPageSearchConfigGrabber, ISingletonAppService
    {
        private PageSearchService _pageSearchService;

        public DefaultPageSearchConfigGrabber()
        {
            _pageSearchService = new PageSearchService();
        }
        public Dictionary<string,string> GetPageSearchConfig(string controllerName, string actionName)
        {
            return _pageSearchService.GetPageFilterByRoute(controllerName, actionName);
        }
    }
}
