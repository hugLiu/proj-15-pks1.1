using System;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>
    /// 服务信息
    /// </summary>
    public class ServiceInfo
    {
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>描述</summary>
        public string Description { get; set; }
        /// <summary>开发者</summary>
        public string Developer { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}