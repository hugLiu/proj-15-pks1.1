using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using PKS.Utils;

namespace PKS.SubmissionTool
{
    /// <summary>Excel单元属性</summary>
    public class CellProperty
    {
        #region 单元属性
        /// <summary>值</summary>
        public object Value { get; set; }
        /// <summary>值类型</summary>
        public Type ValueType { get; set; }
        /// <summary>列跨度</summary>
        public int Span { get; set; }
        /// <summary>列宽度</summary>
        public int Width { get; set; }
        /// <summary>列宽度</summary>
        public int[] Widths { get; set; }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
        #endregion
    }

    /// <summary>Excel工具</summary>
    public static class ExcelUtil
    {
        /// <summary>是否是Excel文件</summary>
        public static bool Support(string ext)
        {
            return ext.In("xls", "xlsx");
        }
        /// <summary>打开文档中的第一个表单</summary>
        public static ISheet OpenFirst(string file)
        {
            var workbook = WorkbookFactory.Create(file);
            return workbook.GetSheetAt(0);
        }
        /// <summary>创建文档中的第一个表单</summary>
        public static ISheet CreateSheet(string file, string sheetName = "sheet1")
        {
            var isXLSX = file.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase);
            IWorkbook excelWorkBook = null;
            if (isXLSX)
            {
                excelWorkBook = new XSSFWorkbook();
            }
            else
            {
                excelWorkBook = new HSSFWorkbook();
            }
            return excelWorkBook.CreateSheet(sheetName);
        }
        /// <summary>加入一行</summary>
        public static void AddRow(this ISheet sheet, int rowIndex, List<CellProperty> cellData, bool setWidth)
        {
            var row = sheet.CreateRow(rowIndex);
            var colIndex = 0;
            for (int i = 0; i < cellData.Count; i++)
            {
                var property = cellData[i];
                var cell = row.CreateCell(colIndex++);
                var valueType = property.ValueType;
                if (valueType == null)
                {
                    if (property.Value == null)
                    {
                        valueType = typeof(string);
                    }
                    else
                    {
                        valueType = property.Value.GetType();
                    }
                }
                if (valueType == typeof(int))
                {
                    cell.SetCellType(CellType.Numeric);
                    if (property.Value == null)
                    {
                        cell.SetCellValue((string)null);
                    }
                    else
                    {
                        cell.SetCellValue((int)property.Value);
                    }
                }
                else if (valueType == typeof(double))
                {
                    cell.SetCellType(CellType.Numeric);
                    if (property.Value == null)
                    {
                        cell.SetCellValue((string)null);
                    }
                    else
                    {
                        cell.SetCellValue((double)property.Value);
                    }
                }
                else if (valueType == typeof(DateTime))
                {
                    cell.SetCellType(CellType.Numeric);
                    if (property.Value == null)
                    {
                        cell.SetCellValue((string)null);
                    }
                    else
                    {
                        cell.SetCellValue(DateUtil.GetExcelDate((DateTime)property.Value));
                    }
                }
                else
                {
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(property.Value?.ToString());
                }
                if (setWidth)
                {
                    var width = property.Widths == null ? property.Width : property.Widths[0];
                    if (width <= 0)
                    {
                        sheet.SetColumnHidden(cell.ColumnIndex, true);
                    }
                    else
                    {
                        sheet.SetColumnWidth(cell.ColumnIndex, width * 256);
                    }
                }
                if (property.Span > 1)
                {
                    for (int j = 1; j < property.Span; j++)
                    {
                        var cell2 = row.CreateCell(colIndex++);
                        cell2.SetCellType(CellType.String);
                        if (setWidth)
                        {
                            var width = property.Widths[j];
                            sheet.SetColumnWidth(cell2.ColumnIndex, width * 256);
                        }
                    }
                    var region = new CellRangeAddress(row.RowNum, row.RowNum, cell.ColumnIndex, cell.ColumnIndex + property.Span - 1);
                    sheet.AddMergedRegion(region);
                }
            }
        }
        /// <summary>生成合并单元格</summary>
        public static void BuildMergedRegion(this ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            var region = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            sheet.AddMergedRegion(region);
        }
        /// <summary>设置标题风格</summary>
        public static void SetTitleStyle(this ISheet sheet, int firstRow, int lastRow)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            var font = style.GetFont(sheet.Workbook);
            var newFont = sheet.Workbook.CreateFont();
            newFont.FontName = font.FontName;
            //newFont.Charset = font.Charset;
            //newFont.Color = font.Color;
            //newFont.FontHeight = font.FontHeight + 2;
            newFont.FontHeightInPoints = (short)(font.FontHeightInPoints + 2);
            //newFont.IsItalic = font.IsItalic;
            //newFont.IsStrikeout = font.IsStrikeout;
            newFont.Boldweight = (short)FontBoldWeight.Bold;
            style.SetFont(newFont);
            for (int i = firstRow; i <= lastRow; i++)
            {
                sheet.GetRow(i).Cells.ForEach(e => e.CellStyle = style);
            }
            //if (setBorder)
            //{
            //    RegionUtil.SetBorderLeft((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //    RegionUtil.SetBorderTop((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //    RegionUtil.SetBorderRight((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //    RegionUtil.SetBorderBottom((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //}
        }
        /// <summary>设置数字风格</summary>
        public static void SetNumberStyle(this ISheet sheet, int firstRow, int column)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            var lastRow = 65535;
            for (int i = firstRow; i <= lastRow; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) break;
                var cell = row.GetCell(column);
                if (cell == null)
                {
                    cell = row.CreateCell(column);
                }
                cell.SetCellType(CellType.Numeric);
                //cell.SetCellValue((string)null);
                cell.CellStyle = style;
            }
        }
        /// <summary>设置日期风格</summary>
        public static void SetDateTimeStyle(this ISheet sheet, int firstRow, int column)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Left;
            var format = sheet.Workbook.CreateDataFormat();
            style.DataFormat = format.GetFormat(DateTimeUtil.StandardFormat);
            var lastRow = 65535;
            for (int i = firstRow; i <= lastRow; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) break;
                var cell = row.GetCell(column);
                if (cell == null)
                {
                    cell = row.CreateCell(column);
                }
                //cell.SetCellType(CellType.Numeric);
                //cell.SetCellValue((string)null);
                cell.CellStyle = style;
            }
        }
        /// <summary>设置列下拉列表</summary>
        public static void SetColumnDropdownList(this ISheet sheet, string[] values, int startRow, int column)
        {
            var regions = new CellRangeAddressList(startRow, 65535, column, column);
            var helper = sheet.GetDataValidationHelper();
            var constraint = helper.CreateExplicitListConstraint(values);
            var validation = helper.CreateValidation(constraint, regions);
            validation.CreateErrorBox("输入不合法", "请输入下拉列表中的值。");
            validation.ShowPromptBox = true;
            sheet.AddValidationData(validation);
        }
        /// <summary>转换为数据表</summary>
        public static DataTable ToDataTable(this ISheet sheet, int fieldRowIndex, int startRowIndex)
        {
            var table = new DataTable(sheet.SheetName);
            table.Rows.Clear();
            table.Columns.Clear();
            var fieldRow = sheet.GetRow(fieldRowIndex);
            if (fieldRow == null) return table;
            var mapper = new Dictionary<int, DataColumn>();
            foreach (var cell in fieldRow.Cells)
            {
                if (cell.CellType != CellType.String) continue;
                var colName = cell.StringCellValue;
                if (colName.IsNullOrEmpty()) continue;
                var newCol = new DataColumn(colName, typeof(object));
                mapper[cell.ColumnIndex] = newCol;
                table.Columns.Add(newCol);
            }
            var lastRowNum = sheet.LastRowNum;
            for (int rowIndex = startRowIndex; rowIndex <= lastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null) continue;
                var newRow = table.NewRow();
                foreach (var cell in row.Cells)
                {
                    if (cell == null) continue;
                    var dataColumn = mapper.GetValueOrDefaultBy(cell.ColumnIndex, null);
                    if (dataColumn == null) continue;
                    newRow[dataColumn] = GetCellValue(cell, cell.CellType);
                }
                table.Rows.Add(newRow);
            }
            table.AcceptChanges();
            return table;
        }
        /// <summary>获得列宽</summary>
        public static double GetColumnWidthInPoints(this ISheet sheet, int colIndex)
        {
            var width = sheet.GetColumnWidth(colIndex);
            if (width == 0)
            {
                width = sheet.DefaultColumnWidth;
                if (width == 0) return 10.0;
            }
            return width / 256.0;
        }
        /// <summary>获得全部合并单元</summary>
        public static List<CellRangeAddress> GetMergedRegions(this ISheet sheet)
        {
            var regions = new List<CellRangeAddress>();
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                var region = sheet.GetMergedRegion(i);
                regions.Add(region);
            }
            return regions;
        }
        /// <summary>获得合并区域</summary>
        public static CellRangeAddress GetMergedRegion(this List<CellRangeAddress> regions, ICell cell)
        {
            return regions.First(e => cell.RowIndex >= e.FirstRow && cell.RowIndex <= e.LastRow && cell.ColumnIndex >= e.FirstColumn && cell.ColumnIndex <= e.LastColumn);
        }
        /// <summary>获得合并区域宽度</summary>
        public static double GetMergedRegionWidthInPoints(this ISheet sheet, CellRangeAddress region)
        {
            var width = 0.0;
            for (int i = region.FirstColumn; i <= region.LastColumn; i++)
            {
                width += sheet.GetColumnWidthInPoints(i);
            }
            return width;
        }
        /// <summary>获得合并区域的第一个单元</summary>
        public static ICell GetMergedRegionFirstCell(this List<CellRangeAddress> regions, ICell cell)
        {
            var region = regions.GetMergedRegion(cell);
            return cell.Sheet.GetRow(region.FirstRow).GetCell(region.FirstColumn);
        }
        /// <summary>是否合并区域的第一个单元</summary>
        public static bool IsMergedRegionFirstCell(this CellRangeAddress region, ICell cell)
        {
            return cell.ColumnIndex == region.FirstColumn && cell.RowIndex == region.FirstRow;
        }
        /// <summary>获得某行数据集合</summary>
        public static List<object> GetRowValues(this ISheet sheet, IRow row, int firstColInex, int lastColIndex, List<CellRangeAddress> regions = null, List<int> excludeColumns = null)
        {
            var values = new List<object>();
            for (int i = firstColInex; i <= lastColIndex; i++)
            {
                if (!excludeColumns.IsNullOrEmpty() && excludeColumns.Contains(i)) continue;
                var cell = row.GetCell(i);
                object value = null;
                if (cell == null)
                {
                    //无值
                }
                else if (!cell.IsMergedCell)
                {
                    value = GetCellValue(cell, cell.CellType);
                }
                else if (regions.IsNullOrEmpty())
                {
                    value = string.Empty;
                }
                else
                {
                    var valueCell = regions.GetMergedRegionFirstCell(cell);
                    value = GetCellValue(valueCell, valueCell.CellType);
                }
                values.Add(value);
            }
            return values;
        }
        /// <summary>获得单元字符串值</summary>
        public static ICell GetCell(this ISheet sheet, int rowIndex, int colIndex)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) return null;
            return row.GetCell(colIndex);
        }
        /// <summary>获得单元字符串值</summary>
        public static string GetCellValueAsString(this ISheet sheet, int rowIndex, int colIndex)
        {
            var cell = sheet.GetCell(rowIndex, colIndex);
            if (cell == null) return null;
            return GetCellValue(cell, cell.CellType).ToString();
        }
        /// <summary>获得单元值</summary>
        public static object GetCellValue(this ICell cell, CellType cellType)
        {
            switch (cellType)
            {
                case CellType.String: return cell.StringCellValue.Trim();
                case CellType.Boolean: return cell.BooleanCellValue;
                case CellType.Numeric: return cell.NumericCellValue;
                case CellType.Formula: return GetCellValue(cell, cell.CachedFormulaResultType);
                case CellType.Error: return cell.StringCellValue;
                default: return string.Empty;
            }
        }
        /// <summary>从Excel日期值转换</summary>
        public static DateTime GetDateFromCell(this ISheet sheet, double value)
        {
            return DateUtil.GetJavaDate(value, sheet.IsUsing1904DateWindowing());
        }
        /// <summary>日期使用方式</summary>
        public static bool IsUsing1904DateWindowing(this ISheet sheet)
        {
            var workbook = sheet.Workbook;
            if (workbook is HSSFWorkbook)
            {
                return workbook.As<HSSFWorkbook>().Workbook.IsUsing1904DateWindowing;
            }
            return workbook.As<XSSFWorkbook>().IsDate1904();
        }
        /// <summary>获得某列数据单元集合</summary>
        public static List<ICell> GetColumnDataCells(this ISheet sheet, int colIndex, int startRowIndex)
        {
            var lastRowNum = sheet.LastRowNum;
            var cells = new List<ICell>();
            for (int rowIndex = startRowIndex; rowIndex < lastRowNum; rowIndex++)
            {
                cells.Add(sheet.GetRow(rowIndex)?.GetCell(colIndex));
            }
            return cells;
        }
        /// <summary>是否有数据</summary>
        public static bool HaveData(this List<ICell> cells)
        {
            return cells.Any(e => e != null && e.CellType != CellType.Unknown && e.CellType != CellType.Blank);
        }
        /// <summary>获得行集合</summary>
        public static List<IRow> GetRows(this ISheet sheet, int firstRow, int lastRow)
        {
            var rows = new List<IRow>();
            for (int i = firstRow; i <= lastRow; i++)
            {
                rows.Add(sheet.GetRow(i));
            }
            return rows;
        }
    }
}
