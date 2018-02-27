using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jurassic.AppCenter.Models
{
    /// <summary>
    /// 执行Setup操作,指定管理员角色和账户名的ViewModel
    /// </summary>
    public class SetupModel
    {
        /// <summary>
        /// 管理员角色名称
        /// </summary>
        [Required(ErrorMessage="请输入管理员角色名称")]
        [Display(Name = "管理员角色名称")]
        public string AdminRoleName { get; set; }

        /// <summary>
        /// 管理员用户名
        /// </summary>
        [Required(ErrorMessage="请输入管理员用户名称")]
        [Display(Name = "管理员用户名")]
        public string AdminUserName { get; set; }
    }
}