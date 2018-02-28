using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>HTML文件转换器</summary>
    public interface IHtmlConverter : IFileConverter
    {
    }

    /// <summary>组合HTML文件转换器</summary>
    public interface ICompositeHtmlConverter : IFileConverter
    {
    }
}