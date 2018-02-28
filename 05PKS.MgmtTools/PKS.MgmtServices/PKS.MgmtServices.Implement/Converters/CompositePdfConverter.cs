using System.Collections.Generic;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PDF文件转换器</summary>
    public class CompositePdfConverter : CompositeFileConverter, ICompositePdfConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public CompositePdfConverter(IEnumerable<IPdfConverter> converters) : base(converters)
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "pdf"; }
        }
    }
}