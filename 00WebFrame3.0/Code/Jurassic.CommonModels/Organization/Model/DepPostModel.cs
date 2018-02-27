using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.CommonModels.Organization
{
    /// <summary>
    /// 部门与岗位关系表
    /// </summary>
    [Serializable]
    [Table("DEP_DEPPOST")]
    public class DepPostModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>		
        [Column("ID")]
        public int? Id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>		
        [Column("DEPID")]
        public int? DepId { get; set; }

        /// <summary>
        /// 岗位ID
        /// </summary>		
        [Column("POSTID")]
        public int? PostId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// 岗位人数
        /// </summary>		
        [Column("PLANNUMBER")]
        public int? PlanNumber { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>		
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }

        /// <summary>
        /// 岗位描述
        /// </summary>		
        [Column("DESCRIBE")]
        public string Describe { get; set; }

        /// <summary>
        /// 岗位责任
        /// </summary>		
        [Column("DUTY")]
        public string Duty { get; set; }

        /// <summary>
        /// 岗位要求
        /// </summary>		
        [Column("REQUIREMENT")]
        public string Requirement { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>		
        [Column("ISACTIVE")]
        public int? IsActive { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>		
        [Column("ISDISABLED")]
        public int? IsDisabled { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>		
        [Column("ISDELETED")]
        public int? IsDeleted { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATETIME")]
        public DateTime? CreateDatetime { get; set; }

        /// <summary>
        /// 数据状态
        /// 新增=ADDED 编辑=MODIFIED 删除=REMOVED
        /// </summary>		
        [NotMapped]
        public string _State { get; set; }
    }
}
