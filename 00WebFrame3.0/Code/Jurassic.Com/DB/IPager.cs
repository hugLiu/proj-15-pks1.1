using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace Jurassic.Com.DB
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 分页类接口,定义从大数据集返回部分数据所需的信息
    /// </summary>
    public interface IPager
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        int RecordCount { get; set; } 

        /// <summary>
        /// 页大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 页号
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 绝对行号
        /// </summary>
        int AbsRowIndex { get; set; }

    }
}