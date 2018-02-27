using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.KManage.Model
{
    public enum BoType
    {
        /// <summary>
        /// 井
        /// </summary>
        Well,

        /// <summary>
        /// 圈闭
        /// </summary>
        Trap,

        /// <summary>
        /// 构造
        /// </summary>
        Structure,

        /// <summary>
        /// 一级构造
        /// </summary>
        FirstStructure,

        /// <summary>
        /// 二级构造
        /// </summary>
        SecondStructure,

        /// <summary>
        /// 盆地
        /// </summary>
        Basin
    }
}
