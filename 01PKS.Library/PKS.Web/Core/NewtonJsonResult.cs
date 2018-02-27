using Newtonsoft.Json;
using PKS.Utils;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PKS.Web.MVC
{
    /// <summary>Represents a class that is used to send JSON-formatted content to the response.</summary>
    public class NewtonJsonResult : ActionResult
    {
        /// <summary>Gets or sets the content encoding.</summary>
        /// <returns>The content encoding.</returns>
        public Encoding ContentEncoding { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <returns>The type of the content.</returns>
        public string ContentType { get; set; }

        /// <summary>Gets or sets the data.</summary>
        /// <returns>The data.</returns>
        public object Data { get; set; }

        /// <summary>Gets or sets a value that indicates whether HTTP GET requests from the client are allowed.</summary>
        /// <returns>A value that indicates whether HTTP GET requests from the client are allowed.</returns>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }


        /// <summary>获取或设置序列化参数</summary>
        public JsonSerializerSettings Settings { get; set; }

        /// <summary>应答处理器</summary>
        public Action<HttpResponseBase> ResponseHandler { get; set; }
        /// <summary>Initializes a new instance of the class.</summary>
        public NewtonJsonResult()
        {
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        /// <summary>Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.</summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("GET请求不允许");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!this.ContentType.IsNullOrEmpty())
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = MimeTypes.JSON;
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (ResponseHandler != null)
            {
                ResponseHandler(response);
            }
            if (this.Data != null)
            {
                response.Write(this.Data.ToJson(this.Settings));
            }
        }
    }
}