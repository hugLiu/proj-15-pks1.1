using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class SRC : JsonBase
    {
        public SRC()
        {
            this.AdapterId = string.Empty;
            this.DataSourceName = string.Empty;
            this.DataType = string.Empty;
            this.NatureKey = string.Empty;
            this.Parameters = new UMD();
        }
        public string AdapterId { get; set; }
        public string DataSourceName { get; set; }
        public string DataType { get; set; }
        public string NatureKey { get; set; }
        public UMD Parameters { get; set; }
    }
}
