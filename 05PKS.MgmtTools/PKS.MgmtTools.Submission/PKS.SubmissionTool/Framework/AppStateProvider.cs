using System;
using Jurassic.AppCenter;
using Jurassic.AppCenter.Models;

namespace PKS.SubmissionTool
{
    public class AppStateProvider : IStateProvider
    {
        public AppStateProvider()
        {
        }

        public virtual LoginState Login(LoginModel model)
        {
            return LoginState.OK;
        } 

        public virtual LoginState SendPasswordResetMessage(string userName, string email, string resetUrl)
        {
                return LoginState.OK;
        }

        public virtual LoginState ResetPassword(PasswordResetModel model)
        {
            return LoginState.OK;
        }

        public virtual LoginState ChangePassword(PasswordChangeModel model)
        {
            return LoginState.OK;
        }

        public virtual void Logout(string userName)
        {
            //WebSecurity.Logout();
        }

        /// <summary>
        /// 指定登录过期时间(以秒为单位)
        /// </summary>
        public virtual int Timeout
        {
            get { return (int)TimeSpan.FromDays(1).TotalSeconds; }
        }
    }
}