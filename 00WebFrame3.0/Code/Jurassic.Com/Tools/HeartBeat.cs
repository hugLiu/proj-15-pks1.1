using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 利用定时器实现的简单心跳
    /// </summary>
    public class HeartBeat
    {
        System.Timers.Timer timer;
        bool inprocess = false;
        public string Name { get; set; }

        /// <summary>
        /// 根据周期（秒数）新建心跳对象
        /// </summary>
        /// <param name="interval">周期（秒数）</param>
        public HeartBeat(string name, int interval, Action beat, Action<Exception> error = null)
        {
            this.Name = name;
            Interval = interval;
            timer = new System.Timers.Timer(interval * 1000);
            timer.Elapsed += (s, e) =>
            {
                if (beat != null && !inprocess)
                {
                    inprocess = true;
                    try
                    {
                        beat();
                        LastTime = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        if (error != null) error(ex);
                    }
                }
                inprocess = false;
            };
        }

        DateTime LastTime;
        public override string ToString()
        {
            return String.Format("{0}:{1}s={2},{3}", Name, Interval, Enabled, LastTime);
        }

        /// <summary>
        /// 心跳是否在进行中
        /// </summary>
        public bool Enabled
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }

        /// <summary>
        /// 开始心跳
        /// </summary>
        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// 停止心跳
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }


        /// <summary>
        /// 心跳时间间隔，以秒为单位
        /// </summary>
        int Interval
        {
            get;
            set;
        }
    }
}
