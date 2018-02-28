using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System.Net.Http.Formatting;
using System.Web.Http.ModelBinding;

namespace PKS.WebAPI.Controllers
{
    /// <summary>PKS API控制器基类</summary>
    public abstract class PKSApiController : ApiController
    {
        #region 依赖服务解析方法
        /// <summary>获得指定的依赖服务</summary>
        internal protected T GetService<T>()
        {
            return (T)this.Configuration.DependencyResolver.GetService(typeof(T));
        }
        #endregion

        #region 模型绑定方法
        /// <summary>从内容绑定模型</summary>
        protected async Task<T> BindModelFromBody<T>(string uploadPath)
        {
            var httpContent = this.Request.Content;
            NameValueCollection formData = null;
            if (httpContent.IsFormData())
            {
                formData = await httpContent.ReadAsFormDataAsync();
            }
            else if (httpContent.IsMimeMultipartContent())
            {
                var provider = await httpContent.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(uploadPath));
                foreach (var fileData in provider.FileData)
                {
                    File.Delete(fileData.LocalFileName);
                }
                formData = provider.FormData;
            }
            if (formData != null)
            {
                var pairs = new List<KeyValuePair<string, string>>(formData.Count);
                foreach (string key in formData.Keys)
                {
                    var values = formData.GetValues(key);
                    foreach (var value in values)
                    {
                        pairs.Add(new KeyValuePair<string, string>(key, value));
                    }
                }
                var formData2 = new FormDataCollection(pairs);
                return formData2.ReadAs<T>();
            }
            return default(T);
        }
        #endregion

        #region 认证方法
        /// <summary>认证令牌</summary>
        internal protected string Token { get; set; }
        /// <summary>获得登录用户</summary>
        protected IPKSPrincipal PKSUser
        {
            get { return (IPKSPrincipal)base.User; }
        }
        #endregion

        #region 文件上传方法
        /// <summary>载入上传的第一个文件流</summary>
        protected async Task<ServerUploadRequest> LoadFileStreamFirstOrDefault(UploadRequest request, string uploadPath)
        {
            var request2 = new ServerUploadRequest(request);
            var httpContent = this.Request.Content;
            NameValueCollection formData = null;
            if (httpContent.IsFormData())
            {
                formData = await httpContent.ReadAsFormDataAsync();
            }
            else if (httpContent.Headers.ContentType.MediaType.IsJsonMedia())
            {
                var content = await httpContent.ReadAsStringAsync();
                request2 = content.JsonTo<ServerUploadRequest>();
            }
            else if (httpContent.IsMimeMultipartContent())
            {
                var provider = await httpContent.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(uploadPath));
                var fileData = provider.FileData.FirstOrDefault();
                if (fileData != null)
                {
                    request2.FileName = fileData.Headers.ContentDisposition.FileName.Divest();
                    request2.CharSet = fileData.Headers.ContentType.CharSet;
                    request2.ServerFile = fileData.LocalFileName;
                }
                formData = provider.FormData;
            }
            if (formData != null)
            {
                string formDataValue = null;
                if (request2.FileName.IsNullOrEmpty())
                {
                    request2.FileName = formData[nameof(request2.FileName)];
                }
                if (request2.CharSet.IsNullOrEmpty())
                {
                    request2.CharSet = formData[nameof(request2.CharSet)];
                }
                if (request2.Md5.IsNullOrEmpty())
                {
                    formDataValue = formData[nameof(request2.Md5)];
                    if (formDataValue != null) request2.Md5 = formDataValue.Split(',');
                }
                if (request2.Guid.IsNullOrEmpty())
                {
                    request2.Guid = formData[nameof(request2.Guid)];
                }
                if (request2.Chunk < 0)
                {
                    formDataValue = formData[nameof(request2.Chunk)];
                    if (formDataValue != null) request2.Chunk = formDataValue.ToInt32();
                }
                if (request2.Chunks < 0)
                {
                    formDataValue = formData[nameof(request2.Chunks)];
                    if (formDataValue != null) request2.Chunks = formDataValue.ToInt32();
                }
            }
            return request2;
        }
        /// <summary>载入上传的第一个文件流</summary>
        protected async Task<UploadFileRequest> LoadFileStreamFirstOrDefault(string uploadPath)
        {
            var request = new UploadFileRequest();
            var httpContent = this.Request.Content;
            if (httpContent.IsMimeMultipartContent())
            {
                var provider = await httpContent.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(uploadPath));
                var fileData = provider.FileData.First();
                request.FileName = fileData.Headers.ContentDisposition.FileName.Divest();
                request.ServerFile = fileData.LocalFileName;
                var formData = provider.FormData;
                if (request.FileName.IsNullOrEmpty())
                {
                    request.FileName = formData[nameof(request.FileName)];
                }
                var formDataValue = formData[nameof(request.EnablePattern)];
                if (formDataValue != null) request.EnablePattern = bool.Parse(formDataValue);
            }
            return request;
        }
        /// <summary>生成下载文件流</summary>
        protected IHttpActionResult Download(DownloadRequest request, DownloadResult result)
        {
            var response = new ApiStreamResult(result.ContentType, result.Content, this);
            if (request.Download) response.FileName = request.FileName.IsNullOrEmpty() ? result.FileName : request.FileName;
            response.ResponseHandler = responseMessage => responseMessage.Headers.UseMaxAgeCacheControl();
            return response;
        }
        #endregion

        #region 服务能力方法
        /// <summary>获得服务信息</summary>
        protected virtual ServiceInfo GetServiceInfo()
        {
            throw new NotImplementedException();
        }
        /// <summary>获得服务能力数据</summary>
        protected virtual Task<ServiceCapabilities> GetServiceCapabilities()
        {
            return Task.FromResult(new ServiceCapabilities());
        }
        /// <summary>获得服务支持能力数据</summary>
        /// <returns>服务支持能力数据</returns>
        [HttpGet]
        public async Task<ServiceCapabilities> GetCapabilities()
        {
            var result = await GetServiceCapabilities();
            result.Service = GetServiceInfo();
            if (result.Service.Name.IsNullOrEmpty())
            {
                result.Service.Name = this.ControllerContext.ControllerDescriptor.ControllerName;
            }
            if (result.Service.Developer.IsNullOrEmpty())
            {
                result.Service.Developer = "武汉侏罗纪知识管理业务部";
            }
            var requests = new List<MethodInfo>();
            var controllerType = this.GetType();
            var bflags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
            while (true)
            {
                requests.AddRange(controllerType.GetMethods(bflags));
                if (controllerType == typeof(PKSApiController)) break;
                controllerType = controllerType.BaseType;
            }
            result.Requests = requests.Select(e => e.Name).ToArray();
            return result;
        }
        #endregion
    }
}