using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Controllers
{
    /// <summary>自定义流结果</summary>
    public class ApiStreamResult : ApiActionResult
    {
        /// <summary>构造函数</summary>
        public ApiStreamResult(string mimeType, object content, ApiController controller) : base(
            mimeType, content, controller)
        {
        }

        /// <summary>下载文件名</summary>
        public string FileName { get; set; }
        /// <summary>应答处理器</summary>
        public Action<HttpResponseMessage> ResponseHandler { get; set; }
        /// <summary>是否能转换为输出</summary>
        public static bool CanConvert(object content)
        {
            return content is Stream || content is byte[];
        }
        /// <summary>执行</summary>
        protected override HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var stream = BuildContent();
                if (stream.CanSeek) stream.Position = 0;
                response.Content = new StreamContent(stream);
                var contentHeaders = response.Content.Headers;
                if (!this.FileName.IsNullOrEmpty())
                {
                    response.Content.UseContentDisposition(this.FileName, false);
                }
                contentHeaders.ContentType = new MediaTypeHeaderValue(this.MimeType);
                contentHeaders.ContentLength = stream.Length;
                if (this.ResponseHandler != null) this.ResponseHandler(response);
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
        protected Stream BuildContent()
        {
            var stream = this.Content.As<Stream>();
            if (stream == null)
            {
                var content = this.Content.As<byte[]>();
                stream = new MemoryStream(content);
            }
            return stream;
        }
    }
}
