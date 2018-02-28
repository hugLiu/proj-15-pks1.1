using System;
using Jurassic.AppCenter;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;

namespace PKS.SubmissionTool
{
    public class LoginService : ILoginService
    {
        public AppUser Login()
        {
            var loginer = new AppUser();
            loginer.Id = "0";
            loginer.Name = "pmis";
            loginer.Password = "888888";
            loginer.IsDefaultRole = true;
            loginer.IsOnline = true;
            loginer.LastOpTime = DateTime.Now;
            return loginer;
        }
    }
}
