using System;
namespace Jurassic.AppCenter.Logs
{
    public interface IJLogManager
    {
        IJLog GetLogger(string name);
    }
}
