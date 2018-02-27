using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.CommonModels.Organization
{
    /// <summary>
    /// 部门与用户关系信息
    /// </summary>
    [Serializable]
    [Table("DEP_DEPUSER")]
    public class DepUserModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
        [Column("ID")]
        public int? Id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>		
        [Column("DEPID")]
        public int? DepId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>		
        [Column("USERID")]
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>		
        [Column("USERNAME")]
        public string UserName { get; set; }

        /// <summary>
        /// 审核类型
        /// </summary>		
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }

        /// <summary>
        /// 合同类型
        /// </summary>		
        [Column("CONTRACTTYPE")]
        public string ContractType { get; set; }

        /// <summary>
        /// 合同期限
        /// </summary>		
        [Column("CONTRACTLENGHT")]
        public int? ContractLenght { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>		
        [Column("JOINDATETIME")]
        public DateTime? JoinDateTime { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>		
        [Column("OUTDATETIME")]
        public DateTime? OutDateTime { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATETIME")]
        public DateTime? CreateDatetime { get; set; }

        /// <summary>
        /// 是否停职
        /// 0否 1是
        /// </summary>		
        [Column("ISSUSPENSION")]
        public int? IsSuspension { get; set; }

        /// <summary>
        /// 是否主管
        /// 0否 1是
        /// </summary>		
        [Column("ISLEADER")]
        public int? IsLeader { get; set; }

        /// <summary>
        /// 是否主部门
        /// 确定该账号直属的部门
        /// 0否 1是
        /// </summary>		
        [Column("ISMAIN")]
        public int? IsMain { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>		
        [Column("ISDELETED")]
        public int? IsDeleted { get; set; }
       
        /// <summary>
        /// 岗位主键ID
        /// </summary>		
        [Column("POSTID")]
        public int? PostId { get; set; }

        /// <summary>
        /// 数据状态
        /// 新增=ADDED 编辑=MODIFIED 删除=REMOVED
        /// </summary>		
        [NotMapped]
        public string _State { get; set; }

    }


    public class ViewDepUserModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
        public int? Id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>		
        public int? DepId { get; set; }

        /// <summary>
        /// 部门名称 wang加
        /// </summary>
        public string DepName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>		
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>		
        public string UserName { get; set; }

        /// <summary>
        /// 审核类型
        /// </summary>		
        public string ExamineType { get; set; }

        /// <summary>
        /// 合同类型
        /// </summary>		
        public string ContractType { get; set; }

        /// <summary>
        /// 合同期限
        /// </summary>		
        public int? ContractLenght { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>		
        public DateTime? JoinDateTime { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>		
        public DateTime? OutDateTime { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
        public DateTime? CreateDatetime { get; set; }

        /// <summary>
        /// 是否停职
        /// 0否 1是
        /// </summary>		
        public int? IsSuspension { get; set; }

        /// <summary>
        /// 是否主管
        /// 0否 1是
        /// </summary>		
        public int? IsLeader { get; set; }

        /// <summary>
        /// 是否主部门
        /// 确定该账号直属的部门
        /// 0否 1是
        /// </summary>		
        public int? IsMain { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>		
        public int? IsDeleted { get; set; }

        /// <summary>
        /// 岗位主键ID
        /// </summary>		
        public int? PostId { get; set; }

        //wang加
        public string OrgNode { get; set; }

        /// <summary>
        /// 数据状态
        /// 新增=ADDED 编辑=MODIFIED 删除=REMOVED
        /// </summary>		
        [NotMapped]
        public string _State { get; set; }

    }
}
