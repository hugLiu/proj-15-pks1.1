using System;
using System.Collections.Generic;

namespace PKS.Models
{
    /// <summary>权限类型</summary>
    public static class PKSPermissionTypes
    {
        /// <summary>菜单页面</summary>
        public static readonly int Menu = 1;
        /// <summary>非菜单页面</summary>
        public static readonly int Page = 2;
        /// <summary>页面中功能项</summary>
        public static readonly int Function = 3;
    }
}