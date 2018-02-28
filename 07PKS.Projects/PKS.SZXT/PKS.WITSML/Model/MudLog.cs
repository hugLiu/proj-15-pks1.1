using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKS.WITSML.Model
{
    /// <summary>
    /// 录井油气发现数据模型
    /// </summary>
    public class MudLog
    {
        private string[] columns;
        public string[] Columns
        {
            get
            {
                return new string[] {
                    "dTim", 
                    "DRTM", 
                    "MTHA", 
                    "ETHA", 
                    "PRPA", 
                    "IBTA", 
                    "NBTA", 
                    "IPNA", 
                    "NPNA", 
                    "TOTG" 
                };                      
            }
            set
            {
                this.columns = value;
            }
        }

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
        public double LAGDepth { get; set; }
        /// <summary>
        /// 甲烷
        /// </summary>
        public double C1 { get; set; }
        /// <summary>
        /// 乙烷
        /// </summary>
        public double C2 { get; set; }
        /// <summary>
        /// 丙烷
        /// </summary>
        public double C3 { get; set; }
        /// <summary>
        /// 异丁烷
        /// </summary>
        public double IC4 { get; set; }
        /// <summary>
        /// 异戊烷
        /// </summary>
        public double NC4 { get; set; }
        /// <summary>
        /// 新戊烷
        /// </summary>
        public double IC5 { get; set; }
        /// <summary>
        /// 戊烷
        /// </summary>
        public double NC5 { get; set; }
        /// <summary>
        /// 气全量
        /// </summary>
        public double TotalGas { get; set; }

        public bool Check()
        {
            return C1 > 0
                && C2 > 0
                && C3 > 0
                && IC4 > 0
                && NC4 > 0
                && IC5 > 0
                && NC5 > 0
                && TotalGas > 0;
        }

    }
}
