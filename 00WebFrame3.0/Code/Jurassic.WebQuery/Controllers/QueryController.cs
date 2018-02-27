using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebQuery.Models;
using Jurassic.WebQuery.Repository;
using Jurassic.WebFrame;

namespace Jurassic.WebQuery.Controllers
{
    public class QueryController : BaseController
    {
        public QueryManage QueryManage { get; set; }

        public static string Sql;

        public QueryController(QueryManage queryManage)
        {
            QueryManage = queryManage;
        }
 
        /// <summary>
        /// 主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 显示结果和SQL语句的视图
        /// </summary>
        /// <returns></returns>
        public ActionResult SQLView()
        {
            ViewBag.Sql = Sql;
            ViewBag.Table = QueryManage.GetData(Sql);

            return View();
        }
      
 
        /// <summary>
        /// 获取表的树结构
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTableTree()
        {
            return Json(QueryManage.GetAllTables(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将前台的字符串设置转化成SQL模型
        /// </summary>
        /// <param name="table"></param>
        /// <param name="selfil"></param>
        /// <param name="join"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public QueriesAndReportingModel ToSQLString(string table, string selfil, string join, string where,string combo, string order)
        {
            //参数的判断放在页面端。
            string[] tableArr = string.IsNullOrWhiteSpace(table) ? new string[] { } : table.Split(',');
            string[] selfilArr = string.IsNullOrWhiteSpace(selfil) ? new string[] { } : selfil.Split(',');
            string[] joinArr = string.IsNullOrWhiteSpace(join) ? new string[] { } : join.Split(',');
            string[] whereArr = string.IsNullOrWhiteSpace(where) ? new string[] { } : where.Split(',');
            string[] comboArr = string.IsNullOrWhiteSpace(combo) ? new string[] { } : combo.Split(',');
            string[] orderArr = string.IsNullOrWhiteSpace(order) ? new string[] { } : order.Split(',');
            var model = new QueriesAndReportingModel();
            if (tableArr.Length > 0)
            {
                var tables = new List<SettingTableModel>();
                foreach (var m in tableArr)
                {
                    var mArr = m.Split('.');
                    tables.Add(new SettingTableModel
                    {
                        Id = mArr[0],
                        ENName = mArr[1]
                    });
                }
                model.Tables = tables;
            }
            if (selfilArr.Length > 0)
            {
                var selfils = new List<SettingFieldModel>();
                foreach (var m in selfilArr)
                {
                    var mArr = m.Split('.');
                    selfils.Add(new SettingFieldModel
                    {
                        TableId = mArr[0],
                        Id = mArr[1],
                        TableENName = mArr[2],
                        ENName = mArr[3]
                    });
                }
                model.Fields = selfils;
            }
            if (joinArr.Length >= 3)
            {
                var joins = new List<JoinSettingModel>();

                for (int j = 0; j < joinArr.Length; j+=3)
                {
                    if (string.IsNullOrWhiteSpace(joinArr[j]) || string.IsNullOrWhiteSpace(joinArr[j+1]) || string.IsNullOrWhiteSpace(joinArr[j+2]))
                        continue;
                    joins.Add(new JoinSettingModel
                    {
                        LeftTableENName = joinArr[j].Split('.')[0],
                        LeftAttributeENName = joinArr[j].Split('.')[1],
                        Connectors = joinArr[j+1],
                        RightTableENName =   joinArr[j+2].Split('.')[0],
                        RightAttributeENName = joinArr[j+2].Split('.')[1]
                    });
                }
                model.JoinSettings = joins;
            }
            //if (whereArr.Length >= 3 )
            //{
            //    var wheres = new List<WhereSettingModel>
            //    {
            //        new WhereSettingModel
            //        {
            //            LeftTableENName = whereArr[0].Split('.')[0],
            //            LeftAttributeENName = whereArr[0].Split('.')[1],
            //            Connectors = whereArr[1],
            //            RightTableENName = whereArr[2].Split('.').Length>1?whereArr[2].Split('.')[0]:null,
            //            RightAttributeENName = whereArr[2].Split('.').Length>1?whereArr[2].Split('.')[1]:whereArr[2]
            //        }
            //    };
            //    if (whereArr.Length >= 3)
            //    {
            //        for (int i = 3; i < whereArr.Length; i += 4)
            //        {
            //            var addmodel = new WhereSettingModel
            //            {
            //                Operator = whereArr[i],
            //                LeftTableENName = whereArr[i+1].Split('.')[0],
            //                LeftAttributeENName = whereArr[i+1].Split('.')[1],
            //                Connectors = whereArr[i + 2],
            //                RightTableENName = whereArr[i + 3].Split('.').Length > 1 ? whereArr[i + 3].Split('.')[0] : null,
            //                RightAttributeENName = whereArr[i + 3].Split('.').Length > 1 ? whereArr[i + 3].Split('.')[1] : whereArr[i + 3]
            //            };
            //            wheres.Add(addmodel);
            //        }
            //    }
            //    model.WhereSettings = wheres;
            //}

            if (whereArr.Length >= 2 && string.IsNullOrWhiteSpace(whereArr[0]) && string.IsNullOrWhiteSpace(whereArr[1]))
            {
                var wheres = new List<WhereSettingModel>
                    {
                        new WhereSettingModel
                        {
                            LeftTableENName = whereArr[0].Split('.')[0],
                            LeftAttributeENName = whereArr[0].Split('.')[1],
                            Connectors = whereArr[1],
                            //RightTableENName = whereArr[2].Split('.').Length>1?whereArr[2].Split('.')[0]:null,
                            //RightAttributeENName = whereArr[2].Split('.').Length>1?whereArr[2].Split('.')[1]:whereArr[2]
                        }
                    };
                if (whereArr.Length >= 3)
                {
                    for (int i = 2; i < whereArr.Length; i += 3)
                    {
                        if (string.IsNullOrWhiteSpace(whereArr[i]) || string.IsNullOrWhiteSpace(whereArr[i + 1]) || string.IsNullOrWhiteSpace(whereArr[i + 2]))
                            continue;
                        var addmodel = new WhereSettingModel
                        {
                            Operator = whereArr[i],
                            LeftTableENName = whereArr[i + 1].Split('.')[0],
                            LeftAttributeENName = whereArr[i + 1].Split('.')[1],
                            Connectors = whereArr[i + 2],
                            //RightTableENName = whereArr[i + 3].Split('.').Length > 1 ? whereArr[i + 3].Split('.')[0] : null,
                            //RightAttributeENName = whereArr[i + 3].Split('.').Length > 1 ? whereArr[i + 3].Split('.')[1] : whereArr[i + 3]
                        };
                        wheres.Add(addmodel);
                    }
                }
                model.WhereSettings = wheres;
            }

            if (comboArr.Length >= 1 && string.IsNullOrWhiteSpace(comboArr[0]))
            {
                var combos = new List<ComboSettingModel>();
                //{
                //    new ComboSettingModel
                //    //{
                //    //    //RightTableENName = comboArr[0].Split('.').Length>1?comboArr[0].Split('.')[2]:comboArr[0],
                //    //    //RightAttributeENName =  comboArr[0].Split('.').Length>1?comboArr[0].Split('.')[3]:comboArr[0],
                //    //}
                //};
                if (comboArr.Length >= 1)
                {
                    for (int i = 0; i < comboArr.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(comboArr[i]))
                            continue;
                        var addmodel = new ComboSettingModel
                        {
                            RightTableENName = comboArr[i].Split('.').Length>3?comboArr[i].Split('.')[2]:comboArr[i],//如果输入小数，就会报错，超出数组界限
                            RightAttributeENName = comboArr[i].Split('.').Length>3?comboArr[i].Split('.')[3]:null,
                        };
                        combos.Add(addmodel);
                    }
                }
                model.ComboSettings = combos;
            }

            if (orderArr.Length >= 2)
            {
                var orders = new List<OrderSettingModel>();
                for (int i = 0; i < orderArr.Length; i+=2)
                {
                    if (string.IsNullOrWhiteSpace(orderArr[i]) || string.IsNullOrWhiteSpace(orderArr[i+1]))
                        continue;
                    orders.Add(new OrderSettingModel
                    {
                        TableENName = orderArr[i].Split('.')[0],
                        AttributeENName = orderArr[i].Split('.')[1],
                        SortBy = orderArr[i+1]
                    });
                }
                model.OrderSettings = orders;
            }
            return model;
        }

        /// <summary>
        /// 用用户设置的参数获取SQL语句
        /// </summary>
        /// <param name="selfil"></param>
        /// <param name="join"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSQL(string table, string selfil, string join, string where, string combo,string order)
        {
            var  model =  ToSQLString(table, selfil, join, where,combo,order);
            //sqlStr为最终想要得到的SQL语句
            var sqlStr = "";
            if (model.Tables == null || model.Tables.Count == 0) return sqlStr;
            //选中的要查询的的字段
            var fields="";
            if (model.Fields != null)
            {
                for (int i = 0, l = model.Fields.Count; i < l; i++)
                {
                    var field = model.Fields[i].TableENName + "." + model.Fields[i].ENName + ",";
                    fields += field;
                }
                fields = fields.Substring(0, fields.Length - 1);
            }
            else
                fields = "*";
            sqlStr = " SELECT " + fields + " FROM ";
            
            //Bug:选择多张表，字段选择多张表中的字段，然后没有Join条件的时候会报错。
            //连接条件JoinSettings,考虑选中表多于两张的情况
            if (model.JoinSettings != null && model.JoinSettings.Count > 0 && model.Tables != null && model.Tables.Count >= 2)
            {
                for (int i = 0, l = model.JoinSettings.Count; i < l; i++)
                {
                    if (l > 1)
                    {
                        var t = "";
                        for (int j = 0; j < i; j++)
                        {
                            //判断哪一张表是还没有执行过的新表
                            var tl = model.JoinSettings[i].LeftTableENName;
                            var tr = model.JoinSettings[i].RightTableENName;
                            if (tl != model.JoinSettings[j].LeftTableENName &&
                                tl != model.JoinSettings[j].RightTableENName)
                            {
                                t = model.JoinSettings[i].LeftTableENName;
                                sqlStr += model.JoinSettings[i].Connectors + " JOIN " + t + " ON " +
                                          model.JoinSettings[i].LeftTableENName + "." +
                                          model.JoinSettings[i].LeftAttributeENName + "=" +
                                          model.JoinSettings[i].RightTableENName + "." +
                                          model.JoinSettings[i].RightAttributeENName + " ";
                            }
                            if (tr != model.JoinSettings[j].RightTableENName &&
                                tr != model.JoinSettings[j].LeftTableENName)
                            {
                                t = model.JoinSettings[i].RightTableENName;
                                sqlStr += model.JoinSettings[i].Connectors + " JOIN " + t + " ON " +
                                          model.JoinSettings[i].LeftTableENName + "." +
                                          model.JoinSettings[i].LeftAttributeENName + "=" +
                                          model.JoinSettings[i].RightTableENName + "." +
                                          model.JoinSettings[i].RightAttributeENName + " ";
                            }
                        }

                    }
                    sqlStr += model.JoinSettings[i].LeftTableENName + " " + model.JoinSettings[i].Connectors + " JOIN " +
                              model.JoinSettings[i].RightTableENName + " ON " + model.JoinSettings[i].LeftTableENName +
                              "." + model.JoinSettings[i].LeftAttributeENName + "=" +
                              model.JoinSettings[i].RightTableENName + "." + model.JoinSettings[i].RightAttributeENName +
                              " ";
                }
            }
            else
            {
                sqlStr += model.Tables[0].ENName;
            }
            //筛选条件
            if (model.WhereSettings != null && model.WhereSettings.Count > 0)
            {
                sqlStr += " WHERE ";
                for (int i = 0, l = model.WhereSettings.Count; i < l; i++)
                {
                    if (l > 1)
                    {
                        sqlStr += model.WhereSettings[i].Operator + " ";
                    }

                    if (model.ComboSettings[i].RightAttributeENName == null)
                    {
                        sqlStr += model.WhereSettings[i].LeftTableENName + "." +
                                  model.WhereSettings[i].LeftAttributeENName + " " + model.WhereSettings[i].Connectors +
                                  " " + model.ComboSettings[i].RightTableENName + " ";
                    }
                    else
                    {
                        sqlStr += model.WhereSettings[i].LeftTableENName + "." +
                                  model.WhereSettings[i].LeftAttributeENName + " " + model.WhereSettings[i].Connectors +
                                  " " + model.ComboSettings[i].RightTableENName + "." +
                                  model.ComboSettings[i].RightAttributeENName + " ";
                    }
                }
            }
            //排序
            if (model.OrderSettings != null && model.OrderSettings.Count > 0)
            {
                var ord = model.OrderSettings;
                sqlStr += " ORDER BY " + " ";
                for (int i = 0, l = model.OrderSettings.Count; i < l; i++)
                {
                    sqlStr += model.OrderSettings[i].TableENName + "." + model.OrderSettings[i].AttributeENName + " " +
                              model.OrderSettings[i].SortBy + ",";
                }
                sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
            }
            Sql = sqlStr;
            return sqlStr;
            //return Json(ToSQLString(table, selfil, join, where, order), JsonRequestBehavior.AllowGet);
        }
    }
}