using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>GDB转图片文件转换器</summary>
    public class GDBToImageConverter : ImageConverter
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "gdb", "gdbx" }; }
        }
        /// <summary>转换源文件为目标文件</summary>
        public override bool Execute(string sourceFile, string destFile, Size size)
        {
            var path = Path.GetDirectoryName(destFile).NormalizePath().TrimEnd(Path.DirectorySeparatorChar);
            var bmpFile = $@"{path}\{Guid.NewGuid().ToString()}.bmp";
            var thread = new Thread(Convert);
            thread.SetApartmentState(ApartmentState.STA);
            var state = new List<object>() { sourceFile, bmpFile };
            thread.Start(state);
            thread.Join();
            if (state.Count == 3)
            {
                throw state[2].As<Exception>();
            }
            if (!File.Exists(bmpFile)) return false;
            return base.Execute(bmpFile, destFile, size);
        }
        /// <summary>转换</summary>
        private void Convert(object state)
        {
            var state2 = state.As<List<object>>();
            var sourceFile = state2[0].ToString();
            var bmpFile = state2[1].ToString();
            frmGDBToBmp form = null;
            try
            {
                //form = new frmGDBToBmp();
                //var joGis = form.axJoGIS4;
                //var isok = joGis.LoadMapFile(sourceFile);
                //if (!isok) return;
                //joGis.ExportBMP(bmpFile);
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                state2.Add(ex);
            }
            finally
            {
                if (form != null)
                {
                    //form.axJoGIS4.Dispose();
                    form.Dispose();
                }
            }
        }
    }
}