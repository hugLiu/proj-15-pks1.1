using Jurassic.AppCenter.Logs;
using Jurassic.AppCenter.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 自定义异常类，用于主动抛出的异常
    /// </summary>
    public class JException : InvalidOperationException
    {
        public JException(Exception ex)
            : base(ResHelper.GetStr(ex.Message), ex)
        {
        }

        public JException(string message)
            : base(ResHelper.GetStr(message))
        {
        }
    }
}
