using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Jurassic.AppCenter.Models
{
    class MvcFuncInfo
    {
        public Type Controller { get; set; }

        public MethodInfo Action { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public ParameterInfo[] Parameters { get; set; }

        public override string ToString()
        {
            return "/" + ControllerName + "/" + ActionName;
        }

        /// <summary>
        /// 授权类型
        /// </summary>
        public JAuthType AuthType { get; set; }

        /// <summary>
        /// 是否标记了[POST]
        /// </summary>
        public bool HasPost { get; set; }
    }
}