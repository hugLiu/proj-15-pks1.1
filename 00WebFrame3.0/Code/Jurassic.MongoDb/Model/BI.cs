using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public class BI : JsonBase
    {
        public BI()
        {
            this.BD = new List<string>();
            this.BP = new List<string>();
            this.BA = new List<string>();
            this.BT = new List<string>();
        }
        /// <summary>
        /// 业务域
        /// </summary>
        public List<string> BD { get; set; }
        /// <summary>
        /// 业务过程
        /// </summary>
        public List<string> BP { get; set; }
        /// <summary>
        /// 业务活动
        /// </summary>
        public List<string> BA { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public List<string> BT { get; set; }
    }
}
