using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PKS.PortalMgmt.Common
{
    public class ExcelHelper
    {
        public static IList<T> ReadExcelToEntityList<T>(string filePath) where T : class, new()
        {
            DataTable table = ReadExcelToDataTable(filePath);
            IList<T> list = DataTableToList<T>(table);
            return list;
        }

        public static DataTable ReadExcelToDataTable(string filePath)
        {
            if (filePath == string.Empty)
            {
                throw new ArgumentNullException("路径参数不能为空");
            }
            DataTable table = null;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = null;
                if (filePath.IndexOf(".xlsx") > 0) // 2007版本  
                {
                    workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
                }
                else if (filePath.IndexOf(".xls") > 0) // 2003版本  
                {
                    workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
                }
                ISheet sheet1 = workbook.GetSheetAt(0);
                table = new DataTable();

                var row1 = sheet1.GetRow(0);
                int cellCount = row1.LastCellNum;
                for(int i= row1.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(row1.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet1.LastRowNum;
                for(int i = (sheet1.FirstRowNum + 1); i <= sheet1.LastRowNum; i++)
                {
                    var row = sheet1.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for(int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).StringCellValue;
                        }
                    }
                    table.Rows.Add(dataRow);
                }
                workbook = null;
                sheet1 = null;
            }        
            return table;
        }

        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            if (dt == null) return null;
            List<T> list = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();

                PropertyInfo[] propertys = t.GetType().GetProperties();

                foreach (PropertyInfo pro in propertys)
                {
                    if (dt.Columns.Contains(pro.Name))
                    {
                        object value = dr[pro.Name];
                        value = Convert.ChangeType(value, pro.PropertyType);

                        if (value != DBNull.Value)
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }
    }
}