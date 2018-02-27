using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Com.Tools;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.CommonModels.ServerAuth
{
    /// <remarks>by_zjf</remarks>
    /// <summary>
    /// 服务节点授权关系对象 
    /// </summary>
    public class ViServiceAuth
    {
        /// <summary>
        /// 
        /// </summary>
        public ViServiceAuth() { }
      
        #region 基础字段
        /// <summary>
        /// 
        /// </summary>
        public virtual string SID { get; set; }

        /// <summary>
        /// 客户组名称 
        /// </summary>
        public virtual string ClientName { get; set; }

        /// <summary>
        /// 客户组编码
        /// </summary>
        public virtual string ClientId { get; set; }

        /// <summary>
        /// 客户授权Key
        /// </summary>
        public virtual string TokeyCode { get; set; }

        /// <summary>
        /// 服务ID
        /// </summary>
        public virtual string ServiceID { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// 服务方法名称
        /// </summary>
        public virtual string ServiceFunctionName { get; set; }

        /// <summary>
        /// 服务全称
        /// </summary>
        public virtual string ServiceFullName { get; set; }
        
        /// <summary>
        /// 授权方式 
        /// 0 有权限的用户(NeedAuth)有权限的客户组
        /// 1 所有登录用户可访问(AllUsers)所有授权客户端
        /// 2 所有人(EveryOne)所有人
        /// 4 禁止所有人(Forbidden)禁止所有人
        /// </summary>
        public virtual string AuthWay { get; set; }
        
        /// <summary>
        /// 请求方式
        /// </summary>
        public virtual string RequestWay { get; set; }


        #endregion









    }
}
