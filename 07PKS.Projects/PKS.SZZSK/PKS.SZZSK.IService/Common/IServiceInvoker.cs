using PKS.SZZSK.Core.Model;

namespace PKS.SZZSK.IService.Common
{
    public interface IServiceInvoker
    {
         T GetData<T>(ServiceType serviceName, string actionName, string query);
         T GetDataByPost<T>(ServiceType serviceName,string actionName, object arguments);
    }
}
