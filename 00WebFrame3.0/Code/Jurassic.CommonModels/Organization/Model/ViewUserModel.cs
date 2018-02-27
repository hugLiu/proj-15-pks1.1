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
    public class ViewUserModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>		
        public int? Id { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>		
        public int? ParentId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>		
        public int? DepId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>		
        public string DepName { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>		
        public string DepHID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>		
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>		
        public string UserName { get; set; }

        /// <summary>
        /// 直属领导用户名称
        /// </summary>		
        public string LeaderUserName { get; set; }

        /// <summary>
        /// 是否主管
        /// 0否 1是
        /// </summary>		
        public int? IsLeader { get; set; }

       

    }
}
