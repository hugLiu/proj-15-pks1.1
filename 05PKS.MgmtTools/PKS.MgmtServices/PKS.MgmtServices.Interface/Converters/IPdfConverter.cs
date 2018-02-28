using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PDF文件转换器</summary>
    public interface IPdfConverter : IFileConverter
    {
    }

    /// <summary>组合全文文件转换器</summary>
    public interface ICompositePdfConverter : IFileConverter
    {
    }
}