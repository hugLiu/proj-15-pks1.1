using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using PKS.Core;
using PKS.Models;
using PKS.Services;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using TDocument = PKS.WebAPI.Models.MongoUploadFile;

namespace PKS.WebAPI.Services
{
    /// <summary>上传文件处理器</summary>
    internal class UploadFileHandler
    {
        /// <summary>构造函数</summary>
        public UploadFileHandler(IMongoConfig config, IMongoCollection<TDocument> uploadFilesCollection)
        {
            this.UploadDir = config.IndexUploadFilesDir;
            this.UploadPath = config.IndexUploadFilesPath;
            if (!Directory.Exists(this.UploadPath)) Directory.CreateDirectory(this.UploadPath);
            this.TempPath = config.IndexUploadTempPath;
            if (!Directory.Exists(this.TempPath)) Directory.CreateDirectory(this.TempPath);
            this.UploadFilesCollection = uploadFilesCollection;
            this.Bucket = CreateBucket(config.Database.As<IMongoDatabase>());
            ClearTempFiles();
        }
        /// <summary>获得文件桶</summary>
        private GridFSBucket CreateBucket(IMongoDatabase database)
        {
            var options = new GridFSBucketOptions();
            options.BucketName = ImmutableGridFSBucketOptions.Defaults.BucketName;
            options.ChunkSizeBytes = 1024 * 1024 * 1;
            return new GridFSBucket(database, options);
        }
        /// <summary>文件桶访问器</summary>
        private IGridFSBucket Bucket { get; }
        /// <summary>上传目录</summary>
        public string UploadDir { get; private set; }
        /// <summary>上传路径</summary>
        public string UploadPath { get; private set; }
        /// <summary>上传临时路径</summary>
        private string TempPath { get; set; }
        /// <summary>上传文件Collection</summary>
        public IMongoCollection<TDocument> UploadFilesCollection { get; set; }
        /// <summary>Mongo文件元数据之上传文件ID</summary>
        private const string Metadata_UploadFileId = "UploadFileId";
        /// <summary>处理文件上传</summary>
        public async Task<UploadResult> UploadAsync(ServerUploadRequest request)
        {
            var result = new UploadResult();
            var files = new List<string>();
            if (request.Guid.IsNullOrEmpty())
            {
                if (request.ServerFile.IsNullOrEmpty())
                {
                    await HandleNoneAsync(request, result, files);
                }
                else
                {
                    await HandleSingleAsync(request, result, files);
                }
            }
            else
            {
                await HandleChunkAsync(request, result, files);
            }
            result.TempFileIds = files.ToArray();
            return result;
        }

        /// <summary>处理秒传或无文件上传</summary>
        private async Task HandleNoneAsync(ServerUploadRequest request, UploadResult result, List<string> files)
        {
            if (request.Md5.IsNullOrEmpty()) return;
            var md5s = request.Md5.Select(e => e.ToUpperInvariant()).ToArray();
            var filter = Builders<TDocument>.Filter.In(f => f.Md5, md5s);
            using (var cursor = await this.UploadFilesCollection.FindAsync(filter))
            {
                var docs = await cursor.ToListAsync();
                foreach (var md5 in md5s)
                {
                    var doc = docs.FirstOrDefault(e => e.Md5 == md5);
                    if (doc == null)
                    {
                        files.Add(null);
                        continue;
                    }
                    if (request.CharSet.IsNullOrEmpty() || (md5s.Length == 1 && request.FileName.IsNullOrEmpty()))
                    {
                        files.Add(doc.FileId);
                        continue;
                    }
                    await CheckCharSetAsync(md5s.Length > 1, doc, request);
                    files.Add(doc.Utf8FileId ?? doc.FileId);
                }
            }
        }
        /// <summary>处理单文件上传</summary>
        private async Task HandleSingleAsync(ServerUploadRequest request, UploadResult result, List<string> files)
        {
            var uploadMD5 = request.Md5?.FirstOrDefault();
            var doc = await BuildNew(request, uploadMD5);
            files.Add(doc.Utf8FileId ?? doc.FileId);
        }

        /// <summary>处理分片文件上传</summary>
        private async Task HandleChunkAsync(ServerUploadRequest request, UploadResult result, List<string> files)
        {
            if (request.Chunk < 0 || request.Chunks < 1 || request.Chunk > request.Chunks)
            {
                ExceptionCodes.ParameterParsingFailed.ThrowUserFriendly("参数无效！", "分片参数无效！");
            }
            var uploadMD5 = request.Md5?.FirstOrDefault();
            result.Chunk = request.Chunk;
            if (request.Chunk < request.Chunks)
            {
                if (request.ServerFile.IsNullOrEmpty())
                {
                    ExceptionCodes.MissingParameterValue.ThrowUserFriendly("参数无效！", "分片上传时流不存在！");
                }
                //VerifyMD5(request.ServerFile, uploadMD5);
                var chunkFile = GetNewChunkFile(request, request.Chunk);
                File.Move(request.ServerFile, chunkFile);
            }
            else
            {
                #region 合并分片文件
                var mergeFile = GetNewMergeFile(request);
                var chunkFiles = new List<string>();
                using (var mergeStream = new FileStream(mergeFile, FileMode.Create, FileAccess.Write))
                {
                    for (var i = 0; i < request.Chunks; i++)
                    {
                        var chunkFile = GetNewChunkFile(request, i);
                        using (var chunkStream = new FileStream(chunkFile, FileMode.Open, FileAccess.Read))
                        {
                            await chunkStream.CopyToAsync(mergeStream);
                        }
                        chunkFiles.Add(chunkFile);
                    }
                }
                request.ServerFile = mergeFile;
                var doc = await BuildNew(request, uploadMD5);
                files.Add(doc.Utf8FileId ?? doc.FileId);
                chunkFiles.ForEach(File.Delete);
                #endregion
            }
        }
        /// <summary>处理文件上传</summary>
        public async Task<UploadFileResult> UploadFileAsync(UploadFileRequest request)
        {
            var md5 = BuildMD5(request.ServerFile, null);
            var filter = Builders<TDocument>.Filter.Eq(f => f.Md5, md5);
            var result = new UploadFileResult();
            using (var cursor = await this.UploadFilesCollection.FindAsync(filter))
            {
                var doc = await cursor.FirstOrDefaultAsync();
                if (doc == null)
                {
                    doc = BuildNew(null, request.FileName, null);
                    doc.Md5 = md5;
                    var docFile = this.UploadPath + doc.RelativePath;
                    File.Move(request.ServerFile, docFile);
                    await this.UploadFilesCollection.InsertOneAsync(doc);
                }
                result.RelativePath = $"/{this.UploadDir}{doc.RelativePath}";
            }
            return result;
        }
        /// <summary>离线上传</summary>
        public async Task<TDocument> OfflineUploadAsync(string sourceFile)
        {
            var request = new ServerUploadRequest();
            request.RelativePath = sourceFile.NormalizeRelativePath();
            request.FileName = Path.GetFileName(request.RelativePath);
            request.ServerFile = this.UploadPath + request.RelativePath;
            return await BuildNew(request, null);
        }
        /// <summary>在线上传</summary>
        public async Task OnlineUploadAsync(TDocument doc, string sourceFile)
        {
            var relativePath = sourceFile.NormalizeRelativePath();
            if (doc.RelativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase)) return;
            var fullFile = this.UploadPath + relativePath;
            var fullPath = Path.GetDirectoryName(fullFile);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            else if (File.Exists(fullFile))
            {
                ApiServiceExceptionCodes.FileExists.ThrowUserFriendly("参数无效！", $"{nameof(sourceFile)}[{sourceFile}]指定的文件已经存在!");
            }
            File.Copy(this.UploadPath + doc.RelativePath, this.UploadPath + relativePath, true);
            var fileName = Path.GetFileName(relativePath);
            var filter = Builders<TDocument>.Filter
                .Eq(f => f.FileId, doc.FileId);
            var update = Builders<TDocument>.Update
                .Set(u => u.FileName, fileName)
                .Set(u => u.RelativePath, relativePath);
            //var options = new UpdateOptions();
            await this.UploadFilesCollection.UpdateOneAsync(filter, update);
            File.Delete(this.UploadPath + doc.RelativePath);
            doc.FileName = fileName;
            doc.RelativePath = relativePath;
        }
        /// <summary>生成新文档</summary>
        private async Task<TDocument> BuildNew(ServerUploadRequest request, string uploadMD5)
        {
            var doc = BuildNew(request.RelativePath, request.FileName, request.CharSet);
            doc.Md5 = BuildMD5(request.ServerFile, uploadMD5);
            doc.Utf8FileId = await ConvertCharSetAsync(doc, request);
            if (request.RelativePath.IsNullOrEmpty())
            {
                var docFile = this.UploadPath + doc.RelativePath;
                File.Move(request.ServerFile, docFile);
            }
            await this.UploadFilesCollection.InsertOneAsync(doc);
            return doc;
        }
        /// <summary>生成新文档</summary>
        private TDocument BuildNew(string relativePath, string fileName, string charSet)
        {
            var doc = new TDocument();
            doc.FileId = Guid.NewGuid().ToString();
            doc.FileName = fileName;
            doc.CreateDate = DateTime.Now;
            var ext = Path.GetExtension(fileName);
            if (relativePath.IsNullOrEmpty())
            {
                var levelOneDir = doc.CreateDate.ToString("yyyyMM");
                var levelTwoDir = doc.CreateDate.ToString("ddHH");
                var docPath = $@"{this.UploadPath}\{levelOneDir}\{levelTwoDir}";
                if (!Directory.Exists(docPath)) Directory.CreateDirectory(docPath);
                doc.RelativePath = $@"\{levelOneDir}\{levelTwoDir}\{doc.FileId}{ext}";
            }
            else
            {
                doc.RelativePath = relativePath;
            }
            doc.ContentType = ext.GetMediaType().BuildMediaType(charSet);
            return doc;
        }
        /// <summary>校验MD5</summary>
        private void VerifyMD5(string serverFile, string uploadMD5)
        {
            if (uploadMD5.IsNullOrEmpty()) return;
            BuildMD5(serverFile, uploadMD5);
        }
        /// <summary>生成MD5</summary>
        private string BuildMD5(string serverFile, string uploadMD5)
        {
            var buffer = File.ReadAllBytes(serverFile);
            var md5 = buffer.ToMD5().ToUpperInvariant();
            if (!uploadMD5.IsNullOrEmpty() && !md5.Equals(uploadMD5, StringComparison.OrdinalIgnoreCase))
            {
                ApiServiceExceptionCodes.MD5VerifyFailed.ThrowUserFriendly("文件上传失败！", "上传文件MD5校验失败！");
            }
            return md5;
        }
        /// <summary>检查编码</summary>
        private async Task CheckCharSetAsync(bool uploadManyFile, TDocument doc, ServerUploadRequest request)
        {
            if (!uploadManyFile)
            {
                var fileFormat = request.FileName.GetExtension().GetFileFormat();
                if (fileFormat.IsStream)
                {
                    doc.Utf8FileId = null;
                    return;
                }
            }
            var docContentType = MediaTypeHeaderValue.Parse(doc.ContentType);
            var newFileDoc = doc;
            if (request.CharSet.Equals(docContentType.CharSet, StringComparison.OrdinalIgnoreCase))
            {
                if (doc.Utf8FileId != null) return;
            }
            var utf8 = Encoding.UTF8;
            if (!utf8.WebName.Equals(request.CharSet, StringComparison.OrdinalIgnoreCase))
            {
                newFileDoc = BuildNew(null, doc.FileName, utf8.WebName);
                var file = this.UploadPath + doc.RelativePath;
                var content = File.ReadAllText(file, Encoding.GetEncoding(request.CharSet));
                var newFile = this.UploadPath + newFileDoc.RelativePath;
                File.WriteAllText(newFile, content, utf8);
                newFileDoc.Utf8FileId = newFileDoc.FileId;
                newFileDoc.Md5 = BuildMD5(newFile, null);
                await this.UploadFilesCollection.InsertOneAsync(newFileDoc);
            }
            docContentType.CharSet = request.CharSet;
            var newContentType = docContentType.ToString();
            var filter = Builders<TDocument>.Filter.Eq(f => f.FileId, doc.FileId);
            var update = Builders<TDocument>.Update
                .Set(u => u.Utf8FileId, newFileDoc.FileId)
                .Set(u => u.ContentType, newContentType);
            await this.UploadFilesCollection.UpdateOneAsync(filter, update);
            doc.Utf8FileId = newFileDoc.FileId;
            doc.ContentType = newContentType;
        }
        /// <summary>转换编码</summary>
        private async Task<string> ConvertCharSetAsync(TDocument doc, ServerUploadRequest request)
        {
            if (request.CharSet.IsNullOrEmpty()) return null;
            if (request.FileName.GetMediaTypeFromFile().IsStream()) return null;
            var utf8 = Encoding.UTF8;
            if (request.CharSet.Equals(utf8.WebName, StringComparison.OrdinalIgnoreCase)) return doc.FileId;
            var newFileDoc = BuildNew(null, request.FileName, utf8.WebName);
            var content = File.ReadAllText(request.ServerFile, Encoding.GetEncoding(request.CharSet));
            var newFile = this.UploadPath + newFileDoc.RelativePath;
            File.WriteAllText(newFile, content, utf8);
            newFileDoc.Md5 = BuildMD5(newFile, null);
            newFileDoc.Utf8FileId = newFileDoc.FileId;
            await this.UploadFilesCollection.InsertOneAsync(newFileDoc);
            return newFileDoc.FileId;
        }
        /// <summary>获得新分片文件路径</summary>
        private string GetNewChunkFile(ServerUploadRequest request, int chunk)
        {
            var ext = Path.GetExtension(request.FileName);
            return $@"{this.TempPath}\{request.Guid}_PART_{chunk.ToString()}{ext}";
        }
        /// <summary>获得分片合并文件路径</summary>
        private string GetNewMergeFile(ServerUploadRequest request)
        {
            var ext = Path.GetExtension(request.FileName);
            return $@"{this.TempPath}\{request.Guid}{ext}";
        }
        /// <summary>开始生成文档</summary>
        public TDocument BeginBuild(string fileName, string charSet = null)
        {
            return BuildNew(null, fileName, charSet);
        }
        /// <summary>结束生成文档</summary>
        public async Task EndBuild(bool success, FileConvertType type, TDocument newFileDoc, TDocument uploadFileDoc, TDocument normalizedFileDoc)
        {
            if (success)
            {
                var destFile = this.UploadPath + newFileDoc.RelativePath;
                newFileDoc.Md5 = BuildMD5(destFile, null);
                switch (type)
                {
                    case FileConvertType.Pdf: newFileDoc.PdfFileId = newFileDoc.FileId; break;
                    case FileConvertType.Image: newFileDoc.ImageFileId = newFileDoc.FileId; break;
                    case FileConvertType.Thumbnail: newFileDoc.ThumbnailFileId = newFileDoc.FileId; break;
                    case FileConvertType.FullText: newFileDoc.FullTextFileId = newFileDoc.FileId; break;
                    case FileConvertType.Html: newFileDoc.HtmlFileId = newFileDoc.FileId; break;
                }
                await this.UploadFilesCollection.InsertOneAsync(newFileDoc);
            }
            await EndUpdate(success, type, newFileDoc, uploadFileDoc);
            if (normalizedFileDoc != null && normalizedFileDoc != uploadFileDoc)
            {
                await EndUpdate(success, type, newFileDoc, normalizedFileDoc);
            }
        }
        /// <summary>结束更新文档</summary>
        private async Task EndUpdate(bool success, FileConvertType type, TDocument newFileDoc, TDocument uploadFileDoc)
        {
            var filter = Builders<TDocument>.Filter.Eq(f => f.FileId, uploadFileDoc.FileId);
            var updateBuilder = Builders<TDocument>.Update;
            UpdateDefinition<TDocument> update = null;
            var updateFileId = success ? newFileDoc.FileId : uploadFileDoc.FileId;
            switch (type)
            {
                case FileConvertType.Pdf: update = updateBuilder.Set(u => u.PdfFileId, updateFileId); break;
                case FileConvertType.Image: update = updateBuilder.Set(u => u.ImageFileId, updateFileId); break;
                case FileConvertType.Thumbnail: update = updateBuilder.Set(u => u.ThumbnailFileId, updateFileId); break;
                case FileConvertType.FullText: update = updateBuilder.Set(u => u.FullTextFileId, updateFileId); break;
                case FileConvertType.Html: update = updateBuilder.Set(u => u.HtmlFileId, updateFileId); break;
            }
            await this.UploadFilesCollection.UpdateOneAsync(filter, update);
            switch (type)
            {
                case FileConvertType.Pdf: uploadFileDoc.PdfFileId = updateFileId; break;
                case FileConvertType.Image: uploadFileDoc.ImageFileId = updateFileId; break;
                case FileConvertType.Thumbnail: uploadFileDoc.ThumbnailFileId = updateFileId; break;
                case FileConvertType.FullText: uploadFileDoc.FullTextFileId = updateFileId; break;
                case FileConvertType.Html: uploadFileDoc.HtmlFileId = updateFileId; break;
            }
        }
        /// <summary>获得上传文件</summary>
        public async Task<TDocument> GetAsync(string uploadFileId)
        {
            var filter = Builders<TDocument>.Filter.Eq(f => f.FileId, uploadFileId);
            using (var cursor = await this.UploadFilesCollection.FindAsync(filter))
            {
                return await cursor.FirstOrDefaultAsync();
            }
        }
        /// <summary>插入到Mongo库GridFS中</summary>
        public async Task<ObjectId> InsertMongoAsync(TDocument doc)
        {
            using (var stream = File.OpenRead(this.UploadPath + doc.RelativePath))
            {
                var options = new GridFSUploadOptions();
                options.Metadata = new BsonDocument();
                options.Metadata[Metadata_UploadFileId] = doc.FileId;
                return await this.Bucket.UploadFromStreamAsync(doc.FileName, stream, options);
            }
        }
        /// <summary>根据DataID或FileID获得相关文件流</summary>
        public async Task<DownloadResult> DownloadAsync(DownloadRequest request, IndexAppData doc)
        {
            var result = new DownloadResult();
            string ext = null;
            if (doc != null)
            {
                if (request.Source)
                {
                    if (doc.SourceContentRef.IsNullOrEmpty())
                    {
                        ExceptionCodes.ParameterParsingFailed.ThrowUserFriendly("下载失败！", $"DataId[{request.DataId}]不存在源文件！");
                    }
                    request.StorageType = doc.SourceStorageType.Value;
                    request.ContentRef = doc.SourceContentRef;
                    result.FileName = doc.SourceName;
                }
                else
                {
                    if (doc.ContentRef.IsNullOrEmpty())
                    {
                        ExceptionCodes.ParameterParsingFailed.ThrowUserFriendly("下载失败！", $"DataId[{request.DataId}]不存在文件！");
                    }
                    request.StorageType = doc.StorageType;
                    request.ContentRef = doc.ContentRef;
                    ext = Path.GetExtension(doc.Name);
                    if (!ext.IsNullOrEmpty()) result.FileName = doc.Name;
                }
            }
            if (request.StorageType == IndexStorageType.File)
            {
                var fileObjectId = new ObjectId(request.ContentRef);
                var bucketStream = await Bucket.OpenDownloadStreamAsync(fileObjectId);
                if (result.FileName.IsNullOrEmpty()) result.FileName = bucketStream.FileInfo.Filename;
                result.Content = bucketStream;
            }
            else if (request.StorageType == IndexStorageType.Url)
            {
                var relativePath = request.ContentRef.Replace('/', '\\');
                if (result.FileName.IsNullOrEmpty()) result.FileName = Path.GetFileName(relativePath);
                result.Content = File.OpenRead(this.UploadPath + relativePath);
            }
            else
            {
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly("下载失败！", $"缺少参数！");
            }
            if (!request.FileName.IsNullOrEmpty()) ext = Path.GetExtension(request.FileName);
            if (ext.IsNullOrEmpty()) ext = Path.GetExtension(result.FileName);
            result.ContentType = ext.GetMediaType();
            return result;
        }
        /// <summary>获得上传文件夹下全部子文件夹</summary>
        public Task<List<UploadFolder>> GetUploadFoldersAsync()
        {
            var path = this.UploadPath;
            var folders = Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                .Select(e => new UploadFolder() { FullName = e.Substring(path.Length) })
                .ToList();
            for (int i = 0; i < folders.Count; i++)
            {
                var folder = folders[i];
                folder.Id = i + 1;
                folder.Name = folder.FullName.Substring(folder.FullName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                folder.Level = folder.FullName.Count(e => e == Path.DirectorySeparatorChar);
                folder.FullName = folder.FullName.Replace(Path.DirectorySeparatorChar, '/');
            }
            for (int i = 0; i < folders.Count; i++)
            {
                var folder = folders[i];
                if (folder.Level == 1) continue;
                folder.ParentId = folders.First(e => e.Level == folder.Level - 1 && folder.FullName.StartsWith(e.FullName, StringComparison.OrdinalIgnoreCase)).Id;
            }
            return Task.FromResult(folders);
        }
        /// <summary>获得某个上传文件夹下全部文件列表</summary>
        public Task<List<string>> GetUploadFolderFilesAsync(string folderFullName)
        {
            folderFullName = folderFullName.Replace('/', Path.DirectorySeparatorChar).Trim(Path.DirectorySeparatorChar);
            var path = $@"{this.UploadPath}\{folderFullName}";
            var files = Directory.GetFiles(path).Select(e => e.Substring(path.Length + 1)).ToList();
            return Task.FromResult(files);
        }
        /// <summary>清除临时文件夹中的过期文件</summary>
        public void ClearTempFiles()
        {
            var files = Directory.GetFiles(this.TempPath);
            var now = DateTime.Now;
            var delay = TimeSpan.FromHours(1);
            foreach (var file in files)
            {
                if ((now - File.GetCreationTime(file)) > delay)
                {
                    //删除超过一小时的
                    File.Delete(file);
                }
            }
        }
    }
}