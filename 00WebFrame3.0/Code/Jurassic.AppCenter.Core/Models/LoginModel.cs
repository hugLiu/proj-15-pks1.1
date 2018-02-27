using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jurassic.AppCenter.Models
{
    /// <summary>
    /// 用于用户登录的ViewModel
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// 是否记住我
        /// </summary>
        [Display(Name = "Remember+Me")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Auto+Login")]
        public bool AutoLogin { get; set; }

        /// <summary>
        /// 是否强制修改密码
        /// </summary> 
        public bool IsChangedPassword { get; set; }
    }

    /// <summary>
    /// 用户重设密码的数据定义实体类
    /// </summary>
    public class PasswordResetModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "User+Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm+Password")]
        public string ConfirmPassword { get; set; }

        public string ConfirmToken { get; set; }

        /// <summary>
        /// 是否属于系统重置密码
        /// IsResetPass=1 系统重置密码
        /// IsResetPass = 0  邮箱方式重置密码
        /// </summary>
        public int IsResetPass { get; set; }
    }

    /// <summary>
    /// 用户修改密码的数据定义实体类
    /// </summary>
    public class PasswordChangeModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old+Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm+Password")]
        public string ConfirmPassword { get; set; }
    }
}