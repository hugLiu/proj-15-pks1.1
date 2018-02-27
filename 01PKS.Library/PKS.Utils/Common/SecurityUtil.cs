using System.Management;

namespace PKS.Utils
{
    /// <summary>安全工具</summary>
    public static class SecurityUtil
    {
        #region 转换方法
        /// <summary>获本机域名</summary>
        public static string GetLocalDomainName()
        {
            var query = new SelectQuery("Win32_ComputerSystem");
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    if ((bool)mo["partofdomain"]) return mo["domain"].ToString();
                }
            }
            return null;
        }
        /// <summary>本机是否在域中</summary>
        public static bool LocalIsInDomain()
        {
            var domainName = GetLocalDomainName();
            return domainName.IsNullOrEmpty();
        }
        #endregion
    }
}
