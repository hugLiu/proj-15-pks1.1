using PKS.Core;
using PKS.SZXT.Core.IInfrastructure;
using PKS.SZXT.Core.Model;
using PKS.SZXT.IService.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.SZXT.Infrastructure
{
    public class ServiceInvoker : IServiceInvoker, ISingletonAppService
    {
        private static HttpClient _client;
        public IApiServiceConfig ApiServiceConfig { get; set; }
        static ServiceInvoker()
        {
            _client = new HttpClient();
            SetAccept();
        }
        private static void SetAccept()
        {
            var accept = _client.DefaultRequestHeaders.Accept;
            accept.Clear();
            accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public ServiceInvoker(IApiServiceConfig apiServiceConfig)
        {
            ApiServiceConfig = apiServiceConfig;
        }
        public T GetData<T>(ServiceType serviceName, string actionName, string query)
        {
            var url = GetUrl(serviceName, actionName, query);
            var rec = _client.GetStringAsync(url).Result;
            return DeserializeObject<T>(rec);
        }
        public T GetDataByPost<T>(ServiceType serviceName, string actionName, object arguments)
        {
            var url = GetUrl(serviceName,actionName).ToString();
            var send = SerializeArgument(arguments);
            var res =  _client.PostAsync(url,send).Result;
            var rec = res.EnsureSuccessStatusCode()
                         .Content
                         .ReadAsStringAsync()
                         .Result;
            return DeserializeObject<T>(rec);
        }
        private string GetServiceBaseUrl(ServiceType serviceName)
        {
            return ApiServiceConfig.GetApiServiceConfig(serviceName);
        }
        private string GetUrl(ServiceType serviceName, string actionName,string query = null)
        {
            var url = GetServiceBaseUrl(serviceName);
            if (!url.EndsWith("/"))
                url += "/";
            var relative = actionName;
            if (!string.IsNullOrEmpty(query))
                relative = $"{relative}?{query}";
            url += relative;
            return url;
        }
        private HttpContent SerializeArgument(object obj)
        {
            return new StringContent(obj.ToString());
        }
    }
}
