using Jurassic.Com.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

namespace Jurassic.AppCenter.AppServices
{
    /// <summary>
    /// 用于WCF服务中客户端登录和会话管理的类
    /// </summary>
    public class ClientManager
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public LoginResult LoginResult { get; set; }

        public ClientManager()
        {
        }

        public void Login(string userName, string password)
        {
            UserName = userName;
            Password = password;

            var authService = GetClient<IAuthService>();
            LoginResult = authService.Login();

            if (LoginResult.State == LoginState.OK)
            {
                Password = LoginResult.SessionID;
            }
        }

        public void Logout()
        {
            var authService = GetClient<IAuthService>();
            authService.Logout();
        }
        
        /// <summary>
        /// 获取指定接口的服务的客户端代理
        /// </summary>
        /// <typeparam name="T">服务接口类型</typeparam>
        /// <returns>服务接口的客户端代理对象</returns>
        public T GetClient<T>() where T : class
        {
            //var wcfUrl = new Uri(new Uri(RootUrl), _svcNameDict[typeof(T)]).ToString();
            //if (UserName.IsEmpty())
            //{
            //    return _clientHelper.CreateClient<T>(BindingName, wcfUrl);
            //}
            //var t = _clientHelper.CreateClient<T>(BindingName, wcfUrl, UserName, Password, IdentityName);

            WcfClientBase<T> clientBase = new WcfClientBase<T>();
            clientBase.AppendClientCredentials(UserName, Password);
            return clientBase.GetClientInstance();
        }
    }
}
