using Jurassic.Com.OfficeLib;
using Jurassic.CommonModels;
using Jurassic.GeoFeature.BLL;
using Jurassic.GeoFeature.Model;
using Jurassic.WebFrame;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoFeature.Controllers
{
    public class BORelationManageController : BaseController
    {
        //
        // GET: /BORelationManage/

        public ActionResult Index()
        {
            return View();
        }

        public string ListName { get; set; }
        private DataTable DataExcel
        {
            set { Session["TmpDataTable"] = value; }
            get { return (DataTable)Session["TmpDataTable"]; }
        }
        string CheckBOTError = "";//对象类型错误信息
        string CheckBOError = "";//对象错误信息
        public DataTable ResultTable;
        public ActionResult LoadXls(ResourceFileInfo[] results)
        {
            try
            {
                ExcelHelper helper = new ExcelHelper(results[0].FileStream);
                DataSet ds = helper.ExcelToDataSet(true);
                if (ds != null)
                {
                    if (ds.Tables[0].Columns[0].ColumnName != "关系名称" && ds.Tables[0].Columns[2].ColumnName != "关系类型" && ds.Tables[0].Columns[2].ColumnName != "对象类型1" && ds.Tables[0].Columns[3].ColumnName != "对象类型2")
                    {
                        return JsonNT("Error");
                    }
                    else
                    {
                        DataExcel = ds.Tables[0];
                        return JsonNT(ds.Tables[0]);
                    }
                }
                else
                    return JsonNT("Error");
            }
            catch
            {
                return JsonNT("Error");
            }

        }
        /// <summary>
        /// 校验数据通过后才能进行入库操作,True：通过，False不通过
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            bool Rtn = true;
            ResultTable = DataExcel.Clone();//复制当前表的结构，同时记录对象类型ID和对象ID，用于写数据库。数据库中存储关系都是以ID来表示的，名称不符合要求
            RelTypeManager RelManage = new RelTypeManager();
            RelationManager RelBOManage = new RelationManager();
            List<string> BOTIDList = new List<string>();
            List<string> BOTUseGeoList = new List<string>();
            List<string> BOIDList = new List<string>();
            foreach (DataRow dr in DataExcel.Rows)
            {
                BOTIDList = new List<string>();
                BOIDList = new List<string>();
                //校验Excle表格中的对象类型和对象实例是否存在
                if (dr.ItemArray[0].ToString() != "" && dr.ItemArray[1].ToString() != "" && dr.ItemArray[2].ToString() != "" && dr.ItemArray[3].ToString() != "")//校验对象类型
                {
                    BOTIDList = RelManage.GetBOTbyName(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString());
                    if (BOTIDList[0] == "0" || BOTIDList[1] == "0")
                    {
                        if (BOTIDList[0] == "0")
                        {
                            CheckBOTError = CheckBOTError + "对象类型为：" + dr.ItemArray[2].ToString() + " 有误，基础库中没有此对象类型\r\n";
                        }
                        if (BOTIDList[1] == "0")
                        {
                            CheckBOTError = CheckBOTError + "对象类型为：" + dr.ItemArray[3].ToString() + " 有误，基础库中没有此对象类型\r\n";
                        }
                        
                        Rtn = false;
                        break;
                    }
                    else
                    {
                        BOTUseGeoList = RelManage.GetBOTRelByName(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString());
                        if (BOTUseGeoList[0] == "1" && BOTUseGeoList[1] == "1")
                        {
                            CheckBOTError = CheckBOTError + "具有空间坐标的对象类型：【"+dr.ItemArray[2].ToString() + "】和【" + dr.ItemArray[3].ToString() + "】,无法创建上下级关系\r\n";
                            //空间关系校验，有空间坐标不允许建立上下级关系
                            Rtn = false;
                            break;
                        }
                        else
                        {
                            DataRow drchk = ResultTable.NewRow();
                            drchk[0] = dr.ItemArray[0].ToString();
                            drchk[1] = dr.ItemArray[1].ToString();
                            drchk[2] = BOTIDList[0];//返回的对象类型ID1
                            drchk[3] = BOTIDList[1];//返回的对象类型ID2
                            ResultTable.Rows.Add(drchk);
                        }
                    }
                }
                else//校验对象实例
                {
                    if (dr.ItemArray[2].ToString() != "" && dr.ItemArray[3].ToString() != "")//校验对象
                    {
                        BOIDList = RelBOManage.GetBObyName(dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString());
                        if (BOIDList[0] == "0" || BOIDList[1] == "0")
                        {
                            if (BOIDList[0] == "0")
                            {
                                CheckBOError = CheckBOError + "对象名称为：" + dr.ItemArray[2].ToString() + " 有误，基础库中没有此对象\r\n";
                            }
                            if (BOIDList[1] == "0")
                            {
                                CheckBOError = CheckBOError + "对象名称为：" + dr.ItemArray[3].ToString() + " 有误，基础库中没有此对象\r\n";
                            }
                            Rtn = false;
                        }
                        else
                        {
                            DataRow drchk = ResultTable.NewRow();
                            drchk[0] = "";
                            drchk[1] = "";
                            drchk[2] = BOIDList[0];//返回的对象ID1
                            drchk[3] = BOIDList[1];//返回的对象ID2
                            ResultTable.Rows.Add(drchk);
                        }
                    }
                }
            }
            return Rtn;
        }
        /// <summary>
        /// 提交Excel模板数据到数据库中
        /// </summary>
        /// <returns></returns>
        public string SaveData()
        {
            string result = "保存成功！";
            string TITLE = "";
            string BOTID1 = "";
            string BOTID2 = "";
            string RTID = "";
            string RT = "";
            List<RelTypeModel> Rlist = new List<RelTypeModel>();
            List<RelationModel> BOlist = new List<RelationModel>();
            RelTypeModel RelModel = new RelTypeModel();
            //调用已经在写好的方法来遍历,具体的在DLL中
            if (DataExcel != null)
            {
                if (CheckData())
                {
                    try
                    {
                        foreach (DataRow dr in ResultTable.Rows)
                        {

                            if (dr.ItemArray[0].ToString() != "" && dr.ItemArray[1].ToString() != "" && dr.ItemArray[2].ToString() != "" && dr.ItemArray[3].ToString() != "")//取对象类型的关系
                            {
                                RelTypeManager manager = new RelTypeManager();
                                RelModel = new RelTypeModel();
                                BOlist = new List<RelationModel>();
                                TITLE = dr.ItemArray[0].ToString();
                                RT = dr.ItemArray[1].ToString();
                                BOTID1 = dr.ItemArray[2].ToString();
                                BOTID2 = dr.ItemArray[3].ToString();
                                if (!manager.Exist(new RelTypeModel() { Title = TITLE, Botid1 = BOTID1, Botid2 = BOTID2 }, ref RTID))//不存在添加对象类型之间的关系，存在则忽略此关系，只添加对象实例关系
                                {
                                    RelModel.Rtid = RTID;                                  
                                    RelModel.RT = RT;
                                    RelModel.Title = TITLE;
                                    RelModel.Botid1 = BOTID1;
                                    RelModel.Botid2 = BOTID2;
                                    Rlist.Add(RelModel);
                                    manager.AddRelationModel(Rlist);
                                }
                            }
                            else//对象实例的关系
                            {
                                RelationManager Rmanage = new RelationManager();
                                RelationModel tmpbo = new RelationModel();
                                tmpbo.RelationID = System.Guid.NewGuid().ToString();
                                tmpbo.RTID = RTID;
                                tmpbo.BOId1 = dr.ItemArray[2].ToString();
                                tmpbo.BOId2 = dr.ItemArray[3].ToString();
                                Rmanage.Add(tmpbo);
                            }

                        }
                    }
                    catch
                    {
                        result = "保存失败！";
                    }
                    finally
                    {
                        DataExcel = null;
                    }
                }
                else
                {
                    result = CheckBOTError + "\r\n" + CheckBOError;
                }
            }
            else
                result = "保存失败！";
            return result;
        }
        /// <summary>
        /// 获取对象类型关系名称
        /// </summary>
        /// <returns></returns>
        public string GetRelTypeName(string RelName)
        {
            string data = "";
            if (RelName == null)
                RelName = "";
            RelTypeManager manager = new RelTypeManager();
            IList<RelTypeModel> ListRTM = manager.GetList();
            foreach (RelTypeModel rtm in ListRTM)
            {
                if (rtm.RT.Contains((RelName)))
                {
                    if (data == "")
                        data = "{id:'" + rtm.Rtid + "',text:'" + rtm.Title + "'}";
                    else
                        data = data + "," + "{id:'" + rtm.Rtid + "',text:'" + rtm.Title + "'}";
                }
            }
            if (data != "")
                data = "[" + data + "]";
            return data;
        }

        public string GetRelTypeNameByBOTID(string BOTID)
        {
            string data = "";
            RelTypeManager manager = new RelTypeManager();
            List<RelTypeModel> ListRTM = manager.GetRelTypeNameByID(BOTID);
            
            foreach (RelTypeModel rtm in ListRTM)
            {
                
                    if (data == "")
                        data = "{id:'" + rtm.Rtid + "',text:'" + rtm.Title + "'}";
                    else
                        data = data + "," + "{id:'" + rtm.Rtid + "',text:'" + rtm.Title + "'}";
               
            }
            if (data != "")
                data = "[" + data + "]";
            return data;
        }
        IList<ObjectTypeModel> list;
        public string GetBOTName()
        {
            ObjectTypeManager manager = new ObjectTypeManager();
            list = manager.GetAllObjType();
            string data = "";
            foreach (ObjectTypeModel rtm in list)
            {

                if (data == "")
                    data = "{id:'" + rtm.Botid + "',text:'" + rtm.Bot + "'}";
                else
                    data = data + "," + "{id:'" + rtm.Botid + "',text:'" + rtm.Bot + "'}";

            }
            if (data != "")
                data = "[" + data + "]";
            return data;

        }

        public ActionResult GetRelTypeNameByID()
        {
            DataTable dt = new DataTable();
            string ItemID = Request["ItemID"];
            if (!string.IsNullOrEmpty(ItemID))
            {
                RelTypeManager manager = new RelTypeManager();
                dt = manager.GetRelTable(ItemID);
            }
            return JsonNT(dt);

        }

        public bool DelBORel()
        {
            RelTypeManager manager = new RelTypeManager();
            String json = Request["data"];
            ArrayList rows = (ArrayList)Decode(json);
            string ISDelType = "BO";
            string RTID = "";
            List<string> TypeRTID = new List<string>();
            foreach (Hashtable row in rows)
            {
                if (row.Count == 5)//删除对象实例
                {
                    RTID = row["RTID"].ToString();
                    TypeRTID.Add(RTID);
                }
                else//删除类型及类型下的对象实例
                {
                    TypeRTID = new List<string>();
                    ISDelType = "BOT";
                    RTID = row["RTID"].ToString();
                    TypeRTID.Add(RTID);
                    break;
                }
            }
            return manager.DelBOTRel(TypeRTID, ISDelType);
        }

        public ActionResult DownLoadFile()
        {
            string filepaths = "../data/对象关系模板表.xlsx";
            string filePath = Server.MapPath(filepaths);
            FileInfo fileinfo = new FileInfo(filePath);
            string fileName = fileinfo.Name;
            string ss = Request.Url.AbsoluteUri.ToString();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);



            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        private static object Decode(string json)
        {
            if (String.IsNullOrEmpty(json)) return "";
            object o = JsonConvert.DeserializeObject(json);
            if (o.GetType() == typeof(String) || o.GetType() == typeof(string))
            {
                o = JsonConvert.DeserializeObject(o.ToString());
            }
            object v = toObject(o);
            return v;
        }
        private static object toObject(object o)
        {
            if (o == null) return null;

            if (o.GetType() == typeof(string))
            {
                //判断是否符合2010-09-02T10:00:00的格式
                string s = o.ToString();
                if (s.Length == 19 && s[10] == 'T' && s[4] == '-' && s[13] == ':')
                {
                    o = System.Convert.ToDateTime(o);
                }
            }
            else if (o is JObject)
            {
                JObject jo = o as JObject;

                Hashtable h = new Hashtable();

                foreach (KeyValuePair<string, JToken> entry in jo)
                {
                    h[entry.Key] = toObject(entry.Value);
                }

                o = h;
            }
            else if (o is IList)
            {

                ArrayList list = new ArrayList();
                list.AddRange((o as IList));
                int i = 0, l = list.Count;
                for (; i < l; i++)
                {
                    list[i] = toObject(list[i]);
                }
                o = list;

            }
            else if (typeof(JValue) == o.GetType())
            {
                JValue v = (JValue)o;
                o = toObject(v.Value);
            }
            else
            {
            }
            return o;
        }


    }
}
