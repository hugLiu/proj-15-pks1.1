using System;
namespace Jurassic.AppCenter.Logs
{
    public interface IJLog
    {
        void Write(JLogInfo logInfo, JLogType logType, Exception ex);
    }
}
