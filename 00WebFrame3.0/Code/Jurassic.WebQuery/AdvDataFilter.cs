using Jurassic.AppCenter;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 用于在AdvData表单显示前、保存前和保存后插入自定义逻辑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdvDataFilter<T> where T : class, IId<int>
    {
        /// <summary>
        /// 调用的控制器
        /// </summary>
        public BaseController Controller { get; set; }

        /// <summary>
        /// 保存前的操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool BeforeSaving(T t)
        {
            return true;
        }

        /// <summary>
        /// 成功保存后的操作
        /// </summary>
        /// <param name="t"></param>
        public virtual void AfterSaved(T t)
        {
        }

        /// <summary>
        /// 显示表单前的操作
        /// </summary>
        /// <param name="t"></param>
        public virtual void BeforeShowing(T t)
        {
        }
    }
}