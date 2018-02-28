using System.Collections.Generic;
using System.Threading.Tasks;
using PKS.Models;
using PKS.WebAPI.Models;
using TDocument = PKS.WebAPI.Models.IndexPageData;

namespace PKS.WebAPI.Services
{
    /// <summary>服务端页面数据服务接口</summary>
    public interface IServerPageDataService : IPageDataService
    {
        /// <summary>上传文件，支持秒传和分片</summary>
        /// <remarks>
        /// 如果Guid为空表示不分片，否则表示分片
        /// </remarks>
        UploadResult Upload(ServerUploadRequest request);

        /// <summary>上传文件，支持秒传和分片</summary>
        Task<UploadResult> UploadAsync(ServerUploadRequest request);
    }

    public interface IPageDataServiceWrapper : IPageDataService, IApiServiceWrapper
    {
        /// <summary>上传文件，支持秒传和分片</summary>
        /// <remarks>
        /// 如果Guid为空表示不分片，否则表示分片
        /// </remarks>
        UploadResult Upload(UploadRequest request);

        /// <summary>上传文件，支持秒传和分片</summary>
        Task<UploadResult> UploadAsync(UploadRequest request);
    }

    /// <summary>页面数据服务接口</summary>
    public interface IPageDataService
    {
        /// <summary>批量插入</summary>
        string[] InsertMany(IndexDataSaveRequest<TDocument> request);

        /// <summary>批量插入</summary>
        Task<string[]> InsertManyAsync(IndexDataSaveRequest<TDocument> request);
        /// <summary>批量保存</summary>
        string[] SaveMany(IndexDataSaveRequest<TDocument> request);

        /// <summary>批量保存</summary>
        Task<string[]> SaveManyAsync(IndexDataSaveRequest<TDocument> request);
        /// <summary>批量删除</summary>
        string[] DeleteMany(List<string> pageIds);

        /// <summary>批量删除</summary>
        Task<string[]> DeleteManyAsync(List<string> pageIds);
        /// <summary>根据PageID获得一条应用数据</summary>
        TDocument Get(string pageId);

        /// <summary>根据PageID获得一条应用数据</summary>
        Task<TDocument> GetAsync(string pageId);
        /// <summary>根据PageID数组获得对应的多条应用数据</summary>
        Dictionary<string, TDocument> GetMany(List<string> pageIds);

        /// <summary>根据PageID数组获得对应的多条应用数据</summary>
        Task<Dictionary<string, TDocument>> GetManyAsync(List<string> pageIds);
        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        IndexDataMatchResult<TDocument> Match(IndexDataMatchRequest request);

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        Task<IndexDataMatchResult<TDocument>> MatchAsync(IndexDataMatchRequest request);
        /// <summary>根据DataID或ContentRef获得相关文件流</summary>
        DownloadResult Download(DownloadRequest request);

        /// <summary>根据PageID或FileID获得相关文件流</summary>
        Task<DownloadResult> DownloadAsync(DownloadRequest request);
    }
}