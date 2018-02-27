using System;
using System.Globalization;
using CacheManager.Core.Utility;
using PKS.Utils;

namespace CacheManager.Logging
{
#pragma warning disable SA1600
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NLogLoggerFactoryAdapter : Core.Logging.ILoggerFactory
    {
        public CacheManager.Core.Logging.ILogger CreateLogger(string categoryName)
        {
            return new NLogLoggerAdapter(NLog.LogManager.GetLogger(categoryName));
        }

        public CacheManager.Core.Logging.ILogger CreateLogger<T>(T instance)
        {
            return new NLogLoggerAdapter(NLog.LogManager.GetLogger(typeof(T).FullName));
        }
    }

    internal class NLogLoggerAdapter : CacheManager.Core.Logging.ILogger
    {
        private readonly NLog.ILogger _logger;

        public NLogLoggerAdapter(NLog.ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
        }

        public IDisposable BeginScope(object state)
        {
            return null;
        }

        public bool IsEnabled(CacheManager.Core.Logging.LogLevel logLevel)
        {
            return logLevel > CacheManager.Core.Logging.LogLevel.Information && _logger.IsEnabled(GetExternalLogLevel(logLevel));
        }

        public void Log(CacheManager.Core.Logging.LogLevel logLevel, int eventId, object message, Exception exception)
        {
            if (eventId == 0) return;
            var message2 = eventId.ToString() + ":";
            message2 += message == null ? string.Empty : message.ToString();
            _logger.Log(GetExternalLogLevel(logLevel), exception, message2);
        }

        private static NLog.LogLevel GetExternalLogLevel(CacheManager.Core.Logging.LogLevel level)
        {
            switch (level)
            {
                case CacheManager.Core.Logging.LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case CacheManager.Core.Logging.LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case CacheManager.Core.Logging.LogLevel.Information:
                    return NLog.LogLevel.Info;
                case CacheManager.Core.Logging.LogLevel.Warning:
                    return NLog.LogLevel.Warn;
                case CacheManager.Core.Logging.LogLevel.Error:
                    return NLog.LogLevel.Error;
                case CacheManager.Core.Logging.LogLevel.Critical:
                    return NLog.LogLevel.Fatal;
            }

            return NLog.LogLevel.Off;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1600
}