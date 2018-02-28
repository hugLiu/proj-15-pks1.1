using System;
using System.Collections.Generic;

namespace PKS.SZXT.Core.DTO
{
    public class TableData
    {
        public string TableName { set; get; }
        public List<List<TableHeader>> Header { get; set; }
        public List<TableColumn> Columns { get; set; }
        public Array Rows { get; set; }
    }
}
