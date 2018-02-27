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
    [Table("API_DATA_RELATION")]
    public class DataRelation 
    {
        /// <summary>
        /// 
        /// </summary>
        public DataRelation() { }

        #region 基础字段
        /// <summary>
        /// 主键
        /// </summary>
        [Column("RID")]
        public virtual string RID { get; set; }                   
       
        /// <summary>                                                
        /// 数据节点主键ID                                             
        /// </summary>                                               
        [Column("DATAID")]                                     
        public virtual string DataID { get; set; }

        /// <summary>
        /// 身份令牌主键ID
        /// </summary>
        [Column("TOKEYID")]
        public virtual string TokeyID { get; set; }


        #endregion
         

          
                           
                           
                           
                           
                           
                           
    }
}
