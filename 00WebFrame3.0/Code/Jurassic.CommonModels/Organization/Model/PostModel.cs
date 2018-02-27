using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.CommonModels.Organization
{
    /// <summary>
    /// 岗位基本信息
    /// </summary>
    [Serializable]
    [Table("DEP_POST")]
    public class PostModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
        [Column("ID")]
        public int? Id { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>		
        [Column("POSTNAME")]
        public string PostName { get; set; }

        /// <summary>
        /// 岗位类型
        /// </summary>		
        [Column("POSTTYPE")]
        public string PostType { get; set; }

        /// <summary>
        /// 岗位级别
        /// </summary>		
        [Column("POSTLEVELTYPE")]
        public string PostLevelType { get; set; }

        /// <summary>
        /// 岗位雇佣类型
        /// </summary>		
        [Column("POSTENGAGETYPE")]
        public string PostEngageType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>		
        [Column("OPERATORID")]
        public int? OperatorID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATETIME")]
        public DateTime? CreateDatetime { get; set; }

        /// <summary>
        /// 是否删除 0否 1是
        /// </summary>		
        [Column("ISDELETED")]
        public int? IsDeleted { get; set; }

        /// <summary>
        /// 数据状态
        /// 新增=ADDED 编辑=MODIFIED 删除=REMOVED
        /// </summary>		
        [NotMapped]
        public string _State { get; set; }
    }
}
