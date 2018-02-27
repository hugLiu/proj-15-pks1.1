using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 方法执行的参数信息类
    /// </summary>
    public class AppParameter :IEquatable<AppParameter>
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数能接收的值的模式描述,使用正则表达式
        /// </summary>
        public string ValuePattern { get; set; }

        public bool Equals(AppParameter other)
        {
            return other.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
