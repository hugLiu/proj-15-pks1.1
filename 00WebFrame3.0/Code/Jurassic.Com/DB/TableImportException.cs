using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Com.DB
{
    /// <summary>
    /// 从DataTable导入到数据库表出错时引发的异常
    /// </summary>
    [Serializable]
    public class TableImportException : Exception
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public TableImportException(Exception ex, int row, int col)
            :base(ex.Message, ex)
        {
            Row = row;
            Col = col;
        }
    }
}
