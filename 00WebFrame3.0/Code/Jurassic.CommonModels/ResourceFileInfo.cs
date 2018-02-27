using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels
{
    public class ResourceFileInfo
    {
        public ResourceFileInfo()
        {
            StartPos = 0;
            EndPos = 0;
            CatalogId = 0;
        }

        public int Id { get; set; }

        public Stream FileStream { get; set; }
        
        public StreamChunkInfo StreamChunkInfo { get; set; }

        public long EndPos { get; set; }

        public long StartPos { get; set; }

        public string FileName { get; set; }


        public long FileSize { get; set; }

        public string FileSizeStr
        {
            set { FileSize = Convert.ToInt32(value); }
        }

        public string ContentType { get; set; }

        public string FileKey { get; set; }

        public string MD5Code { get; set; }

        public string UserId { get; set; }
        /// <summary>
        /// 所属目录Id
        /// </summary>
        public int CatalogId { get; set; }

        public string Keywords { get; set; }

        public string Abstract { get; set; }

        public DateTime CreateTime { set; get; }

        public int ArticleId { get; set; }
    }

    public class StreamChunkInfo
    {
        public long Size { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
    }

}
