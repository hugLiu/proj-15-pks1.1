using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.SysFrame.Model
{
    /// <summary>
    /// 用户认证Session缓存模型
    /// </summary>
    [Serializable]
    public class SessionCacheItem
    {
        public string AppKey { get; set; }

        public string UserName { get; set; }

        public DateTime InvalidTime { get; set; }
    }
}
