using System.IO;
using System.Threading.Tasks;
using PKS.Core;
using PKS.MgmtServices.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>全文文件转换器</summary>
    public class FulltextConverterWrapper : FileConverter, IFulltextConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public FulltextConverterWrapper()
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "txt"; }
        }
        /// <summary>生成全文文件</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            return await ExecuteAsync(sourceFile, null, destFile);
        }
        /// <summary>生成全文</summary>
        public bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            return Task.Run(() => ExecuteAsync(sourceFile, pdfFile, destFile)).Result;
        }
        /// <summary>生成全文</summary>
        public async Task<bool> ExecuteAsync(string sourceFile, string pdfFile, string destFile)
        {
            var sourceFileExt = GetExt(sourceFile);
            if (sourceFileExt == this.NewExt) return false;
            var client = new MgmtServiceWrapper();
            return await client.InvokeAsync(proxy => proxy.GenerateFulltextAsync(sourceFile, pdfFile, destFile));
        }
    }
}