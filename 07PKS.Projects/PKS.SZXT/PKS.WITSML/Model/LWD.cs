using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKS.WITSML.Model
{
    /// <summary>
    /// 随钻测井情况数据模型
    /// </summary>
    public class LWD
    {
        /// <summary>
        /// 井号
        /// </summary>
        public string UidWell { get; set; }
        /// <summary>
        /// 子井号
        /// </summary>
        public string UidLog { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 测量深度（斜深）
        /// </summary>
        public double DMEA { get; set; }
        /// <summary>
        /// 1号伽马校正值
        /// </summary>
        public double MG1C { get; set; }
        /// <summary>
        /// 1号电阻率校正值
        /// </summary>
        public double MR1C { get; set; }
        /// <summary>
        /// 2号电阻率校正值
        /// </summary>
        public double MR2C { get; set; }
        /// <summary>
        /// 3号电阻率校正值
        /// </summary>
        public double MR3C { get; set; }
        /// <summary>
        /// 4号电阻率校正值
        /// </summary>
        public double MR4C { get; set; }
    }
}
