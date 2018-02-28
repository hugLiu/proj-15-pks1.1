using PKS.SZXT.Core.Model;

namespace PKS.SZXT.Core.IInfrastructure
{
    public interface IServiceInvoker
    {
         T GetData<T>(ServiceType serviceName, string actionName, string query);
         T GetDataByPost<T>(ServiceType serviceName,string actionName, object arguments);
    }
}
