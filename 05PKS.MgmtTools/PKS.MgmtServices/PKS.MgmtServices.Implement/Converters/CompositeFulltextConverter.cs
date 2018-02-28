using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using PKS.Core;
using PKS.Utils;
using System;

namespace PKS.MgmtServices.Converters
{
    /// <summary>全文文件转换器</summary>
    public class CompositeFulltextConverter : CompositeFileConverter, ICompositeFulltextConverter, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public CompositeFulltextConverter(IEnumerable<IFulltextConverter> converters) : base(converters)
        {
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "txt"; }
        }
        /// <summary>生成全文</summary>
        public bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            return Task.Run(() => ExecuteAsync(sourceFile, pdfFile, destFile)).Result;
        }
        /// <summary>生成全文</summary>
        public async Task<bool> ExecuteAsync(string sourceFile, string pdfFile, string destFile)
        {
            try
            {
                var ext = GetExt(sourceFile);
                var converter = this.Mappers.GetValueBy(ext).As<IFulltextConverter>();
                if (converter == null)
                {
                    throw new NotSupportedException($"{ext}不支持转换!");
                }
                return await converter.ExecuteAsync(sourceFile, pdfFile, destFile);
            }
            catch (Exception ex)
            {
                ex.Data[nameof(sourceFile)] = sourceFile;
                ex.Data[nameof(pdfFile)] = pdfFile;
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