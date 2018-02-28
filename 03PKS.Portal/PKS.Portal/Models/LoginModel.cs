namespace PKS.Models
{
    /// <summary>登录数据</summary>
    public class LoginModel
    {
        /// <summary>用户名</summary>
        public string UserName { get; set; }
        /// <summary>密码</summary>
        public string Password { get; set; }
        /// <summary>记住我</summary>
        public bool RememberMe { get; set; }
        /// <summary>登录成功后跳转Url</summary>
        public string ReturnUrl { get; set; }
    }
}