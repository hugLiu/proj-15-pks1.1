using System.Threading.Tasks;
using PKS.Core;
using PKS.MgmtServices.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PDF文件转换器</summary>
    public class PdfConverterWrapper : FileConverter, IPdfConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public PdfConverterWrapper()
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "pdf"; }
        }
        /// <summary>生成PDF文档</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            var client = new MgmtServiceWrapper();
            return await client.InvokeAsync(proxy => proxy.GeneratePdfAsync(sourceFile, destFile));
        }
    }
}