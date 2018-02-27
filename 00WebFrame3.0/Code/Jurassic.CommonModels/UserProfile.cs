using System.Web.UI.WebControls;

namespace Jurassic.CommonModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 在数据库表中映射的用户基本信息实体类
    /// </summary>
    [Table("USERPROFILE")]
    public partial class UserProfile
    {
        public UserProfile()
        {
            UserInRoles = new HashSet<UserInRole>();
        }

        [Column("USERID")]
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column("USERNAME")]
        public string UserName { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("PHONENUMBER")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户的角色列表
        /// 如果这里不声明为virtural, 在Find(id)时将得不到该属性
        /// </summary>
        public virtual ICollection<UserInRole> UserInRoles { get; set; }

    }

    [Table("WEBPAGES_ROLES")]
    public class Role
    {
        [Column("ROLEID")]
       [Key]
        public int RoleId { get; set; }

        [Column("ROLENAME")]
        public string RoleName { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }

    [Table("WEBPAGES_USERSINROLES")]
    public class UserInRole
    {
        [Column("USERID", Order = 0)]
        [Key]
        public int UserId { get; set; }

        [Column("ROLEID", Order = 1)]
        [Key]
        public int RoleId { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual Role Role { get; set; }
    }

    [Table("WEBPAGES_MEMBERSHIP")]
    public class MemberShip : UserProfile
    {
        [Column("CREATEDATE")]
        public DateTime CreateDate { get; set; }
        [Column("CONFIRMATIONTOKEN")]
        public string ConfirmationToken { get; set; }

        /// <summary>
        /// 判断用户是否被删除
        /// IsConfirmed = true 有效用户
        /// IsConfirmed = false 无效用户
        /// </summary>
        [Column("ISCONFIRMED")]
        public bool IsConfirmed { get; set; }

        [Column("LASTPASSWORDFAILUREDATE")]
        public DateTime? LastPasswordFailureDate { get; set; }

        //[Column("PASSWORDFAILURESSINCELASTSUCCESS")]
        //public int PasswordFailuresSinceLastSuccess { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("PASSWORD")]
        public string Password { get; set; }

        /// <summary>
        /// 密码修改时间
        /// </summary>
        [Column("PASSWORDCHANGEDDATE")]
        public DateTime? PasswordChangedDate { get; set; }

        //密码的盐值
        [Column("PASSWORDSALT")]
        public string PasswordSalt { get; set; }

        [Column("PASSWORDVERIFICATIONTOKEN")]
        public string PasswordVerificationToken { get; set; }

        //[Column("PASSWORDVERIFICATIONTOKENEXPIRATIONDATE")]
        //public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
    }

}
