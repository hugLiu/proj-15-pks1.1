using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 定义用于延时处理一些逻辑的处理器工厂
    /// </summary>
    public class ProcesserFactory
    {
        private readonly ConcurrentDictionary<string, ProcesserBase> procTable = new ConcurrentDictionary<string, ProcesserBase>();

        private static Lazy<ProcesserFactory> _factory = new Lazy<ProcesserFactory>(() => new ProcesserFactory());

        /// <summary>
        /// 处理器工厂的单实例
        /// </summary>
        public static ProcesserFactory Instance
        {
            get { return _factory.Value; }
        }

        /// <summary>
        /// 将待处理对象添加进工厂中的指定处理器中
        /// </summary>
        /// <typeparam name="T">待处理对象的类型</typeparam>
        /// <param name="procName">处理器名称</param>
        /// <param name="obj">待处理对象</param>
        public void Add(string procName, object obj)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Add(obj);
        }

        /// <summary>
        /// 启动某个处理器
        /// </summary>
        /// <param name="procName"></param>
        public void Start(string procName)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Start();
        }

        /// <summary>
        /// 停止某个处理器
        /// </summary>
        /// <param name="procName"></param>
        public void Stop(string procName)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Stop();
        }

        /// <summary>
        /// 获取所有处理器列表
        /// </summary>
        /// <returns></returns>
        public IList<ProcesserBase> GetAllProcessers()
        {
            return procTable.Values.ToList();
        }

        /// <summary>
        /// 清除所有处理器
        /// </summary>
        public void ClearAll()
        {
            procTable.Clear();
        }

        /// <summary>
        /// 在工厂中立即处理待处理对象
        /// </summary>
        /// <typeparam name="T">待处理对象的类型</typeparam>
        /// <param name="processerName">处理器名称</param>
        /// <param name="obj">待处理对象</param>
        public void Process(string processerName, object obj)
        {
            var proc = procTable[processerName];
            if (proc == null) return;
            procTable[processerName].Process(obj);
        }

        /// <summary>
        /// 注册新的处理器
        /// </summary>
        /// <typeparam name="T">待处理对象的类型</typeparam>
        /// <param name="processerName">处理器名称</param>
        /// <param name="processer">处理器实例</param>
        public virtual void Register(string processerName, ProcesserBase processer)
        {
            processer.Name = processerName ?? processerName;
            procTable[processer.Name] = processer;
            AfterRegister(processer);
        }

        protected virtual void AfterRegister(ProcesserBase processer)
        {

        }
    }
}