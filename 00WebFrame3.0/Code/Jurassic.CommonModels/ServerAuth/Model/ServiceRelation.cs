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
    [Table("API_SERVICE_RELATION")]
    public class ServiceRelation 
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceRelation() { }

        #region 基础字段
        /// <summary>
        /// 主键
        /// </summary>
        [Column("SID")]
        public virtual string SID { get; set; }                   
       
        /// <summary>                                                
        /// 数据节点主键ID                                             
        /// </summary>                                               
        [Column("SERVICEID")]                                     
        public virtual string ServiceID { get; set; }

        /// <summary>
        /// 身份令牌主键ID
        /// </summary>
        [Column("TOKEYID")]
        public virtual string TokeyID { get; set; }


        #endregion
         

          
                           
                           
                           
                           
                           
                           
    }
}
