using Jurassic.AppCenter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using PKS.Models;
using PKS.WebAPI;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PKS.Web;

namespace PKS.PortalMgmt.Controllers
{
    public class GeoFeatureController : PKSBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult BOT()
        {
            return View();
        }





        #region 初始页面

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult BO()
        {
            //获取验证token
            ViewData["ACCESS_TOKEN"] = base.Token;
            //获取服务连接地址
            ViewData["API_SERVICE_URL"] = this.HttpContext.GetWebApiServiceUrl();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult BOForm()
        {
            ViewData["ACCESS_TOKEN"] = base.Token;
            ViewData["API_SERVICE_URL"] = this.HttpContext.GetWebApiServiceUrl();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult BOTForm()
        {
            ViewData["ACCESS_TOKEN"] = base.Token;
            ViewData["API_SERVICE_URL"] = this.HttpContext.GetWebApiServiceUrl();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult BOImport()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult DownTemplate(string col, string colP)
        {
            //获取模板列数据
            var colData = Request.Form["colData"];
            //获取动态属性列
            var colProperties = Request.Form["colProperties"];
            //业务类型
            string botName = Request.Form["botName"];

            string tmp = DateTime.Now.ToFileTime().ToString();

            tmp = botName + "_" + "模板_" + tmp.Substring(tmp.Length - 6) + ".xls";

            //
            List<BOTPropertyDefinition> propertiesList = JsonHelper.FormJson(colProperties, typeof(List<BOTPropertyDefinition>)) as List<BOTPropertyDefinition>;
            DataTable colDt = JsonHelper.FormJson(colData, typeof(DataTable)) as DataTable;
            //获取模板文件
            byte[] data = DownExcelTemplate(colDt, propertiesList, botName);

            return File(data, "application/ms-excel", tmp);
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="SourceTable">数据源</param>
        /// <param name="propertiesList">属性集合</param>
        /// <param name="botName">bot名称</param>
        /// <returns>返回文件二进制</returns>
        public byte[] DownExcelTemplate(DataTable SourceTable, List<BOTPropertyDefinition> propertiesList, string botName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("sheet1");

            #region 标题头
            int i = -1;
            int rowIndex = 0;
            //创建第1行
            IRow headerRow = sheet.CreateRow(rowIndex);
            //根据读取数据库字段写标题名字
            foreach (DataRow drC in SourceTable.Rows)
            {
                i++;

                string field = drC["field"] != null ? drC["field"].ToString() : "";

                if (string.IsNullOrEmpty(field))
                    continue;

                ICell cell = headerRow.CreateCell(i);
                //
                cell.SetCellValue(field);

                //输入列设置宽度
                int w = Convert.ToInt32(150 * 26.54);
                sheet.SetColumnWidth(i, w);

                if (field.ToUpper() == "BOT")
                {
                    //创建下拉框序列
                    CellRangeAddressList regions = new CellRangeAddressList(0, 65535, i, i);
                    DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(new string[] { botName });
                    HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                    sheet.AddValidationData(dataValidate);
                    continue;
                }

                var tmpList = propertiesList.Where(p => p.Name.ToUpper() == field.ToUpper()).ToList();

                if (tmpList.Any() && tmpList[0].Options != null && tmpList[0].Options.Count > 0)
                {
                    //如果是日期类型不做下拉框
                    if (tmpList[0].Type == MetadataTagType.ISODate)
                        continue;
                    //获取对应属性的配置信息,默认仅查询到一个对象
                    var optionsStr = tmpList[0].Options.ToArray();
                    //判断所配置的选项数组是否为空,如果为空就不添加选项卡
                    if (optionsStr.Length == 0 || string.IsNullOrEmpty(optionsStr[0]))
                        continue;
                    //创建下拉框序列
                    CellRangeAddressList regions = new CellRangeAddressList(0, 65535, i, i);
                    DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(optionsStr);
                    HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                    sheet.AddValidationData(dataValidate);
                }
            }
            #endregion

            //写入内存流获取二进制数组
            MemoryStream streamMemory = new MemoryStream();
            hssfworkbook.Write(streamMemory);
            byte[] data = streamMemory.ToArray();

            return data;
        }


        /// <summary>
        /// 获取上传文件并返回数据集合
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadDataTemp()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files["myFile"];
            var lType = Request.Form["locationtype"];

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    //获取上传文件后缀
                    string suffix = Path.GetExtension(file.FileName);
                    //上传文件处理返回消息
                    string msg = "";
                    DataTable excelDt = GetDataTableFromExcel(file.InputStream, suffix, ref msg);

                    //当msg有除了“完成”的返回值的时候说明在GetDataTableFromExcel中报错了
                    if (msg != "完成") throw new Exception(msg);

                    //上传的文件集合
                    List<object> boList = new List<object>();

                    #region  
                    foreach (DataRow dr in excelDt.Rows)
                    {
                        Dictionary<string, object> b = new Dictionary<string, object>();
                        //没有坐标类型,不需要添加该节点
                        if (!string.IsNullOrEmpty(lType))
                        {
                            //初始坐标对象
                            b.Add("location", new Dictionary<string, object>());
                            //初始坐标类型对象
                            Dictionary<string, object> type = new Dictionary<string, object>();
                            type.Add("type", lType);
                            ((Dictionary<string, object>)b["location"]).Add("type", lType);
                            b.Add("locationType", lType);
                        }

                        //初始属性列对象
                        b.Add("properties", new Dictionary<string, object>());

                        boList.Add(b);

                        foreach (DataColumn col in excelDt.Columns)
                        {
                            string rValue = Convert.ToString(dr[col.ColumnName]);
                            if (col.ColumnName.ToUpper() == "BOID")
                            {
                                if (string.IsNullOrEmpty(rValue)) rValue = Convert.ToString(Guid.NewGuid());
                                b.Add("boid", rValue);
                            }
                            else if (col.ColumnName.ToUpper() == "BO")
                            {
                                b.Add("bo", rValue);
                            }
                            else if (col.ColumnName.ToUpper() == "BOT")
                            {
                                b.Add("bot", rValue);
                            }
                            else if (col.ColumnName.ToUpper() == "ALIAS")
                            {
                                string tmp = rValue;
                                //当alias与bo相同的时候就设置alias为空
                                if (b["bo"].ToString() != tmp)
                                {
                                    if (!string.IsNullOrEmpty(tmp))
                                        b.Add("alias", tmp.Split(','));
                                }
                                else
                                {
                                    b.Add("alias", "");
                                }
                            }
                            else if (col.ColumnName.ToUpper() == "LOCATION")
                            {
                                if (string.IsNullOrEmpty(rValue) || string.IsNullOrEmpty(lType))
                                    continue;

                                string str = rValue.Replace("\n", ",").Replace("\r", ",");

                                object coordinates = ConvertLocation(lType, str);

                                if (lType.ToUpper() == "GEOMETRYCOLLECTION")
                                    ((Dictionary<string, object>)b["location"]).Add("geometries", coordinates);
                                else
                                    ((Dictionary<string, object>)b["location"]).Add("coordinates", coordinates);
                            }
                            else
                            {
                                ((Dictionary<string, object>)b["properties"]).Add(col.ColumnName, rValue);

                                //并且保存数据到根级别对象
                                if (b.ContainsKey(col.ColumnName))
                                    b[col.ColumnName] = rValue;
                                else
                                    b.Add(col.ColumnName, rValue);
                            }
                        }
                    }

                    #endregion

                    return JsonTips("success", "上传成功", JsonNT(boList));
                }
                catch (Exception ex)
                {
                    return JsonTips("error", "上传失败，" + ex.Message);
                    throw;
                }
            }
            return JsonTips("error", "未获取到上传文件,请选择Excel文件重新上传！");
        }

        /// <summary>
        /// 获取上传文件的坐标数据并返回数据集合
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadLocationTemp()
        {
            HttpFileCollectionBase files = Request.Files;
            //HttpPostedFileBase file = files["myFile"];
            var lType = Request.Form["locationtype_hd"];

            if (files.Count == 0)
                return JsonTips("error", "未获取到上传文件,请选择Excel文件重新上传！");

            //上传的文件集合
            List<object> boList = new List<object>();

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    Dictionary<string, object> b = new Dictionary<string, object>();
                    //初始坐标对象
                    b.Add("location", new Dictionary<string, object>());
                    //初始坐标类型
                    ((Dictionary<string, object>)b["location"]).Add("type", lType);
                    b.Add("locationType", lType);

                    //初始属性列对象
                    b.Add("bo", files[i].FileName.Split('.')[0]);

                    boList.Add(b);

                    using (System.IO.StreamReader file = new System.IO.StreamReader(files[i].InputStream, Encoding.UTF8))
                    {

                        string str = file.ReadToEnd();
                        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(lType))
                            continue;

                        str = str.Replace("\n", ",").Replace("\r", ",");

                        object coordinates = ConvertLocation(lType, str);

                        if (lType.ToUpper() == "GEOMETRYCOLLECTION")
                        {
                            ((Dictionary<string, object>)b["location"]).Add("geometries", coordinates);
                        }
                        else
                        {
                            ((Dictionary<string, object>)b["location"]).Add("coordinates", coordinates);
                        }

                    }//end StreamReader
                }

                return new JsonResult()
                {
                    Data = new { data = boList, state = "success" },
                    MaxJsonLength = int.MaxValue,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return JsonTips("error", "上传失败" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 转换坐标格式
        /// </summary>
        /// <returns></returns>
        public object ConvertLocation(string lType, string location)
        {
            lType = lType.ToUpper();
            #region 坐标数据格式

            #endregion
            //适用于多组混合坐标对象
            string[] tmpGroupType = location.Split('=');

            //适用于多组坐标对象
            string[] tmpType = location.Split('-');

            //适用用单组坐标对象
            string[] tmp = new string[] { };

            foreach (var lItem in tmpType)
            {
                if (!string.IsNullOrEmpty(lItem))
                {
                    tmp = lItem.Split(',');
                    break;
                }
            }

            //一维数组
            List<float> locationList = null;
            //二维数组
            List<List<float>> locationLevel2 = null;
            //三维数组
            List<List<List<float>>> locationLevel3 = null;
            //四维数组
            List<List<List<List<float>>>> locationLevel4 = null;
            switch (lType)
            {

                case "POINT":
                    #region POINT Point = [1,2]
                    /*
                        Point = [1,2]
                     */
                    locationList = new List<float>();
                    foreach (string item in tmp)
                    {
                        if (string.IsNullOrEmpty(item) || !IsFloat(item))
                            continue;
                        locationList.Add(ConvertToFloat(item));
                        if (locationList.Count == 2)
                            break;
                    }
                    return locationList;
                #endregion
                case "MULTIPOINT":
                case "LINESTRING":
                    #region MultiPoint,LineString  [[0,0],[3,6]]
                    /*
                        MultiPoint = [[0,0],[3,6]]
                        LineString = [[0,0],[3,6]]
                     */
                    locationLevel2 = new List<List<float>>();

                    locationList = new List<float>();

                    foreach (var item in tmp)
                    {
                        if (string.IsNullOrEmpty(item) || !IsFloat(item))
                            continue;
                        locationList.Add(ConvertToFloat(item));
                        if (locationList.Count == 2)
                        {
                            locationLevel2.Add(locationList);
                            locationList = new List<float>();
                        }
                    }
                    return locationLevel2;
                #endregion
                case "MULTILINESTRING":
                    #region MultiLineString [ [[1,2],[2,3]], [[1,2],[2,3]] ]

                    /*
                        MultiLineString = [ [[1,2],[2,3]], [[1,2],[2,3]] ]
                    */
                    locationLevel3 = new List<List<List<float>>>();

                    locationList = new List<float>();

                    foreach (var lItem in tmpType)
                    {
                        if (string.IsNullOrEmpty(lItem))
                            continue;

                        tmp = lItem.Split(',');
                        //
                        locationLevel2 = new List<List<float>>();

                        foreach (var item in tmp)
                        {
                            if (string.IsNullOrEmpty(item) || !IsFloat(item))
                                continue;

                            locationList.Add(ConvertToFloat(item));
                            if (locationList.Count == 2)
                            {
                                locationLevel2.Add(locationList);
                                locationList = new List<float>();
                            }
                        }
                        if (locationLevel2.Any())
                            locationLevel3.Add(locationLevel2);
                    }
                    //
                    return locationLevel3;
                #endregion
                case "POLYGON":
                    #region  Polygon [ [[0, 0],[3,6],[6,1],[0,0]] ]

                    /*
                        Polygon = [ [[0, 0],[3,6],[6,1],[0,0]] ]
                    */
                    locationLevel3 = new List<List<List<float>>>();
                    //
                    locationLevel2 = new List<List<float>>();
                    //
                    locationList = new List<float>();
                    //
                    foreach (var item in tmp)
                    {
                        if (string.IsNullOrEmpty(item) || !IsFloat(item))
                            continue;

                        locationList.Add(ConvertToFloat(item));
                        if (locationList.Count == 2)
                        {
                            locationLevel2.Add(locationList);
                            locationList = new List<float>();
                        }
                    }
                    locationLevel3.Add(locationLevel2);
                    return locationLevel3;
                #endregion
                case "MULTIPOLYGON":
                    #region MultiPolygon [ [[[0, 0],[3,6],[6,1],[0,0]]], [[[0,0],[3,6],[6,1],[0,0]]] ]

                    /*
                        MultiPolygon  = [ 
                                            [ [[0, 0],[3,6],[6,1],[0,0]] ], [ [[0,0],[3,6],[6,1],[0,0]] ] 
                                        ]
                    */
                    locationLevel4 = new List<List<List<List<float>>>>();

                    locationList = new List<float>();

                    foreach (var lItem in tmpType)
                    {
                        if (string.IsNullOrEmpty(lItem))
                            continue;

                        tmp = lItem.Split(',');
                        //
                        locationLevel2 = new List<List<float>>();

                        foreach (var item in tmp)
                        {
                            if (string.IsNullOrEmpty(item) || !IsFloat(item))
                                continue;

                            locationList.Add(ConvertToFloat(item));
                            if (locationList.Count == 2)
                            {
                                locationLevel2.Add(locationList);
                                locationList = new List<float>();
                            }
                        }
                        if (locationLevel2.Any())
                        {
                            locationLevel3 = new List<List<List<float>>>();
                            locationLevel3.Add(locationLevel2);
                            //
                            locationLevel4.Add(locationLevel3);
                        }
                    }

                    //
                    return locationLevel4;
                #endregion
                case "GEOMETRYCOLLECTION":
                    #region GeometryCollection json对象的模式
                    #region 格式
                    /*
                        GeometryCollection = 
                        [
                            {
                                type: "MultiPoint",
                                coordinates: [[1,2],[2,3],[4,5],[6,7]]
                            },
                            {
                                type: "MultiLineString",
                                coordinates:[
                                          [[1,2],[3,4]],
                                          [[2,3],[4,5]],
                                          [[4,5],[6,7]],
                                          [[1,2],[2,3]]
                                       ]
                             },
                             ....(等等其他类型)
                        ]
                    */
                    #endregion
                    List<object> objList = new List<object>();

                    foreach (var guoupItem in tmpGroupType)
                    {
                        if (string.IsNullOrEmpty(guoupItem))
                            continue;

                        tmp = guoupItem.Split(',');

                        //
                        locationLevel2 = new List<List<float>>();

                        foreach (var item in tmp)
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;

                            object resObject = ConvertLocation(item, guoupItem);
                            //
                            objList.Add(new
                            {
                                type = item,
                                coordinates = resObject
                            });
                            break;
                        }
                    }
                    //
                    return objList;
                #endregion
                default:
                    return "";
            }
        }

        /// <summary>
        /// 从EXCEL获取DataTable
        /// </summary>
        /// <param name="excelStream"></param>
        /// <param name="suffix"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromExcel(Stream excelStream, string suffix, ref string msg)
        {
            msg = "完成";
            DataTable dt = new DataTable();
            try
            {
                #region 初始excel
                IWorkbook hssfworkbook;
                if (suffix == ".xls") //2003
                    hssfworkbook = new HSSFWorkbook(excelStream);
                else
                    hssfworkbook = new XSSFWorkbook(excelStream); //2007+

                //
                ISheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                #endregion

                #region 获取第一行作为标题
                //总列数
                IRow firstRow = sheet.GetRow(0);
                for (int j = 0; j < firstRow.LastCellNum; j++)
                {
                    dt.Columns.Add(firstRow.GetCell(j).StringCellValue);
                }
                int maxColsCount = firstRow.LastCellNum;
                #endregion

                #region 获取数据行
                for (int j = 1; j < sheet.PhysicalNumberOfRows; j++)
                {
                    IRow row = sheet.GetRow(j);
                    DataRow dr = dt.NewRow();
                    if (row == null)
                        continue;
                    bool IsNUll = false;

                    #region 获取单元格数据
                    for (int i = 0; i < maxColsCount; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            switch (cell.CellType) //单元格数据类型
                            {
                                case CellType.Formula: //公式
                                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(hssfworkbook);
                                    dr[i] = e.Evaluate(cell).StringValue;
                                    if (dr[i] == DBNull.Value)
                                        dr[i] = e.Evaluate(cell).NumberValue;
                                    break;
                                case CellType.Numeric:
                                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                                    {
                                        dr[i] = cell.DateCellValue; //日期
                                    }
                                    else
                                    {
                                        dr[i] = cell.NumericCellValue; //数字
                                    }
                                    break;
                                case CellType.Blank: //空
                                case CellType.Boolean:
                                case CellType.Error:
                                case CellType.String: //字符串
                                case CellType.Unknown:
                                default:
                                    dr[i] = cell.StringCellValue;
                                    break;
                            }

                        }
                        if (dr[i] != null && Convert.ToString(dr[i]) != "" && !IsNUll)
                            IsNUll = true;
                    }
                    #endregion
                    if (IsNUll)
                        dt.Rows.Add(dr);
                }
                #endregion
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return dt;
        }

        /// <summary>
        /// 返回当前object的Float类型
        /// 数据类型转换
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public float ConvertToFloat(object b)
        {
            float val = 0f;
            if (float.TryParse(Convert.ToString(b), out val))
            {
                return val;
            }
            return 0f;
        }

        /// <summary>
        /// 是否可以转换哪位小数数字
        /// </summary>
        /// <param name="b"></param>
        /// <returns>是 true 否 false</returns>
        public bool IsFloat(object b)
        {
            float val = 0f;
            if (float.TryParse(Convert.ToString(b), out val))
            {
                return true;
            }
            return false;
        }


        #region 各类型坐标导入txt文档的格式
        #region GeometryCollection
        /*
         * 注多个类型坐标用 '=类型名称' 分割并且换行
         * 注单个类型存在多组坐标用 '-' 分割并且换行
         * 
         * 
        =Point
        1,2
        =Polygon
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456
        =LineString
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456
        =MultiPoint
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456
        =MultiLineString
        -
        1.123456,2.3456
        2.123456,3.3456
        -
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456
        =MultiPolygon
        -
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        1.123456,2.3456
        -
        3.123456,4.3456
        5.123456,6.3456
        7.123456,8.3456
        3.123456,4.3456
        */
        #endregion

        #region LineString
        /*
            1.123456,2.3456
            2.123456,3.3456
            3.123456,4.3456
            5.123456,6.3456
            1.123456,2.3456
         */
        #endregion

        #region MultiLineString
        /*
         * 注多个线坐标用 '-' 分割并且换行
        -
        1.123456,2.3456
        2.123456,3.3456
        -
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456
        */
        #endregion

        #region Point
        /*
         1,2
         */
        #endregion

        #region MultiPoint
        /*
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456
        */
        #endregion

        #region Polygon
        /*
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        5.123456,6.3456
        1.123456,2.3456 
        */
        #endregion

        #region MultiPolygon
        /*
         * 注多个面坐标用 '-' 分割并且换行
        
        -
        1.123456,2.3456
        2.123456,3.3456
        3.123456,4.3456
        1.123456,2.3456
        -
        3.123456,4.3456
        5.123456,6.3456
        7.123456,8.3456
        3.123456,4.3456
        */
        #endregion

        #endregion

    }
}