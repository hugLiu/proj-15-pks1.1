using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebApi.Providers
{
    /// <summary>
    /// WebApi服务扩展属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthAttribute : Attribute
    {
        private bool isIgnoreAuth = false;

        /// <summary>
        /// 是否忽略授权
        /// 对于使用了全局授权的情况下可以单独对个别服务设置该属性
        /// true 进行授权验证 false不进行授权验证
        /// </summary>
        public bool IsIgnoreAuth { get; set; }







    }
}