using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>缩略图文件转换器</summary>
    public interface IThumbnailConverterBase : IFileConverter
    {
        /// <summary>生成指定大小的缩略图</summary>
        bool Execute(string sourceFile, string destFile, Size size);
        /// <summary>生成指定大小的缩略图</summary>
        Task<bool> ExecuteAsync(string sourceFile, string destFile, Size size);
    }

    /// <summary>缩略图文件转换器</summary>
    public interface IThumbnailConverter : IThumbnailConverterBase
    {
        /// <summary>获得默认大小</summary>
        Size DefaultSize { get; }
    }

    /// <summary>组合缩略图文件转换器</summary>
    public interface ICompositeThumbnailConverter : IThumbnailConverterBase
    {
    }
}