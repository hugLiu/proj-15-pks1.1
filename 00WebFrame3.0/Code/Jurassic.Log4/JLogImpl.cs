using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using Jurassic.AppCenter.Logs;

namespace Jurassic.Log4
{
    /// <summary>
    /// 基于Log4的日志实现
    /// </summary>
    public class JLogImpl : LogImpl, IJLog
    {
        /// <summary>
        /// The fully qualified name of this declaring type not the type of any subclass.
        /// </summary>
        private readonly static Type ThisDeclaringType = typeof(JLogImpl);

        public JLogImpl(ILogger logger) 
            : base(logger)
        {
        }

        public void Write(JLogInfo logInfo, JLogType logType, Exception ex)
        {
            Level level = ConvertToLevel(logType);
            LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository,
                                   Logger.Name, level, logInfo.Message, ex);
            if (ex != null)
            {
                logInfo.Message = ex.Message + ex.StackTrace;
            }

            foreach (var property in logInfo.GetType().GetProperties())
            {
                loggingEvent.Properties[property.Name] = property.GetValue(logInfo, null);
            }
            Logger.Log(loggingEvent);
        }

        static Level ConvertToLevel(JLogType logType)
        {
            switch (logType)
            {
                case JLogType.Info:
                    return Level.Info;
                case JLogType.Fatal:
                    return Level.Fatal;
                case JLogType.Error:
                    return Level.Error;
                case JLogType.Debug:
                    return Level.Debug;
                case JLogType.Warning:
                    return Level.Warn;
                default:
                    return Level.All;
            }
        }
    }
}