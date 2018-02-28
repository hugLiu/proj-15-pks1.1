using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>图片文件转换器基类</summary>
    public interface IImageConverterBase : IFileConverter
    {
        /// <summary>生成指定大小的图片</summary>
        bool Execute(string sourceFile, string destFile, Size size);
        /// <summary>生成指定大小的图片</summary>
        Task<bool> ExecuteAsync(string sourceFile, string destFile, Size size);
    }


    /// <summary>图片文件转换器</summary>
    public interface IImageConverter : IImageConverterBase
    {
        /// <summary>获得最大尺寸</summary>
        Size MaxSize { get; }
    }

    /// <summary>组合图片文件转换器</summary>
    public interface ICompositeImageConverter : IImageConverterBase
    {
    }
}