using PKS.Models;
using System.Collections.Generic;
using System.Security.Principal;
using PKS.Core.Models.ADIdentity;

namespace PKS.Core
{
    /// <summary>AD身份接口</summary>
    public interface IADIdentityService
    {
        /// <summary>是否自动注册域用户</summary>
        bool AutoRegisterUser { get; }
        /// <summary>验证凭据</summary>
        bool ValidateCredentials(WindowsIdentity identity);
        /// <summary>验证凭据</summary>
        OperationResult ValidateCredentials(string domain, string userName, string password);
        /// <summary>获得用户组Id</summary>
        string GetUserGroupId(string userName);
        /// <summary>同步域的组织结构</summary>
        List<AdDept> GetDepts(List<string> ouNames);
    }
}
