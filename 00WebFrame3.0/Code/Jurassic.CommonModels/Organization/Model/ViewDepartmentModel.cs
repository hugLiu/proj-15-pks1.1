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
    public class ViewDepartmentModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>		
        public int? DepId { get; set; }

        /// <summary>
        /// 组织机构节点编码
        /// </summary>		
        public string OrgNode { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>		
        public string DepHID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>		
        public string DepName { get; set; }

        /// <summary>
        /// 部门类型1.单位2.部门
        /// </summary>		
        public string DepType { get; set; }

        /// <summary>
        /// 部门级别
        /// </summary>		
        public int DepLevel { get; set; }

        /// <summary>
        /// 父部门编号
        /// </summary>		
        public int? ParentId { get; set; }

        /// <summary>
        /// 父节点部门名称
        /// </summary>		
        public string ParentDepName { get; set; }

        /// <summary>
        /// 父对象节点值
        /// </summary>		
        public string ParentDepHID { get; set; }

        /// <summary>
        /// 部门主管ID
        /// </summary>		
        public int? LeaderUserId { get; set; }

        /// <summary>
        /// 部门主管名称
        /// </summary>		
        public string LeaderUserName { get; set; }


    }
}