using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.Xml;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using Jurassic.GF.Server;
using Jurassic.GF.Interface.Model;
using DevExpress.XtraGrid.Views.Grid;
using GGGXParse;
using DevExpress.XtraGrid;
using System.IO;
using System.Net;
using Jurassic.GF.Interface;
using Jurassic.GF.DataGatherTool;

namespace DataGatherTool
{
    public partial class ImpExcelData : XtraForm
    {
        List<ObjectTypeModel> objTypelist;
        List<GeoFeature> AllFeature;
        List<GeoFeature> SaveFeature;
        List<GeoFeature> TempFeature;
        List<Tuple<int, string, string, bool>> ErrorFeature;
        bool HasError = false;
        int currRow = 0;
        List<string> columnDate = new List<string>();
        List<string> columnDecimal = new List<string>();
        List<string> bonameL = new List<string>();
        string bot;
        string Botid;
        List<XmlDocument> xmlPropertySetL = new List<XmlDocument>();
        System.Data.DataTable data = new System.Data.DataTable();
        System.Data.DataTable LayerData = new System.Data.DataTable();
        Microsoft.Office.Interop.Excel.Application excel;
        private bool tempFile = false;
        DataRow dr;//记录选择的数据
        string lzid;//记录选择的图元
        Worksheet ws;
        Workbook wb;
        //图件名称
        string mapSource = string.Empty;
        public ImpExcelData()
        {
            InitializeComponent();
            Load += ImpExcelData_Load;
        }
        /// <summary>
        ///初始化右键菜单
        /// </summary>
        private void InitRightMenu()
        {
            axJoWeb.PM_ResetTools();
            axJoWeb.PM_AddTool(13);
            axJoWeb.PM_AddTool(14);
            axJoWeb.PM_AddTool(15);
            axJoWeb.PM_AddTool(17);
            axJoWeb.PM_AddTool(-1);
            axJoWeb.PM_AddTool(0);
            this.axJoWeb.OnSelectOneElement += new AxJoWebLib._DJoWebEvents_OnSelectOneElementEventHandler(axJoWeb_OnSelectOneElement);
        }
        void ImpExcelData_Load(object sender, EventArgs e)
        {
            labelControl8.Text = Dns.GetHostName();
            string _dal = ConfigurationManager.AppSettings["GFDAL"];//获取App.Config中DAL
            string _conn = ConfigurationManager.AppSettings["GFSqlConn"];//获取App.Config中DAL
            if (!string.IsNullOrEmpty(_dal) && !string.IsNullOrEmpty(_conn))
            {
                ObjectTypeServer objTypeServer = new ObjectTypeServer();
                objTypelist = objTypeServer.GetAllObjectType();
                this.comboObjType.DataSource = objTypelist;
                comboObjType.DisplayMember = "Bot";
                comboObjType.ValueMember = "Botid";
                string botid = (comboObjType.SelectedItem as ObjectTypeModel).BOTID;
                GetDatagatherByBOTID(botid);
                dateEdit1.Text = DateTime.Now.ToString();
                //删除临时文件
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\TempFile\\"))
                {
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\TempFile\\");
                }
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\LogFile\\"))
                {
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\LogFile\\");
                }
                File.Delete(System.Windows.Forms.Application.StartupPath + "\\TempFile\\" + "临时文件数据.xlsx");
                List<string> list = GetAllFilesInDirectory(System.Windows.Forms.Application.StartupPath + "\\TempFile\\");
                if (list.Count > 0)
                {
                    list.Insert(0, "请选择未导入完成的数据");
                }
                comFilePath.DataSource = list;
            }
            else
            {
                DataBaseSetting dbSetting = new DataBaseSetting();
                dbSetting.ShowDialog();
                ImpExcelData_Load(null, null);
            }
        }

        /// <summary>
        /// 获取临时文件目录下载文件
        /// </summary>
        /// <param name="strDirectory"></param>
        /// <returns></returns>
        public List<string> GetAllFilesInDirectory(string strDirectory)
        {
            List<string> list = new List<string>();
            DirectoryInfo mydir = new DirectoryInfo(strDirectory);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo)fsi;
                    list.Add(fi.FullName);
                }
            }
            return list;
        }
        /// <summary>
        /// 绑定版本
        /// </summary>
        /// <param name="botid"></param>
        private void GetDatagatherByBOTID(string botid)
        {
            DataGatherServer dataGatherServer = new DataGatherServer();
            List<DataGatherModel> dataGartherList = dataGatherServer.GetDataGatherByBOTID(botid);
            dataGartherList.Insert(0, new DataGatherModel() { ENVENT = "--创建新版本--" });
            combVersion.DataSource = dataGartherList;
            combVersion.DisplayMember = "ENVENT";
            combVersion.ValueMember = "GATHERID";
            combVersion.SelectedIndex = 0;
            textEdit1.Text = null;
        }

        /// <summary>
        /// 选择图元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axJoWeb_OnSelectOneElement(object sender, AxJoWebLib._DJoWebEvents_OnSelectOneElementEvent e)
        {
            lzid = e.lpszID;
            string shapeName = (comboObjType.SelectedItem as ObjectTypeModel).SHAPE;
            if (radioGroup3.SelectedIndex == 2)
            {
                if (dr != null)
                {
                    if (shapeName == "Point")
                    {
                        if (axJoWeb.PM_GetElementType(lzid) != 1 && axJoWeb.PM_GetElementType(lzid) != 11)
                        {
                            MessageBox.Show("图元类型与目标几何类型不匹配！");
                            return;
                        }
                    }
                    if (shapeName == "Line")
                    {
                        if (axJoWeb.PM_GetElementType(lzid) != 2 && axJoWeb.PM_GetElementType(lzid) != 16 && axJoWeb.PM_GetElementType(lzid) != 17 && axJoWeb.PM_GetElementType(lzid) != 20)
                        {

                            MessageBox.Show("图元类型与目标几何类型不匹配！");
                            return;
                        }
                    }
                    if (shapeName == "Polygon")
                    {
                        if (axJoWeb.PM_GetElementType(lzid) != 3)
                        {

                            MessageBox.Show("图元类型与目标几何类型不匹配！");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(dr["layerName"].ToString()))
                    {
                        DialogResult result = MessageBox.Show("坐标已经存在是否替换!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (result == DialogResult.OK)
                        {
                            dr["layerName"] = lzid;
                            dr["sourceDB"] = mapSource + lzid;
                            dr["匹配完成"] = true;
                            gridView1.FocusedRowHandle = currRow;
                            dr = null;
                        }
                    }
                    else
                    {
                        dr["layerName"] = lzid;
                        dr["sourceDB"] = mapSource + lzid;
                        dr["匹配完成"] = true;
                        gridView1.FocusedRowHandle = currRow;
                        dr = null;
                    }

                }
            }
            else
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    DataRow currdr = gridView1.GetDataRow(i);
                    if (currdr["layerName"].ToString() == lzid)
                    {
                        this.gridView1.FocusedRowHandle = i;
                        continue;
                    }
                }
                lzid = null;
                dr = null;
                currRow = 0;
            }
            //图元类型

        }
        /// <summary>
        /// 选择对象类型获取对象类型的应用场景以及参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combObjType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboObjType.SelectedItem != null)
            {
                string botid = (comboObjType.SelectedItem as ObjectTypeModel).BOTID;
                GetDatagatherByBOTID(botid);
                List<string> list = new List<string>();
                if (ConfigurationManager.AppSettings[(comboObjType.SelectedItem as ObjectTypeModel).BOT] != null)
                {
                    list = ConfigurationManager.AppSettings[(comboObjType.SelectedItem as ObjectTypeModel).BOT].Split(' ').ToList();
                }
                comboBox5.DataSource = list;
                List<ObjTypePropertyModel> listProperty = new ObjTypePropertyServer().GetObjPropertyByBOTID(botid);
                comboNS.Properties.DataSource = listProperty;
                comboNS.Properties.DisplayMember = "NS";
                comboNS.Properties.ValueMember = "NS";

            }
        }

        /// <summary>
        ///生成模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreatTemplet_Click(object sender, EventArgs e)
        {
            List<object> obj = this.comboNS.Properties.Items.GetCheckedValues().ToList();
            List<ObjTypePropertyModel> objTypePro = new List<ObjTypePropertyModel>();
            if (obj != null && obj.Count > 0)
            {
                List<ObjTypePropertyModel> list = new ObjTypePropertyServer().GetObjPropertyByBOTID((comboObjType.SelectedItem as ObjectTypeModel).BOTID);
                foreach (object ob in obj)
                {
                    string botid = ob.ToString();
                    objTypePro.Add(list.Find(p => p.BOTID == (comboObjType.SelectedItem as ObjectTypeModel).BOTID && p.NS == ob.ToString()));
                }
                ExportExcel(objTypePro);
            }
        }


        string columnstr = ConfigurationManager.AppSettings["导出Excel模板"].ToString();



        /// <summary>
        /// Excel模板导出
        /// </summary>
        /// <param name="objTypePro"></param>
        protected void ExportExcel(List<ObjTypePropertyModel> objTypePro)
        {
            if (objTypePro == null || objTypePro.Count == 0)
            {
                MessageBox.Show("请选择参数应用场景!");
                wb.Close();
                wb = null;
                excel.Quit();
                KeyMyExcelProcess.Kill(excel);//调用方法关闭进程
                GC.Collect();
                return;
            }
            try
            {
                SaveFileDialog savefileDialog = new SaveFileDialog { Filter = "Excel files|*.xlsx", DefaultExt = "xlsx" };

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    return;
                }
                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Workbooks workbooks = xlApp.Workbooks;
                Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet worksheet = (Worksheet)workbook.Worksheets[1];
                Range range;

                string[] column = columnstr.Split(',');
                for (int i = 0; i < column.Count(); i++)
                {
                    worksheet.Cells[1, i + 1] = column[i];
                    range = (Range)worksheet.get_Range(ExcelColumnFromNumber(i + 1) + "1", ExcelColumnFromNumber(i + 1) + "3");
                    range.Font.Bold = true;
                    range.Borders.LineStyle = 1;
                    range.EntireColumn.AutoFit();
                    range.Merge(0);
                }
                Regex RegexWords = new Regex(",");
                int botIndex = RegexWords.Matches(columnstr.Substring(0, columnstr.IndexOf("对象类型"))).Count + 1;
                //前三行是表头，第四行开始是数据行
                worksheet.Cells[4, 1] = comboObjType.Text;
                int tr = 0;

                foreach (ObjTypePropertyModel objtp in objTypePro)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(objtp.MD);
                    XmlNodeList P = xmldoc.SelectNodes("PropertySet/P");
                    worksheet.Cells[1, column.Count() + 1 + tr] = objtp.NS;
                    range = (Range)worksheet.get_Range(ExcelColumnFromNumber(tr + column.Count() + 1) + "1", ExcelColumnFromNumber(tr + column.Count() + P.Count) + "1");
                    range.Font.Bold = true;
                    range.Borders.LineStyle = 1;
                    range.EntireColumn.AutoFit();
                    range.Merge(0);
                    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    for (int r = 0; r < P.Count; r++)
                    {
                        worksheet.Cells[2, column.Count() + 1 + tr + r] = P[r].Attributes["n"].Value;
                        worksheet.Cells[3, column.Count() + 1 + tr + r] = P[r].Attributes["t"].Value;
                        range = (Range)worksheet.Cells[2, column.Count() + 1 + tr + r];
                        range.Font.Bold = true;
                        range.Borders.LineStyle = 1;
                        range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        range = (Range)worksheet.Cells[3, column.Count() + 1 + tr + r];
                        range.Font.Bold = true;
                        range.Borders.LineStyle = 1;
                        range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    tr = tr + P.Count;
                }

                xlApp.Visible = false;
                if (savefileDialog.ShowDialog() == DialogResult.OK)
                {
                    xlApp.DisplayAlerts = false;
                    xlApp.AlertBeforeOverwriting = true;
                    workbook.SaveAs(savefileDialog.FileName);
                    workbook.Saved = true;
                    xlApp.Quit();
                    MessageBox.Show("模版生成完成！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cancelled Operation:" + ex.Message);
                wb.Close();
                wb = null;
                excel.Quit();
                KeyMyExcelProcess.Kill(excel);//调用方法关闭进程
                GC.Collect();
                return;
            }
        }

        /// <summary>
        /// 浏览加载EXCEL文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLookExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "2007Excel Files|*.xlsx|2003Excel Files|*.xls";
            openFileDialog1.Title = "Select a Excel File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadExcel(openFileDialog1.FileName);
            }
        }
        /// <summary>
        /// 加载已有文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comFilePath_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comFilePath.SelectedItem != null && !string.IsNullOrEmpty(comFilePath.Text) && comFilePath.Text != "请选择未导入完成的数据")
            {
                tempFile = true;
                LoadExcel(comFilePath.Text);
            }
        }
        /// <summary>
        /// 选择未完成的任务加载
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadExcel(string filePath)
        {
            xmlPropertySetL.Clear();
            bonameL.Clear();
            HasError = false;
            data = new System.Data.DataTable();
            comFilePath.Text = filePath;
            excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false; excel.UserControl = true;
            wb = excel.Workbooks._Open(filePath);
            ws = (Worksheet)wb.Worksheets.get_Item(1);
            Regex RegexWords = new Regex(",");
            int botIndex = RegexWords.Matches(columnstr.Substring(0, columnstr.IndexOf("对象类型"))).Count + 1;
            bot = ws.Cells.get_Range(ExcelColumnFromNumber(botIndex) + 4).Value2;//获取对象类型
            if (bot == null)
            {
                MessageBox.Show("对象类型缺失！");
                wb.Close();
                wb = null;
                excel.Quit();
                KeyMyExcelProcess.Kill(excel);//调用方法关闭进程
                GC.Collect();
                return;
            }
            comboObjType.Text = bot;
            List<ObjectTypeModel> objtype = new ObjectTypeServer().GetAllObjectType().Where(s => s.BOT == bot).ToList();
            if (objtype.Count != 1)
            {
                MessageBox.Show("对象类型不一致，请检查！");
                wb.Close();
                wb = null;
                excel.Quit();
                KeyMyExcelProcess.Kill(excel);//调用方法关闭进程
                GC.Collect();
                return;
            }
            else Botid = objtype[0].BOTID;
            int rowsint = ws.UsedRange.Rows.Count; //得到行数
            int columnsint = ws.UsedRange.Columns.Count;//得到列数
            BandedGridView view = advBandedGridView1 as BandedGridView;
            view.BeginUpdate(); //开始视图的编辑，防止触发其他事件
            view.Bands.Clear();

            //修改附加选项
            view.OptionsView.ShowColumnHeaders = false;                         //因为有Band列了，所以把ColumnHeader隐藏
            view.OptionsView.EnableAppearanceEvenRow = false;                   //是否启用偶数行外观
            view.OptionsView.EnableAppearanceOddRow = true;                     //是否启用奇数行外观
            view.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;   //是否显示过滤面板
            view.OptionsCustomization.AllowColumnMoving = false;                //是否允许移动列
            view.OptionsCustomization.AllowColumnResizing = false;              //是否允许调整列宽
            view.OptionsCustomization.AllowGroup = false;                       //是否允许分组
            view.OptionsCustomization.AllowFilter = false;                      //是否允许过滤
            view.OptionsCustomization.AllowSort = true;                         //是否允许排序
            view.OptionsSelection.EnableAppearanceFocusedCell = true;
            //是否启用焦点单元格的外观设置
            view.OptionsBehavior.Editable = true;                               //是否允许用户编辑单元格


            //获取表头信息；判断列的数据类型，作为校验依据；生成propertySet模板；
            List<GridBand> gbL = new List<GridBand>();
            StringBuilder filter = new StringBuilder();
            for (int i = 1; i < columnsint + 1; ++i)
            {
                Range rng1 = ws.Cells.get_Range(ExcelColumnFromNumber(i + 1) + 1);
                Range rng2 = ws.Cells.get_Range(ExcelColumnFromNumber(i + 1) + 2);
                if (rng1.MergeArea.Row == rng2.MergeArea.Row && rng1.MergeArea.Column == rng2.MergeArea.Column)
                {
                    DataColumn column = new DataColumn(rng1.Value2);
                    data.Columns.Add(column);
                    DataColumn[] cols = new DataColumn[] { column };
                    data.PrimaryKey = cols;


                    filter.Append(column.Caption + " IS NOT NULL OR ");
                    GridBand gb = view.Bands.AddBand(column.Caption);
                    gbL.Add(gb);
                }
                else if (rng1.Value2 != null)
                {
                    GridBand gb = view.Bands.AddBand(rng1.Value2);
                    List<GridBand> gbsub = new List<GridBand>();
                    XmlDocument myXmlDoc = new XmlDocument();
                    XmlElement propertySetElement = myXmlDoc.CreateElement("PropertySet");
                    propertySetElement.SetAttribute("name", rng1.Value2);
                    myXmlDoc.AppendChild(propertySetElement);

                    for (int j = 0; j < rng1.MergeArea.Count; j++)
                    {
                        Range rng3 = ws.Cells.get_Range(ExcelColumnFromNumber(i + j + 1) + 2);
                        Range rng4 = ws.Cells.get_Range(ExcelColumnFromNumber(i + j + 1) + 3);
                        DataColumn column = new DataColumn(rng3.Value2);
                        if (rng4.Value2 == "Date") { columnDate.Add(rng3.Value2); }
                        if (rng4.Value2 == "Decimal") { columnDecimal.Add(rng3.Value2); }

                        data.Columns.Add(column);
                        filter.Append(column.Caption + " IS NOT NULL OR ");

                        GridBand gbs = view.Bands.AddBand(column.Caption);
                        gbsub.Add(gbs);
                        gbL.Add(gbs);
                        XmlElement pElement = myXmlDoc.CreateElement("P");
                        pElement.SetAttribute("n", rng3.Value2);
                        pElement.SetAttribute("t", rng4.Value2);
                        propertySetElement.AppendChild(pElement);
                    }
                    gb.Children.AddRange(gbsub.ToArray());
                    xmlPropertySetL.Add(myXmlDoc);
                }
            }
            data.Columns.Add(new DataColumn("保留/覆盖", typeof(bool)));
            //获取对象记录
            for (int i = 4; i <= rowsint; ++i)
            {
                Range rng2 = ws.Cells.get_Range("B" + i, ExcelColumnFromNumber(columnsint) + i);
                object[,] row = (object[,])rng2.Value2;

                int bonameIndex = RegexWords.Matches(columnstr.Substring(0, columnstr.IndexOf("对象名称"))).Count + 1;
                DataRow dataRow = data.NewRow();
                for (int j = 2; j <= columnsint; ++j)
                {
                    if (row[1, j - 1] != null)
                    {
                        dataRow[j - 2] = row[1, j - 1].ToString();
                        if (j == bonameIndex)
                        {
                            bonameL.Add(row[1, j - 1].ToString());
                        }
                    }
                }
                dataRow[columnsint - 1] = false;
                data.Rows.Add(dataRow);
            }
            DataView dv = new DataView(data);
            dv.RowFilter = filter.ToString().Remove(filter.ToString().Length - 3);
            gridControl1.DataSource = dv;
            gridControl1.MainView.PopulateColumns();

            GridBand gbUpdate = view.Bands.AddBand("保留/覆盖");
            gbL.Add(gbUpdate);

            RepositoryItemToggleSwitch edit = new RepositoryItemToggleSwitch();
            view.Columns["保留/覆盖"].ColumnEdit = edit;
            int c = 1;
            foreach (DataColumn col in data.Columns)
            {
                view.Columns[col.Caption].OwnerBand = gbL[c - 1];
                c++;
            }
            view.EndUpdate();
            ws.SaveAs(System.Windows.Forms.Application.StartupPath + "\\TempFile\\" + "临时文件数据.xlsx");
            //结束视图的编辑
            wb.Close();
            wb = null;
            excel.Quit();
            KeyMyExcelProcess.Kill(excel);//调用方法关闭进程
            GC.Collect();
            data.Columns.Add(new DataColumn("匹配完成", typeof(bool)) { ReadOnly = false, DefaultValue = false });
            data.Columns.Add(new DataColumn("layerName"));
            data.Columns.Add(new DataColumn("sourceDB"));
            ErrorFeature = new List<Tuple<int, string, string, bool>>();
            DataValidation();
            //数据验证
            // advBandedGridView1.RowCellStyle += advBandedGridView1_RowCellStyle;
            BindGeometryGrid();
            if (tempFile)
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindGeometryGrid()
        {
            gridControl2.DataSource = null;
            gridView1.Columns.Clear();
            foreach (DataColumn dc in data.Columns)
            {
                if (dc.ColumnName == "对象名称" || dc.ColumnName == "保留/覆盖" || dc.ColumnName == "是否入库" || dc.ColumnName == "是否存在" || dc.ColumnName == "已有坐标" || dc.ColumnName == "匹配完成")
                {
                    gridView1.Columns.Add(new GridColumn() { Name = dc.ColumnName, FieldName = dc.ColumnName, Visible = true });
                }
                else if (columnDecimal.Exists(x => x == dc.ColumnName))
                {
                    gridView1.Columns.Add(new GridColumn() { Name = dc.ColumnName, FieldName = dc.ColumnName, Visible = false, UnboundType = DevExpress.Data.UnboundColumnType.Decimal });
                }
                else
                {
                    gridView1.Columns.Add(new GridColumn() { Name = dc.ColumnName, FieldName = dc.ColumnName, Visible = false });
                }
            }
            gridControl2.DataSource = data;
        }

        /// <summary>
        /// 导入执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (HasError)
            {
                MessageBox.Show("请先修改错误数据：\n     红色部分：\n 1.对象名称不能为空；\n 2.对象名称不能重复；\n 3.数字格式的值不能包含字母、汉字、特殊符号等；\n 4.日期格式的值须为有效日期；\n     橙色部分：\n 导入对象库中已存在，将覆盖已有对象的属性数据", "导入提示");
            }
            else
            {
                var result = MessageBox.Show("导入对象库中已存在(橙色部分)，将覆盖已有对象的属性数据",
                                     " 覆盖选项",
                                      MessageBoxButtons.OKCancel
                                      );
                if (result == DialogResult.OK)
                {
                    //List<FeatureModel> bo = DataRowToBo((DataView)this.advBandedGridView1.DataSource);
                    // bool y = new BOManager().UploadBoPropertyTran(bo);
                    // MessageBox.Show("导入完成！");
                }
            }
        }

        private void advBandedGridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (ErrorFeature != null && ErrorFeature.Count > 0)
            {
                foreach (var item in ErrorFeature)
                {
                    if (e.Column.FieldName == item.Item2 && e.RowHandle == item.Item1)
                    {
                        //根据对象类型和对象名称判断对象是否已经存在
                        AppearanceDefault appBlueRed = new AppearanceDefault(Color.White, Color.Red, Color.Empty, Color.Blue, LinearGradientMode.Horizontal);
                        object val = advBandedGridView1.GetRowCellValue(e.RowHandle, e.Column);
                        string bot = comboObjType.Text;
                        if (new BOServer().ExistBO(val.ToString(), bot))
                        {
                            AppearanceHelper.Apply(e.Appearance, appBlueRed);//appError
                            HasError = true;
                        }
                        List<string> matchedItems = bonameL.FindAll(x =>
                             x == val.ToString());
                        if ((columnDate.Exists(x => x == e.Column.FieldName) && !IsDate(val.ToString()) && val.ToString() != "")            //参数值不为空时要符合格式要求
                            || (columnDecimal.Exists(x => x == e.Column.FieldName) && !IsNumeric(val.ToString()) && val.ToString() != "")   //参数值不为空时要符合格式要求
                            || e.Column.FieldName == "对象名称" && val.ToString() == ""                                                     //对象名称不能为空
                            || e.Column.FieldName == "对象名称" && matchedItems.Count > 1
                            )//对象名称不允许重复
                        {
                            AppearanceHelper.Apply(e.Appearance, appError);
                            HasError = true;
                        }
                        else if (e.Column.FieldName == "对象名称" && new BOServer().GetBoListByName(val.ToString(), bot) != null)
                        {
                            //判断数据库中是否已存在
                            AppearanceHelper.Apply(e.Appearance, appWarn);
                        }
                    }
                }
            }
        }

        #region 类型转换
        /// <summary>
        /// Excel中列的转换
        /// 1 -> A<br/>
        /// 2 -> B<br/>
        /// 3 -> C<br/>
        /// ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        /// <summary>
        /// DataView数据转化成Feature
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        //private List<GeoFeature> DataRowToBo(DataView dv)
        //{
        //    List<GeoFeature> boML = new List<GeoFeature>();
        //    List<XmlDocument> propertySL = new List<XmlDocument>();
        //    propertySL = xmlPropertySetL.ToList();

        //    foreach (DataRow dr in dv.Table.Rows)
        //    {
        //        GeoFeature boEx = new GeoFeature();
        //        boEx.BOT = bot;
        //        boEx.BOTID = Botid;
        //        for (int i = 0; i < dv.Table.Columns.Count; i++)
        //        {
        //            foreach (string col in columnstr.Split(','))
        //            {
        //                if (col == "对象名称") boEx.NAME = dr[col].ToString();
        //            }
        //            boEx.BOID = Guid.NewGuid().ToString();
        //            List<PropertyModel> propertyM = new List<PropertyModel>();
        //            boEx.PropertyList = propertyM;
        //            for (int j = 0; j < propertySL.Count(); j++)
        //            {
        //                for (int k = 0; k < propertySL[j].GetElementsByTagName("P").Count; k++)
        //                {
        //                    propertySL[j].GetElementsByTagName("P")[k].InnerText = dr[propertySL[j].GetElementsByTagName("P")[k].Attributes["n"].Value].ToString();
        //                }
        //                PropertyModel boProperty = new PropertyModel();
        //                boProperty.BOID = boEx.BOID;
        //                boProperty.MD = propertySL[j].InnerXml.ToString();
        //                boProperty.NS = propertySL[j].GetElementsByTagName("PropertySet")[0].Attributes["name"].Value;
        //                propertyM.Add(boProperty);
        //            }
        //        }
        //        boML.Add(boEx);
        //    }
        //    return boML;
        //}
        #endregion

        #region 数据格式验证
        //红色为错误数据
        AppearanceDefault appError = new AppearanceDefault(Color.White, Color.LightCoral, Color.Empty, Color.Red, LinearGradientMode.ForwardDiagonal);
        //橙色为警告数据，与已有数据重复
        AppearanceDefault appWarn = new AppearanceDefault(Color.White, Color.LightCoral, Color.Empty, Color.Orange, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);

        //  public object ObjTyptProperty { get; private set; }

        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^(-?\d+)(\.\d+)?$");
        }
        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDate(string value)
        {
            return Regex.IsMatch(value, @"((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))");
        }
        #endregion

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 是否创建新版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combVersion.Text.ToString() == "--创建新版本--")
            {
                textEdit1.Enabled = true;
                dateEdit1.Enabled = true;
            }
            else
            {
                DataGatherModel model = combVersion.SelectedItem as DataGatherModel;
                textEdit1.Text = model.ENVENT;
                dateEdit1.Text = model.ENVENTDATA.ToString();
                textEdit1.Enabled = false;
                dateEdit1.Enabled = false;
            }
        }

        /// <summary>
        /// 采集方式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup3.SelectedIndex == 1)
            {
                comboBox5.Enabled = true;
                comboBox6.Enabled = true;
                simpleButton5.Enabled = true;
            }
            else
            {
                comboBox5.Enabled = false;
                comboBox6.Enabled = false;
                simpleButton5.Enabled = true;
            }
        }

        private void advBandedGridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            // e.Cancel = advBandedGridView1.FocusedColumn.FieldName != "保留/覆盖";
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkEdit4.Checked)
            //{
            //    advBandedGridView1.RowCellStyle += new RowCellStyleEventHandler(advBandedGridView1_RowCellStyle);
            //    advBandedGridView1.RefreshData();
            //}
            //else
            //{
            //    advBandedGridView1.RowCellStyle -= new RowCellStyleEventHandler(advBandedGridView1_RowCellStyle);
            //}
        }

        /// <summary>
        /// 绑定独享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {

        }

        /// <summary>
        /// 保留还是覆盖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description == "保留")
            {
                for (int i = 0; i < gridView1.DataRowCount; ++i)
                {
                    DataRow dataRow = gridView1.GetDataRow(i);
                    dataRow["保留/覆盖"] = false;
                }
            }
            else
            {
                for (int i = 0; i < gridView1.DataRowCount; ++i)
                {
                    DataRow dataRow = gridView1.GetDataRow(i);
                    dataRow["保留/覆盖"] = true;
                }

            }
        }

        /// <summary>
        /// 打开图件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEdit3_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "GDB Files|*.gdbx";
            openFileDialog1.Title = "Select a GDB File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mapSource = openFileDialog1.FileName.Split('\\')[openFileDialog1.FileName.Split('\\').Length - 1];
                buttonEdit3.Text = openFileDialog1.FileName;
                axJoWeb.PM_LoadMap(openFileDialog1.FileName, 0);
                RefreshLayerList();
                InitRightMenu();

                gridView2.Columns.Clear();
                //data = new System.Data.DataTable();
                LayerData = new System.Data.DataTable();
                gridControl3.DataSource = null;
                BingLayerOption();
                //绑定图层列表
            }
        }

        /// <summary>
        /// 图层列表
        /// </summary>
        private void BingLayerOption()
        {
            LayerData.Columns.Clear();
            GridView view = gridView2 as GridView;
            DataColumn colLayerNmae = new DataColumn("图层名称");
            LayerData.Columns.Add(colLayerNmae);
            DataColumn colLayerView = new DataColumn("显示/隐藏", typeof(bool));
            LayerData.Columns.Add(colLayerView);
            DataColumn colLayerReadOnly = new DataColumn("只读/非只读", typeof(bool));
            LayerData.Columns.Add(colLayerReadOnly);
            List<string> layerList = EnumLayer();
            //获取对象记录
            for (int i = 0; i < layerList.Count; ++i)
            {
                DataRow dataRow = LayerData.NewRow();
                dataRow["图层名称"] = layerList[i];
                string layerStatus = axJoWeb.PM_GetLayerStatus(layerList[i]);
                dataRow["显示/隐藏"] = layerStatus.Split(' ')[0] == "1" ? false : true;
                dataRow["只读/非只读"] = layerStatus.Split(' ')[1] == "1" ? false : true;
                LayerData.Rows.Add(dataRow);
            }
            DataView dv = new DataView(LayerData);
            gridControl3.DataSource = dv;
            RepositoryItemToggleSwitch reView = new RepositoryItemToggleSwitch();
            view.Columns["显示/隐藏"].ColumnEdit = reView;
            RepositoryItemToggleSwitch reReadOnly = new RepositoryItemToggleSwitch();
            view.Columns["只读/非只读"].ColumnEdit = reReadOnly;
        }

        /// <summary>
        /// 刷新图层列表
        /// </summary>
        private void RefreshLayerList()
        {
            this.comboChoseLayers.Properties.Items.Clear();
            //enumerate layer list
            List<string> list = EnumLayer();
            for (int i = 0; i < list.Count; i++)
            {
                this.comboChoseLayers.Properties.Items.Add(list[i]);
                // 获取当前图层
                if (axJoWeb.PM_GetCurrentLayer() == list[i])
                {
                    comboChoseLayers.SelectedIndex = i;
                }
            }
            this.comboBox6.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                this.comboBox6.Items.Add(list[i]);
                // 获取当前图层
                if (axJoWeb.PM_GetCurrentLayer() == list[i])
                {
                    comboBox6.SelectedIndex = i;
                }
            }
        }
        /// <summary>
        /// 遍历图层
        /// </summary>
        /// <returns></returns>
        private List<string> EnumLayer()
        {
            List<string> szLayers = new List<string>();
            for (int i = 0; i < axJoWeb.PM_GetLayerCount(); i++)
            {
                szLayers.Add(axJoWeb.PM_GetLayerName(i));
            }
            return szLayers;
        }
        /// <summary>
        ///匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboChoseLayers.Text))
            {
                string layerName = comboChoseLayers.Text;
                string layerName1 = comboBox6.Text;
                //自动匹配
                if (radioGroup3.SelectedIndex == 0)
                {
                    for (int nIndex = 0; nIndex < axJoWeb.PM_GetElementCount
                        (layerName); nIndex++)
                    {
                        string szID = axJoWeb.PM_GetElementID(layerName, nIndex);
                        string szTitle = axJoWeb.PM_GetElementCaption(szID); // 图元名称
                        DataRow row = data.Rows.Find(szTitle);
                        if (row != null)
                        {
                            row["匹配完成"] = true;
                            row["layerName"] = szID;
                            row["sourceDB"] = mapSource + szID;
                        }
                    }
                    FindLayerByBoName();
                }
                //智能匹配
                if (radioGroup3.SelectedIndex == 1)
                {
                    if (string.IsNullOrEmpty(comboBox5.Text))
                    {
                        FindLayerByBoName();
                    }
                    //根据采集方案 智能匹配
                    //目标图层   layerName
                    //参考图层   layerName1
                    //根据圈闭的名称查找与图上井名相似的井
                    //循环参考图层上的图元
                    //根据文字图元获取目标对象
                    //1.根据目标的名称 拆分目标名称  根据单个字检索目标
                    //List<char> strList = data.Rows[i]["对象名称"].ToString().ToList();
                    switch (comboBox5.Text)
                    {
                        case "根据井找圈闭":
                            int ElementCount = axJoWeb.PM_GetElementCount(layerName1);//参考图层上的图元个数
                            for (int nIndex = 0; nIndex < ElementCount; nIndex++)
                            {
                                string szID = axJoWeb.PM_GetElementID(layerName1, nIndex);
                                string szTitle = axJoWeb.PM_GetElementCaption(szID); //图元名称
                                //循环数据行
                                for (int i = 0; i < data.Rows.Count; i++)
                                {
                                    //根据参考图层上的图元对比查找对象
                                    if (szTitle.Contains(data.Rows[i]["对象名称"].ToString()))
                                    {
                                        //循环目标图层  目标图层上的图元个数：axJoWeb.PM_GetElementCount(layerName)
                                        int count = axJoWeb.PM_GetElementCount(layerName);
                                        for (int Index = 0; Index < count; Index++)
                                        {
                                            string szID1 = axJoWeb.PM_GetElementID(layerName, Index);
                                            if (axJoWeb.PM_PtInPolygon(szID, szID1) == 1)
                                            {
                                                data.Rows[i]["匹配完成"] = true;
                                                data.Rows[i]["layerName"] = szID1;
                                                data.Rows[i]["sourceDB"] = mapSource + szID1;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                //人工匹配
                if (radioGroup3.SelectedIndex == 2)
                {
                    //1.选择图元再选择对象
                    //2.选择对象再选择图元
                }
                gridControl2.DataSource = null;
                gridControl2.DataSource = data;
                MessageBox.Show("匹配完成！");
            }
            else
            {
                MessageBox.Show("请选择目标图层！");
            }
        }

        /// <summary>
        ///根據對象名稱獲取對應的圖元
        /// </summary>
        private void FindLayerByBoName()
        {
            for (var j = 0; j < axJoWeb.PM_GetLayerCount(); j++) // 当前图件中图层的数目
            {
                axJoWeb.RecalcRecNo(axJoWeb.PM_GetLayerName(j));
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(data.Rows[i]["layerName"].ToString()))
                {
                    FindLayersByBoName(data.Rows[i]);
                }
            }
        }
        //目标图层图元列表
        List<string> layers = new List<string>();
        private void FindLayersByBoName(DataRow row)
        {
            bool isInPolagon = false;
            GetLayerCount(comboChoseLayers.Text);
            List<string> lzids;
            for (int i = 0; i < row["对象名称"].ToString().Length; i++)
            {
                lzids = new List<string>();
                for (var j = 0; j < axJoWeb.PM_GetLayerCount(); j++) // 当前图件中图层的数目
                {
                    //  axJoWeb.RecalcRecNo(axJoWeb.PM_GetLayerName(j));
                    //查找结果列表
                    if (!string.IsNullOrEmpty(axJoWeb.FindElementByName(row["对象名称"].ToString()[i].ToString(), -1, axJoWeb.PM_GetLayerName(j)) == null ? null : axJoWeb.FindElementByName(row["对象名称"].ToString()[i].ToString(), -1, axJoWeb.PM_GetLayerName(j))))
                    {
                        lzids.AddRange(axJoWeb.FindElementByName(row["对象名称"].ToString()[i].ToString(), -1, axJoWeb.PM_GetLayerName(j)).Split(';').ToList());
                    }
                }
                lzids = lzids.FindAll(p => p != "" && p != null);

                //axJoWeb.PM_SelectElement()
                //在目标图层上查找包含关键字的信息

                for (int m = 0; m < layers.Count;)
                {
                    for (int k = 0; k < lzids.Count; k++)
                    {
                        if (axJoWeb.PM_PtInPolygon(lzids[k], layers[m]) == 1 || lzids[k] == layers[m])
                        {
                            isInPolagon = true;
                            break;
                        }
                        else
                        {
                            isInPolagon = false;
                        }
                    }
                    if (isInPolagon == false)
                    {
                        layers.Remove(layers[m]);
                    }
                    else
                    {
                        m++;
                    }
                }
            }
            if (string.IsNullOrEmpty(row["layerName"].ToString()))
            {
                if (layers.Count > 0)
                {
                    row["匹配完成"] = true;
                    row["layerName"] = layers[0];
                    row["sourceDB"] = mapSource + layers[0];
                }
            }

        }
        /// <summary>
        /// 获取图层总数量
        /// </summary>
        /// <param name="layername"></param>
        private void GetLayerCount(string layername)
        {
            layers.Clear();
            int count = axJoWeb.PM_GetElementCount(layername);
            for (int Index = 0; Index < count; Index++)
            {
                string szID1 = axJoWeb.PM_GetElementID(comboChoseLayers.Text, Index);
                layers.Add(szID1);
            }
        }

        ///// <summary>
        ///// 获取多边形的外包矩形
        ///// </summary>
        ///// <param name="ptList"></param>
        ///// <returns></returns>
        //private RectangleF GetPolygonEnvelopRect(List<System.Drawing.PointF> ptList)
        //{
        //    RectangleF rect = new RectangleF();
        //    float minx = ptList[0].X;
        //    float maxx = ptList[0].X;
        //    float miny = ptList[0].Y;
        //    float maxy = ptList[0].Y;

        //    for (int i = 1; i < ptList.Count; i++)
        //    {

        //        if (ptList[i].X < minx)

        //            minx = ptList[i].X;

        //        if (ptList[i].X > maxx)

        //            maxx = ptList[i].X;

        //        if (ptList[i].Y < miny)

        //            miny = ptList[i].Y;

        //        if (ptList[i].Y > maxy)

        //            maxy = ptList[i].Y;
        //    }
        //    return rect = new System.Drawing.RectangleF(minx, miny, maxx, maxy);
        //}

        //private int pnpoly(int nvert, List<float> vertx, List<float> verty, float testx, float testy)
        //{
        //    int i, j, c = 0;
        //    for (i = 0, j = nvert - 1; i < nvert; j = i++)
        //    {
        //        if (((verty[i] > testy) != (verty[j] > testy)) &&
        //(testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
        //            c = 1;
        //    }
        //    return c;
        //}

        //private bool PtInPolygon(System.Drawing.PointF p, List<System.Drawing.PointF> ptPolygon, int nCount)
        //{
        //    int nCross = 0;
        //    for (int i = 0; i < nCount; i++)
        //    {
        //        PointF p1 = ptPolygon[i];
        //        PointF p2 = ptPolygon[(i + 1) % nCount];
        //        // 求解 y=p.y 与 p1p2 的交点 
        //        if (p1.Y == p2.Y) // p1p2 与 y=p0.y平行 
        //            continue;
        //        if (p.Y < Math.Min(p1.Y, p2.Y)) // 交点在p1p2延长线上 
        //            continue;
        //        if (p.Y >= Math.Max(p1.Y, p2.Y)) // 交点在p1p2延长线上 
        //            continue;
        //        // 求交点的 X 坐标 -------------------------------------------------------------- 
        //        double x = (double)(p.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;
        //        if (x > p.X)
        //            nCross++; // 只统计单边交点 
        //    }
        //    // 单边交点为偶数，点在多边形之外 --- 
        //    return (nCross % 2 == 1);
        //}

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //e.CellValue
            AppearanceDefault appBlueRed =
           new AppearanceDefault
           (Color.White, Color.Red, Color.Empty, Color.Blue,
           LinearGradientMode.Horizontal);
            string name = e.CellValue.ToString();
            string bot = comboObjType.Text;
            if (new BOServer().ExistBO(name, bot))
            {
                AppearanceHelper.Apply(e.Appearance, appError);
                HasError = true;
            }
        }

        /// <summary>
        /// 选择数据行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            dr = gridView1.GetDataRow(e.RowHandle);
            currRow = e.RowHandle;
            if (Convert.ToBoolean(dr["匹配完成"]) == false)
            {
                return;
            }
            else
            {
                axJoWeb.PM_SelectElement(gridView1.GetDataRow(e.RowHandle)["layerName"].ToString(), 0);
            }
        }

        /// <summary>
        /// 另存为文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_2(object sender, EventArgs e)
        {
            //属性空间坐标 存储
            AllFeature = DataRowToFeature(gridView1);
            XmlDocument doc = ConvertFT.FeatureToGGGX(AllFeature);
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.AddExtension = true;
            saveFileDlg.Title = "另存为";
            saveFileDlg.OverwritePrompt = true;
            saveFileDlg.FileName = "";
            saveFileDlg.DefaultExt = "xml";
            saveFileDlg.Filter = "3GX files (*.xml)|*.xml";
            if (DialogResult.OK == saveFileDlg.ShowDialog())
            {
                using (StreamWriter outfile = new StreamWriter(saveFileDlg.FileName))
                {
                    outfile.Write(doc.InnerXml);
                }
            }
        }

        /// <summary>
        ///数据转换
        /// </summary>
        /// <param name="gridView1"></param>
        /// <returns></returns>
        private List<GeoFeature> DataRowToFeature(GridView gridView1)
        {
            List<GeoFeature> FTlist = new List<GeoFeature>();
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                List<XmlDocument> propertySL = new List<XmlDocument>();
                xmlPropertySetL.ForEach(e =>
                {
                    XmlDocument mydoc = new XmlDocument();
                    mydoc.LoadXml(e.OuterXml);
                    propertySL.Add(mydoc);
                });
                GeoFeature ft = new GeoFeature();
                ft.BOT = bot;
                ft.FT = (comboObjType.SelectedItem as ObjectTypeModel).FT;
                DataRow dr = gridView1.GetDataRow(i);
                ft.UNCHANGOROVERRIDE = Convert.ToBoolean(dr["保留/覆盖"]);

                foreach (string col in columnstr.Split(','))
                {
                    if (col == "对象名称")
                        ft.NAME = dr[col].ToString().Trim();
                }
                ft.BOID = Guid.NewGuid().ToString();
                List<Property> propertyM = new List<Property>();
                ft.PropertyList = propertyM;
                for (int x = 0; x < propertySL.Count(); x++)
                {
                    for (int k = 0; k < propertySL[x].GetElementsByTagName("P").Count;)
                    {
                        if (dr[propertySL[x].GetElementsByTagName("P")[k].Attributes["n"].Value].ToString().Trim() == "")
                        {
                            propertySL[x].GetElementsByTagName("P")[k].ParentNode.RemoveChild(propertySL[x].GetElementsByTagName("P")[k]);
                        }
                        else
                        {
                            propertySL[x].GetElementsByTagName("P")[k].InnerText = dr[propertySL[x].GetElementsByTagName("P")[k].Attributes["n"].Value].ToString().Trim();
                            k++;
                        }
                    }
                    Property boProperty = new Property();
                    boProperty.BOID = ft.BOID;
                    boProperty.MdSource = "";
                    boProperty.MD = propertySL[x].InnerXml.ToString();
                    boProperty.NS = propertySL[x].GetElementsByTagName("PropertySet")[0].Attributes["name"].Value;
                    propertyM.Add(boProperty);
                }

                if ((comboObjType.SelectedItem as ObjectTypeModel).USEGEOMETRY == "1")
                {
                    List<Geometry> GeometryList = new List<Geometry>();
                    string searchText = ft.NAME;
                    ColumnView view = (ColumnView)gridControl1.FocusedView;
                    GridColumn column = view.Columns["对象名称"];
                    string strGeometryWKT;
                    if (column != null)
                    {
                        // 如果用数据源中的列值，请用ColumnView.LocateByValue 
                        int rhFound = view.LocateByDisplayText(0, column, searchText);
                        if (rhFound != GridControl.InvalidRowHandle)
                        {
                            DataRow dr1 = gridView1.GetDataRow(rhFound);
                            string szid = dr1["layerName"].ToString();
                            //根据图层ID生成WKT坐标  
                            int x = axJoWeb.PM_GetPointCount(szid);
                            //点
                            if (axJoWeb.PM_GetElementType(szid) == 11)
                            {

                                for (int j = 0; j < x; j++)
                                {
                                    strGeometryWKT = "POINT(";
                                    Geometry geometry = new Geometry();
                                    string str = axJoWeb.PM_GetPoint(szid, j);
                                    List<string> listGeometry = str.Split(new char[] { '\t', ' ' }).ToList();
                                    string str1 = axJoWeb.PM_UPtoLL(double.Parse(listGeometry[0]), double.Parse(listGeometry[1]));
                                    strGeometryWKT += str1.Split(new char[] { '\t', ' ' })[0] + " " + str1.Split(new char[] { '\t', ' ' })[1];
                                    strGeometryWKT = strGeometryWKT.TrimEnd(',');
                                    strGeometryWKT += ")";
                                    geometry.NAME = j == 0 ? "井口" : "井底";
                                    geometry.GEOMETRY = strGeometryWKT;
                                    geometry.SOURCEDB = dr["sourceDB"].ToString();
                                    GeometryList.Add(geometry);
                                }
                            }
                            //线
                            if (axJoWeb.PM_GetElementType(szid) == 2 || axJoWeb.PM_GetElementType(szid) == 35 || axJoWeb.PM_GetElementType(szid) == 17)
                            {
                                Geometry geometry = new Geometry();
                                strGeometryWKT = "LINESTRING(";
                                for (int j = 0; j < x; j++)
                                {
                                    string str = axJoWeb.PM_GetPoint(szid, j);
                                    List<string> listGeometry = str.Split(new char[] { '\t', ' ' }).ToList();
                                    string str1 = axJoWeb.PM_UPtoLL(double.Parse(listGeometry[0]), double.Parse(listGeometry[1]));
                                    if (j == x - 1)
                                    {
                                        strGeometryWKT += str1.Split(new char[] { '\t', ' ' })[0] + " " + str1.Split(new char[] { '\t', ' ' })[1];
                                    }
                                    else
                                    {
                                        strGeometryWKT += str1.Split(new char[] { '\t', ' ' })[0] + " " + str1.Split(new char[] { '\t', ' ' })[1] + ",";
                                    }
                                }

                                strGeometryWKT = strGeometryWKT.TrimEnd(',');
                                strGeometryWKT += ")";
                                geometry.GEOMETRY = strGeometryWKT;
                                geometry.SOURCEDB = dr["sourceDB"].ToString();
                                GeometryList.Add(geometry);
                            }
                            //面
                            if (axJoWeb.PM_GetElementType(szid) == 3 || axJoWeb.PM_GetElementType(szid) == 4 || axJoWeb.PM_GetElementType(szid) == 36)
                            {
                                Geometry geometry = new Geometry();
                                strGeometryWKT = "POLYGON((";
                                for (int j = 0; j < x; j++)
                                {
                                    string str = axJoWeb.PM_GetPoint(szid, j);
                                    List<string> listGeometry = str.Split(new char[] { '\t', ' ' }).ToList();
                                    string str1 = axJoWeb.PM_UPtoLL(double.Parse(listGeometry[0]), double.Parse(listGeometry[1]));
                                    if (j == x - 1)
                                    {
                                        strGeometryWKT += str1.Split(new char[] { '\t', ' ' })[0] + " " + str1.Split(new char[] { '\t', ' ' })[1];
                                    }
                                    else
                                    {
                                        strGeometryWKT += str1.Split(new char[] { '\t', ' ' })[0] + " " + str1.Split(new char[] { '\t', ' ' })[1] + ",";
                                    }
                                }
                                strGeometryWKT = strGeometryWKT.TrimEnd(',');
                                strGeometryWKT += "))";
                                geometry.GEOMETRY = strGeometryWKT;
                                geometry.SOURCEDB = dr["sourceDB"].ToString();
                                GeometryList.Add(geometry);
                            }
                        }
                    }
                    ft.GeometryList = GeometryList;
                }
                else
                {
                    ft.GeometryList = new List<Geometry>();
                }
                FTlist.Add(ft);
            }
            return FTlist;
        }


        /// <summary>
        /// 读取GGGX文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //读取文件  
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML文件|*.XML";
            openFileDialog1.Title = "选择XML文件";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(openFileDialog1.FileName);
                AllFeature = ConvertFT.ConvertToFTListByXML(xmlDoc.InnerXml);
                //数据绑定
                xmlPropertySetL.Clear();
                bonameL.Clear();
                HasError = false;
                //    data = new System.Data.DataTable();
                LayerData = new System.Data.DataTable();
                comFilePath.Text = openFileDialog1.FileName;
            }
        }
        /// <summary>
        /// 保存入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                MessageBox.Show("请验证数据有效性！");
                return;
            }
            if (!string.IsNullOrEmpty(textEdit1.Text) && !string.IsNullOrEmpty(dateEdit1.Text))
            {
                DataGatherModel datagather = new DataGatherModel();
                if (combVersion.Text == "--创建新版本--")
                {
                    //新增版本信息
                    //增量备份数据
                    datagather.GATHERID = null;
                    datagather.ENVENT = textEdit1.Text;
                    datagather.ENVENTDATA = Convert.ToDateTime(dateEdit1.Text);
                    datagather.BOTID = (comboObjType.SelectedItem as ObjectTypeModel).BOTID;
                    datagather.GATHERER = Dns.GetHostName();
                    datagather.UPLOADDATE = DateTime.Now;
                    datagather.NOTE = "";
                }
                else
                {
                    //修改版本信息
                    datagather.GATHERID = (combVersion.SelectedItem as DataGatherModel).GATHERID;
                    datagather.ENVENT = textEdit1.Text;
                    datagather.ENVENTDATA = Convert.ToDateTime(dateEdit1.Text);
                    datagather.BOTID = (comboObjType.SelectedItem as ObjectTypeModel).BOTID;
                    datagather.GATHERER = Dns.GetHostName();
                    datagather.UPLOADDATE = DateTime.Now;
                    datagather.NOTE = "";
                }
                AllFeature = DataRowToFeature(gridView1);
                //采集选择
                if (checkEdit1.Checked)
                {
                    foreach (var item in AllFeature)
                    {
                        item.IsImportProperty = true;
                    }
                }
                else
                {
                    foreach (var item in AllFeature)
                    {
                        item.IsImportProperty = false;
                    }
                }
                if (checkEdit2.Checked)
                {
                    foreach (var item in AllFeature)
                    {
                        item.IsImportExtendProperty = true;
                    }
                }
                else
                {
                    foreach (var item in AllFeature)
                    {
                        item.IsImportExtendProperty = false;
                    }
                }
                if (checkEdit3.Checked)
                {
                    foreach (var item in AllFeature)
                    {
                        item.IsImportGeometry = true;
                    }
                }
                else
                {
                    foreach (var item in AllFeature)
                    {
                        item.IsImportGeometry = false;
                    }
                }
                if (ErrorFeature.Count > 0 && ErrorFeature.FindAll(x => x.Item4 == true).Count > 0)
                {
                    var ErrorList = ErrorFeature.FindAll(x => x.Item4 == true);
                    string errBoName = string.Empty;
                    //存在错误的属性信息
                    for (int i = 0; i < ErrorList.Count; i++)
                    {
                        if (i == ErrorList.Count - 1)
                        {
                            errBoName += "'" + ErrorList[i].Item3.Trim() + "'";
                        }
                        else
                        {
                            errBoName += "'" + ErrorList[i].Item3.Trim() + "',";
                        }
                    }
                    MessageBox.Show("属性值" + errBoName + "|" + "有误,请修改!");
                    return;
                }
                SaveFeature = new List<GeoFeature>();

                #region  需要带坐标存储
                //不存在错误的属性信息
                if ((comboObjType.SelectedItem as ObjectTypeModel).USEGEOMETRY == "1")
                {
                    TempFeature = new List<GeoFeature>();
                    foreach (GeoFeature ft in AllFeature)
                    {
                        //主参数
                        if (ft.GeometryList == null || ft.GeometryList.Count == 0)
                        {
                            //未保存的临时数据信息
                            TempFeature.Add(ft);
                        }
                        else
                        {
                            //需要保存的数据信息
                            SaveFeature.Add(ft);
                        }
                    }
                    if (SaveFeature.Count == 0)
                    {
                        MessageBox.Show("请完善坐标信息！");
                        return;
                    }
                }
                else
                {
                    SaveFeature = AllFeature;
                }
                #endregion
                SubmissionResult result = new DataGatherServer().Submit(datagather, SaveFeature);
                WriteLogFile(result);
                MessageBox.Show("保存成功!");
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    for (int i = 0; i < result.Errors.Count; i++)
                    {
                        SaveFeature.Remove(SaveFeature.Find(p => p.NAME == result.Errors[i].BOName));
                    }
                }
                if (TempFeature.Count > 0)
                {
                    if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\TempFile\\" + "临时文件数据.xlsx"))
                    {
                        excel = new Microsoft.Office.Interop.Excel.Application();
                        excel.Visible = false; excel.UserControl = true;
                        wb = excel.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\TempFile\\" + "临时文件数据.xlsx");
                        ws = (Worksheet)wb.Worksheets.get_Item(1);
                        Regex RegexWords = new Regex(",");
                        int botIndex = RegexWords.Matches(columnstr.Substring(0, columnstr.IndexOf("对象类型"))).Count + 1;
                        bot = ws.Cells.get_Range(ExcelColumnFromNumber(botIndex) + 4).Value2;//获取对象类型
                        for (int i = 0; i < SaveFeature.Count; i++)
                        {
                            Range range = ws.Rows.Find(SaveFeature[i].NAME, Type.Missing,//要查找的字符串
               XlFindLookIn.xlValues,//查找值，或者xlFormulas查找公式等
               XlLookAt.xlPart, //这里用xlWhole返回的一直是空指针
               XlSearchOrder.xlByRows,//按行查找
               XlSearchDirection.xlNext, //建议就用xlNext
    false, false);
                            if (range != null)
                            {
                                range = (Range)ws.Rows[range.Row, Type.Missing];
                                range.Delete(XlDeleteShiftDirection.xlShiftUp);
                            }
                            else
                            {
                                ws.Cells[4, 1] = comboObjType.Text;
                            }
                        }
                        ws.Cells[4, 1] = comboObjType.Text;
                        ws.SaveAs(System.Windows.Forms.Application.StartupPath + "\\TempFile\\" + textEdit1.Text + "未采集完成的数据.xlsx");
                        wb.Close();
                        wb = null;
                        excel.Quit();
                        KeyMyExcelProcess.Kill(excel);//调用方法关闭进程
                        GC.Collect();
                    }
                }
                File.Delete(System.Windows.Forms.Application.StartupPath + "\\TempFile\\" + "临时文件数据.xlsx");
                if (tempFile == true)
                {
                    File.Delete(comFilePath.Text);
                }
            }
            else
            {
                MessageBox.Show("请完善版本信息！");
            }
        }

        //// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        public void WriteLogFile(SubmissionResult result)
        {
            /**/
            ///指定日志文件的目录
            string fname = System.Windows.Forms.Application.StartupPath + "\\LogFile" + "\\LogFile.txt";
            /**/
            ///定义文件信息对象
            FileInfo finfo = new FileInfo(fname);

            if (!finfo.Exists)
            {
                FileStream fs;
                fs = File.Create(fname);
                fs.Close();
                finfo = new FileInfo(fname);
            }

            /**/
            ///判断文件是否存在以及是否大于2K
            //if (finfo.Length > 1024 * 1024 * 10)
            //{
            //    /**/
            //    ///文件超过10MB则重命名
            //    File.Move(Directory.GetCurrentDirectory() + "\\LogFile.txt", Directory.GetCurrentDirectory() + DateTime.Now.TimeOfDay + "\\LogFile.txt");
            //    /**/
            //    ///删除该文件
            //    //finfo.Delete();
            //}
            //finfo.AppendText();
            /**/
            ///创建只写文件流
            using (FileStream fs = finfo.OpenWrite())
            {
                /**/
                ///根据上面创建的文件流创建写数据流
                StreamWriter w = new StreamWriter(fs);

                /**/
                ///设置写数据流的起始位置为文件流的末尾
                w.BaseStream.Seek(0, SeekOrigin.End);

                /**/
                ///写入“Log Entry : ”
                w.Write("\r\nLog Entry : ");

                /**/
                ///写入当前系统时间并换行
                w.Write("操作时间:{0} {1} \r\n",
                    DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

                /**/
                ///写入日志内容并换行
                w.Write("操作:" + result.TotalBO + "条" + "\r\n");
                w.Write("插入:" + result.InsertedBO + "条" + "\r\n");
                w.Write("修改:" + result.UpdatedBO + "条" + "\r\n");
                w.Write("失败:" + result.FailedBO + "条" + "\r\n");
                w.Write("错误:" + result.Errors == null ? "0" : result.Errors.Count.ToString() + "条" + "\r\n");
                /**/
                foreach (var item in result.Errors)
                {
                    w.Write(item.BOName + "失败原因：" + item.Error + "\r\n");
                }

                ///写入------------------------------------“并换行
                w.Write("------------------------------------\r\n");

                /**/
                ///清空缓冲区内容，并把缓冲区内容写入基础流
                w.Flush();
                /**/
                ///关闭写数据流
                w.Close();
            }
        }
        //关闭打开的Excel方法
        public void CloseExcel(Microsoft.Office.Interop.Excel.Application ExcelApplication, Workbook ExcelWorkbook)
        {
            ExcelWorkbook.Close(false, Type.Missing, Type.Missing);
            ExcelWorkbook = null;
            ExcelApplication.Quit();
            GC.Collect();
            KeyMyExcelProcess.Kill(ExcelApplication);
        }
        /// <summary>
        /// 选择参考图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < axJoWeb.PM_GetLayerCount(); i++)
            {
                string szID1 = axJoWeb.PM_GetElementID(axJoWeb.PM_GetLayerName(i), i);
                string szTitle1 = axJoWeb.PM_GetElementCaption(szID1); // 图元名称
                if (szTitle1 == comboBox6.Text)
                {
                    axJoWeb.PM_SetLayerStatus(szTitle1, 1, 1);
                }
                else
                {
                    axJoWeb.PM_SetLayerStatus(szTitle1, 0, 0);
                }
            }

        }

        private void xtraTabControl1_SelectedPageChanging_1(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            BindGeometryGrid();
        }

        /// <summary>
        /// 数据有效性验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                advBandedGridView1.RowCellStyle += new RowCellStyleEventHandler(advBandedGridView1_RowCellStyle);
                advBandedGridView1.RefreshData();
            }
            else
            {
                advBandedGridView1.RowCellStyle -= new RowCellStyleEventHandler(advBandedGridView1_RowCellStyle);
            }
        }

        /// <summary>
        /// 数据有效性验证
        /// </summary>
        private void DataValidation()
        {
            for (int i = 0; i < advBandedGridView1.DataRowCount; i++)
            {
                for (int j = 0; j < advBandedGridView1.Columns.Count; j++)
                {
                    DataRow dr = advBandedGridView1.GetDataRow(i);
                    if (advBandedGridView1.Columns[j].FieldName == "对象名称" && new BOServer().GetBoListByName(dr["对象名称"].ToString(), comboObjType.Text) != null)
                    {
                        //判断数据库中是否已存在
                        ErrorFeature.Add(new Tuple<int, string, string, bool>(i, advBandedGridView1.Columns[j].FieldName, dr["对象名称"].ToString(), false));
                    }
                    List<string> matchedItems = bonameL.FindAll(x =>
                            x == advBandedGridView1.Columns[j].FieldName);
                    if ((columnDate.Exists(x => x == advBandedGridView1.Columns[j].FieldName) && !IsDate(dr[j].ToString()) && dr[j].ToString() != "")            //参数值不为空时要符合格式要求
                        || (columnDecimal.Exists(x => x == advBandedGridView1.Columns[j].FieldName) && !IsNumeric(dr[j].ToString()) && dr[j].ToString() != "")   //参数值不为空时要符合格式要求
                        || advBandedGridView1.Columns[j].FieldName == "对象名称" && dr[j].ToString() == ""                                                     //对象名称不能为空
                        || advBandedGridView1.Columns[j].FieldName == "对象名称" && matchedItems.Count > 1
                        )//对象名称不允许重复
                    {
                        if (ErrorFeature.Count > 0)
                        {
                            if (ErrorFeature.FindAll(x => x.Item1 == i && x.Item2 == advBandedGridView1.Columns[j].FieldName && x.Item3 == dr[j].ToString()).Count == 0)
                            {
                                ErrorFeature.Add(new Tuple<int, string, string, bool>(i, advBandedGridView1.Columns[j].FieldName, dr[j].ToString(), true));
                            }
                        }
                        else
                        {
                            ErrorFeature.Add(new Tuple<int, string, string, bool>(i, advBandedGridView1.Columns[j].FieldName, dr[j].ToString(), true));
                        }
                    }
                }
            }
        }

        private void advBandedGridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (ErrorFeature != null && ErrorFeature.Count > 0)
            {
                List<string> matchedItems = bonameL.FindAll(x =>
          x == e.Column.FieldName);
                if (ErrorFeature.FindAll(x => x.Item1 == e.RowHandle && x.Item2 == e.Column.FieldName).Count == 0)
                {
                    if ((columnDate.Exists(x => x == e.Column.FieldName) && !IsDate(e.Value.ToString()) && e.Value.ToString() != "")            //参数值不为空时要符合格式要求
     || (columnDecimal.Exists(x => x == e.Column.FieldName) && !IsNumeric(e.Value.ToString()) && e.Value.ToString() != "")   //参数值不为空时要符合格式要求
     || e.Column.FieldName == "对象名称" && e.Value.ToString() == ""                                                     //对象名称不能为空
     || e.Column.FieldName == "对象名称" && matchedItems.Count > 1
     )//对象名称不允许重复
                    {
                        if (ErrorFeature != null && ErrorFeature.Count > 0)
                        {
                            if (ErrorFeature.FindAll(x => x.Item1 == e.RowHandle && x.Item2 == e.Column.FieldName && x.Item3 == e.Value.ToString()).Count == 0)
                            {
                                ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, e.Value.ToString(), true));
                            }
                        }
                        else
                        {
                            ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, e.Value.ToString(), true));
                        }
                    }
                }
                else
                {
                    var tuple = ErrorFeature.Find(p => p.Item1 == e.RowHandle && p.Item2 == e.Column.FieldName);
                    //没有在错误列表中找到
                    if ((columnDate.Exists(x => x == e.Column.FieldName) && !IsDate(e.Value.ToString()) && e.Value.ToString() != "")            //参数值不为空时要符合格式要求
          || (columnDecimal.Exists(x => x == e.Column.FieldName) && !IsNumeric(e.Value.ToString()) && e.Value.ToString() != "")   //参数值不为空时要符合格式要求
          || e.Column.FieldName == "对象名称" && e.Value.ToString() == ""                                                     //对象名称不能为空
          || e.Column.FieldName == "对象名称" && matchedItems.Count > 1
          )//对象名称不允许重复
                    {
                        ErrorFeature.Remove(tuple);
                        ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, e.Value.ToString(), true));
                    }
                    else
                    {
                        ErrorFeature.Remove(tuple);
                    }
                }
            }
            else
            {
                List<string> matchedItems = bonameL.FindAll(x =>
               x == e.Column.FieldName);
                //没有在错误列表中找到
                if ((columnDate.Exists(x => x == e.Column.FieldName) && !IsDate(e.Value.ToString()) && e.Value.ToString() != "")            //参数值不为空时要符合格式要求
      || (columnDecimal.Exists(x => x == e.Column.FieldName) && !IsNumeric(e.Value.ToString()) && e.Value.ToString() != "")   //参数值不为空时要符合格式要求
      || e.Column.FieldName == "对象名称" && e.Value.ToString() == ""                                                     //对象名称不能为空
      || e.Column.FieldName == "对象名称" && matchedItems.Count > 1
      )//对象名称不允许重复
                {
                    if (ErrorFeature != null && ErrorFeature.Count > 0)
                    {
                        if (ErrorFeature.FindAll(x => x.Item1 == e.RowHandle && x.Item2 == e.Column.FieldName && x.Item3 == e.Value.ToString()).Count == 0)
                        {
                            ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, e.Value.ToString(), true));
                        }
                    }
                    else
                    {
                        ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, e.Value.ToString(), true));
                    }
                }
            }
        }

        /// <summary>
        /// 重新绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advBandedGridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            //根据对象类型和对象名称判断对象是否已经存在
            AppearanceDefault appBlueRed = new AppearanceDefault(Color.White, Color.Red, Color.Empty, Color.Blue, LinearGradientMode.Horizontal);
            object val = advBandedGridView1.GetRowCellValue(e.RowHandle, e.Column);
            string bot = comboObjType.Text;
            if (new BOServer().ExistBO(val.ToString(), bot))
            {
                AppearanceHelper.Apply(e.Appearance, appBlueRed);//appError
                HasError = true;
            }
            List<string> matchedItems = bonameL.FindAll(x =>
                 x == val.ToString());
            if ((columnDate.Exists(x => x == e.Column.FieldName) && !IsDate(val.ToString()) && val.ToString() != "")            //参数值不为空时要符合格式要求
                || (columnDecimal.Exists(x => x == e.Column.FieldName) && !IsNumeric(val.ToString()) && val.ToString() != "")   //参数值不为空时要符合格式要求
                || e.Column.FieldName == "对象名称" && val.ToString() == ""                                                     //对象名称不能为空
                || e.Column.FieldName == "对象名称" && matchedItems.Count > 1
                )//对象名称不允许重复
            {
                //e.RowHandle
                //e.Column.FieldName
                if (ErrorFeature != null && ErrorFeature.Count > 0)
                {
                    if (ErrorFeature.FindAll(x => x.Item1 == e.RowHandle && x.Item2 == e.Column.FieldName && x.Item3 == val.ToString()).Count == 0)
                    {
                        ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, val.ToString(), true));
                    }
                }
                else
                {
                    ErrorFeature.Add(new Tuple<int, string, string, bool>(e.RowHandle, e.Column.FieldName, val.ToString(), true));
                }
                AppearanceHelper.Apply(e.Appearance, appError);
                HasError = true;
            }
            else if (e.Column.FieldName == "对象名称" && new BOServer().GetBoListByName(val.ToString(), bot) != null)
            {
                //判断数据库中是否已存在
                AppearanceHelper.Apply(e.Appearance, appWarn);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> layerList = EnumLayer();
            if (radioGroup2.Properties.Items[radioGroup2.SelectedIndex].Description == "显示")
            {
                foreach (var item in layerList)
                {
                    string layerStatus = axJoWeb.PM_GetLayerStatus(item);
                    axJoWeb.PM_SetLayerStatus(item, 1, Convert.ToInt32(layerStatus.Split(' ')[1]));
                }
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    DataRow dr = gridView2.GetDataRow(i);
                    dr["显示/隐藏"] = false;
                }
            }
            else
            {
                foreach (var item in layerList)
                {
                    string layerStatus = axJoWeb.PM_GetLayerStatus(item);
                    axJoWeb.PM_SetLayerStatus(item, 0, Convert.ToInt32(layerStatus.Split(' ')[1]));
                }
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    DataRow dr = gridView2.GetDataRow(i);
                    dr["显示/隐藏"] = true;
                }
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup4_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> layerList = EnumLayer();
            if (radioGroup4.Properties.Items[radioGroup4.SelectedIndex].Description == "只读")
            {
                foreach (var item in layerList)
                {
                    string layerStatus = axJoWeb.PM_GetLayerStatus(item);
                    axJoWeb.PM_SetLayerStatus(item, Convert.ToInt32(layerStatus.Split(' ')[0]), 0);
                }
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    DataRow dr = gridView2.GetDataRow(i);
                    dr["只读/非只读"] = false;
                }
            }
            else
            {
                foreach (var item in layerList)
                {
                    string layerStatus = axJoWeb.PM_GetLayerStatus(item);
                    axJoWeb.PM_SetLayerStatus(item, Convert.ToInt32(layerStatus.Split(' ')[0]), 1);
                }
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    DataRow dr = gridView2.GetDataRow(i);
                    dr["只读/非只读"] = true;
                }
            }
        }

        /// <summary>
        /// 操作显示隐藏或只读非只读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(e.RowHandle);
            if (e.Column.FieldName == "显示/隐藏")
            {
                if (!Convert.ToBoolean(e.Value))
                {
                    dr["显示/隐藏"] = false;
                    string layerStatus = axJoWeb.PM_GetLayerStatus(dr["图层名称"].ToString());
                    axJoWeb.PM_SetLayerStatus(dr["图层名称"].ToString(), 1, Convert.ToInt32(layerStatus.Split(' ')[1]));
                }
                else
                {
                    dr["显示/隐藏"] = true;
                    string layerStatus = axJoWeb.PM_GetLayerStatus(dr["图层名称"].ToString());
                    axJoWeb.PM_SetLayerStatus(dr["图层名称"].ToString(), 0, Convert.ToInt32(layerStatus.Split(' ')[1]));
                }
            }
            if (e.Column.FieldName == "只读/非只读")
            {
                if (!Convert.ToBoolean(e.Value) == true)
                {
                    dr["只读/非只读"] = false;
                    string layerStatus = axJoWeb.PM_GetLayerStatus(dr["图层名称"].ToString());
                    axJoWeb.PM_SetLayerStatus(dr["图层名称"].ToString(), Convert.ToInt32(layerStatus.Split(' ')[0]), 0);
                }
                else
                {
                    dr["只读/非只读"] = true;
                    string layerStatus = axJoWeb.PM_GetLayerStatus(dr["图层名称"].ToString());
                    axJoWeb.PM_SetLayerStatus(dr["图层名称"].ToString(), Convert.ToInt32(layerStatus.Split(' ')[0]), 1);
                }
            }
        }

        /// <summary>
        /// 选择目标图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboChoseLayers_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < axJoWeb.PM_GetLayerCount(); i++)
            {
                string szID1 = axJoWeb.PM_GetElementID(axJoWeb.PM_GetLayerName(i), i);
                string szTitle1 = axJoWeb.PM_GetElementCaption(szID1); // 图元名称
                if (szTitle1 == comboChoseLayers.Text)
                {
                    axJoWeb.PM_SetLayerStatus(szTitle1, 1, 1);
                }
                else
                {
                    axJoWeb.PM_SetLayerStatus(szTitle1, 0, 0);
                }
            }
        }
        /// <summary>
        /// 数据源设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDataBaseSetting_Click(object sender, EventArgs e)
        {
            DataBaseSetting dbSetting = new DataBaseSetting();
            dbSetting.ShowDialog();
        }

        /// <summary>
        /// 查看操作日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLookLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", System.Windows.Forms.Application.StartupPath + "\\LogFile" + "\\LogFile.txt");
        }

        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            // gridView1.FocusedRowHandle=e.

        }
        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (gridView1.FocusedColumn.FieldName == "匹配完成" && dr["layerName"] != null)//控制行、列
            {
                dr["匹配完成"] = false;
                dr["layerName"] = null;
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
