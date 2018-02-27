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
    /// 数据节点对象 
    /// </summary>
    [Table("API_DATA_NODE_INFO")]
    public class DataNodeInfo 
    {
        /// <summary>
        /// 
        /// </summary>
        public DataNodeInfo()   {     }

        #region 基础字段
        /// <summary>
        /// 主键
        /// </summary>
        [Column("DATAID")]
        public virtual string DataID { get; set; }                   
                                                                     
        /// <summary>                                                
        /// 数据父节点ID                                             
        /// </summary>                                               
        [Column("DATAPARENTID")]                                     
        public virtual string DataParentID { get; set; }

        /// <summary>
        /// 数据父节点名称
        /// </summary>
        [Column("DATANODENAME")]
        public virtual string DataNodeName { get; set; }

        /// <summary>
        /// 数据节点ID
        /// </summary>
        [Column("DATANODEID")]
        public virtual string DataNodeID { get; set; }

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


        //public virtual ICollection<AuthToken> AuthTokenList { get; set; }
        #endregion
         

          
                           
                           
                           
                           
                           
                           
    }
}
