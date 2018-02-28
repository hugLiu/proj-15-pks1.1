using System.Collections.Generic;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>HTML文件转换器</summary>
    public class CompositeHtmlConverter : CompositeFileConverter, ICompositeHtmlConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public CompositeHtmlConverter(IEnumerable<IHtmlConverter> converters) : base(converters)
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "html"; }
        }
    }
}