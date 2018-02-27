using Jurassic.AppCenter.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用于定义菜单项
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class JAuthAttribute : Attribute
    {
        /// <summary>
        /// 根据默认值新建一个菜单标记，默认是需要有角色的用户
        /// </summary>
        public JAuthAttribute()
        {
        }

        /// <summary>
        /// 根据授权类型创建一个权限标签
        /// </summary>
        /// <param name="authType">授权类型</param>
        public JAuthAttribute(JAuthType authType)
        {
            AuthType = authType;
        }

        private JAuthType mAuthType = JAuthType.NeedAuth;
        /// <summary>
        /// 授权类型
        /// </summary>
        public JAuthType AuthType
        {
            get { return mAuthType; }
            set { mAuthType = value; }
        }

        /// <summary>
        /// 用此构造函数构造一个显式定义不显示的菜单
        /// </summary>
        /// <param name="visible">当为false时扫描程程序忽略该菜单</param>
        public JAuthAttribute(VisibleType visible)
        {
            Visible = visible;
        }

        public JAuthAttribute(JAuthType authType, VisibleType visible)
        {
            AuthType = authType;
            Visible = visible;
        }

        /// <summary>
        /// 根据名称，ID和排序位创建一个菜单标记。
        /// </summary>
        /// <param name="name">菜单名称</param>
        /// <param name="id">菜单ID</param>
        /// <param name="ord">排序位</param>
        public JAuthAttribute(string name, string id = "", int ord = 0)
        {
            Name = name;
            Id = id;
            Ord = ord;
        }

        /// <summary>
        /// 根据授权类型、名称、ID和排序位创建一个菜单标记。
        /// </summary>
        /// <param name="authType">授权类型</param>
        /// <param name="name">菜单名称</param>
        /// <param name="id">菜单ID</param>
        /// <param name="ord">排序位</param>
        public JAuthAttribute(JAuthType authType, string name = "", string id = "", int ord = 0)
        {
            AuthType = authType;
            Name = name;
            Id = id;
            Ord = ord;
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 功能所在区域名
        /// </summary>
        public string AreaName { get; set; }

        private int ord = 1;
        /// <summary>
        /// 功能在菜单中的排序位
        /// </summary>
        public int Ord
        {
            get { return ord; }
            set { ord = value; }
        }

        private VisibleType visible = VisibleType.All;
        /// <summary>
        /// 功能是否会在菜单中出现
        /// </summary>
        public VisibleType Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private JLogType mLogType = JLogType.Info;
        /// <summary>
        /// 日志记录的级别
        /// </summary>
        public JLogType LogType
        {
            get { return mLogType; }
            set { mLogType = value; }
        }

        /// <summary>
        /// 显式定义该功能的ID,不能和其他的显式定义的ID重复
        /// 默认为空, 表示由系统自动分配ID
        /// </summary>
        public string Id { get; set; }

    }
}