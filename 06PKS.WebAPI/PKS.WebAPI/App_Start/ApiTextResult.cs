using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace PKS.WebAPI.Controllers
{
    /// <summary>自定义文本结果</summary>
    public class ApiTextResult : ApiActionResult
    {
        /// <summary>构造函数</summary>
        public ApiTextResult(string mimeType, object content, ApiController controller) : base(mimeType, content, controller)
        {
            this.Encoding = Encoding.UTF8;
        }
        /// <summary>编码</summary>
        public Encoding Encoding { get; set; }
        /// <summary>执行</summary>
        protected override HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                response.Content = BuildContent();
                var mediaTypeHeaderValue = new MediaTypeHeaderValue(this.MimeType);
                mediaTypeHeaderValue.CharSet = this.Encoding.WebName;
                response.Content.Headers.ContentType = mediaTypeHeaderValue;
                response.RequestMessage = this.Request;
            }
            catch
            {
                response.Dispose();
                throw;
            }
            return response;
        }
        /// <summary>生成内容</summary>
        protected ByteArrayContent BuildContent()
        {
            var content = this.Content.ToString();
            //var content = this.Content.ToJson();
            var buffer = this.Encoding.GetBytes(content);
            return new ByteArrayContent(buffer);
        }
    }
}
