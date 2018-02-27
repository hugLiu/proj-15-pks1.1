using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.Models
{
    /// <summary>
    /// 自动更新服务向客户端传递的文件信息
    /// </summary>
    public class NetFileInfo
    {
        /// <summary>
        /// 服务端自动更新目录的默认名称
        /// </summary>
        public const string DefaultServerDir = "UpdateFiles";

        /// <summary>
        /// 文件全名
        /// </summary>
        public string FullName;

        /// <summary>
        /// 文件大小(字节数)
        /// </summary>
        public long Length;

        /// <summary>
        /// 文件改写时间
        /// </summary>
        public DateTime UpdateTime;
    }
}
