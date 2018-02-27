using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Jurassic.Com.Tools;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections;

namespace Jurassic.AppCenter
{

    /// <summary>
    /// 定义可以排队处理的一些业务操作的基类
    /// </summary>
    public abstract class ProcesserBase
    {
        ConcurrentQueue<object> _itemsQueue = new ConcurrentQueue<object>();
        private int _processed = 0;

        public ProcesserBase()
        {
            Enabled = true;
        }

        public virtual bool Enabled { get; set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 已经处理的项数目
        /// </summary>
        public int Processed { get { return _processed; } }

        /// <summary>
        /// 所有项数目
        /// </summary>
        public int Total { get { return _processed + _itemsQueue.Count; } }

        /// <summary>
        /// 剩余的项数目
        /// </summary>
        public int Remain { get { return _itemsQueue.Count; } }

        /// <summary>
        /// 其他信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 完成百分比
        /// </summary>
        public int ProcessedPercent { get { return Total == 0 ? 0 : Processed * 100 / Total; } }

        /// <summary>
        /// 报告进度的委托
        /// </summary>
        public event EventHandler ProgressChanged;

        /// <summary>
        /// 在处理器中添加一个元素并开始处理
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            _itemsQueue.Enqueue(item);
            if (!_inProcess && Enabled)
            {
                _inProcess = true;
                Start();
            }
        }

        ~ProcesserBase()
        {
            if (!_inProcess)
            {
                ProcessCall(null);
            }
        }

        /// <summary>
        /// 待处理资讯出队
        /// </summary>
        /// <returns></returns>
        Object DeQueue()
        {
            object item = default(object);
            _itemsQueue.TryDequeue(out item);
            return item;
        }

        bool _inProcess = false;
        void ProcessCall(Object stateInfo)
        {
            object item = default(object);
            try
            {
                //http://www.cnblogs.com/happyhippy/archive/2011/06/21/2086441.html
                while (!EqualityComparer<object>.Default.Equals(item = DeQueue(), default(object)) && Enabled)
                {
                    Process(item);
                    _processed++;
                    if (ProgressChanged != null)
                    {
                        ProgressChanged(this, EventArgs.Empty);
                    }
                }
            }
            finally
            {
                _inProcess = false;
            }
        }

        /// <summary>
        /// 实际用于处理item的方法
        /// </summary>
        /// <param name="item"></param>
        public abstract void Process(object item);


        internal virtual void Start()
        {
            Enabled = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessCall));
        }

        internal virtual void Stop()
        {
            Enabled = false;
        }
    }

    /// <summary>
    /// 用于定时执行某些任务的处理器
    /// </summary>
    public abstract class TimerProcesser : ProcesserBase
    {
        System.Timers.Timer _timer = new System.Timers.Timer();

        private double _interval;
        /// <summary>
        /// 定时器间隔的毫秒数
        /// </summary>
        public double Interval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                _timer.Interval = value;
            }
        }

        public TimerProcesser()
            : this(60000)
        {
        }

        public TimerProcesser(double interval)
        {
            Interval = interval;
            _timer.Elapsed += _timer_Elapsed;
        }



        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Remain == 0)
            {
                var data = LoadData();
                foreach (object obj in data)
                {
                    Add(obj);
                }
            }
            Start();
        }

        public override bool Enabled
        {
            get
            {
                return _timer.Enabled;
            }
            set
            {
                _timer.Enabled = value;
            }
        }

        /// <summary>
        /// 当待处理队列为空时，从数据库或其他方法获取待处理数据的方法
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable LoadData();
    }
}