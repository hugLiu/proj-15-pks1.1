using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 对Forms身份验证的扩展类，使用此类进行权限验证可比使用AppManager中更简洁一些。
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <returns>用户ID</returns>
        public static string GetUserId(this IIdentity identity)
        {
            var user = AppManager.Instance.UserManager.GetByName(identity.Name);
            return user == null ? "" : user.Id;
        }

        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <returns>用户名称</returns>
        public static string GetUserName(this IIdentity identity)
        {
            return identity.Name;
        }

        /// <summary>
        /// 判断当前用户是否有某地址的访问权限
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <param name="location">URL地址(不带http://)</param>
        /// <param name="method">GET或POST</param>
        /// <returns>有或没有</returns>
        public static bool HasRight(this IIdentity identity, string location, WebMethod method = WebMethod.GET)
        {
            return AppManager.Instance.HasRight(identity.Name, location, method);
        }

        /// <summary>
        /// 判断当前用户是否有权限ID的权限
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <param name="functionId">功能ID</param>
        /// <returns>有或没有</returns>
        public static bool HasRightId(this IIdentity identity, string functionId)
        {
            return AppManager.Instance.HasRightId(identity.Name, functionId);
        }

        /// <summary>
        /// 获取当前用户无权限的功能ID列表
        /// </summary>
        /// <param name="identity">用户标识</param>
        /// <returns>功能ID列表</returns>
        public static IEnumerable<string> GetForbiddenIds(this IIdentity identity)
        {
            return AppManager.Instance.GetForbiddenIds(identity.Name);
        }
    }
}
