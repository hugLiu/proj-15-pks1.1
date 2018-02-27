using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;
using Jurassic.Com.Tools;
using NPOI.HPSF;
using System.Collections;
using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;

namespace Jurassic.Com.OfficeLib
{
    /// <summary>
    /// NPOI读写Excel 的帮助类
    /// 来源：http://www.cnblogs.com/luxiaoxun/p/3374992.html
    /// </summary>
    public class ExcelHelper : IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private bool disposed;
        private Stream _innerStream;

        /// <summary>
        /// 构造器1
        /// </summary>
        /// <param name="fileName">文件名全路径</param>
        public ExcelHelper(string fileName)
        {
            disposed = false;
            this.fileName = fileName;
            if (File.Exists(fileName))
            {
                OpenFile();
            }
            else
            {
                OpenNewBook();
            }
        }

        void OpenFile()
        {
            _innerStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)) // 2007版本
                workbook = new XSSFWorkbook(_innerStream);
            else if (fileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase)) // 2003版本
                workbook = new HSSFWorkbook(_innerStream);
        }

        /// <summary>
        /// 构造器2
        /// </summary>
        /// <param name="stream">文件流</param>
        public ExcelHelper(Stream stream)
        {
            _innerStream = stream;
            OpenNewBook();
        }

        void OpenNewBook()
        {
            if (workbook != null)
            {
                return;
            }
            if (fileName.IsEmpty())
            {
                //修改：使用流初始化一个Excel文件  licp  2016/4/26
                //（由于暂时没找到方法区分形成流的文件是xls还是xlsx类型，暂时使用try-catch的方式）
                try
                {
                    workbook = new XSSFWorkbook(_innerStream);
                }
                catch
                {
                    workbook = new HSSFWorkbook(_innerStream);
                }
            }
            else
            {
                _innerStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new XSSFWorkbook();
                }
                else
                {
                    workbook = new HSSFWorkbook();
                }
            }
        }

        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            sheet = workbook.CreateSheet(sheetName);

            if (isColumnWritten == true) //写入DataTable的列名
            {
                IRow row = sheet.CreateRow(0);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                }
                count = 1;
            }
            else
            {
                count = 0;
            }

            for (i = 0; i < data.Rows.Count; ++i)
            {
                IRow row = sheet.CreateRow(count);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                }
                ++count;
            }

            workbook.Write(_innerStream); //写入到excel

            return count;
        }

        /// <summary>
        /// 将对象型数据导入到excel中， wang
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int ObjectToExcel<T>(IEnumerable<T> data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            sheet = workbook.GetSheet(sheetName) ?? workbook.CreateSheet(sheetName);
            var props = RefHelper.GetElementType(data).GetProperties();
            if (isColumnWritten == true) //写入DataTable的列名
            {
                IRow row = sheet.CreateRow(0);
                for (j = 0; j < props.Length; ++j)
                {
                    row.CreateCell(j).SetCellValue(ResHelper.GetStr(props[j].Name));
                }
                count = 1;
            }
            else
            {
                count = 0;
            }

            for (j = 0; j < data.Count(); ++j)
            {
                var element = data.ElementAt(j);
                IRow row = sheet.CreateRow(count);
                for (i = 0; i < props.Length; ++i)
                {
                    ICell cell = row.CreateCell(i);
                    object val = props[i].GetValue(element, null);
                    SetCellValue(cell, val);
                }
                count++;
            }
            workbook.Write(_innerStream); //写入到excel
            _innerStream.Close();
            _innerStream = null;

            return count;
        }

        private void SetCellValue(ICell cell, object obj)
        {
            if (obj == null)
            {
                cell.SetCellValue("");
                return;
            }
            Type type = obj.GetType();
            if (type == typeof(bool) || type == typeof(bool?))
            {
                cell.SetCellValue(CommOp.ToBool(obj));
            }
            else if (CommOp.ToDecimal(obj).ToString() == obj.ToString())
            {
                cell.SetCellValue(CommOp.ToDouble(obj));
            }
            else if (type == typeof(DateTime))
            {
                cell.SetCellValue(CommOp.ToDateTime(obj));
            }
            else
            {
                cell.SetCellValue(CommOp.ToStr(obj));
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// 修正后的版本 wang
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            int startRow = 0;

            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);
                if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                {
                    sheet = workbook.GetSheetAt(0);
                }
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }
            if (sheet == null) return null;

            DataTable dt = new DataTable(sheetName);
            IRow firstRow = sheet.GetRow(0);
            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

            if (isFirstRowColumn)
            {
                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell != null)
                    {
                        string cellValue = cell.StringCellValue;
                        if (cellValue != null)
                        {
                            DataColumn column = new DataColumn(cellValue);
                            dt.Columns.Add(column);
                        }
                    }
                }
                startRow = sheet.FirstRowNum + 1;
            }
            else
            {
                //如果是不规则表格，则用A,B,C...作列名
                var cols = Enumerable.Range(0, cellCount).Select(i => new DataColumn(_excelColNumArr[i])).ToArray();
                dt.Columns.AddRange(cols);

                startRow = sheet.FirstRowNum;
            }

            //最后一列的标号
            int rowCount = sheet.LastRowNum;
            //解决最后一行未读取问题 liufeng 2016年11月28日15:42:09
            for (int i = startRow; i <= rowCount; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue; //没有数据的行默认是null　　　　　　　

                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; ++j)
                {
                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                        dataRow[j] = GetCellValue(row.GetCell(j));
                }
                dt.Rows.Add(dataRow);
            }

            return dt;

        }


        /// <summary>
        /// 将Excel中的所有sheet页的内容导入到DataSet中
        /// </summary>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataSet</returns>
        public DataSet ExcelToDataSet(bool isFirstRowColumn)
        {
            using (DataSet ds = new DataSet())
            {
                int sheetCount = workbook.NumberOfSheets;
                for (int i = 0; i < sheetCount; ++i)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    DataTable dt = ExcelToDataTable(sheet.SheetName, isFirstRowColumn);
                    ds.Tables.Add(dt);
                }

                return ds;
            }
        }

        /// <summary>
        /// 将excel中的cell的值做转换
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private object GetCellValue(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Formula:   //公式
                    object result;
                    try
                    {
                        try
                        {
                            result = cell.StringCellValue;
                        }
                        catch
                        {
                            result = cell.NumericCellValue;
                        }
                    }
                    catch
                    {
                        result = null;
                    }
                    return result;
                case CellType.Blank:
                    return cell.StringCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                case CellType.Numeric:
                {
                    // 解决日期类型值读取问题 liufeng 2016年11月28日15:42:45
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                        return cell.DateCellValue;
                    else
                        return cell.NumericCellValue;
                }
                case CellType.String:
                    // 排除首尾空格 liufeng 2016年11月29日17:01:11
                    return cell.StringCellValue.Trim();
                default:
                    return null;
            }
        }

        //当excel无第一行列头时，添加A、B、C，此处为列头枚举
        private string[] _excelColNumArr = {
"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y","Z",
"AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
 "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
  "CA","CB","CC","CD","CE","CF","CG","CH","CI","CJ","CK","CL","CM","CN","CO","CP","CQ","CR","CS","CT","CU","CV","CW","CX","CY","CZ",
  "DA","DB","DC","DD","DE","DF","DG","DH","DI","DJ","DK","DL","DM","DN","DO","DP","DQ","DR","DS","DT","DU","DV","DW","DX","DY","DZ",
    "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
    "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
    "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ"};


        /// <summary>
        /// 实现IDispose接口
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);   //告诉垃圾回收器不要调用指定对象的Dispose方法，因为之前Dispose(true);已经做过了。防止两次执行。提高性能。
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_innerStream != null)
                        _innerStream.Close();
                }
                _innerStream = null;
                disposed = true;
            }
        }

    }
}
