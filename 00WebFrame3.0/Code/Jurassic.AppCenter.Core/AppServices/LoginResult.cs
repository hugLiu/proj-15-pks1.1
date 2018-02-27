using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.AppServices
{
    /// <summary>
    /// 用于在WCF服务中给客户端呈现登录结果，并返回用户有权限的菜单和角色列表
    /// 这是为了减少登录请求次数，所以把相关数据一并返回
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 用户对象
        /// </summary>
        public AppUser User { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public LoginState State { get; set; }

        /// <summary>
        /// 会话ID，下次请求要带上以识别身份
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 用户可见的菜单
        /// </summary>
        public IList<AppFunction> UserMenus { get; set; }

        /// <summary>
        /// 用户的角色列表
        /// </summary>
        public IList<AppRole> Roles { get; set; }
    }
}
