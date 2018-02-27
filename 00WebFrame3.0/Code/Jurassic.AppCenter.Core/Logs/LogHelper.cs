using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Data.Common;
using System.Text;
using System.Data;
using System.Configuration;
using Jurassic.Com.Tools;
using System.Threading;
using System.Reflection;

namespace Jurassic.AppCenter.Logs
{
    /// <summary>
    /// 系统日志帮助类
    /// </summary>
    public static class LogHelper
    {
        static IJLog log;
        static IJLogManager mLogManager;

        public static void Init(IJLogManager logManager, string logName)
        {
            mLogManager = logManager;
            log = mLogManager.GetLogger(logName);
        }

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="ex"></param>
        public static void Write(JLogInfo logInfo, Exception ex = null)
        {
            //修改人：卢英杰
            //修改于: 2015.8.26。
            //原因：发现写入日志文件的时候有许多空日志，所以加入一些判断来限制空日志信息的产生。
            //if (log != null)   原方法
            if (log != null && logInfo != null && !string.IsNullOrWhiteSpace(logInfo.ActionName) 
                && !string.IsNullOrWhiteSpace(logInfo.ModuleName))
                log.Write(logInfo, CommOp.ToEnum<JLogType>(logInfo.LogType), ex);
        }

        /// <summary>   
        /// 根据 User Agent 获取操作系统名称   
        /// </summary>   
        public static string GetOSNameByUserAgent(string userAgent)
        {
            if (String.IsNullOrEmpty(userAgent)) return userAgent;
            userAgent = userAgent.ToLower();
            if (userAgent.Contains("nt 6.1"))
            {
                return "Windows 7";
            }
            if (userAgent.Contains("nt 5.1"))
            {
                return "Windows XP";
            }
            if (userAgent.Contains("nt 5.2"))
            {
                return "Windows Server 2003";
            }
            if (userAgent.Contains("nt 6.0"))
            {
                return "Vista/Server 2008";
            }
            if (userAgent.Contains("nt 6.2"))
            {
                return "Windows 8";
            }
            if (userAgent.Contains("nt 6."))
            {
                return "Windows 8.1";
            }
            if (userAgent.Contains("nt 5"))
            {
                return "Windows 2000";
            }
            if (userAgent.Contains("nt 10."))
            {
                return "Windows 10";
            }
            if (userAgent.Contains("ipad"))
            {
                return "iPad";
            }
            if (userAgent.Contains("iphone"))
            {
                return "iPhone";
            }
            if (userAgent.Contains("android"))
            {
                return "android";
            }
            if (userAgent.Contains("mac"))
            {
                return "Mac";
            }
            if (userAgent.Contains("unix"))
            {
                return "UNIX";
            }
            if (userAgent.Contains("linux"))
            {
                return "Linux";
            }
            if (userAgent.Contains("nt 4"))
            {
                return "Windows NT4";
            }
            if (userAgent.Contains("me"))
            {
                return "Windows Me";
            }
            if (userAgent.Contains("98"))
            {
                return "Windows 98";
            }
            if (userAgent.Contains("95"))
            {
                return "Windows 95";
            }
            if (userAgent.Contains("sunos"))
            {
                return "SunOS";
            }
            return "Unknown";
        }
    }
}
