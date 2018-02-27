using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.AppCenter;

namespace Jurassic.WebFrame.Models
{
    /// <summary>
    /// 用户个性化配置
    /// </summary>
    public class UserConfig : AppUser
    {
        /// <summary>
        /// 主题名称
        /// </summary>
        public string Theme
        {
            get
            {
                if (_theme.IsEmpty())
                {
                    return "blue";
                }
                return _theme;
            }
            set
            {
                _theme = value;
            }
        }

        private string _theme;
        /// <summary>
        /// 显示Tab页风格
        /// </summary>
        public bool ShowTab { get; set; }

        /// <summary>
        /// 表格线样式
        /// </summary>
        public GridLineStyle GridLineStyle { get; set; }

        /// <summary>
        /// 是否转向最后一次页面
        /// </summary>
        public bool LoginToLastPage { get; set; }

        /// <summary>
        /// 用户最后一次访问的页面地址,下次登录可以直接转到此地址
        /// </summary>
        public string LastPageUrl { get; set; }

        /// <summary>
        /// 用户自定义的主页
        /// </summary>
        public string UserHomePage { get; set; }

        public UserCookieModel UserStartPageConfig { get; set; }
    }

    [Flags]
    public enum GridLineStyle
    {
        None = 0,

        Horizental = 1,

        Vertical = 2,

        All = 3
    }
}