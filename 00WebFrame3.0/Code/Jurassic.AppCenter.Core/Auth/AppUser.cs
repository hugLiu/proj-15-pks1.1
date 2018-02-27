using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Com.Tools;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用户实体类 
    /// </summary>
    public class AppUser : IIdName, IUniqueName
    {
        /// <summary>
        /// 新建一个用户对象
        /// </summary>
        public AppUser()
        {
            FunctionIds = new List<string>();
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// E-Mails
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户的角色名称以,号分隔的字符串
        /// </summary>
        public string RoleNames
        {
            get
            {
                if (RoleIds.IsEmpty())
                {
                    return String.Empty;
                }
                return String.Join(", ", RoleIds.Select(rId =>
                {
                    var role = AppManager.Instance.RoleManager.GetById(rId);
                    if (role == null) return null;
                    return role.Name;
                }));
            }
        }

        private bool _isDefaultRule = true;
        /// <summary>
        /// by_zjf
        /// 用于判断当设置了用户与模块关系的情况下所采用授权方式 
        /// 以角色授权为主 true
        /// 以用户授权为主 false
        /// 默认值为true(王家新补充)
        /// </summary>
        public bool IsDefaultRole
        {
            get { return _isDefaultRule; }
            set { _isDefaultRule = value; }
        }

        /// <summary>
        /// 判断用户是否属于某个角色ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool IsInRoleId(string roleId)
        {
            return RoleIds.Contains(roleId);
        }

        /// <summary>
        /// 判断用户是否属于一个角色名称
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsInRole(string roleName)
        {
            var role = AppManager.Instance.RoleManager.GetByName(roleName);
            return role != null && IsInRoleId(role.Id);
        }

        /// <summary>
        /// 给用户新增一个角色ID
        /// </summary>
        /// <param name="roleId"></param>
        public void AddRoleId(string roleId)
        {
            if (!mRoleIds.Contains(roleId))
            {
                mRoleIds.Add(roleId);
            }
        }

        /// <summary>
        /// 用户是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 用户最后一次的操作时间
        /// </summary>
        public DateTime LastOpTime { get; set; }

        //public virtual bool IsOnline
        //{
        //    get;
        //    set;
        //}

        //public bool Login(string password, bool rememberMe)
        //{
        //    if (LoginCore(password, rememberMe))
        //    {
        //        IsOnline = true;
        //        return true;
        //    }
        //    return false;
        //}

        //protected virtual bool LoginCore(string password, bool rememberMe)
        //{
        //    return true;
        //}

        //protected virtual void LogoutCore()
        //{
        //    IsOnline = false;
        //}

        private List<string> mRoleIds = new List<string>();

        /// <summary>
        /// 用户的角色ID列表
        /// </summary>
        public virtual IEnumerable<string> RoleIds
        {
            get
            {
                return mRoleIds;
            }
            set
            {
                mRoleIds = (value ?? new string[0]).ToList();
            }
        }

        /// <summary>
        /// 为用户单独指定的功能ID列表
        /// </summary>
        public virtual ICollection<string> FunctionIds { get; set; }
    }
}
