using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Jurassic.AppCenter.AppServices
{
    /// <summary>
    /// 一个用于支持WCF的用户登录服务契约
    /// </summary>
    [ServiceContract]
    [ServiceKnownType("GetAppTypes", typeof(AppManager))]
    public interface IAuthService
    {
        [OperationContract]
        LoginResult Login();

        [OperationContract]
        void Logout();
    }
}
