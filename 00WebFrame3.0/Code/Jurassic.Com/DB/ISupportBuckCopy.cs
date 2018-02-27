using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Jurassic.Com.DB
{
    /// <summary>
    /// 标识数据库是否支持批量导入的接口
    /// </summary>
    interface ISupportBuckCopy
    {
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <param name="notifyAfter"></param>
        /// <param name="onRowsCopied"></param>
        /// <returns></returns>
        int BuckCopy(DataTable dt, string tableName, int notifyAfter, Action<int> onRowsCopied);
    }
}
