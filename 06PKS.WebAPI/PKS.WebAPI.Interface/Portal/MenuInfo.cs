using System;
using System.Collections.Generic;
using System.Drawing;

namespace PKS.WebAPI.Models
{
    /// <summary>菜单信息</summary>
    [Serializable]
    public class MenuInfo
    {
        /// <summary>Id</summary>
        public int Id { get; set; }
        /// <summary>键</summary>
        public string Key { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>链接</summary>
        /// <remarks></remarks>
        public string URL { get; set; }
        /// <summary>链接打开目标:_blank|_self</summary>
        public string Target { get; set; }
        /// <summary>是否有第三级子菜单(用于第一级菜单，表示本菜单是否有第三级菜单)</summary>
        public bool HasThird { get; set; }
        /// <summary>子菜单信息集合</summary>
        public List<MenuInfo> Children { get; set; }
    }
}
