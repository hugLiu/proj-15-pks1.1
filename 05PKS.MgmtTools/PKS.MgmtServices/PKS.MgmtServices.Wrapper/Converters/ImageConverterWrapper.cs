using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using PKS.Core;
using PKS.MgmtServices.Core;
using PKS.Models;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>图片文件转换器</summary>
    public class ImageConverterWrapper : FileConverter, IImageConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public ImageConverterWrapper()
        {
            var config = ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_ImageMaxSize];
            if (config.IsNullOrEmpty()) config = "800,600";
            var size = config.Split(',');
            this.MaxSize = new Size(size[0].ToInt32(), size[1].ToInt32());
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "png"; }
        }
        /// <summary>图片最大尺寸</summary>
        public Size MaxSize { get; }
        /// <summary>生成图片文件</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            return await ExecuteAsync(sourceFile, destFile, this.MaxSize);
        }
        /// <summary>生成指定大小的图片文件</summary>
        public bool Execute(string sourceFile, string destFile, Size size)
        {
            return Task.Run(() => ExecuteAsync(sourceFile, destFile, size)).Result;
        }
        /// <summary>生成指定大小的图片文件</summary>
        public async Task<bool> ExecuteAsync(string sourceFile, string destFile, Size size)
        {
            var client = new MgmtServiceWrapper();
            return await client.InvokeAsync(proxy => proxy.GenerateImageAsync(sourceFile, destFile, size));
        }
    }
}