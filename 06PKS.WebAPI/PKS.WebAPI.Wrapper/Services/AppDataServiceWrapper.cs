using PKS.Core;
using PKS.Models;
using PKS.Services;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TDataSave = PKS.WebAPI.Models.AppDataSaveRequest;
using TDocument = PKS.WebAPI.Models.IndexAppData;

namespace PKS.WebAPI.Services
{
    /// <summary>应用数据服务包装器</summary>
    public class AppDataServiceWrapper : ApiServiceWrapper, IAppDataServiceWrapper, IFileFormatService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public AppDataServiceWrapper(string serviceUrl) : base(serviceUrl)
        {
        }
        /// <summary>构造函数</summary>
        public AppDataServiceWrapper(IApiServiceConfig config) : base(config, nameof(IAppDataService).Substring(1))
        {
        }
        /// <summary>上传文件，支持秒传和分片</summary>
        public UploadResult Upload(UploadRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => UploadAsyncInternal(client, request)).Result;
        }
        /// <summary>上传文件，支持秒传和分片</summary>
        public async Task<UploadResult> UploadAsync(UploadRequest request)
        {
            var client = InitHttpClient();
            return await UploadAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>上传文件，支持秒传和分片</summary>
        private async Task<UploadResult> UploadAsyncInternal(HttpClientWrapper client, UploadRequest request)
        {
            return await client.PostObjectAsync<UploadResult>(GetActionUrl(nameof(Upload)), request).ConfigureAwait(false);
        }
        /// <summary>上传一个文件，支持秒传和分片(单位为K)</summary>
        public string Upload(string file, string charSet, int chunkSize)
        {
            var client = InitHttpClient();
            return Task.Run(() => UploadAsyncInternal(client, file, charSet, chunkSize)).Result;
        }

        /// <summary>上传一个文件，支持秒传和分片(单位为K)</summary>
        public async Task<string> UploadAsync(string file, string charSet, int chunkSize)
        {
            var client = InitHttpClient();
            return await UploadAsyncInternal(client, file, charSet, chunkSize).ConfigureAwait(false);
        }
        /// <summary>上传一个文件，支持秒传和分片(单位为K)</summary>
        private async Task<string> UploadAsyncInternal(HttpClientWrapper client, string file, string charSet, int chunkSize)
        {
            using (var stream = File.OpenRead(file))
            {
                var request = new UploadRequest();
                var md5 = stream.ToByteArray().ToMD5();
                request.Md5 = new string[] { md5 };
                request.CharSet = charSet;
                var fileName = Path.GetFileName(file);
                request.FileName = fileName;
                stream.Position = 0;
                var result = await UploadAsync(request);
                var fileId = result.FirstFileId;
                if (!fileId.IsNullOrEmpty()) return fileId;
                var url = GetActionUrl(nameof(Upload));
                var chunkByteSize = chunkSize * 1024;
                if (stream.Length <= chunkByteSize)
                {
                    var url2 = url + nameof(request.Md5).GetFirstQueryString(md5);
                    result = await client.UploadAsync<UploadResult>(url2, fileName, stream, charSet);
                    return result.FirstFileId;
                }
                var remainder = 0;
                var chunks = Math.DivRem((int)stream.Length, chunkByteSize, out remainder);
                if (remainder > 0) chunks++;
                var guid = Guid.NewGuid().ToString();
                var url3 = url + nameof(request.Guid).GetFirstQueryString(guid);
                url3 += nameof(request.Chunks).GetNextQueryString(chunks.ToString());
                for (var i = 0; i < chunks; i++)
                {
                    var buffer = new byte[chunkByteSize];
                    var readCount = await stream.ReadAsync(buffer, 0, chunkByteSize);
                    using (var memoryStream = new MemoryStream(buffer, 0, readCount))
                    {
                        var url4 = url3 + nameof(request.Chunk).GetNextQueryString(i.ToString());
                        await client.UploadAsync<UploadResult>(url4, fileName, memoryStream);
                    }
                }
                request.Guid = guid;
                request.Chunks = chunks;
                request.Chunk = chunks;
                result = await UploadAsync(request);
                return result.FirstFileId;
            }
        }
        /// <summary>批量插入</summary>
        public AppDataSaveResult[] InsertMany(IndexDataSaveRequest<TDataSave> request)
        {
            var client = InitHttpClient();
            return Task.Run(() => InsertManyAsyncInternal(client, request)).Result;
        }

        /// <summary>批量插入</summary>
        public async Task<AppDataSaveResult[]> InsertManyAsync(IndexDataSaveRequest<TDataSave> request)
        {
            var client = InitHttpClient();
            return await InsertManyAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>批量插入</summary>
        private async Task<AppDataSaveResult[]> InsertManyAsyncInternal(HttpClientWrapper client, IndexDataSaveRequest<TDataSave> request)
        {
            return await client.PostObjectAsync<AppDataSaveResult[]>(GetActionUrl(nameof(InsertMany)), request).ConfigureAwait(false);
        }

        /// <summary>保存</summary>
        public AppDataSaveResult Save(AppDataSaveRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => SaveAsyncInternal(client, request)).Result;
        }

        /// <summary>保存</summary>
        public async Task<AppDataSaveResult> SaveAsync(AppDataSaveRequest request)
        {
            var client = InitHttpClient();
            return await SaveAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>保存</summary>
        private async Task<AppDataSaveResult> SaveAsyncInternal(HttpClientWrapper client, AppDataSaveRequest request)
        {
            return await client.PostObjectAsync<AppDataSaveResult>(GetActionUrl(nameof(Save)), request).ConfigureAwait(false);
        }
        /// <summary>批量保存</summary>
        public AppDataSaveResult[] SaveMany(IndexDataSaveRequest<TDataSave> request)
        {
            var client = InitHttpClient();
            return Task.Run(() => SaveManyAsyncInternal(client, request)).Result;
        }

        /// <summary>批量保存</summary>
        public async Task<AppDataSaveResult[]> SaveManyAsync(IndexDataSaveRequest<TDataSave> request)
        {
            var client = InitHttpClient();
            return await SaveManyAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>批量保存</summary>
        private async Task<AppDataSaveResult[]> SaveManyAsyncInternal(HttpClientWrapper client, IndexDataSaveRequest<TDataSave> request)
        {
            return await client.PostObjectAsync<AppDataSaveResult[]>(GetActionUrl(nameof(SaveMany)), request).ConfigureAwait(false);
        }

        /// <summary>批量删除</summary>
        public string[] DeleteMany(List<string> dataIds)
        {
            var client = InitHttpClient();
            return Task.Run(() => DeleteManyAsyncInternal(client, dataIds)).Result;
        }

        /// <summary>批量删除</summary>
        public async Task<string[]> DeleteManyAsync(List<string> dataIds)
        {
            var client = InitHttpClient();
            return await DeleteManyAsyncInternal(client, dataIds).ConfigureAwait(false);
        }

        /// <summary>批量删除</summary>
        private async Task<string[]> DeleteManyAsyncInternal(HttpClientWrapper client, List<string> dataIds)
        {
            return await client.PostObjectAsync<string[]>(GetActionUrl(nameof(DeleteMany)), dataIds).ConfigureAwait(false);
        }
        /// <summary>根据DataID获得一条应用数据</summary>
        public TDocument Get(string dataId)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetAsyncInternal(client, dataId)).Result;
        }

        /// <summary>根据DataID获得一条应用数据</summary>
        public async Task<TDocument> GetAsync(string dataId)
        {
            var client = InitHttpClient();
            return await GetAsyncInternal(client, dataId).ConfigureAwait(false);
        }
        /// <summary>根据DataID获得一条应用数据</summary>
        private async Task<TDocument> GetAsyncInternal(HttpClientWrapper client, string dataId)
        {
            var queryString = $"?{nameof(dataId)}={dataId}";
            return await client.GetAsync<TDocument>(GetActionUrl(nameof(Get)) + queryString).ConfigureAwait(false);
        }

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        public Dictionary<string, TDocument> GetMany(List<string> dataIds)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetManyAsyncInternal(client, dataIds)).Result;
        }

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        public async Task<Dictionary<string, TDocument>> GetManyAsync(List<string> dataIds)
        {
            var client = InitHttpClient();
            return await GetManyAsyncInternal(client, dataIds).ConfigureAwait(false);
        }
        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        private async Task<Dictionary<string, TDocument>> GetManyAsyncInternal(HttpClientWrapper client, List<string> dataIds)
        {
            return await client.PostObjectAsync<Dictionary<string, TDocument>>(GetActionUrl(nameof(GetMany)), dataIds).ConfigureAwait(false);
        }

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        public IndexDataMatchResult<TDocument> Match(IndexDataMatchRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => MatchAsyncInternal(client, request)).Result;
        }

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        public async Task<IndexDataMatchResult<TDocument>> MatchAsync(IndexDataMatchRequest request)
        {
            var client = InitHttpClient();
            return await MatchAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        private async Task<IndexDataMatchResult<TDocument>> MatchAsyncInternal(HttpClientWrapper client, IndexDataMatchRequest request)
        {
            return await client.PostObjectAsync<IndexDataMatchResult<TDocument>>(GetActionUrl(nameof(Match)), request).ConfigureAwait(false);
        }
        /// <summary>获得上传文件夹下全部子文件夹</summary>
        public List<UploadFolder> GetUploadFolders()
        {
            var client = InitHttpClient();
            return Task.Run(() => GetUploadFoldersAsyncInternal(client)).Result;
        }

        /// <summary>获得上传文件夹下全部子文件夹</summary>
        public async Task<List<UploadFolder>> GetUploadFoldersAsync()
        {
            var client = InitHttpClient();
            return await GetUploadFoldersAsyncInternal(client).ConfigureAwait(false);
        }
        /// <summary>获得上传文件夹下全部子文件夹</summary>
        private async Task<List<UploadFolder>> GetUploadFoldersAsyncInternal(HttpClientWrapper client)
        {
            return await client.GetAsync<List<UploadFolder>>(GetActionUrl(nameof(GetUploadFolders))).ConfigureAwait(false);
        }
        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        public List<string> GetUploadFolderFiles(string fullName)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetUploadFolderFilesAsyncInternal(client, fullName)).Result;
        }

        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        public async Task<List<string>> GetUploadFolderFilesAsync(string fullName)
        {
            var client = InitHttpClient();
            return await GetUploadFolderFilesAsyncInternal(client, fullName).ConfigureAwait(false);
        }
        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        private async Task<List<string>> GetUploadFolderFilesAsyncInternal(HttpClientWrapper client, string fullName)
        {
            fullName = HttpUtility.UrlEncode(fullName);
            var queryString = $"?{nameof(fullName)}={fullName}";
            return await client.GetAsync<List<string>>(GetActionUrl(nameof(GetUploadFolderFiles)) + queryString).ConfigureAwait(false);
        }
        /// <summary>清除临时文件夹中的过期文件</summary>
        public void ClearTempFiles()
        {
            var client = InitHttpClient();
            Task.Run(() => ClearTempFilesAsyncInternal(client));
        }

        /// <summary>清除临时文件夹中的过期文件</summary>
        public async Task ClearTempFilesAsync()
        {
            var client = InitHttpClient();
            await ClearTempFilesAsyncInternal(client).ConfigureAwait(false);
        }
        /// <summary>清除临时文件夹中的过期文件</summary>
        private async Task ClearTempFilesAsyncInternal(HttpClientWrapper client)
        {
            await client.GetAsync(GetActionUrl(nameof(ClearTempFiles))).ConfigureAwait(false);
        }
        /// <summary>根据DataID获得一条应用数据</summary>
        public DownloadResult Download(DownloadRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => DownloadAsyncInternal(client, request)).Result;
        }

        /// <summary>根据DataID获得相关文件流</summary>
        public async Task<DownloadResult> DownloadAsync(DownloadRequest request)
        {
            var client = InitHttpClient();
            return await DownloadAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据DataID或FileID获得相关文件流</summary>
        private async Task<DownloadResult> DownloadAsyncInternal(HttpClientWrapper client, DownloadRequest request)
        {
            var result = new DownloadResult();
            var response = await client.PostObjectAsync<HttpResponseMessage>(GetActionUrl(nameof(Download)), request).ConfigureAwait(false);
            result.Content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            result.ContentType = response.Content.Headers.ContentType.ToString();
            result.FileName = response.Content.Headers.ContentDisposition?.FileName ?? request.FileName;
            return result;
        }

        /// <summary>获得全部文件格式信息</summary>
        public List<FileFormat> GetFileFormats()
        {
            var client = InitHttpClient();
            return Task.Run(() => GetFileFormatsAsyncInternal(client)).Result;
        }

        /// <summary>获得全部文件格式信息</summary>
        public async Task<List<FileFormat>> GetFileFormatsAsync()
        {
            var client = InitHttpClient();
            return await GetFileFormatsAsyncInternal(client).ConfigureAwait(false);
        }
        /// <summary>获得全部文件格式信息</summary>
        private async Task<List<FileFormat>> GetFileFormatsAsyncInternal(HttpClientWrapper client)
        {
            return await client.GetAsync<List<FileFormat>>(GetActionUrl(nameof(GetFileFormats))).ConfigureAwait(false);
        }
        /// <summary>获得全部文件格式信息</summary>
        List<FileFormat> IFileFormatService.GetAll()
        {
            return GetFileFormats();
        }
    }
}