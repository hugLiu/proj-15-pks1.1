using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Ninject;
using PKS.Core;
using PKS.MgmtServices.Converters;
using PKS.Models;
using PKS.Services;
using PKS.Utils;
using PKS.WebAPI.Models;
using TDataSave = PKS.WebAPI.Models.AppDataSaveRequest;
using TDocument = PKS.WebAPI.Models.IndexAppData;

namespace PKS.WebAPI.Services
{
    /// <summary>应用数据服务</summary>
    public class ServerAppDataService : AppService, IInitializable, IServerAppDataService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public ServerAppDataService(IMongoConfig config, IMongoCollection<TDocument> accessor, IMongoCollection<MongoUploadFile> uploadFilesCollection)
        {
            Accessor = accessor;
            FileFormatExtension.Init();
            this.ExtConverterMappers = new Dictionary<string, FileFormatConverters>();
            UploadFileHandler = new UploadFileHandler(config, uploadFilesCollection);
        }

        /// <summary>访问器</summary>
        private IMongoCollection<TDocument> Accessor { get; }

        /// <summary>上传文件处理器</summary>
        private UploadFileHandler UploadFileHandler { get; }

        /// <summary>文件转换器扩展名映射</summary>
        private Dictionary<string, FileFormatConverters> ExtConverterMappers { get; }

        /// <summary>初始化</summary>
        public void Initialize()
        {
            var fileFormatConverters = new FileFormatConverters();
            fileFormatConverters.PdfConverter = GetService<IPdfConverter>();
            fileFormatConverters.ImageConverter = GetService<IImageConverter>();
            fileFormatConverters.ThumbnailConverter = GetService<IThumbnailConverter>();
            fileFormatConverters.FulltextConverter = GetService<IFulltextConverter>();
            fileFormatConverters.HtmlConverter = GetService<IHtmlConverter>();
            foreach (var fileFormat in FileFormatExtension.Values)
            {
                var self = new FileFormatConverters();
                if (fileFormat.GeneratePdf) self.PdfConverter = fileFormatConverters.PdfConverter;
                if (fileFormat.GenerateImage) self.ImageConverter = fileFormatConverters.ImageConverter;
                if (fileFormat.GenerateThumbnail) self.ThumbnailConverter = fileFormatConverters.ThumbnailConverter;
                if (fileFormat.GenerateFulltext) self.FulltextConverter = fileFormatConverters.FulltextConverter;
                if (fileFormat.GenerateHtml) self.HtmlConverter = fileFormatConverters.HtmlConverter;
                foreach (var ext in fileFormat.Ext)
                {
                    this.ExtConverterMappers[ext] = self;
                }
            }
        }
        /// <summary>获得文件格式转换器</summary>
        private FileFormatConverters GetFileFormatConverters(string ext)
        {
            ext = ext.TrimStart('.');
            var result = ExtConverterMappers.GetValueBy(ext);
            if (result == null) result = ExtConverterMappers.GetValueBy(string.Empty);
            return result;
        }
        /// <summary>上传文件，支持秒传和分片</summary>
        public UploadResult Upload(ServerUploadRequest request)
        {
            return Task.Run(() => UploadAsync(request)).Result;
        }

        /// <summary>上传文件，支持秒传和分片</summary>
        public async Task<UploadResult> UploadAsync(ServerUploadRequest request)
        {
            return await UploadFileHandler.UploadAsync(request);
        }

        /// <summary>上传文件，小资源文件直接上传内容</summary>
        public async Task<UploadFileResult> UploadFileAsync(UploadFileRequest request)
        {
            return await UploadFileHandler.UploadFileAsync(request);
        }
        /// <summary>批量插入</summary>
        public AppDataSaveResult[] InsertMany(IndexDataSaveRequest<TDataSave> request)
        {
            return Task.Run(() => InsertManyAsync(request)).Result;
        }

        /// <summary>批量插入</summary>
        public async Task<AppDataSaveResult[]> InsertManyAsync(IndexDataSaveRequest<TDataSave> request)
        {
            var pairs = new List<PKSKeyValuePair<TDocument, AppDataSaveResult>>();
            foreach (var value in request.Values)
            {
                var pair = await Validate(value, false);
                pairs.Add(pair);
            }
            var options = new InsertManyOptions();
            await Accessor.InsertManyAsync(pairs.Select(e => e.Key));
            return pairs.Select(e => e.Value).ToArray();
        }

        /// <summary>保存</summary>
        public AppDataSaveResult Save(AppDataSaveRequest request)
        {
            return Task.Run(() => SaveAsync(request)).Result;
        }

        /// <summary>保存</summary>
        public async Task<AppDataSaveResult> SaveAsync(AppDataSaveRequest request)
        {
            var pair = await Validate(request, true);
            var doc = pair.Key;
            var filter = Builders<TDocument>.Filter.Eq(e => e.Id, doc.Id);
            var options = new UpdateOptions { IsUpsert = true };
            await Accessor.ReplaceOneAsync(filter, doc, options);
            return pair.Value;
        }
        /// <summary>验证</summary>
        private async Task<PKSKeyValuePair<TDocument, AppDataSaveResult>> Validate(AppDataSaveRequest request, bool replace)
        {
            #region 检查键
            var doc = new TDocument();
            IndexAppData oldDoc = null;
            if (request.DataId.IsNullOrEmpty())
            {
                if (request.ResourceKey.IsNullOrEmpty())
                {
                    ApiServiceExceptionCodes.ResourceKeyNotExists.ThrowUserFriendly("缺少参数！", $"ResourceKey不存在！");
                }
                request.DataId = request.ResourceKey.ToUpperInvariant().ToMD5();
            }
            else
            {
                oldDoc = await Accessor.AsQueryable().FirstAsync(e => e.Id == request.DataId);
            }
            doc.Id = request.DataId;
            doc.DataId = request.DataId;
            doc.Name = request.Name;
            doc.IsOnline = false; 
            #endregion
            var context = new UploadFileContext();
            if (request.ContentType != IndexAppContentType.File)
            {
                #region 非文件应用数据
                if (request.Content == null)
                {
                    if (request.UploadFileId.IsNullOrEmpty())
                    {
                        ExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺少参数！", $"Content不存在！");
                    }
                    context.UploadFileDoc = await this.UploadFileHandler.GetAsync(request.UploadFileId);
                    doc.SourceName = context.UploadFileDoc.FileName;
                    doc.SourceStorageType = IndexStorageType.Url;
                    doc.SourceContentRef = context.UploadFileDoc.RelativePath.Replace('\\', '/');
                    context.UploadFileFormat = Path.GetExtension(context.UploadFileDoc.FileName).GetFileFormat();
                }
                switch (request.ContentType)
                {
                    case IndexAppContentType.Html:
                        doc.DataType = IndexAppDataType.Html;
                        if (context.UploadFileDoc != null)
                        {
                            if (context.UploadFileFormat.GenerateHtml)
                            {
                                request.Content = await GenerateHtmlAsync(context);
                            }
                            else
                            {
                                request.Content = BuildContent(context);
                            }
                        }
                        break;
                    case IndexAppContentType.Json:
                        doc.DataType = IndexAppDataType.Json;
                        if (context.UploadFileDoc != null)
                        {
                            var content = BuildContent(context);
                            request.Content = content.JsonTo();
                        }
                        break;
                    default:
                        ExceptionCodes.ParameterParsingFailed.ThrowUserFriendly("参数解析失败！", $"ContentType[{request.ContentType.ToString()}]值无效！");
                        break;
                }
                doc.StorageType = IndexStorageType.Content;
                doc.Content = JsonUtil.ToObject(request.Content);
                #endregion
            }
            else
            {
                #region 文件应用数据
                if (request.StorageType == FileStorageType.FileSystem && !request.IsOnline)
                {
                    if (request.SourceFile.IsNullOrEmpty())
                    {
                        CopyFromOld(doc, oldDoc);
                    }
                    else
                    {
                        context.UploadFileDoc = await this.UploadFileHandler.OfflineUploadAsync(request.SourceFile);
                        await BuildUrl(doc, context);
                    }
                }
                else
                {
                    if (request.UploadFileId.IsNullOrEmpty())
                    {
                        CopyFromOld(doc, oldDoc);
                    }
                    else
                    {
                        doc.IsOnline = true;
                        context.UploadFileDoc = await this.UploadFileHandler.GetAsync(request.UploadFileId);
                        if (request.StorageType == FileStorageType.Mongo)
                        {
                            var fileObjectId = await this.UploadFileHandler.InsertMongoAsync(context.UploadFileDoc);
                            doc.SourceStorageType = IndexStorageType.File;
                            doc.SourceContentRef = fileObjectId.ToString();
                            await BuildUploadFile(doc, context);
                        }
                        else
                        {
                            if (!request.SourceFile.IsNullOrEmpty())
                            {
                                await this.UploadFileHandler.OnlineUploadAsync(context.UploadFileDoc, request.SourceFile);
                            }
                            await BuildUrl(doc, context);
                        }
                    }
                }
                #endregion
            }
            #region 检查属性
            if (!replace || oldDoc == null)
            {
                doc.CreateBy = request.Uploader;
                doc.CreateDate = DateTime.UtcNow;
            }
            else
            {
                doc.CreateBy = oldDoc.CreateBy;
                doc.CreateDate = oldDoc.CreateDate;
            }
            doc.LastUpdatedBy = request.Uploader;
            doc.LastUpdatedDate = DateTime.UtcNow;
            doc.RawAdapter = request.RawAdapter;
            doc.System = request.System;
            doc.ResourceType = request.ResourceType;
            doc.ResourceKey = request.ResourceKey;
            var pair = new PKSKeyValuePair<TDocument, AppDataSaveResult>();
            pair.Key = doc;
            var result = new AppDataSaveResult();
            result.DataId = doc.DataId;
            if (request.GenerateThumbnail && context.UploadFileDoc != null && context.UploadFileFormat.GenerateThumbnail)
            {
                result.Thumbnail = await GenerateThumbnailAsync(context);
            }
            if (request.GenerateFulltext && context.UploadFileDoc != null && context.UploadFileFormat.GenerateFulltext)
            {
                result.Fulltext = await GenerateFulltextAsync(context);
            }
            pair.Value = result;
            return pair;
            #endregion
        }
        /// <summary>复制老文档数据</summary>
        private void CopyFromOld(TDocument doc, TDocument oldDoc)
        {
            doc.SourceName = oldDoc.SourceName;
            doc.SourceStorageType = oldDoc.SourceStorageType;
            doc.SourceContentRef = oldDoc.SourceContentRef;
            if (doc.Name.IsNullOrEmpty()) doc.Name = oldDoc.Name;
            doc.DataType = oldDoc.DataType;
            doc.StorageType = oldDoc.StorageType;
            doc.ContentRef = oldDoc.ContentRef;
            doc.IsOnline = oldDoc.IsOnline;
        }
        /// <summary>生成URL方式的数据</summary>
        private async Task BuildUrl(TDocument doc, UploadFileContext context)
        {
            doc.SourceName = context.UploadFileDoc.FileName;
            doc.SourceStorageType = IndexStorageType.Url;
            doc.SourceContentRef = context.UploadFileDoc.RelativePath.Replace('\\', '/');
            await BuildUploadFile(doc, context);
        }
        /// <summary>生成上传文件相关的数据</summary>
        private async Task BuildUploadFile(TDocument doc, UploadFileContext context)
        {
            context.UploadFileFormat = Path.GetExtension(context.UploadFileDoc.FileName).GetFileFormat();
            if (context.UploadFileFormat.GeneratePdf)
            {
                await GeneratePdfAsync(context);
            }
            else if (context.UploadFileFormat.GenerateImage)
            {
                await GenerateImageAsync(context);
            }
            else
            {
                context.NormalizedFileDoc = context.UploadFileDoc;
            }
            if (doc.Name.IsNullOrEmpty()) doc.Name = context.NormalizedFileDoc.FileName;
            if (context.NormalizedFileDoc == context.UploadFileDoc)
            {
                doc.StorageType = doc.SourceStorageType.Value;
                doc.ContentRef = doc.SourceContentRef;
                context.NormalizedFileFormat = context.UploadFileFormat;
            }
            else
            {
                if (doc.SourceStorageType.Value == IndexStorageType.File)
                {
                    var fileObjectId = await this.UploadFileHandler.InsertMongoAsync(context.NormalizedFileDoc);
                    doc.StorageType = IndexStorageType.File;
                    doc.ContentRef = fileObjectId.ToString();
                }
                else
                {
                    doc.StorageType = IndexStorageType.Url;
                    doc.ContentRef = context.NormalizedFileDoc.RelativePath.Replace('\\', '/');
                }
                context.NormalizedFileFormat = Path.GetExtension(context.NormalizedFileDoc.FileName).GetFileFormat();
            }
            doc.DataType = context.NormalizedFileFormat.AppDataType.ToEnum<IndexAppDataType>();
        }
        /// <summary>生成PDF</summary>
        private async Task GeneratePdfAsync(UploadFileContext context)
        {
            if (context.UploadFileDoc.FileId == context.UploadFileDoc.PdfFileId)
            {
                context.NormalizedFileDoc = context.UploadFileDoc;
                return;
            }
            if (!context.UploadFileDoc.PdfFileId.IsNullOrEmpty())
            {
                var newFileId = context.UploadFileDoc.PdfFileId;
                context.NormalizedFileDoc = await this.UploadFileHandler.GetAsync(newFileId);
                return;
            }
            var ext = context.UploadFileFormat.Ext.First();
            var converter = GetFileFormatConverters(ext).PdfConverter;
            var sourceFile = context.UploadFileDoc.RelativePath;
            var newFileName = Path.GetFileNameWithoutExtension(sourceFile) + "." + converter.NewExt;
            var newFileDoc = this.UploadFileHandler.BeginBuild(newFileName);
            sourceFile = this.UploadFileHandler.UploadPath + sourceFile;
            var destFile = this.UploadFileHandler.UploadPath + newFileDoc.RelativePath;
            var success = await converter.ExecuteAsync(sourceFile, destFile);
            await this.UploadFileHandler.EndBuild(success, FileConvertType.Pdf, newFileDoc, context.UploadFileDoc, null);
            context.NormalizedFileDoc = newFileDoc;
        }
        /// <summary>生成图片</summary>
        private async Task GenerateImageAsync(UploadFileContext context)
        {
            if (context.UploadFileDoc.FileId == context.UploadFileDoc.ImageFileId)
            {
                context.NormalizedFileDoc = context.UploadFileDoc;
                return;
            }
            if (!context.UploadFileDoc.ImageFileId.IsNullOrEmpty())
            {
                var newFileId = context.UploadFileDoc.ImageFileId;
                context.NormalizedFileDoc = await this.UploadFileHandler.GetAsync(newFileId);
                return;
            }
            var ext = context.UploadFileFormat.Ext.First();
            var converter = GetFileFormatConverters(ext).ImageConverter;
            var sourceFile = context.UploadFileDoc.RelativePath;
            var newFileName = Path.GetFileNameWithoutExtension(sourceFile) + "." + converter.NewExt;
            var newFileDoc = this.UploadFileHandler.BeginBuild(newFileName);
            sourceFile = this.UploadFileHandler.UploadPath + sourceFile;
            var destFile = this.UploadFileHandler.UploadPath + newFileDoc.RelativePath;
            var success = await converter.ExecuteAsync(sourceFile, destFile);
            await this.UploadFileHandler.EndBuild(success, FileConvertType.Image, newFileDoc, context.UploadFileDoc, null);
            context.NormalizedFileDoc = newFileDoc;
        }
        /// <summary>生成缩略图</summary>
        private async Task<string> GenerateThumbnailAsync(UploadFileContext context)
        {
            string ext = null;
            string sourceFile = null;
            if (context.NormalizedFileDoc != context.UploadFileDoc)
            {
                if (!context.NormalizedFileDoc.ThumbnailFileId.IsNullOrEmpty())
                {
                    return context.NormalizedFileDoc.ThumbnailFileId;
                }
                if (context.NormalizedFileFormat.GenerateThumbnail)
                {
                    ext = context.NormalizedFileFormat.Ext.First();
                    sourceFile = context.NormalizedFileDoc.RelativePath;
                }
            }
            if (!context.UploadFileDoc.ThumbnailFileId.IsNullOrEmpty())
            {
                return context.UploadFileDoc.ThumbnailFileId;
            }
            if (ext == null)
            {
                ext = context.UploadFileFormat.Ext.First();
                sourceFile = context.UploadFileDoc.RelativePath;
            }
            var converter = GetFileFormatConverters(ext).ThumbnailConverter;
            var newFileName = Path.GetFileNameWithoutExtension(sourceFile) + "." + converter.NewExt;
            var newFileDoc = this.UploadFileHandler.BeginBuild(newFileName);
            sourceFile = this.UploadFileHandler.UploadPath + sourceFile;
            var destFile = this.UploadFileHandler.UploadPath + newFileDoc.RelativePath;
            var success = await converter.ExecuteAsync(sourceFile, destFile);
            await this.UploadFileHandler.EndBuild(success, FileConvertType.Thumbnail, newFileDoc, context.UploadFileDoc, context.NormalizedFileDoc);
            if (!success) newFileDoc = context.NormalizedFileDoc;
            return newFileDoc.FileId;
        }
        /// <summary>生成缩略图</summary>
        public async Task BuildThumbnail(Metadata metadata, string thumbnail)
        {
            if (thumbnail.Length != Utility.GuidLength) return;
            var fileDoc = await this.UploadFileHandler.GetAsync(thumbnail);
            if (fileDoc == null) return;
            var file = this.UploadFileHandler.UploadPath + fileDoc.RelativePath;
            var content = File.ReadAllBytes(file);
            metadata.Thumbnail = Convert.ToBase64String(content);
        }
        /// <summary>生成全文</summary>
        private async Task<string> GenerateFulltextAsync(UploadFileContext context)
        {
            if (!context.UploadFileDoc.FullTextFileId.IsNullOrEmpty())
            {
                return context.UploadFileDoc.FullTextFileId;
            }
            var ext = context.UploadFileFormat.Ext.First();
            var converter = GetFileFormatConverters(ext).FulltextConverter;
            var sourceFile = context.UploadFileDoc.RelativePath;
            var newFileName = Path.GetFileNameWithoutExtension(sourceFile) + "." + converter.NewExt;
            var newFileDoc = this.UploadFileHandler.BeginBuild(newFileName, Encoding.UTF8.WebName);
            sourceFile = this.UploadFileHandler.UploadPath + sourceFile;
            string pdfFile = null;
            if (context.UploadFileDoc != context.NormalizedFileDoc)
            {
                pdfFile = this.UploadFileHandler.UploadPath + context.NormalizedFileDoc.RelativePath;
            }
            var destFile = this.UploadFileHandler.UploadPath + newFileDoc.RelativePath;
            var success = await converter.ExecuteAsync(sourceFile, pdfFile, destFile);
            await this.UploadFileHandler.EndBuild(success, FileConvertType.FullText, newFileDoc, context.UploadFileDoc, context.NormalizedFileDoc);
            if (!success) newFileDoc = context.NormalizedFileDoc;
            return newFileDoc.FileId;
        }
        /// <summary>生成全文</summary>
        public async Task BuildFulltext(Metadata metadata, string fulltext)
        {
            if (fulltext.Length != Utility.GuidLength) return;
            var fileDoc = await this.UploadFileHandler.GetAsync(fulltext);
            if (fileDoc == null) return;
            var file = this.UploadFileHandler.UploadPath + fileDoc.RelativePath;
            metadata.Fulltext = File.ReadAllText(file, Encoding.UTF8);
        }
        /// <summary>生成文本文件内容</summary>
        private string BuildContent(UploadFileContext context)
        {
            var fileDoc = context.UploadFileDoc;
            var file = this.UploadFileHandler.UploadPath + fileDoc.RelativePath;
            return File.ReadAllText(file, Encoding.UTF8);
        }
        /// <summary>生成HTML</summary>
        private async Task<string> GenerateHtmlAsync(UploadFileContext context)
        {
            string destFile = null;
            if (context.UploadFileDoc.HtmlFileId.IsNullOrEmpty())
            {
                var ext = context.UploadFileFormat.Ext.First();
                var converter = GetFileFormatConverters(ext).HtmlConverter;
                var sourceFile = context.UploadFileDoc.RelativePath;
                var newFileName = Path.GetFileNameWithoutExtension(sourceFile) + "." + converter.NewExt;
                var newFileDoc = this.UploadFileHandler.BeginBuild(newFileName, Encoding.UTF8.WebName);
                sourceFile = this.UploadFileHandler.UploadPath + sourceFile;
                destFile = this.UploadFileHandler.UploadPath + newFileDoc.RelativePath;
                var success = await converter.ExecuteAsync(sourceFile, destFile);
                await this.UploadFileHandler.EndBuild(success, FileConvertType.Html, newFileDoc, context.UploadFileDoc, null);
            }
            else
            {
                var newFileDoc = await this.UploadFileHandler.GetAsync(context.UploadFileDoc.HtmlFileId);
                destFile = this.UploadFileHandler.UploadPath + newFileDoc.RelativePath;
            }
            return File.ReadAllText(destFile, Encoding.UTF8);
        }
        /// <summary>批量保存</summary>
        public AppDataSaveResult[] SaveMany(IndexDataSaveRequest<TDataSave> request)
        {
            return Task.Run(() => SaveManyAsync(request)).Result;
        }

        /// <summary>批量保存</summary>
        public async Task<AppDataSaveResult[]> SaveManyAsync(IndexDataSaveRequest<TDataSave> request)
        {
            var pairs = new List<PKSKeyValuePair<TDocument, AppDataSaveResult>>();
            foreach (var value in request.Values)
            {
                var pair = await Validate(value, true);
                pairs.Add(pair);
            }
            var updateOptions = new UpdateOptions { IsUpsert = true };
            foreach (var pair in pairs)
            {
                var doc = pair.Key;
                var filter = Builders<TDocument>.Filter.Eq(e => e.Id, doc.Id);
                await Accessor.ReplaceOneAsync(filter, doc, updateOptions);
            }
            return pairs.Select(e => e.Value).ToArray();
        }

        /// <summary>批量删除</summary>
        public string[] DeleteMany(List<string> dataIds)
        {
            return Task.Run(() => DeleteManyAsync(dataIds)).Result;
        }

        /// <summary>批量删除</summary>
        public async Task<string[]> DeleteManyAsync(List<string> dataIds)
        {
            if (dataIds == null) return null;
            if (dataIds.Count == 0) return new string[0];
            var filter = Builders<TDocument>.Filter.In(e => e.Id, dataIds);
            await Accessor.DeleteManyAsync(filter);
            return dataIds.ToArray();
        }

        /// <summary>根据DataID获得一条应用数据</summary>
        public TDocument Get(string dataId)
        {
            return Task.Run(() => GetAsync(dataId)).Result;
        }

        /// <summary>根据DataID获得一条应用数据</summary>
        public async Task<TDocument> GetAsync(string dataId)
        {
            var result = await Accessor
                .AsQueryable()
                .Where(e => e.Id == dataId)
                .FirstOrDefaultAsync();
            //if (result == null)
            //{
            //    var message = "DataId相关数据不存在";
            //    ApiServiceExceptionCodes.DataIdNotExists.ThrowUserFriendly(message, message);
            //}
            return result;
        }

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        public Dictionary<string, TDocument> GetMany(List<string> dataIds)
        {
            return Task.Run(() => GetManyAsync(dataIds)).Result;
        }

        /// <summary>根据DataID数组获得对应的多条应用数据</summary>
        public async Task<Dictionary<string, TDocument>> GetManyAsync(List<string> dataIds)
        {
            if (dataIds == null) return null;
            if (dataIds.Count == 0) return new Dictionary<string, TDocument>();
            var result = await Accessor
                .AsQueryable()
                .Where(e => dataIds.Contains(e.Id))
                .ToListAsync();
            return result.ToDictionary(e => e.DataId);
        }

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        public IndexDataMatchResult<TDocument> Match(IndexDataMatchRequest request)
        {
            return Task.Run(() => MatchAsync(request)).Result;
        }

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        public async Task<IndexDataMatchResult<TDocument>> MatchAsync(IndexDataMatchRequest request)
        {
            var filter = Accessor.BuildFilter<TDocument>(request.Filter);
            var options = Accessor.BuildPager(request);
            options.Sort = Accessor.BuildSort(request.Sort);
            var result = new IndexDataMatchResult<TDocument>();
            result.Total = Convert.ToInt32(await Accessor.CountAsync(filter));
            using (var cursor = await Accessor.FindAsync(filter, options))
            {
                result.Values = await cursor.ToListAsync();
                return result;
            }
        }
        /// <summary>获得上传文件夹下全部子文件夹</summary>
        public List<UploadFolder> GetUploadFolders()
        {
            return Task.Run(() => GetUploadFoldersAsync()).Result;
        }

        /// <summary>获得上传文件夹下全部子文件夹</summary>
        public async Task<List<UploadFolder>> GetUploadFoldersAsync()
        {
            return await this.UploadFileHandler.GetUploadFoldersAsync();
        }
        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        public List<string> GetUploadFolderFiles(string fullName)
        {
            return Task.Run(() => GetUploadFolderFilesAsync(fullName)).Result;
        }

        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        public async Task<List<string>> GetUploadFolderFilesAsync(string fullName)
        {
            return await this.UploadFileHandler.GetUploadFolderFilesAsync(fullName);
        }
        /// <summary>清除临时文件夹中的过期文件</summary>
        public void ClearTempFiles()
        {
            this.UploadFileHandler.ClearTempFiles();
        }
        /// <summary>清除临时文件夹中的过期文件</summary>
        public async Task ClearTempFilesAsync()
        {
            await Task.Run(() => ClearTempFiles());
        }
        /// <summary>根据DataID获得一条应用数据</summary>
        public DownloadResult Download(DownloadRequest request)
        {
            return Task.Run(() => DownloadAsync(request)).Result;
        }

        /// <summary>根据DataID或FileID获得相关文件流</summary>
        public async Task<DownloadResult> DownloadAsync(DownloadRequest request)
        {
            TDocument doc = null;
            if (request.ContentRef.IsNullOrEmpty() && !request.DataId.IsNullOrEmpty())
            {
                doc = await GetAsync(request.DataId);
            }
            return await UploadFileHandler.DownloadAsync(request, doc);
        }
    }
}