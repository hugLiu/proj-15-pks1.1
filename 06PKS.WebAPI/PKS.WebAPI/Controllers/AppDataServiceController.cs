using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using TDocument = PKS.WebAPI.Models.IndexAppData;
using TDataSave = PKS.WebAPI.Models.AppDataSaveRequest;
using PKS.Services;
using PKS.MgmtServices.Converters;
using PKS.Web;
using System.Net.Http.Formatting;
using PKS.Core;

namespace PKS.WebAPI.Controllers
{
    /// <summary>应用数据服务控制器</summary>
    public class AppDataServiceController : PKSApiController
    {
        /// <summary>构造函数</summary>
        public AppDataServiceController(IServerAppDataService service, IMongoConfig config)
        {
            ServiceImpl = service;
            UploadTempPath = config.IndexUploadTempPath;
        }

        /// <summary>服务实例</summary>
        private IServerAppDataService ServiceImpl { get; }

        /// <summary>上传临时路径</summary>
        private string UploadTempPath { get; }

        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "应用数据服务用于应用数据库的增删改查"
            };
        }

        /// <summary>上传文件，支持秒传和分片</summary>
        [HttpPost]
        public async Task<UploadResult> Upload([FromUri]UploadRequest request)
        {
            var request2 = await LoadFileStreamFirstOrDefault(request, UploadTempPath);
            return await ServiceImpl.UploadAsync(request2);
        }

        /// <summary>上传文件，小资源文件直接上传内容</summary>
        [HttpPost]
        public async Task<UploadFileResult> UploadFile()
        {
            var request = await LoadFileStreamFirstOrDefault(UploadTempPath);
            var result = await ServiceImpl.UploadFileAsync(request);
            result.RelativePath = result.RelativePath.Replace('\\', '/');
            result.FileUrl = this.Request.RequestUri.GetDomainUrl().TrimEnd('/') + result.RelativePath;
            if (request.EnablePattern)
            {
                var pattern = BracketPatternUtil.System.BuildBracketPattern();
                result.PatternUrl = pattern + result.RelativePath;
            }
            return result;
        }

        /// <summary>批量插入</summary>
        [HttpPost]
        public async Task<AppDataSaveResult[]> InsertMany(IndexDataSaveRequest<TDataSave> request)
        {
            request.Values.ForEach(Validate);
            return await ServiceImpl.InsertManyAsync(request);
        }

        /// <summary>保存</summary>
        [HttpPost]
        public async Task<AppDataSaveResult> Save(TDataSave request)
        {
            Validate(request);
            return await ServiceImpl.SaveAsync(request);
        }
        /// <summary>批量保存</summary>
        [HttpPost]
        public async Task<AppDataSaveResult[]> SaveMany(IndexDataSaveRequest<TDataSave> request)
        {
            request.Values.ForEach(Validate);
            return await ServiceImpl.SaveManyAsync(request);
        }

        /// <summary>验证</summary>
        private void Validate(TDataSave request)
        {
            if (request.Uploader.IsNullOrEmpty()) request.Uploader = this.PKSUser.Identity.Name;
        }
        /// <summary>批量删除</summary>
        [HttpPost]
        public async Task<string[]> DeleteMany(List<string> dataIds)
        {
            return await ServiceImpl.DeleteManyAsync(dataIds);
        }

        /// <summary>根据DataID获得一条应用数据</summary>
        [HttpGet]
        public async Task<TDocument> Get(string dataId)
        {
            return await ServiceImpl.GetAsync(dataId);
        }

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        [HttpPost]
        public async Task<Dictionary<string, TDocument>> GetMany(List<string> dataIds)
        {
            return await ServiceImpl.GetManyAsync(dataIds);
        }
        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        [HttpPost]
        public async Task<IndexDataMatchResult<TDocument>> Match(IndexDataMatchRequest request)
        {
            return await ServiceImpl.MatchAsync(request);
        }
        /// <summary>根据DataID或FileID获得相关文件流</summary>
        [HttpGet]
        [HttpPost]
        public async Task<IHttpActionResult> Download([FromUri] DownloadRequest urlRequest, DownloadRequest bodyRequest)
        {
            DownloadRequest request = null;
            if (urlRequest != null && (!urlRequest.DataId.IsNullOrEmpty() || !urlRequest.ContentRef.IsNullOrEmpty()))
            {
                request = urlRequest;
            }
            else if (bodyRequest != null && (!bodyRequest.DataId.IsNullOrEmpty() || !bodyRequest.ContentRef.IsNullOrEmpty()))
            {
                request = bodyRequest;
            }
            if (request == null)
            {
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly("下载失败！", $"缺少参数DataId或ContentRef！");
            }
            if (!request.ContentRef.IsNullOrEmpty())
            {
                request.ContentRef = request.ContentRef.UrlDecode();
            }
            var result = await ServiceImpl.DownloadAsync(request);
            return Download(request, result);
        }
        /// <summary>获得上传文件夹下全部子文件夹</summary>
        [HttpGet]
        public async Task<List<UploadFolder>> GetUploadFolders()
        {
            return await ServiceImpl.GetUploadFoldersAsync();
        }
        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        [HttpGet]
        public async Task<List<string>> GetUploadFolderFiles(string fullName)
        {
            fullName = fullName.UrlDecode();
            return await ServiceImpl.GetUploadFolderFilesAsync(fullName);
        }
        /// <summary>清除临时文件夹中的过期文件</summary>
        [HttpGet]
        public async Task<IHttpActionResult> ClearTempFiles()
        {
            await ServiceImpl.ClearTempFilesAsync();
            return Ok();
        }
        /// <summary>重载文件格式</summary>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ReloadFileFormats()
        {
            FileFormatExtension.Reload();
            return Ok();
        }
        /// <summary>获得全部文件格式信息</summary>
        [HttpGet]
        [AllowAnonymous]
        public List<FileFormat> GetFileFormats()
        {
            return GetService<IFileFormatService>().GetAll();
        }
        /// <summary>转换</summary>
        [HttpGet]
        public async Task<IHttpActionResult> Convert(FileConvertType type, string sourceFile, string destFile)
        {
            sourceFile = sourceFile.UrlDecode();
            destFile = destFile.UrlDecode();
            switch (type)
            {
                case FileConvertType.Pdf:
                    await GetService<IPdfConverter>().ExecuteAsync(sourceFile, destFile);
                    break;
                case FileConvertType.Image:
                    await GetService<IImageConverter>().ExecuteAsync(sourceFile, destFile);
                    break;
                case FileConvertType.Thumbnail:
                    await GetService<IThumbnailConverter>().ExecuteAsync(sourceFile, destFile);
                    break;
                case FileConvertType.FullText:
                    await GetService<IFulltextConverter>().ExecuteAsync(sourceFile, destFile);
                    break;
                case FileConvertType.Html:
                    await GetService<IHtmlConverter>().ExecuteAsync(sourceFile, destFile);
                    break;
            }
            return Ok();
        }
    }
}