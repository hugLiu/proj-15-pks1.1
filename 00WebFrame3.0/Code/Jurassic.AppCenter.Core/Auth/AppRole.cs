using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 角色实体
    /// </summary>
    public class AppRole : IIdName, IUniqueName
    {
        /// <summary>
        /// 新建一个角色对象。并初始化功能ID列表为一个空表
        /// </summary>
        public AppRole()
        {
            FunctionIds = new List<string>();
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 角色的功能ID列表
        /// </summary>
        public virtual List<string> FunctionIds { get; set; }
    }
}
