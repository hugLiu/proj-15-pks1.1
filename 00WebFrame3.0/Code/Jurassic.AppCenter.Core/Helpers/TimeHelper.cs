using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 获取服务器时间的帮助类，它可以使客户机/服务器时间保持一致
    /// </summary>
    public class TimeHelper
    {
        private static DateTime mServerTime;
        private static TimeSpan mTimeDiff; //客户端和服务端的时间差
        private static DateTime mCheckPoint; //上次同步的时间点

        public static bool IsServer { get; set; }
        /// <summary>
        /// 当前服务器时间
        /// </summary>
        public static DateTime ServerTime
        {
            get
            {
                if (IsServer) return DateTime.Now;

                //当自上次同步后超过两分钟时再次从服务器同步时间
                if (Math.Abs((DateTime.Now - mCheckPoint).TotalSeconds) > 120)
                {
                    mCheckPoint = DateTime.Now;
                    //获取服务端系统时间
                    mServerTime = GetTimeFromServer();
                    mTimeDiff = mServerTime - DateTime.Now;
                }
                else
                {
                    //没超过两分钟时用上次计算出的时间差计算一个近似服务器时间
                    mServerTime = DateTime.Now + mTimeDiff;
                }
                return mServerTime;
            }
        }

        /// <summary>
        /// 从数据库服务器获取当前时间。
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTimeFromServer()
        {
            try
            {
                //var timeStr = WebHelper.GetWebResponseTextCore(new Uri(new Uri(XpoFactory.Instance.ServiceUrl), "XPOWS/Time.ashx?r=" + DateTime.Now.Ticks).ToString());
                // return DateTime.Parse(timeStr);
                return DateTime.Now;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
