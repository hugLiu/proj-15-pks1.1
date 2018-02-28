using System.Collections.Generic;
using System.Threading.Tasks;
using PKS.Models;
using PKS.WebAPI.Models;
using TDocument = PKS.WebAPI.Models.IndexAppData;
using TDataSave = PKS.WebAPI.Models.AppDataSaveRequest;

namespace PKS.WebAPI.Services
{
    /// <summary>服务端应用数据服务接口</summary>
    public interface IServerAppDataService : IAppDataService
    {
        /// <summary>上传文件，支持秒传和分片</summary>
        /// <remarks>
        /// 如果Guid为空表示不分片，否则表示分片
        /// </remarks>
        UploadResult Upload(ServerUploadRequest request);

        /// <summary>上传文件，支持秒传和分片</summary>
        Task<UploadResult> UploadAsync(ServerUploadRequest request);
        /// <summary>上传文件，小资源文件直接上传内容</summary>
        Task<UploadFileResult> UploadFileAsync(UploadFileRequest request);
        /// <summary>生成缩略图</summary>
        Task BuildThumbnail(Metadata metadata, string thumbnail);
        /// <summary>生成全文</summary>
        Task BuildFulltext(Metadata metadata, string fulltext);
    }

    /// <summary>应用数据包装器接口</summary>
    public interface IAppDataServiceWrapper : IAppDataService, IApiServiceWrapper
    {
        /// <summary>上传文件，支持秒传和分片</summary>
        /// <remarks>
        /// 如果Guid为空表示不分片，否则表示分片
        /// </remarks>
        UploadResult Upload(UploadRequest request);

        /// <summary>上传文件，支持秒传和分片</summary>
        Task<UploadResult> UploadAsync(UploadRequest request);
        /// <summary>上传一个文件，支持秒传和分片(单位为K)</summary>
        string Upload(string file, string charSet, int chunkSize);
        /// <summary>上传一个文件，支持秒传和分片(单位为K)</summary>
        Task<string> UploadAsync(string file, string charSet, int chunkSize);
        /// <summary>获得全部文件格式信息</summary>
        List<FileFormat> GetFileFormats();

        /// <summary>获得全部文件格式信息</summary>
        Task<List<FileFormat>> GetFileFormatsAsync();
    }

    /// <summary>应用数据服务接口</summary>
    public interface IAppDataService
    {
        /// <summary>批量插入</summary>
        AppDataSaveResult[] InsertMany(IndexDataSaveRequest<TDataSave> request);

        /// <summary>批量插入</summary>
        Task<AppDataSaveResult[]> InsertManyAsync(IndexDataSaveRequest<TDataSave> request);
        /// <summary>保存</summary>
        AppDataSaveResult Save(AppDataSaveRequest request);

        /// <summary>保存</summary>
        Task<AppDataSaveResult> SaveAsync(AppDataSaveRequest request);
        /// <summary>批量保存</summary>
        AppDataSaveResult[] SaveMany(IndexDataSaveRequest<TDataSave> request);

        /// <summary>批量保存</summary>
        Task<AppDataSaveResult[]> SaveManyAsync(IndexDataSaveRequest<TDataSave> request);
        /// <summary>批量删除</summary>
        string[] DeleteMany(List<string> dataIds);

        /// <summary>批量删除</summary>
        Task<string[]> DeleteManyAsync(List<string> dataIds);
        /// <summary>根据DataID获得一条应用数据</summary>
        IndexAppData Get(string dataId);

        /// <summary>根据DataID获得一条应用数据</summary>
        Task<IndexAppData> GetAsync(string dataId);
        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        Dictionary<string, TDocument> GetMany(List<string> dataIds);

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        Task<Dictionary<string, TDocument>> GetManyAsync(List<string> dataIds);
        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        IndexDataMatchResult<TDocument> Match(IndexDataMatchRequest request);

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        Task<IndexDataMatchResult<TDocument>> MatchAsync(IndexDataMatchRequest request);
        /// <summary>获得上传文件夹下全部子文件夹</summary>
        List<UploadFolder> GetUploadFolders();

        /// <summary>获得上传文件夹下全部子文件夹</summary>
        Task<List<UploadFolder>> GetUploadFoldersAsync();
        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        List<string> GetUploadFolderFiles(string fullName);

        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        Task<List<string>> GetUploadFolderFilesAsync(string fullName);
        /// <summary>清除临时文件夹中的过期文件</summary>
        void ClearTempFiles();

        /// <summary>清除临时文件夹中的过期文件</summary>
        Task ClearTempFilesAsync();
        /// <summary>根据DataID或ContentRef获得相关文件流</summary>
        DownloadResult Download(DownloadRequest request);

        /// <summary>根据DataID或FileID获得相关文件流</summary>
        Task<DownloadResult> DownloadAsync(DownloadRequest request);
    }
}