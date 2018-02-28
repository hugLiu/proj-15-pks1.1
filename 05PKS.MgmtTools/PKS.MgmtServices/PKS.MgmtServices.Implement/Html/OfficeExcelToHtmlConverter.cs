using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using PKS.Core;
using PKS.Utils;
using PKS.Models;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Excel转HTML文件转换器</summary>
    public class OfficeExcelToHtmlConverter : FileConverter, IHtmlConverter, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "xls", "xlsx" }; }
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "html"; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string destFile)
        {
            SaveAsHtml(sourceFile, destFile);
            var folder = Path.GetDirectoryName(destFile) + @"\";
            folder += Path.GetFileNameWithoutExtension(destFile) + @".files";
            try
            {
                var sheetFile = folder + @"\sheet001.html";
                ExtractTable(sheetFile, destFile);
                return true;
            }
            finally
            {
                Directory.Delete(folder, true);
            }
        }
        /// <summary>生成PDF文件</summary>
        private void SaveAsHtml(string sourceFile, string destFile)
        {
            ApplicationClass application = null;
            Workbook workbook = null;
            try
            {
                application = new ApplicationClass();
                application.Visible = false;
                workbook = application.Workbooks.Open(sourceFile);
                workbook.WebOptions.AllowPNG = false;
                workbook.WebOptions.Encoding = MsoEncoding.msoEncodingUTF8;
                workbook.WebOptions.OrganizeInFolder = true;
                workbook.WebOptions.RelyOnCSS = false;
                workbook.WebOptions.RelyOnVML = false;
                workbook.WebOptions.TargetBrowser = MsoTargetBrowser.msoTargetBrowserIE6;
                workbook.WebOptions.UseLongFileNames = true;
                //var missing = Type.Missing;
                workbook.SaveAs(destFile, XlFileFormat.xlHtml);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                }
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                }
            }
        }
        /// <summary>提取表标签</summary>
        private void ExtractTable(string sheetFile, string destFile)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(sheetFile, Encoding.UTF8);
            var tableNode = htmlDoc.DocumentNode.Element("html").Element("body").Element("table");
            CheckNodes(tableNode);
            var content = tableNode.OuterHtml;
            content = content.Replace(Environment.NewLine, string.Empty);
            content = "<div class='jurassic-table-excel'>" + content + "</div>";
            File.WriteAllText(destFile, content, Encoding.UTF8);
        }
        /// <summary>检查表标签</summary>
        private void CheckNodes(HtmlNode node)
        {
            CheckNodeAttributes(node);
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var child = node.ChildNodes[i];
                if (child.NodeType == HtmlNodeType.Element)
                {
                    CheckNodes(child);
                }
                else if(child.NodeType == HtmlNodeType.Comment)
                {
                    child.Remove();
                    i--;
                }
            }
        }
        /// <summary>检查表标签</summary>
        private void CheckNodeAttributes(HtmlNode node)
        {
            var style = node.Attributes.FirstOrDefault(e => e.Name == "style");
            switch (node.Name)
            {
                case "table":
                    CheckAttributes(node, "style");
                    if (style != null) CheckStyleValues(style, "width");
                    break;
                case "col":
                    CheckAttributes(node, "span", "style");
                    if (style != null) CheckStyleValues(style, "width");
                    break;
                case "tr":
                    CheckAttributes(node, "style");
                    if (style != null) CheckStyleValues(style, "height");
                    break;
                case "td":
                    CheckAttributes(node, "rowspan", "colspan", "style");
                    if (style != null) CheckStyleValues(style, "width");
                    break;
            }
        }
        /// <summary>检查特性值</summary>
        private void CheckAttributes(HtmlNode node, params string[] reservedNames)
        {
            if (reservedNames.IsNullOrEmpty())
            {
                node.Attributes.RemoveAll();
                return;
            }
            for (int i = 0; i < node.Attributes.Count; i++)
            {
                var attribute = node.Attributes[i];
                if (!reservedNames.Any(e => e.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    attribute.Remove();
                    i--;
                }
            }
        }
        /// <summary>检查风格值</summary>
        private void CheckStyleValues(HtmlAttribute style, params string[] reservedNames)
        {
            var stylePairs = style.Value.Trim().Replace(Environment.NewLine, string.Empty)
                .Split(';')
                .Select(e => e.Split(':'))
                .Where(e => e.Length == 2)
                .ToDictionary(e => e[0], e2 => e2[1]);
            var keys = stylePairs.Keys.ToArray();
            var sBuilder = new StringBuilder();
            foreach (var key in keys)
            {
                if (reservedNames.Any(e => e.Equals(key, StringComparison.OrdinalIgnoreCase)))
                {
                    sBuilder.Append(key).Append(":").Append(stylePairs[key]).Append(";");
                }
            }
            style.Value = sBuilder.ToString();
        }
    }
}