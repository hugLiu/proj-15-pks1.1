using System.Threading.Tasks;
using PKS.Core;
using PKS.MgmtServices.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Html文件转换器</summary>
    public class HtmlConverterWrapper : FileConverter, IHtmlConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public HtmlConverterWrapper()
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "html"; }
        }
        /// <summary>生成HTML文件</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            var client = new MgmtServiceWrapper();
            return await client.InvokeAsync(proxy => proxy.GenerateHtmlAsync(sourceFile, destFile));
        }
    }
}