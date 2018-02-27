using PKS.Core.Models.ADIdentity;
using PKS.Models;
using PKS.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace PKS.Core
{
    /// <summary>AD身份认证接口</summary>
    public class ADIdentityService : IADIdentityService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public ADIdentityService()
        {
            var values = ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_WindowsAuth].Split(',');
            var index = 0;
            this.DSContextType = values[index++].ToEnum<ContextType>();
            this.User = values[index++];
            this.Pwd = values[index++];
            this.AutoRegisterUser = values[index++] == "1";
            this.Domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }
        /// <summary>目录服务上下文类型</summary>
        public ContextType DSContextType { get; private set; }
        /// <summary>是否自动注册域用户</summary>
        public bool AutoRegisterUser { get; private set; }
        /// <summary>AD域名</summary>
        public string Domain { get; private set; }
        /// <summary>某个域用户</summary>
        public string User { get; private set; }
        /// <summary>某个域用户密码</summary>
        public string Pwd { get; private set; }

        /// <summary>登录认证</summary>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>验证凭据</summary>
        public bool ValidateCredentials(WindowsIdentity identity)
        {
            using (var principalContext = new PrincipalContext(this.DSContextType))
            {
                using (var userPrincipal = UserPrincipal.FindByIdentity(principalContext, identity.Name))
                {
                    return userPrincipal != null;
                }
            }
        }
        /// <summary>验证凭据</summary>
        public OperationResult ValidateCredentials(string domain, string userName, string password)
        {
            var result = new OperationResult();
            var phToken = IntPtr.Zero;
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;
            if (domain.IsNullOrEmpty()) domain = this.Domain;
            result.Succeed = LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref phToken);
            if (!result.Succeed)
            {
                var ex = new Win32Exception();
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        /// <summary>登录认证</summary>
        public List<string> ADLogin(string domain, string userName, string password)
        {
            using (var entry = new DirectoryEntry(@"LDAP://" + domain, userName, password, AuthenticationTypes.None))
            {
                var properties = new List<string>();
                var searcher = new DirectorySearcher(entry);
                searcher.Filter = $"(&(&(objectCategory=person)(objectClass=user))(SAMAccountName={userName}))";
                searcher.PropertiesToLoad.Add("cn");
                searcher.SearchRoot = entry;
                searcher.SearchScope = SearchScope.Subtree;
                var result = searcher.FindOne();
                if (result == null)
                {
                    ExceptionCodes.LoginFailed.ThrowUserFriendly("登录失败！", "AD域登录失败！");
                }
                var logonEntry = result.GetDirectoryEntry();
                foreach (var property in logonEntry.Properties.PropertyNames)
                {
                    var propName = property.ToString();
                    properties.Add($"{propName}:{logonEntry.Properties[propName].Cast<object>().First().ToString()}");
                }
                return properties;
            }
        }
        /// <summary>获得用户组Id</summary>
        public string GetUserGroupId(string userName)
        {
            using (var entry = new DirectoryEntry(@"LDAP://" + Domain, User, Pwd, AuthenticationTypes.Secure))
            {
                var searcher = new DirectorySearcher(entry);
                searcher.Filter = "(&(&(objectCategory=person)(objectClass=user))(SAMAccountName=" + userName + "))";
                searcher.PropertiesToLoad.Add("cn");
                searcher.SearchRoot = entry;
                searcher.SearchScope = SearchScope.Subtree;
                var result = searcher.FindOne();
                if (result == null)
                {
                    return "";
                }
                var logonEntry = result.GetDirectoryEntry();
                return logonEntry.Parent.NativeGuid;
            }
        }

        /// <summary>
        /// 同步部门
        /// </summary>
        /// <param name="ouNames"></param>
        public List<AdDept> GetDepts(List<string> ouNames)
        {
            var depts = new List<AdDept>();
            try
            {
                DirectoryEntry rootOU;

                var domain = Connected(Domain, User, Pwd);
                if (IsExistOU(domain, out rootOU, ouNames, depts))
                {
                    Sync(rootOU, depts);
                    depts[0].OriginalPId = "";
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException();
            }
            return depts;
        }


        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="root">根</param>
        private void Sync(DirectoryEntry root, List<AdDept> depts)
        {
            DirectorySearcher mySearcher = new DirectorySearcher(root, "(objectclass=organizationalUnit)"); //查询组织单位                 

            DirectoryEntry rootOu = mySearcher.SearchRoot;   //查找根OU

            SyncRootOU(rootOu, depts);
        }

        /// <summary>
        /// 同步根部门
        /// </summary>
        /// <param name="entry"></param>
        private void SyncRootOU(DirectoryEntry entry, List<AdDept> depts)
        {
            if (entry.Properties.Contains("ou") && entry.Properties.Contains("objectGUID"))
            {
                SyncSubOU(entry, depts);
            }
        }

        /// <summary>
        /// 递归，同步根部门的所有下级部门
        /// </summary>
        /// <param name="entry"></param>
        private void SyncSubOU(DirectoryEntry entry, List<AdDept> depts)
        {
            foreach (DirectoryEntry subEntry in entry.Children)
            {
                bool isExist = depts.Exists(d => d.OriginalId == subEntry.NativeGuid);
                if (!isExist)
                {
                    switch (subEntry.SchemaClassName)
                    {
                        case "organizationalUnit":
                            Add(subEntry, depts);
                            SyncSubOU(subEntry, depts);
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        /// <summary>
        /// 检查是否连接到域
        /// </summary>
        /// <param name="domainName">域名或IP</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <param name="domain">域</param>
        /// <returns></returns>
        private DirectoryEntry Connected(string domainName, string userName, string userPwd)
        {
            var domain = new DirectoryEntry();
            domain.Path = string.Format("LDAP://{0}", domainName);
            domain.Username = userName;
            domain.Password = userPwd;
            domain.AuthenticationType = AuthenticationTypes.Secure;
            domain.RefreshCache();
            return domain;
        }

        /// <summary>
        /// 检查域中是否有当前部门
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ou"></param>
        /// <returns></returns>
        private bool IsExistOU(DirectoryEntry entry, out DirectoryEntry ou, List<string> ouNames, List<AdDept> depts)
        {
            ou = entry;
            int i = 0;
            while (i < ouNames.Count)
            {
                ou = ou.Children.Find("OU=" + ouNames[i]);
                Add(ou, depts);
                i++;
            }
            return (ou != null);
        }

        private void Add(DirectoryEntry entry, List<AdDept> depts)
        {
            bool isExist = depts.Exists(d => d.OriginalId == entry.NativeGuid);
            if (!isExist)
            {
                var arr = entry.Name.Split('=');
                var names = arr[1].Split('-');
                var name = names[0];
                depts.Add(new AdDept(name, entry.NativeGuid, entry.Parent.NativeGuid));
            }
        }

    }
}
