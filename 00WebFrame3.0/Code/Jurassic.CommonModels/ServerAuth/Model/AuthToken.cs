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
    /// 授权安全令牌实体类 
    /// </summary>
    [Table("API_AUTH_TOKEN")]
    public class AuthToken 
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthToken()
        {
            
        }

        #region 基础字段
        /// <summary>
        /// 主键
        /// </summary>
        [Column("TOKEYID")]
        public virtual string ToKeyId { get; set; }

        /// <summary>
        /// 客户组名称
        /// </summary>
        [Column("CLIENTNAME")]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        [Column("CLIENTID")]
        public virtual string ClientId { get; set; }

        /// <summary>
        /// 授权key
        /// </summary>
        [Column("TOKEYCODE")]
        public virtual string TokeyCode { get; set; }

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
        /// 授权有效期
        /// </summary>
        [Column("VALIDITYDATE")]
        public virtual DateTime? ValidityDate { get; set; }

        /// <summary>
        /// 授权日期
        /// </summary>
        [Column("ACCREDITDATE")]
        public virtual DateTime? AccreditDate { get; set; }

        /// <summary>
        /// 授权人
        /// </summary>
        [Column("ACCREDITBY")]
        public virtual string AccreditBy { get; set; }


        //public virtual ICollection<DataNodeInfo> DataNodeInfoList { get; set; }

        #endregion
         
    }
}
