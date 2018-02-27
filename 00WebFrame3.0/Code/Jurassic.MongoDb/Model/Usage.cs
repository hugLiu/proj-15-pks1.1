using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public class Usage : JsonBase
    {
        public Usage()
        {
            this.Media = string.Empty;
            this.SRC = new SRC();
            this.View = string.Empty;
            this.Format = string.Empty;
        }
        public string Media { get; set; }
        public SRC SRC { get; set; }
        public string View { get; set; }
        public string Format { get; set; }
    }
}
