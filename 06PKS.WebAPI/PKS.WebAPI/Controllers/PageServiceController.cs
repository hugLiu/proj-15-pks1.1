using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using TDocument = PKS.WebAPI.Models.IndexPageData;

namespace PKS.WebAPI.Controllers
{
    /// <summary>页面数据服务控制器</summary>
    public class PageServiceController : PKSApiController
    {
        /// <summary>构造函数</summary>
        public PageServiceController(IServerPageDataService service, IMongoConfig config)
        {
            ServiceImpl = service;
            UploadTempPath = config.IndexUploadTempPath;
        }

        /// <summary>服务实例</summary>
        private IServerPageDataService ServiceImpl { get; }

        /// <summary>上传临时路径</summary>
        private string UploadTempPath { get; }

        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "页面数据服务用于页面数据库的增删改查"
            };
        }

        /// <summary>上传文件，支持秒传和分片</summary>
        [HttpPost]
        public async Task<UploadResult> Upload([FromUri]UploadRequest request)
        {
            var request2 = await LoadFileStreamFirstOrDefault(request, UploadTempPath);
            return await ServiceImpl.UploadAsync(request2);
        }

        /// <summary>批量插入</summary>
        [HttpPost]
        public async Task<string[]> InsertMany(IndexDataSaveRequest<TDocument> request)
        {
            return await ServiceImpl.InsertManyAsync(request);
        }

        /// <summary>批量保存</summary>
        [HttpPost]
        public async Task<string[]> SaveMany(IndexDataSaveRequest<TDocument> request)
        {
            return await ServiceImpl.SaveManyAsync(request);
        }

        /// <summary>批量删除</summary>
        [HttpPost]
        public async Task<string[]> DeleteMany(List<string> pageIds)
        {
            return await ServiceImpl.DeleteManyAsync(pageIds);
        }

        /// <summary>根据DataID获得一条应用数据</summary>
        [HttpGet]
        public async Task<TDocument> Get(string pageId)
        {
            return await ServiceImpl.GetAsync(pageId);
        }

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        [HttpPost]
        public async Task<Dictionary<string, TDocument>> GetMany(List<string> pageIds)
        {
            return await ServiceImpl.GetManyAsync(pageIds);
        }

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        [HttpPost]
        public async Task<IndexDataMatchResult<TDocument>> Match(IndexDataMatchRequest request)
        {
            return await ServiceImpl.MatchAsync(request);
        }
        /// <summary>根据DataID或FileID获得相关文件流</summary>
        [HttpGet]
        public async Task<IHttpActionResult> Download([FromUri] DownloadRequest request)
        {
            if (!request.ContentRef.IsNullOrEmpty()) request.ContentRef = HttpUtility.UrlDecode(request.ContentRef);
            var result = await ServiceImpl.DownloadAsync(request);
            return Download(request, result);
        }
    }
}