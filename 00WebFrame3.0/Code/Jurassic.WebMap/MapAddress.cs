using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebMap
{
    /// <summary>
    /// 带地图信息的地址
    /// </summary>
    public class MapAddress
    {
        /// <summary>
        /// 起始点经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 起始点纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 目标点经度
        /// </summary>
        public double TargetLongitude { get; set; }

        /// <summary>
        /// 目标点纬度
        /// </summary>
        public double TargetLatitude { get; set; }

        /// <summary>
        /// 文本地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 离目标的距离
        /// </summary>
        public double Distance { get; set; }
    }
}