using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>组合图片文件转换器</summary>
    public class CompositeImageConverter : CompositeFileConverter, ICompositeImageConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public CompositeImageConverter(IEnumerable<IImageConverter> converters) : base(converters)
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "png"; }
        }
        /// <summary>生成指定大小的图片</summary>
        public bool Execute(string sourceFile, string destFile, Size size)
        {
            return Task.Run(() => ExecuteAsync(sourceFile, destFile, size)).Result;
        }
        /// <summary>生成指定大小的图片</summary>
        public async Task<bool> ExecuteAsync(string sourceFile, string destFile, Size size)
        {
            try
            {
                var ext = GetExt(sourceFile);
                var converter = this.Mappers.GetValueBy(ext).As<IImageConverter>();
                if (converter == null)
                {
                    throw new NotSupportedException($"{ext}不支持转换!");
                }
                return await converter.ExecuteAsync(sourceFile, destFile, size);
            }
            catch (Exception ex)
            {
                ex.Data[nameof(sourceFile)] = sourceFile;
                ex.Data[nameof(destFile)] = destFile;
                Bootstrapper.Error(this.GetType().Name + ":", ex);
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}