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
    /// 数据节点授权关系对象 
    /// </summary>
    public class ViDataAuth
    {
        /// <summary>
        /// 
        /// </summary>
        public ViDataAuth() { }

        #region 基础字段
        /// <summary>                                                
        ///                                              
        /// </summary>                                               
        public virtual string RID { get; set; }

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
        /// 客户授权有效期
        /// </summary>
        public virtual DateTime? ValidityDate { get; set; }

        /// <summary>
        /// 数据节点编码
        /// </summary>
        public virtual string DataNodeID { get; set; }

        /// <summary>
        /// 数据节点名称
        /// </summary>
        public virtual string DataNodeName { get; set; }

        /// <summary>
        /// 上级数据节点编码
        /// </summary>
        public virtual string DataParentID { get; set; }


        #endregion









    }
}
