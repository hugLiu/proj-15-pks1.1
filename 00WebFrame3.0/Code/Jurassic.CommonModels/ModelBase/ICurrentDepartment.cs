using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 获取当前用户当前部门ID的接口
    /// </summary>
    public interface ICurrentDepartment
    {
        /// <summary>
        /// 返回当前用户的当前部门
        /// </summary>
        int DeptId { get; set; }
    }
}
