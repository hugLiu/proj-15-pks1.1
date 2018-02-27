using System;
using System.Data;
using System.Reflection;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// DataSet和DataTable的帮助类
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// copy content from source to destination row 
        /// they must be of the same type 
        /// </summary>
        /// <param name="objSource">source reference to a row</param>
        /// <param name="objDestination">reference to a destination row</param>
        public static void CopyRow(object objSource, object objDestination)
        {
            int intCounter = 0;
            DataRow objS = (DataRow)objSource;
            DataRow objD = (DataRow)objDestination;

            // Starts an edit operation on objD.
            objD.BeginEdit();

            // set destination row with the SourceRow.
            for (intCounter = 0; intCounter <= objS.ItemArray.GetUpperBound(0); intCounter++)
            {
                objD[intCounter] = objS[intCounter];
            }

            // Ends the edit occurring on destination row.
            objD.EndEdit();
        }

        /// <summary>
        /// copy content from source to destination row 
        /// they must be of the same type 
        /// </summary>
        /// <param name="objSource">source reference to a row</param>
        /// <param name="objDestination">reference to a destination row</param>
        public static void CopyRow_ByFieldName(object objSource, object objDestination)
        {
            int intCounter = 0;
            int intCounter2 = 0;
            string strFieldName = "";
            DataRow objS = (DataRow)objSource;
            DataRow objD = (DataRow)objDestination;

            // Starts an edit operation on objD.
            objD.BeginEdit();

            // set destination row with the SourceRow. 
            for (intCounter = 0; intCounter <= objS.ItemArray.GetUpperBound(0); intCounter++)
            {
                // it no need to check upper or lower 
                strFieldName = objS.Table.Columns[intCounter].ColumnName;

                //for each objD 
                for (intCounter2 = 0; intCounter2 <= objD.ItemArray.GetUpperBound(0); intCounter2++)
                {
                    if (objD.Table.Columns[intCounter2].ColumnName == strFieldName)
                    {
                        objD[intCounter2] = objS[strFieldName];
                        break;
                    }
                }
            }

            // Ends the edit occurring on destination row.
            objD.EndEdit();
        }

        /// <summary>
        /// copy content from souece datatable to destination data.
        /// they must be of the same type
        /// </summary>
        /// <param name="objSource">source table</param>
        /// <param name="objDestination">destination table</param>
        public static void CopyDataTable(object objSource, object objDestination)
        {
            DataTable objS = (DataTable)objSource;
            DataTable objD = (DataTable)objDestination;
            DataRow objRowD = null;

            foreach (DataRow objRowS in objS.Rows)
            {
                if (objRowS.RowState != DataRowState.Deleted && objRowS.RowState != DataRowState.Detached)
                {
                    objRowD = objD.NewRow();
                    DataHelper.CopyRow(objRowS, objRowD);

                    // add new row
                    objD.Rows.Add(objRowD);
                }
            }
        }

        /// <summary>
        /// copy content from souece datatable to destination data.
        /// they must be of the same type
        /// </summary>
        /// <param name="objSource">source table</param>
        /// <param name="objDestination">destination table</param>
        /// <param name="sourceFilter">filter string for source table</param>
        public static void CopyDataTable(DataTable objSource, DataTable objDestination, string sourceFilter)
        {
            DataView objS = new DataView(objSource);
            DataTable objD = (DataTable)objDestination;
            DataRow objRowD = null;

            objS.RowFilter = sourceFilter;
            foreach (DataRowView objRowS in objS)
            {
                if (objRowS.Row.RowState != DataRowState.Deleted && objRowS.Row.RowState != DataRowState.Detached)
                {
                    objRowD = objD.NewRow();
                    DataHelper.CopyRow(objRowS.Row, objRowD);

                    // add new row
                    objD.Rows.Add(objRowD);
                }
            }
        }

        /// <summary>
        /// copy content from souece datatable to destination data.
        /// </summary>
        /// <param name="objSource"></param>
        /// <param name="objDestination"></param>
        public static void CopyDataTable_ByFieldName(object objSource, object objDestination)
        {
            DataTable objS = (DataTable)objSource;
            DataTable objD = (DataTable)objDestination;
            DataRow objRowD = null;

            foreach (DataRow objRowS in objS.Rows)
            {
                if (objRowS.RowState != DataRowState.Deleted && objRowS.RowState != DataRowState.Detached)
                {
                    objRowD = objD.NewRow();
                    DataHelper.CopyRow_ByFieldName(objRowS, objRowD);

                    // add new row
                    objD.Rows.Add(objRowD);
                }
            }
        }

        /// <summary>
        /// 将源记录合并到目标行上。（适用于将多条记录合并到一条记录上）
        /// </summary>
        /// <param name="sourceRow">源记录</param>
        /// <param name="mergeRow">目标行</param>
        public static void MergeRecord(DataRow sourceRow, DataRow mergeRow)
        {
            if (sourceRow == null)
                return;

            // 行是否可用
            if (DataHelper.RowIsAvailable(sourceRow) && DataHelper.RowIsAvailable(mergeRow))
            {
                // 得到记录的表信息。
                DataTable sourceTable = sourceRow.Table;
                DataTable mergeTable = mergeRow.Table;

                // 将数据源的信息拷贝到目标行上。
                foreach (DataColumn col in sourceTable.Columns)
                {
                    // 判断列是否存在。
                    if (mergeTable.Columns.Contains(col.ColumnName))
                    {
                        // 赋值
                        mergeRow[col.ColumnName] = sourceRow[col.ColumnName];
                    }
                }
            }
        }
        /// <summary>
        /// 将源表的数据合并到目标表的指定行上。（适用于将多条记录合并到一条记录上）
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="mergeTable">目标表</param>
        /// <param name="rowIndex">需合并的行索引（从0开始）</param>
        public static void MergeRecord(DataTable sourceTable, DataTable mergeTable, int rowIndex)
        {
            // 判断传入的行索引是否有效。
            if (sourceTable.Rows.Count >= (rowIndex + 1) && mergeTable.Rows.Count >= (rowIndex + 1))
            {
                // 得到待合并的行
                DataRow sourceRow = sourceTable.Rows[rowIndex];   // 源记录
                DataRow mergeRow = mergeTable.Rows[rowIndex];    // 目标行

                // 执行行合并操作。
                DataHelper.MergeRecord(sourceRow, mergeRow);
            }
        }
        /// <summary>
        /// 将源表的结构合并到目标表上。（适用于将多表结构合并到一个表中）
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="mergeTable">目标表</param>
        public static void MergeTableStructure(DataTable sourceTable, DataTable mergeTable)
        {
            // 配置返回行的表结构
            foreach (DataColumn col in sourceTable.Columns)
            {
                // 判断此列是否已存在.
                if (mergeTable.Columns.Contains(col.ColumnName))
                {
                    continue;
                }

                // 将新列插入到表中。
                DataColumn newCol = mergeTable.Columns.Add(col.ColumnName, col.DataType);
                newCol.MaxLength = col.MaxLength;
            }
        }
        /// <summary>
        /// 得到行集合中指定字段的值。
        /// </summary>
        /// <param name="sourceRow">待取值的集合</param>
        /// <param name="fieldName">目标字段名</param>
        /// <returns>返回字段值的拼接串，以逗号分隔</returns>
        public static string GetRecordValues(DataRow[] sourceRow, string fieldName)
        {
            string fieldValues = "";

            foreach (DataRow rowItem in sourceRow)
            {
                if (!DataHelper.RowIsAvailable(rowItem))
                    continue;

                if (rowItem[fieldName] == DBNull.Value)
                    continue;

                // 将需要取出的字段值拼接起来。
                fieldValues += rowItem[fieldName] + ",";
            }

            // 返回
            return fieldValues == "" ? "" : fieldValues.Substring(0, fieldValues.Length - 1);
        }
        /// <summary>
        /// 得到行集合中指定字段的值（以逗号分隔）。
        /// </summary>
        /// <param name="sourceRow">待取值的集合</param>
        /// <param name="fieldName">目标字段名</param>
        /// <returns>返回字段值的拼接串，以逗号分隔</returns>
        public static string GetRecordValues(DataRowCollection sourceRow, string fieldName)
        {
            string fieldValues = "";

            foreach (DataRow rowItem in sourceRow)
            {
                if (!DataHelper.RowIsAvailable(rowItem))
                    continue;

                if (rowItem[fieldName] == DBNull.Value)
                    continue;

                // 将需要取出的字段值拼接起来。
                fieldValues += rowItem[fieldName] + ",";
            }

            // 返回
            return fieldValues == "" ? "" : fieldValues.Substring(0, fieldValues.Length - 1);
        }
        /// <summary>
        /// 得到行集合中指定字段的值。
        /// </summary>
        /// <param name="sourceTable">待取值的集合</param>
        /// <param name="fieldName">目标字段名</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns>返回字段值的拼接串</returns>
        public static string GetRecordValues(DataTable sourceTable, string fieldName, string splitChar)
        {
            string fieldValues = "";

            foreach (DataRow rowItem in sourceTable.Rows)
            {
                if (!DataHelper.RowIsAvailable(rowItem))
                    continue;

                if (rowItem[fieldName] == DBNull.Value)
                    continue;

                // 将需要取出的字段值拼接起来。
                fieldValues += rowItem[fieldName] + splitChar;
            }

            // 返回
            return fieldValues == "" ? "" : fieldValues.Substring(0, fieldValues.Length - 1);
        }

        /// <summary>
        /// find row in the datatable
        /// </summary>
        /// <param name="strFilter">filter</param>
        /// <param name="objTable">table</param>
        /// <returns></returns>
        public static DataRow FindRow(string strFilter, System.Data.DataTable objTable)
        {
            return DataHelper.FindRow(strFilter, objTable, DataViewRowState.CurrentRows);
        }
        /// <summary>
        /// find row in the datatable
        /// </summary>
        /// <param name="strFilter">filter</param>
        /// <param name="objTable">table</param>
        /// <param name="rowVersion">row data version</param>
        /// <returns></returns>
        public static DataRow FindRow(string strFilter, System.Data.DataTable objTable, DataViewRowState rowVersion)
        {
            if (objTable == null || objTable.Rows.Count <= 0)
            {
                return null;
            }

            System.Data.DataRow objRowReturn = null;
            System.Data.DataRow[] objRow = objTable.Select(strFilter, null, rowVersion);

            if (((objRow != null)) && objRow.Length > 0)
            {
                objRowReturn = (System.Data.DataRow)objRow[0];
            }

            return objRowReturn;
        }

        /// <summary>
        /// find row in the dataview
        /// </summary>
        /// <param name="objKey">key value</param>
        /// <param name="objDataView">dataview</param>
        /// <param name="strSort">sort</param>
        /// <returns></returns>
        public static DataRowView FindRow(object objKey, System.Data.DataView objDataView, string strSort)
        {
            if (objDataView == null || objDataView.Count <= 0)
            {
                return null;
            }

            System.Data.DataRowView[] objRow = null;
            System.Data.DataRowView objRowReturn = null;
            if (!string.IsNullOrEmpty(strSort.Trim()))
            {
                objDataView.Sort = strSort;
            }

            // Sort can't is empty 
            if (string.IsNullOrEmpty(objDataView.Sort.Trim()))
            {
                return null;
            }

            objRow = objDataView.FindRows(objKey);
            if (((objRow != null)) && objRow.Length > 0)
            {
                objRowReturn = objRow[0];
            }

            return objRowReturn;
        }

        /// <summary>
        /// find rows in the dataview
        /// </summary>
        /// <param name="objKey">key value</param>
        /// <param name="objDataView">dataview</param>
        /// <param name="strSort">sort</param>
        /// <returns></returns>
        public static DataRowView FindRow(object[] objKey, System.Data.DataView objDataView, string strSort)
        {
            if (objDataView == null || objDataView.Count <= 0)
            {
                return null;
            }


            System.Data.DataRowView[] objRow = null;
            System.Data.DataRowView objRowReturn = null;

            if (!string.IsNullOrEmpty(strSort.Trim()))
            {
                objDataView.Sort = strSort;
            }

            // Sort can't is empty 
            if (string.IsNullOrEmpty(objDataView.Sort.Trim()))
            {
                return null;
            }

            objRow = objDataView.FindRows(objKey);
            if (((objRow != null)) && objRow.Length > 0)
            {
                objRowReturn = objRow[0];
            }

            return objRowReturn;
        }

        /// <summary>
        /// find rows in the datatable
        /// </summary>
        /// <param name="strFilter">filter</param>
        /// <param name="objTable">table</param>
        /// <returns></returns>
        public static DataRow[] FindRows(string strFilter, System.Data.DataTable objTable)
        {
            // Parameters : 
            // 
            // Descriptions : 
            //  find rows in the datatable
            // 
            // Return : 
            // Author/Date : 2008-10-08, Cuttlebone

            return DataHelper.FindRows(strFilter, DataViewRowState.CurrentRows, objTable);
        }

        /// <summary>
        /// find rows in the datatable
        /// </summary>
        /// <param name="strFilter">filter</param>
        /// <param name="rowState">data row state</param>
        /// <param name="objTable">table</param>
        /// <returns></returns>
        public static DataRow[] FindRows(string strFilter, DataViewRowState rowState, System.Data.DataTable objTable)
        {
            if (objTable == null || objTable.Rows.Count <= 0)
            {
                return null;
            }

            return objTable.Select(strFilter, "", rowState);
        }

        /// <summary>
        /// find rows in the dataview
        /// </summary>
        /// <param name="objKey">key value</param>
        /// <param name="objDataView">dataview</param>
        /// <param name="strSort">sort</param>
        /// <returns></returns>
        public static DataRowView[] FindRows(object objKey, System.Data.DataView objDataView, string strSort)
        {
            if (objDataView == null || objDataView.Count <= 0)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(strSort.Trim()))
            {
                objDataView.Sort = strSort;
            }

            // Sort can't is empty 
            if (string.IsNullOrEmpty(objDataView.Sort.Trim()))
            {
                return null;
            }

            return objDataView.FindRows(objKey);
        }

        /// <summary>
        /// find rows in the dataview
        /// </summary>
        /// <param name="objKey">key value</param>
        /// <param name="objDataView">dataview</param>
        /// <param name="strSort">sort</param>
        /// <returns></returns>
        public static DataRowView[] FindRows(object[] objKey, System.Data.DataView objDataView, string strSort)
        {
            if (objDataView == null || objDataView.Count <= 0)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(strSort.Trim()))
            {
                objDataView.Sort = strSort;
            }

            // Sort can't is empty 
            if (string.IsNullOrEmpty(objDataView.Sort.Trim()))
            {
                return null;
            }

            return objDataView.FindRows(objKey);
        }

        /// <summary>
        /// indicate the datarow whether is available. 
        /// </summary>
        /// <param name="objRow"></param>
        /// <returns></returns>
        public static bool RowIsAvailable(DataRow objRow)
        {
            if (objRow == null)
                return false;

            return (objRow.RowState != DataRowState.Deleted && objRow.RowState != DataRowState.Detached);
        }

        /// <summary>
        /// remove duplicate record for the destination source.
        /// </summary>
        /// <returns></returns>
        public static void RemoveDuplicate(object objSource, object objDestination, string[] objPrimaryKey)
        {
            DataTable objTableS = objSource as DataTable;
            DataTable objTableD = objDestination as DataTable;
            DataView objTableView = new DataView(objTableD);

            if (objTableS != null && objTableD != null && objPrimaryKey.Length > 0)
            {
                object[] objKeyValue = new object[objPrimaryKey.Length];
                DataRowView[] objRowDup = null;

                foreach (DataRow objRowS in objTableS.Rows)
                {
                    // check row state whether the row is available.
                    if (DataHelper.RowIsAvailable(objRowS))
                    {
                        // set key value.
                        for (int intIndex = 0; intIndex <= objPrimaryKey.GetUpperBound(0); intIndex++)
                        {
                            objKeyValue[intIndex] = objRowS[objPrimaryKey[intIndex]];
                        }

                        // find destination table.
                        objRowDup = DataHelper.FindRows(objKeyValue, objTableView, objPrimaryKey[0]);
                        if (objRowDup != null)
                        {
                            for (int intIndex = 0; intIndex <= objRowDup.GetUpperBound(0); intIndex++)
                            {
                                // delete duplicate row.
                                objRowDup[intIndex].Delete();
                            }
                        }
                    }
                }
            }

            // submit changes and reutrn the table.
            objTableView.Table.AcceptChanges();
        }

        /// <summary>
        ///  将数据表中的记录全部设置Delete状态。
        /// </summary>
        /// <param name="objTable">目标数据表</param>
        public static int RemoveAll(object objTable)
        {
            return DataHelper.RemoveAll(objTable, "");
        }

        /// <summary>
        /// 将数据表中相匹配的记录全部设置Delete状态。
        /// </summary>
        /// <param name="objTable">目标数据表</param>
        /// <param name="rowFilter">删除条件</param>
        /// <returns></returns>
        public static int RemoveAll(object objTable, string rowFilter)
        {
            int intCounter = 0;
            if (objTable == null)
            {
                return 0;
            }

            DataRow[] deleteList = DataHelper.FindRows(rowFilter, (DataTable)objTable);
            if (deleteList == null)
                return 0;

            for (int i = 0; i < deleteList.Length; i++)
            {
                deleteList[i].Delete();
                intCounter++;
            }

            return intCounter;
        }

        /// <summary>
        /// 从Xml获取DataTable
        /// </summary>
        /// <param name="xmlFile">Xml文件名</param>
        /// <returns>DataTable</returns>
        public static DataTable XmlToDataTable(string xmlFile)
        {
            DataSet ds = new DataSet();

            ds.ReadXml(xmlFile);

            DataTable dt = ds.Tables.Count == 0 ? null : ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 将DataTable保存到Xml文件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="xmlFile">Xml文件名</param>
        public static void DataTableToXml(DataTable dt, String xmlFile)
        {
            DataSet ds = dt.DataSet;
            if (ds == null)
            {
                ds = new DataSet();
                ds.Tables.Add(dt);
            }
            ds.WriteXml(xmlFile);
        }

        /// <summary>
        /// 将reader[Key]转换为字符串
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ToStr(this IDataReader reader, string key)
        {
            return CommOp.ToStr(reader[key]);
        }

        /// <summary>
        /// 将reader[Key]转换为Int32
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int ToInt(this IDataReader reader, string key)
        {
            return CommOp.ToInt(reader[key]);
        }

        /// <summary>
        /// 将DataRow转换为 实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ReaderToModel<T>(DataRow dr)
        {

            T model = Activator.CreateInstance<T>();
            foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                if (dr.Table.Columns.Contains(pi.Name))
                {
                    if (!CommOp.IsEmpty(dr[pi.Name]))
                    {
                        pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 这个类对可空类型进行判断转换，要不然会报错
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value == DBNull.Value)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }


    }
}