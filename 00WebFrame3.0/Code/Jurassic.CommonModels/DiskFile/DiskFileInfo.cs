using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.DiskFile
{
    public class DiskFileInfo
    {
        public DiskFileInfo()
        {
            FileChunkInfo = new FileChunkInfo();
        }

        public Stream FileStream { get; set; }
        
        public FileChunkInfo FileChunkInfo { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }

        public string ContentType { get; set; }

    }

    public class FileChunkInfo
    {
        public FileChunkInfo()
        {
            Size = 0;
            Start = 0;
            End = 0;
        }
        public long Size { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
    }

}
