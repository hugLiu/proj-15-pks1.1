using System.Configuration;

namespace PKS.Models
{
    /// <summary>WEB常数</summary>
    public static class PKSWebConsts
    {
        #region 调试常数
        /// <summary>调试用Token</summary>
        public static readonly string Token_Debug = "debug";
        #endregion

        #region 缓存过期时间常数
        /// <summary>滑动过期时间,单位为分,用于内部缓存,默认为20分钟</summary>
        public static int SlidingExpireInterval = 20;
        #endregion

        #region AppSettings常数
        /// <summary>配置常数_子系统</summary>
        public static readonly string AppSettings_SubSystem = "PKS_SubSystem";
        /// <summary>获得子系统代码</summary>
        public static string GetSubSystemCode()
        {
            return ConfigurationManager.AppSettings[PKSWebConsts.AppSettings_SubSystem];
        }
        /// <summary>配置常数_缩略图默认尺寸</summary>
        public static readonly string AppSettings_ThumbnailDefaultSize = "ThumbnailDefaultSize";
        /// <summary>配置常数_图片最大尺寸</summary>
        public static readonly string AppSettings_ImageMaxSize = "ImageMaxSize";
        /// <summary>Token过期参数</summary>
        public static readonly string AppSettings_TokenExpireSettings = "PKS_TokenExpireSettings";
        /// <summary>混合认证方式</summary>
        public static readonly string AppSettings_MixedAuth = "PKS_MixedAuth";
        /// <summary>Windows认证</summary>
        public static readonly string AppSettings_WindowsAuth = "PKS_WindowsAuth";
        #endregion

        #region 会话常数
        /// <summary>会话常数_授权键</summary>
        public static readonly string Session_Authentication = "PKS.Session.Token";
        /// <summary>会话常数_MVC控制器上下文</summary>
        public static readonly string Session_MvcControllerContext = "PKS.Session.ControllerContext";
        #endregion

        #region HttpContext.Items常数
        /// <summary>上下文临时常数_子系统Url</summary>
        public static readonly string HttpContext_SubSystemUrls = "PKS.Context.SubSystemUrls";
        /// <summary>上下文临时常数_表单认证票据</summary>
        public static readonly string HttpContext_FormsAuthenticationTicket = "PKS.Context.FormsAuthenticationTicket";
        /// <summary>上下文临时常数_登录用户</summary>
        public static readonly string HttpContext_Principal = "PKS.Context.Principal";
        /// <summary>上下文临时常数_原请求URL</summary>
        public static readonly string HttpContext_RawRequestUrl = "PKS.Context.RawRequestUrl";
        #endregion

        #region 服务常数
        /// <summary>管理服务宿主名</summary>
        public static readonly string MgmtServicesHost = "PKS_MgmtServices_Host";
        #endregion

        #region portal linkedin text
        /// <summary> 嵌入文字用途说明/// </summary>
        public static readonly string copyright = "copyright";
        #endregion
    }
}