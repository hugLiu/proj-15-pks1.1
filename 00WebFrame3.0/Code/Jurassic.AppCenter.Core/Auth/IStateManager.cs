using Jurassic.AppCenter.AppServices;
using System;

namespace Jurassic.AppCenter
{
    public interface IStateManager
    {
        void Add(LoginResult result);
        LoginResult GetLoginResult(string sessionId);
        bool SingleLogin { get; set; }
    }
}
