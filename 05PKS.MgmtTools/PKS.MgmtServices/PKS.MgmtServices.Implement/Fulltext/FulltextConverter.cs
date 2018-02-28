using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>全文转换器</summary>
    public abstract class FulltextConverter : FileConverter, IFulltextConverter
    {
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "txt"; }
        }
        /// <summary>转换源文件为目标文件</summary>
        public override bool Execute(string sourceFile, string destFile)
        {
            return Execute(sourceFile, null, destFile);
        }
        /// <summary>转换源文件为目标文件</summary>
        public override async Task<bool> ExecuteAsync(string sourceFile, string destFile)
        {
            return await ExecuteAsync(sourceFile, null, destFile);
        }
        /// <summary>生成全文</summary>
        public virtual bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            throw new NotImplementedException();
        }
        /// <summary>生成全文</summary>
        public async Task<bool> ExecuteAsync(string sourceFile, string pdfFile, string destFile)
        {
            return await Task.FromResult(Execute(sourceFile, pdfFile, destFile));
        }
        /// <summary>写全文文件</summary>
        protected bool WriteFile(string destFile, string content, bool clearSymbols = false)
        {
            if (clearSymbols)
            {
                var pattern = @"[\s\.,　。，]{2,}";
                content = Regex.Replace(content, pattern, Environment.NewLine);
            }
            File.WriteAllText(destFile, content, Encoding.UTF8);
            return true;
        }
    }
}