using Jurassic.AppCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 为系统登录和退出登录提供接口
    /// </summary>
    public interface IStateProvider
    {
        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="model">登录数据实体</param>
        /// <returns>登录状态</returns>
        LoginState Login(LoginModel model);

        /// <summary>
        /// 设置授权Cookie
        /// </summary>
        /// <param name="model">登录数据实体</param>
        bool SetAuthCookie(LoginModel model);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userName">用户名</param>
        void Logout(string userName);

        /// <summary>
        /// 发送密码重置信息（如邮件）
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="resetUrl"></param>
        /// <returns></returns>
        LoginState SendPasswordResetMessage(string userName, string email, string resetUrl);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        LoginState ResetPassword(PasswordResetModel model);

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        LoginState ChangePassword(PasswordChangeModel model);
        
        /// <summary>
        ///  指定登录过期时间
        /// </summary>
        int Timeout { get; }
    }

    /// <summary>
    /// 表示登录状态的枚举
    /// </summary>
    public enum LoginState
    {
        /// <summary>
        /// 默认值,无状态
        /// </summary>
        UnKnown,

        /// <summary>
        /// 登录成功
        /// </summary>
        OK,

        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError,

        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist,

        /// <summary>
        /// 用户不存在或密码错误
        /// </summary>
        UserOrPasswordError,

        /// <summary>
        /// 帐户被锁定
        /// </summary>
        AccoutLocked,
        TokenError,
        EmailError,
        EmailSendError,
        OldPasswordError,
        OtherSessionLogined
    }
}
