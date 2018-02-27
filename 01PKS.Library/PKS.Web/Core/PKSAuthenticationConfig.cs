using PKS.Models;
using PKS.Utils;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Web.Configuration;

namespace PKS.Web.MVC
{
    /// <summary>认证配置</summary>
    public class PKSAuthenticationConfig
    {
        /// <summary>初始化</summary>
        public void Initialize()
        {
            var mode = AuthenticationSection.Mode;
            if (mode == AuthenticationMode.Windows)
            {
                this.AuthenticationType = AuthenticationType.Windows;
            }
            else
            {
                this.AuthenticationType = AuthenticationType.Forms;
                var enableMixedAuthentication = ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_MixedAuth];
                if (!enableMixedAuthentication.IsNullOrEmpty() && bool.Parse(enableMixedAuthentication))
                {
                    this.AuthenticationType = AuthenticationType.Mixed;
                }
            }
        }
        /// <summary>获得授权节</summary>
        private static AuthenticationSection AuthenticationSection
        {
            get { return ConfigurationManager.GetSection("system.web/authentication").As<AuthenticationSection>(); }
        }
        /// <summary>授权认证类型</summary>
        public AuthenticationType AuthenticationType { get; private set; }
        /// <summary>是否Windows授权认证</summary>
        public bool IsWindowsAuthentication
        {
            get { return AuthenticationType == AuthenticationType.Windows; }
        }
        /// <summary>是否Forms授权认证</summary>
        public bool IsFormsAuthentication
        {
            get { return AuthenticationType == AuthenticationType.Forms; }
        }
        /// <summary>是否混合授权认证</summary>
        public bool IsMixedAuthentication
        {
            get { return AuthenticationType == AuthenticationType.Mixed; }
        }
    }
}