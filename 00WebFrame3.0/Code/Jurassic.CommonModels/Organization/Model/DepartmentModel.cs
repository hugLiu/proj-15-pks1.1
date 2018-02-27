using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.CommonModels.Organization
{

    /// <summary>
    /// 部门主信息表
    /// </summary>
    [Serializable]
    [Table("DEP_DEPARTMENT")]
    public class DepartmentModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>		
        [Column("ID")]
        public int? Id { get; set; }

        /// <summary>
        /// 父部门编号
        /// </summary>		
        [Column("PARENTID")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 组织机构节点编码
        /// </summary>		
        [Column("ORGNODE")]
        public string OrgNode { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>		
        [Column("DEPHID")]
        public string DepHID { get; set; }

        /// <summary>
        /// 部门序号
        /// </summary>		
        [Column("ORD")]
        public int? Ord { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>		
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>		
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// 部门类型1.单位2.部门
        /// </summary>		
        [Column("DEPTYPE")]
        public string DepType { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>		
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }

        /// <summary>
        /// 是否激活 0否 1是
        /// </summary>		
        [Column("ISACTIVE")]
        public int? IsActive { get; set; }

        /// <summary>
        /// 是否禁用 0否 1是
        /// </summary>		
        [Column("ISDISABLED")]
        public int? IsDisabled { get; set; }

        /// <summary>
        /// 是否删除 0否 1是
        /// </summary>		
        [Column("ISDELETED")]
        public int? IsDeleted { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATETIME")]
        public DateTime? CreateDatetime { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>		
        [Column("MODIFIEDDATETIME")]
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// 父对象节点值
        /// </summary>		
        [NotMapped]
        public string ParentOrgNode { get; set; }

        /// <summary>
        /// 组织结构岗位关系集合
        /// </summary>		
        [NotMapped]
        public List<DepPostModel> DepPostModelList { get; set; }

        /// <summary>
        /// 组织结构用户关系集合
        /// </summary>		
        [NotMapped]
        public List<DepUserModel> DepUserModelList { get; set; }




    }
}