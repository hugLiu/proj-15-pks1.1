using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Services;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>文件转换器</summary>
    public interface IFileConverter
    {
        /// <summary>能处理的扩展名集合格式为["ext",...]</summary>
        string[] Exts { get; }
        /// <summary>新文件扩展名格式为"ext"</summary>
        string NewExt { get; }
        /// <summary>转换源文件为目标文件</summary>
        bool Execute(string sourceFile, string destFile);
        /// <summary>转换源文件为目标文件</summary>
        Task<bool> ExecuteAsync(string sourceFile, string destFile);
    }

    /// <summary>文件转换器</summary>
    public abstract class FileConverter : IFileConverter
    {
        /// <summary>能处理的扩展名集合</summary>
        public virtual string[] Exts { get { return null; } }
        /// <summary>新文件扩展名</summary>
        public abstract string NewExt { get; }
        /// <summary>获得文件扩展名</summary>
        public string GetExt(string file)
        {
            return file.GetExtension();
        }
        /// <summary>转换源文件为目标文件</summary>
        public virtual bool Execute(string sourceFile, string destFile)
        {
            return Task.Run(() => ExecuteAsync(sourceFile, destFile)).Result;
        }
        /// <summary>转换源文件为目标文件</summary>
        public virtual async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            return await Task.FromResult(Execute(sourceFile, destFile));
        }
    }

    /// <summary>组合文件转换器</summary>
    public abstract class CompositeFileConverter : FileConverter
    {
        /// <summary>构造函数</summary>
        protected CompositeFileConverter(IEnumerable<IFileConverter> converters)
        {
            this.Mappers = new Dictionary<string, IFileConverter>();
            foreach (var converter in converters)
            {
                foreach (var ext in converter.Exts)
                {
                    this.Mappers[ext.ToLowerInvariant()] = converter;
                }
            }
        }
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return this.Mappers.Keys.ToArray(); }
        }
        /// <summary>扩展名转换器映射</summary>
        protected Dictionary<string, IFileConverter> Mappers { get; private set; }
        /// <summary>生成图片文件</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            try
            {
                var ext = GetExt(sourceFile);
                var converter = this.Mappers.GetValueBy(ext);
                if (converter == null)
                {
                    throw new NotSupportedException($"{ext}不支持转换!");
                }
                return await converter.ExecuteAsync(sourceFile, destFile);
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