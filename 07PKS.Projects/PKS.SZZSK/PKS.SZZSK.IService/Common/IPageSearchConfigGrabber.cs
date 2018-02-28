using System.Collections.Generic;

namespace PKS.SZZSK.IService.Common
{
    public interface IPageSearchConfigGrabber
    {
        Dictionary<string,string> GetPageSearchConfig(string controllerName,string actionName);
    }
}
