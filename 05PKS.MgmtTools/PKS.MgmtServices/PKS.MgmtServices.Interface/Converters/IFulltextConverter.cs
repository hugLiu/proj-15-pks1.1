using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>全文文件转换器基类</summary>
    public interface IFulltextConverterBase : IFileConverter
    {
        /// <summary>生成全文</summary>
        bool Execute(string sourceFile, string pdfFile, string destFile);
        /// <summary>生成全文</summary>
        Task<bool> ExecuteAsync(string sourceFile, string pdfFile, string destFile);
    }

    /// <summary>全文文件转换器</summary>
    public interface IFulltextConverter : IFulltextConverterBase
    {
    }

    /// <summary>组合全文文件转换器</summary>
    public interface ICompositeFulltextConverter : IFulltextConverterBase
    {
    }
}