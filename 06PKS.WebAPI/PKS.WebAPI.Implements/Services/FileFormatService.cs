using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using PKS.Core;
using PKS.Models;
using PKS.Services;
using PKS.Utils;
using PKS.WebAPI.Models;
using TDocument = PKS.Models.FileFormat;

namespace PKS.WebAPI.Services
{
    /// <summary>文件格式服务接口</summary>
    public class FileFormatService : IFileFormatService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public FileFormatService(IMongoCollection<TDocument> accessor)
        {
            this.Accessor = accessor;
        }
        /// <summary>访问器</summary>
        private IMongoCollection<TDocument> Accessor { get; }
        /// <summary>获得全部文件格式信息</summary>
        public List<FileFormat> GetFileFormats()
        {
            return Task.Run(() => GetFileFormatsAsync()).Result;
        }

        /// <summary>获得全部文件格式信息</summary>
        public async Task<List<FileFormat>> GetFileFormatsAsync()
        {
            return await this.Accessor.AsQueryable().ToListAsync().ConfigureAwait(false);
        }
        /// <summary>获得全部文件格式信息</summary>
        List<FileFormat> IFileFormatService.GetAll()
        {
            return GetFileFormats();
        }
    }
}