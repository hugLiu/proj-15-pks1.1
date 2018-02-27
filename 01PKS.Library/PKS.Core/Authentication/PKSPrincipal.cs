using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>
    /// 授权用户
    /// </summary>
    [Serializable]
    public class PKSPrincipal : IPKSPrincipal
    {
        /// <summary>
        /// 用户角色集合
        /// </summary>
        public IEnumerable<IPKSRole> Roles { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool Expired
        {
            get { return DateTime.Now > this.ExpireTime; }
        }
        /// <summary>
        /// 新Token
        /// </summary>
        public string NewToken { get; set; }
        /// <summary>
        /// 用户身份信息
        /// </summary>
        public IPKSIdentity Identity { get; set; }

        /// <summary>
        /// 用户身份信息
        /// </summary>
        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        /// <summary>
        /// 是否是指定角色
        /// </summary>
        public virtual bool IsInRole(string role)
        {
            return this.Roles.Any(e => e.Id == role || e.Name == role);
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>
    /// 授权用户角色
    /// </summary>
    [Serializable]
    public class PKSRole : IPKSRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>
    /// 授权用户身份 
    /// </summary>
    [Serializable]
    public class PKSIdentity : IPKSIdentity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// E-Mails
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 认证方式
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
