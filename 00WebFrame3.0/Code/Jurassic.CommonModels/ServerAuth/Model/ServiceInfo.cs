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
    /// 服务节点对象 
    /// </summary>
    [Table("API_SERVICE_INFO")]
    public class ServiceInfo 
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceInfo()   {     }

        #region 基础字段
        /// <summary>
        /// 主键
        /// </summary>
        [Column("SERVICEID")]
        public virtual string ServiceID { get; set; }                   
                                                                     
        /// <summary>                                                
        /// 数据父节点ID                                             
        /// </summary>                                               
        [Column("PARENTID")]                                     
        public virtual string ParentID { get; set; }                  
                                       
        /// <summary>                                                
        /// 数据父节点名称                                             
        /// </summary>                                               
        [NotMapped]
        public virtual string ParentName { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [Column("SERVICENAME")]
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// 服务函数名称
        /// </summary>
        [Column("SERVICEFUNCTIONNAME")]
        public virtual string ServiceFunctionName { get; set; }

        /// <summary>
        /// 服务全称
        /// </summary>
        [Column("SERVICEFULLNAME")]
        public virtual string ServiceFullName { get; set; }

        /// <summary>
        /// 请求方式
        /// Post,Put,Get,Delete
        /// </summary>
        [Column("REQUESTWAY")]
        public virtual string RequestWay { get; set; }
        
        /// <summary>
        /// 授权方式 
        /// 0 有权限的用户(NeedAuth)有权限的客户组
        /// 1 所有登录用户可访问(AllUsers)所有授权客户端 
        /// 2 所有人(EveryOne)所有人
        /// 4 禁止所有人(Forbidden)禁止所有人
        /// </summary>
        [Column("AUTHWAY")]
        public virtual string AuthWay { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("MEMO")]
        public virtual string Memo { get; set; }

        /// <summary>
        /// 状态
        /// 1启用 0禁用
        /// </summary>
        [Column("ISVALID")]
        public virtual int? IsvalId { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Column("CREATEDDATE")]
        public virtual DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEDBY")]
        public virtual string CreatedBy { get; set; }

     
        #endregion
         

          
                           
                           
                           
                           
                           
                           
    }
}
