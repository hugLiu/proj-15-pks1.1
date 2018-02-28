using System.Collections.Generic;

namespace PKS.SZXT.IService.Common
{
    public interface IPageSearchConfigGrabber
    {
        Dictionary<string,string> GetPageSearchConfig(string controllerName,string actionName);
    }
}
