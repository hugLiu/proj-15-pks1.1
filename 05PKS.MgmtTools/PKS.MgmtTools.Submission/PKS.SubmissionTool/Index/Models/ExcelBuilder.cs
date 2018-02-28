using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using PKS.Models;
using PKS.Utils;
using HAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace PKS.SubmissionTool.Index
{
    /// <summary>Excel表选项</summary>
    public class ExcelTableOptions
    {
        /// <summary>默认值</summary>
        public static string DefaultValue
        {
            get { return "标题行:1;表头行:2,2;记住格式:0;"; }
        }
        /// <summary>模式</summary>
        public static string Pattern
        {
            get { return @"^标题行\:(?<TitleRow>\d*);表头行\:(?<BeginColumnRow>\d+),(?<EndColumnRow>\d+);记住格式\:(?<RememberFormat>[01])"; }
        }
        /// <summary>标题行</summary>
        public int TitleRow { get; set; }
        /// <summary>起始列行</summary>
        public int BeginColumnRow { get; set; }
        /// <summary>结束列行</summary>
        public int EndColumnRow { get; set; }
        /// <summary>记住格式</summary>
        public bool RememberFormat { get; set; }
    }

    /// <summary>Excel生成器</summary>
    public class ExcelBuilder
    {
        /// <summary>图表标题行</summary>
        private static readonly int ChartTilteRow = 1;
        /// <summary>Y轴标题行</summary>
        private static readonly int YAxisCaptionRow = 2;
        /// <summary>图例标题行</summary>
        private static readonly int LegendTilteRow = 3;
        /// <summary>生成直方图</summary>
        public static HtmlChart BuildChart(string excelFile)
        {
            var sheet = ExcelUtil.OpenFirst(excelFile);
            var chart = new HtmlChart();
            chart.Init();
            var setting = chart.Setting;
            setting.Init();
            setting.Smooth = true;
            setting.DefautChart = "bar";
            var colIndex = 0;
            setting.Title = sheet.GetCellValueAsString(ChartTilteRow - 1, colIndex);
            setting.YAxisCaption = sheet.GetCellValueAsString(YAxisCaptionRow - 1, colIndex);
            //setting.XAxisCaption = sheet.GetCellValueAsString(LegendTilteRow - 1, colIndex);
            var firstColIndex = colIndex;
            BuildChartLegend(sheet, chart, setting, LegendTilteRow - 1, firstColIndex);
            if (chart.Columns.Count == 0)
            {
                throw new Exception("图表图例不存在!");
            }
            setting.XAxisField = chart.Columns[0].Field;
            setting.XAxisCaption = chart.Columns[0].Title;
            var lastColIndex = setting.Legend.Count;
            var firstRowIndex = LegendTilteRow;
            BuildChartData(sheet, chart, firstRowIndex, firstColIndex, lastColIndex);
            if (chart.Rows.Count == 0)
            {
                throw new Exception("图表数据不存在!");
            }
            return chart;
        }
        /// <summary>生成图例</summary>
        private static void BuildChartLegend(ISheet sheet, HtmlChart chart, HtmlChartSettings setting, int rowIndex, int firstColIndex)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) return;
            for (int i = firstColIndex; i <= row.LastCellNum; i++)
            {
                var cell = row.GetCell(i);
                if (cell == null) break;
                var title = cell.GetCellValue(cell.CellType).ToString();
                if (title.IsNullOrEmpty()) break;
                var column = new HtmlTableColumn();
                column.Field = "field" + i.ToString();
                column.Title = title;
                if (i == firstColIndex)
                {
                    column.Type = JsonDataType.String;
                }
                else
                {
                    column.Type = JsonDataType.Number;
                    setting.Legend.Add(title);
                }
                chart.Columns.Add(column);
            }
        }
        /// <summary>生成数据</summary>
        private static void BuildChartData(ISheet sheet, HtmlChart chart, int firstRowIndex, int firstColIndex, int lastColIndex)
        {
            var lastRowIndex = sheet.LastRowNum;
            for (int i = firstRowIndex; i <= lastRowIndex; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) break;
                var values = sheet.GetRowValues(row, firstColIndex, lastColIndex);
                if (values.Count == 0) break;
                if (values[0] == null) break;
                var xAxisTitle = values[0].ToString();
                if (xAxisTitle.Length == 0) break;
                values[0] = xAxisTitle;
                for (int j = 1; j < values.Count; j++)
                {
                    var value = values[j];
                    if (value == null) continue;
                    if (value is bool) continue;
                    if (value is double) continue;
                    var sValue = value.ToString();
                    if (sValue.Length == 0)
                    {
                        values[j] = null;
                    }
                    else
                    {
                        values[j] = double.Parse(sValue);
                    }
                }
                chart.Rows.Add(values);
            }
        }
        /// <summary>生成表</summary>
        public static HtmlTable BuildTable(SubmissionProduct product)
        {
            var match = Regex.Match(product.Options, ExcelTableOptions.Pattern);
            if (!match.Success)
            {
                throw new Exception("Excel表格选项无效!");
            }
            var options = new ExcelTableOptions();
            options.TitleRow = match.Groups[nameof(options.TitleRow)].Value.ToInt32();
            options.BeginColumnRow = match.Groups[nameof(options.BeginColumnRow)].Value.ToInt32();
            options.EndColumnRow = match.Groups[nameof(options.EndColumnRow)].Value.ToInt32();
            options.RememberFormat = match.Groups[nameof(options.RememberFormat)].Value.ToInt32() == 1;
            return BuildTable(product.File, options);
        }
        /// <summary>生成表</summary>
        public static HtmlTable BuildTable(string excelFile, ExcelTableOptions options)
        {
            var sheet = ExcelUtil.OpenFirst(excelFile);
            var table = new HtmlTable();
            table.Init();
            table.Unit = "pt";
            IRow row = null;
            if (options.TitleRow > 0)
            {
                table.TableName = GetTitle(sheet, options);
                table.Title = table.TableName;
            }
            var mergedRegions = sheet.GetMergedRegions();
            //生成表头集合
            var firstColumnRow = options.BeginColumnRow - 1;
            var lastColumnRow = options.EndColumnRow - 1;
            var rows = sheet.GetRows(firstColumnRow, lastColumnRow);
            var firstColumn = rows.Min(e => e.FirstCellNum);
            var lastColumn = rows.Max(e => e.LastCellNum);
            var invalidColumns = new List<int>();
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                row = rows[rowIndex];
                var header = new List<HtmlTableColumn>();
                for (int colIndex = firstColumn; colIndex <= lastColumn; colIndex++)
                {
                    if (invalidColumns.Contains(colIndex)) continue;
                    var column = new HtmlTableColumn();
                    var cell = row.GetCell(colIndex);
                    column.Field = $"field{colIndex.ToString()}";
                    //column.Width = sheet.GetColumnWidthInPoints(colIndex);
                    column.Order = colIndex;
                    column.Type = JsonDataType.String;
                    if (cell == null)
                    {
                        if (rowIndex == 0)
                        {
                            var cells = sheet.GetColumnDataCells(colIndex, row.RowNum + 1);
                            if (!cells.HaveData())
                            {
                                invalidColumns.Add(colIndex);
                                continue;
                            }
                        }
                        column.Title = string.Empty;
                    }
                    else if (!cell.IsMergedCell)
                    {
                        column.Title = cell.GetCellValue(cell.CellType).ToString();
                        //column.Align = GetHAlign(cell);
                        //column.VAlign = GetVAlign(cell);
                    }
                    else
                    {
                        var region = mergedRegions.GetMergedRegion(cell);
                        if (!region.IsMergedRegionFirstCell(cell)) continue;
                        column.Title = cell.GetCellValue(cell.CellType).ToString();
                        //column.Align = GetHAlign(cell);
                        //column.VAlign = GetVAlign(cell);
                        //column.Width = sheet.GetMergedRegionWidthInPoints(region);
                        column.RowSpan = region.LastRow - region.FirstRow + 1;
                        column.ColSpan = region.LastColumn - region.FirstColumn + 1;
                        colIndex += region.LastColumn - region.FirstColumn;
                    }
                    header.Add(column);
                }
                table.Headers.Add(header);
                table.HeaderRowHeights.Add(row.HeightInPoints);
            }
            //生成数据行
            var firstDataRow = options.EndColumnRow;
            var lastDataRow = sheet.LastRowNum;
            for (int rowIndex = firstDataRow; rowIndex <= lastDataRow; rowIndex++)
            {
                row = sheet.GetRow(rowIndex);
                if (row == null) continue;
                var rowValues = sheet.GetRowValues(row, firstColumn, lastColumn, mergedRegions, invalidColumns);
                table.Rows.Add(rowValues);
                table.RowHeights.Add(row.HeightInPoints);
            }
            //生成列集合
            if (table.Headers.Count == 1)
            {
                table.Columns.AddRange(table.Headers[0]);
                table.Headers.Clear();
            }
            //var dataFormatter = sheet.Workbook.CreateDataFormat();
            //for (int colIndex = firstColumn; colIndex <= lastColumn; colIndex++)
            //{
            //    if (invalidColumns.Contains(colIndex)) continue;
            //    var column = new HtmlTableColumn();
            //    column.Field = $"field{colIndex.ToString()}";
            //    column.Title = column.Field;
            //    column.Type = GetColumnType(table, colIndex);
            //    column.Order = colIndex;
            //    //column.Width = sheet.GetColumnWidthInPoints(colIndex);
            //    var cell = sheet.GetRow(firstDataRow)?.GetCell(colIndex);
            //    if (cell != null)
            //    {
            //        //column.Align = GetHAlign(cell);
            //        //column.VAlign = GetVAlign(cell);
            //        var format = cell.CellStyle.DataFormat;
            //        if (format > 0) column.Format = dataFormatter.GetFormat(format);
            //    }
            //    //column.Visible = false;
            //    table.Columns.Add(column);
            //}
            return table;
        }
        /// <summary>获得标题</summary>
        public static string GetTitle(ISheet sheet, ExcelTableOptions options)
        {
            var row = sheet.GetRow(options.TitleRow - 1);
            if (row == null) return null;
            var values = row.Cells.Where(cell => cell != null).Select(cell => cell.GetCellValue(cell.CellType).ToString().Trim()).ToArray();
            var values2 = values.Where(e => !e.IsNullOrEmpty()).ToList();
            if (values2.Count == 0) return string.Empty;
            var title = values2[0];
            if (values2.Count == 1) return title;
            values2.RemoveAt(0);
            var subTitle = string.Join("", values2);
            return $"{title} - {subTitle}";
        }
        /// <summary>获得列类型</summary>
        public static JsonDataType GetColumnType(HtmlTable table, int colIndex)
        {
            var columnStat = table.Rows.Select(e => e[colIndex])
                .Where(e => e != null)
                .GroupBy(e => e.GetType().GetTypeCode())
                .Select(e => new { Type = e.Key, Count = e.Count() })
                .OrderByDescending(e => e.Count)
                .FirstOrDefault();
            if (columnStat == null) return JsonDataType.String;
            switch (columnStat.Type)
            {
                case TypeCode.Boolean: return JsonDataType.Boolean;
                case TypeCode.Double: return JsonDataType.Number;
                default: return JsonDataType.String;
            }
        }
        /// <summary>获得水平对齐</summary>
        public static HtmlHAlign GetHAlign(ICell cell)
        {
            switch (cell.CellStyle.Alignment)
            {
                case HAlignment.Left: return HtmlHAlign.Left;
                case HAlignment.Right: return HtmlHAlign.Right;
                case HAlignment.Center: return HtmlHAlign.Center;
                case HAlignment.Justify: return HtmlHAlign.Justify;
                default: return HtmlHAlign.Default;
            }
        }
        /// <summary>获得垂直对齐</summary>
        public static HtmlVAlign GetVAlign(ICell cell)
        {
            switch (cell.CellStyle.VerticalAlignment)
            {
                case VerticalAlignment.Top: return HtmlVAlign.Top;
                case VerticalAlignment.Bottom: return HtmlVAlign.Bottom;
                case VerticalAlignment.Center: return HtmlVAlign.Middle;
                default: return HtmlVAlign.Default;
            }
        }
    }
}
