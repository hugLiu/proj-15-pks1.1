using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace PKS.WebAPI.Controllers
{
    /// <summary>自定义方法输出结果</summary>
    public abstract class ApiActionResult : IHttpActionResult
    {
        /// <summary>构造函数</summary>
        protected ApiActionResult(string mimeType, object content, ApiController controller)
        {
            this.MimeType = mimeType;
            this.Content = content;
            this.Request = controller.Request;
        }
        /// <summary>输出MIME类型</summary>
        protected string MimeType { get; set; }
        /// <summary>输出内容</summary>
        protected object Content { get; set; }
        /// <summary>请求</summary>
        protected HttpRequestMessage Request { get; set; }
        /// <summary>执行</summary>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }
        /// <summary>执行</summary>
        protected abstract HttpResponseMessage Execute();
    }
}
