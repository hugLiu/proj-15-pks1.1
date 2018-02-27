using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Principal;
using Newtonsoft.Json;

namespace PKS.Models
{
    /// <summary>
    /// 授权用户
    /// </summary>
    [JsonConverter(typeof(BindingJsonConverter<PKSPrincipal>))]
    [JsonObject()]
    public interface IPKSPrincipal : IPrincipal
    {
        /// <summary>
        /// 用户角色集合
        /// </summary>
        IEnumerable<IPKSRole> Roles { get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        DateTime ExpireTime { get; }
        /// <summary>
        /// 是否过期
        /// </summary>
        bool Expired { get; }
        /// <summary>
        /// 新Token
        /// </summary>
        string NewToken { get; }
        /// <summary>
        /// 用户身份信息
        /// </summary>
        new IPKSIdentity Identity { get; }
    }

    /// <summary>
    /// 授权用户角色
    /// </summary>
    [JsonConverter(typeof(BindingJsonConverter<PKSRole>))]
    [JsonObject()]
    public interface IPKSRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 角色名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 角色描述
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// 授权用户身份 
    /// </summary>
    [JsonConverter(typeof(BindingJsonConverter<PKSIdentity>))]
    [JsonObject()]
    public interface IPKSIdentity : IIdentity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// E-Mails
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        string PhoneNumber { get; set; }
    }
}
