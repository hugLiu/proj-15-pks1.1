using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 数据权限类
    /// </summary>
    [Table("SYS_DATARULE")]
    public class Sys_DataRule:IId<int>
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 数据项ID
        /// </summary>
        public int BillId { get; set; }

        /// <summary>
        /// 数据实体类型名称
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 授权对象ID
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// 授权对象类型
        /// </summary>
        public int ObjectType { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 允许的操作类型枚举（按位迭加的）
        /// </summary>
        public OperationType OpType { get; set; }
    }
}
