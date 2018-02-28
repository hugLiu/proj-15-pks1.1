using System;
using System.Collections.Generic;
using System.Drawing;

namespace PKS.WebAPI.Models
{
    /// <summary>门户菜单</summary>
    [Serializable]
    public class PortalMenu
    {
        /// <summary>角色默认打开的菜单</summary>
        public string DefaultUrl { get; set; }
        /// <summary>角色有权限的菜单信息集合</summary>
        public List<MenuInfo> Menus { get; set; }
    }

    /// <summary>门户底部菜单</summary>
    [Serializable]
    public class PortalFooterMenu
    {
        /// <summary>底部显示的Html内容</summary>
        public string Content { get; set; }
        /// <summary>分类外链信息集合</summary>
        public List<LinkCategoryInfo> Categories { get; set; }
    }
}
