using PKS.Core;
using PKS.SZZSK.IService.Common;
using PKS.SZZSK.Web.Config.PageSearchService;
using System.Collections.Generic;

namespace PKS.SZZSK.Service.Common
{
    public class DefaultPageSearchConfigGrabber : IPageSearchConfigGrabber, ISingletonAppService
    {
        private PageSearchService _pageSearchService;

        public DefaultPageSearchConfigGrabber()
        {
            _pageSearchService = new PageSearchService();
        }
        public Dictionary<string, string> GetPageSearchConfig(string controllerName, string actionName)
        {
            return _pageSearchService.GetPageFilterByRoute(controllerName, actionName);
        }
    }
}
