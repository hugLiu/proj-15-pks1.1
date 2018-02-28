using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using PKS.Core;
using PKS.MgmtServices.Core;
using PKS.Models;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>缩略图文件转换器</summary>
    public class ThumbnailConverterWrapper : FileConverter, IThumbnailConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public ThumbnailConverterWrapper()
        {
            var size = ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_ThumbnailDefaultSize].Split(',');
            this.DefaultSize = new Size(size[0].ToInt32(), size[1].ToInt32());
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "png"; }
        }
        /// <summary>获得默认大小</summary>
        public Size DefaultSize { get; }
        /// <summary>生成默认大小的缩略图</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            return await ExecuteAsync(sourceFile, destFile, this.DefaultSize);
        }
        /// <summary>生成指定大小的缩略图</summary>
        public bool Execute(string sourceFile, string destFile, Size size)
        {
            return Task.Run(() => ExecuteAsync(sourceFile, destFile, size)).Result;
        }
        /// <summary>生成指定大小的缩略图</summary>
        public async Task<bool> ExecuteAsync(string sourceFile, string destFile, Size size)
        {
            var client = new MgmtServiceWrapper();
            return await client.InvokeAsync(proxy => proxy.GenerateThumbnailAsync(sourceFile, destFile, size));
        }
    }
}