using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebFrame;
//using Jurassic.GeoFeature.;
using Jurassic.GeoFeature.BLL;
using Jurassic.GeoFeature.Model;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GeoFeature.Controllers
{
    public class BOManageController : BaseController
    {
        //
        // GET: /BOManage/
        IList<ObjectTypeModel> list;
        List<BOModel> BOList;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BOEdit()
        {
            return View();
        }

        public ActionResult BOParaEdit()
        {
            return View();
        }

        public ActionResult BONameEdit()
        {
            return View();
        }

        public ActionResult BOBMEdit()
        {
            return View();
        }

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

        public string GetBoListByBOTID()
        {
            string data = "";
            BOList = new List<BOModel>();
            BOManager manager = new BOManager();
            string BOTID = Request["BOTID"];
            string BONAME = Request["BONAME"].Trim();

            BOList = manager.GetBoListByBOTID(BOTID);
            foreach (BOModel rtm in BOList)
            {
                if (rtm.Name.Contains(BONAME))
                {
                    if (data == "")
                        data = "{id:'" + rtm.Boid + "',text:'" + rtm.Name + "',BOC:'" + rtm.Boc + "'}";
                    else
                        data = data + "," + "{id:'" + rtm.Boid + "',text:'" + rtm.Name + "',BOC:'" + rtm.Boc + "'}";
                }

            }
            if (data != "")
                data = "[" + data + "]";

            return data;
        }

        public ActionResult GetBoListByID()
        {
            BOManager manager = new BOManager();
            string BOID = Request["BOID"];
            return JsonNT(manager.GetBoListByID(BOID));
        }
        /// <summary>
        /// 得到当前要编辑对象的参数（对象ID和应用域）
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> valueDomain = new Dictionary<string, string>();
        public string GetEditParaInfo()
        {
            string Boid = Request["BOID"];
            string NS = Request["NS"].Trim();
            string DataType = "";
            string ParaName = "";
            string editor = "";
            string data = "";
            string ParaValue = "";
         
            XmlDataDocument document = new XmlDataDocument();
            PropertyManager manager = new PropertyManager();
            IList<PropertyModel> propertyList = manager.GetListByID(Boid);

            valueDomain = new Dictionary<string, string>();
            foreach (PropertyModel prop in propertyList)
            {
                editor = "";
                if (prop.NS == NS)
                {
                    document.LoadXml(prop.MdSource);//从对象参数值域中得到参数类型
                    XmlNodeList datastore = document.SelectNodes("PropertySet");

                    foreach (XmlNode node in datastore)
                    {
                        XmlNodeList column = node.ChildNodes;
                        foreach (XmlNode nd in column)
                        {
                            DataType = nd.Attributes["t"].Value;
                            ParaName = nd.Attributes["n"].Value;
                            if (DataType == "String")
                                editor = "textbox";
                            else if (DataType == "Decimal")
                            {
                                editor = "spinner";
                            }
                            else
                                editor = "datepicker', format: 'yyyy-MM-dd ";

                            if (nd.InnerText.Length != 0)
                            {
                                editor = "selectvalue";
                            }
                            if (!valueDomain.Keys.Contains(ParaName))//给各个参数赋值类型
                                valueDomain.Add(ParaName, editor);
                        }
                    }
                    document = new XmlDataDocument();
                    document.LoadXml(prop.MD);
                    datastore = document.SelectNodes("PropertySet");
                  
                    foreach (XmlNode node in datastore)
                    {
                        XmlNodeList column = node.ChildNodes;
                        foreach (XmlNode nd in column)
                        {
                            ParaValue = "";
                            if (nd.FirstChild != null)
                                ParaValue = nd.FirstChild.Value;
                         
                            if (data == "")
                                data = "{id:'1', name: '" + nd.Attributes["n"].Value + "', value: '" + ParaValue + "', editor: '" + GetParaType(nd.Attributes["n"].Value) + "'}";
                            else
                                data = data + "," + "{id:'1', name: '" + nd.Attributes["n"].Value + "', value: '" + ParaValue + "', editor: '" + GetParaType(nd.Attributes["n"].Value) + "'}";
                            
                        }
                        if (data != "")
                            data = @"[" + data + "]";
                       
                    }
                    break;
                }
            }
            return data;
        }
        /// <summary>
        /// 得到参数类型
        /// </summary>
        /// <param name="ParaName"></param>
        /// <returns></returns>
        private string GetParaType(string ParaName)
        {
            if (valueDomain.Keys.Contains(ParaName))
            {
                return valueDomain[ParaName];
            }
            else
                return "";
        }

        public bool SaveBOPara()
        {
            String json = Request["data"];
            string BOID = Request["BOID"];
            string Source = "";
            string NS = Request["NS"].Trim();

            BOManager manager = new BOManager();
            List<string> SqlList = new List<string>();
            ArrayList rows = (ArrayList)Decode(json);
            StringBuilder MD = new StringBuilder();
            MD.Append(" <PropertySet" + " name=" + '"' + NS + '"' + "> ");
            foreach (Hashtable row in rows)
            {
                String id = row["id"] != null ? row["id"].ToString() : "";
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";
                string n = row["name"].ToString();
                string v = row["value"].ToString();
                string t = row["editor"].ToString();
                if (t == "textbox")
                    t = "String";
                else if (t == "spinner")
                {
                    t = "Decimal";
                }
                else if (t == "selectvalue")
                {
                    t = "selectvalue";
                }
                else
                {
                    t = "date";
                }
                MD.Append(" <P ");
                MD.Append(" n=" + '"' + n + '"' + " t=" + '"' + t + '"' + ">" + v + "");
                MD.Append(" </P>  ");
            }
            MD.Append(" </PropertySet > ");
            PropertyManager PropManager = new PropertyManager();
            PropertyModel PropModel = new PropertyModel();
            PropModel.MD = MD.ToString();
            PropModel.BOId = BOID;
            PropModel.NS = NS;
            PropModel.MdSource = Source;

            if (PropManager.Update(PropModel) == 1)
                return true;
            else
                return false;
        }
        public ActionResult GetBOParaByID()
        {
            XmlDocument document = new XmlDocument();
            PropertyManager manager = new PropertyManager();
            string BOID = Request["BOID"];
            string NS = Request["NS"];
            //分页
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string ParaValue = "";
            IList<PropertyModel> list = manager.GetListByID(BOID);
            List<ParaInfo> Pinfo = new List<ParaInfo>();
            foreach (PropertyModel tmpList in list)
            {
                if (tmpList.NS == NS)
                {
                    document.LoadXml(tmpList.MD);
                    XmlNodeList datastore = document.SelectNodes("PropertySet");
                    foreach (XmlNode node in datastore)
                    {
                        XmlNodeList column = node.ChildNodes;
                        foreach (XmlNode nd in column)
                        {
                            ParaInfo TmpPara = new ParaInfo();
                            if (nd.FirstChild != null)
                                ParaValue = nd.FirstChild.Value;
                            TmpPara.NS = tmpList.NS;
                            TmpPara.Name = nd.Attributes["n"].Value;
                            TmpPara.Value = ParaValue;
                            Pinfo.Add(TmpPara);
                        }
                    }
                }
            }
            string iTotal = Pinfo.Count.ToString();
            Pinfo = Pinfo.Skip(pageSize * (pageIndex)).Take(pageSize).ToList();
            GridJsonEntity jsonObj = new GridJsonEntity();
            jsonObj.total = iTotal;
            jsonObj.data = Pinfo;
            return JsonNT(jsonObj);
        }

        public ActionResult GetALIASNAME()
        {
            BOManager manager = new BOManager();
            string BOID = Request["BOID"];

            DataTable Dt = manager.GetALIASNAME(BOID);
            return JsonNT(Dt);
        }

        public string DelAliasNameByID()
        {
            AliasNameManager manager = new AliasNameManager();
            AliasNameModel model = new AliasNameModel();
            try
            {
                model.BOId = Request["BOID"];
                model.AppDomain = Request["AppDomain"];
                manager.Delete(model);
                return "OK";
            }
            catch
            {
                return "false";
            }
        }

        public string AddAliasName()
        {
            AliasNameManager manager = new AliasNameManager();
            AliasNameModel model = new AliasNameModel();
            model.BOId = Request["BOID"];
            model.Name = Request["NAME"];
            model.AppDomain = Request["APPDOMAIN"];
            if (manager.Exist(model))
            {
                return "NO";
            }
            else
            {
                try
                {
                    manager.Add(model);
                    return "OK";
                }
                catch
                {
                    return "false";
                }
            }
        }

        public string UpdateAliasName()
        {
            AliasNameManager manager = new AliasNameManager();
            AliasNameModel model = new AliasNameModel();
            model.BOId = Request["BOID"];
            model.Name = Request["NAME"];
            model.AppDomain = Request["APPDOMAIN"];
            model.CreatUser = Request["CREATUSER"];
            model.UploadDate = System.DateTime.Parse(Request["UPLOADDATE"]);
            if (manager.Exist(model))
            {
                return "NO";
            }
            else
            {
                try
                {
                    manager.Update(model);
                    return "OK";
                }
                catch
                {
                    return "false";
                }
            }
        }

        public void DelBOByID()
        {
            BOManager manager = new BOManager();
            BOModel bo = new BOModel();
            string BOID = Request["BOID"];
            bo.Boid = BOID;
            int i = manager.Delete(bo);
        }

        public string GetOBTNS()
        {
            string data = "";
            ObjTypePropertyManager manager = new ObjTypePropertyManager();
            string BOID = Request["BOID"];

            IList<ObjTypePropertyModel> list = manager.GetObjTypePropertyBoid(BOID);
            foreach (ObjTypePropertyModel rtm in list)
            {

                if (data == "")
                    data = "{id:'" + rtm.Ns + "',text:'" + rtm.Ns + "'}";
                else
                    data = data + "," + "{id:'" + rtm.Ns + "',text:'" + rtm.Ns + "'}";

            }
            if (data != "")
                data = "[" + data + "]";
            return data;
        }
        public string GetOBJNS()
        {
            string data = "";
            ObjTypePropertyManager manager = new ObjTypePropertyManager();
            string BOTID = Request["BOTID"];
            IList<ObjTypePropertyModel> list = manager.GetObjTypePropertyBoid(BOTID);
            foreach (ObjTypePropertyModel rtm in list)
            {

                if (data == "")
                    data = "{id:'" + rtm.Ns + "',text:'" + rtm.Ns + "'}";
                else
                    data = data + "," + "{id:'" + rtm.Ns + "',text:'" + rtm.Ns + "'}";

            }
            if (data != "")
                data = "[" + data + "]";
            return data;
        }

        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public string GetParaInfo()
        {
            string data = "";
            string DataType = "";
            string editor = "";
            string Value = "";
            string BOTID = Request["BOTID"];
            string NS = Request["NS"].Trim();
            ObjTypePropertyManager manager = new ObjTypePropertyManager();
            XmlDocument document = new XmlDocument();

            List<ObjTypePropertyModel> list = manager.GetOBJTypePropByName(BOTID, NS);

            document.LoadXml(list[0].Md);


            XmlNodeList datastore = document.SelectNodes("PropertySet");
            foreach (XmlNode node in datastore)
            {
                XmlNodeList column = node.ChildNodes;
                foreach (XmlNode nd in column)
                {
                    DataType = nd.Attributes["t"].Value;
                    if (DataType == "String")
                        editor = "textbox";
                    else if (DataType == "Decimal")
                    {
                        editor = "spinner";
                    }
                    else
                        editor = "datepicker', format: 'yyyy-MM-dd ";

                    if (nd.InnerText.Length != 0)
                    {
                        editor = "selectvalue";
                    }
                    else
                    {
                        Value = "";
                    }
                
                    if (data == "")
                        data = "{id:'1', name: '" + nd.Attributes["n"].Value + "', value: '" + Value + "', editor: '" + editor + "'}";
                    else
                        data = data + "," + "{id:'1', name: '" + nd.Attributes["n"].Value + "', value: '" + Value + "', editor: '" + editor + "'}";
                }
                if (data != "")
                    data = @"[" + data + "]";
            }
            return data;
        }
        public string GetParaValue()
        {
            string BOTID = Request["BOTID"];
            string NS = Request["NS"].Trim();
            string ParaName = Request["ParaName"].Trim();
            XmlDocument document = new XmlDocument();
            string name="";
            string Value = "";
            string data = "";
            ObjTypePropertyManager manager = new ObjTypePropertyManager();           
            List<ObjTypePropertyModel> list = manager.GetOBJTypePropByName(BOTID, NS);

            document.LoadXml(list[0].Md);
            XmlNodeList datastore = document.SelectNodes("PropertySet");
            foreach (XmlNode node in datastore)
            {
                XmlNodeList column = node.ChildNodes;
                foreach (XmlNode nd in column)
                {
                    name = nd.Attributes["n"].Value;
                    if (name == ParaName)
                    {
                        if (nd.InnerText.Length != 0)
                        {
                            string[] TmpPara = nd.InnerText.Replace("，",",").Split(',');
                            foreach (string para in TmpPara)
                            {
                                if (data == "")
                                    data = "{id:'" + para + "',text:'" + para + "'}";
                                else
                                    data = data + "," + "{id:'" + para + "',text:'" + para + "'}";
                            }                            
                        }
                        break;
                    }
                }
                if (data != "")
                    data = "[" + data + "]";
            }
            return data;
        }
        public string GetParaToShow()
        {
            string data = "";
            string BOID = Request["BOID"];
            string NS = Request["NS"].Trim();            
            XmlDocument document = new XmlDocument();
            PropertyManager manager = new PropertyManager();
            string ParaValue = "";
            IList<PropertyModel> list = manager.GetListByID(BOID);
            foreach (PropertyModel TmpList in list)
            {
                if (TmpList.NS == NS)
                {
                    document.LoadXml(TmpList.MD);
                    XmlNodeList datastore = document.SelectNodes("PropertySet");
                    foreach (XmlNode node in datastore)
                    {
                        XmlNodeList column = node.ChildNodes;
                        foreach (XmlNode nd in column)
                        {
                            ParaValue = "";
                            if (nd.FirstChild != null)
                                ParaValue = nd.FirstChild.Value;
                            if (data == "")
                                data = "{ns:'" + TmpList.NS + "',name:'" + nd.Attributes["n"].Value + "',value:'" + ParaValue + "'}";
                            else
                                data = data + "," + "{ns:'" + TmpList.NS + "',name:'" + nd.Attributes["n"].Value + "',value:'" + ParaValue + "'}";
                        }

                    }
                    if (data != "")
                    {
                        data = "[" + data + "]";
                    }
                    break;
                }
            }

            return (data);

        }

        public string UpdatBOBYID()
        {
            BOManager manager = new BOManager();
            BOModel model = new BOModel();

            model.Boid = Request["BOID"]; ;
            model.Name = Request["BONAME"]; ;
            model.Botid = Request["BOTID"];
            if (Request["ISUSE"] == "是")
                model.Isuse = "1";
            else
                model.Isuse = "0";
            string Result = "OK";
            List<BOModel> BoList = manager.GetBoListByName(Request["BONAME"], Request["BOTID"]);
            if (BoList.Count() > 0)//对象名称已经存在！
            {
                Result = "NO";
            }
            else
            {
                if (manager.Update(model) == 1)
                    Result = "OK";
                else
                    Result = "failse";
            }
            return Result;
        }
        public string SaveObjInfo()
        {
            String jsons = Request["data"];
            string BOname = Request["BOname"].Trim();
            string nameBM = Request["nameBM"].Trim();
            string AppDomains = Request["AppDomain"];
            
            string BOTID = Request["BOTID"];
            string BOID = System.Guid.NewGuid().ToString();
            BOManager manager = new BOManager();
            List<string> SqlList = new List<string>();
            string Result = "OK";
            List<BOModel> BoList = manager.GetBoListByName(BOname, BOTID);
            if (BoList.Count() > 0)//对象名称已经存在！
            {
                Result = "NO";
            }
            else
            {
                string Sql = "insert into bo( BOID ,  NAME ,  BOTID,  ISUSE ) " +
                                 " values('" + BOID + "','" + BOname + "','" + BOTID + "','1')";
                SqlList.Add(Sql);
                string[] sjson = jsons.Split('|');
                foreach (string json in sjson)
                {
                    ArrayList rows = (ArrayList)Decode(json);                    
                    foreach (Hashtable row in rows)
                    {
                        foreach (DictionaryEntry de in row) //ht为一个Hashtable实例
                        {
                            StringBuilder Property = new StringBuilder();
                            Property.Append(" <PropertySet" + " name=" + '"' + de.Key + '"' + "> ");
                            foreach (Hashtable tde in (ArrayList)de.Value)
                            {
                                string n = tde["name"].ToString();
                                string v = tde["value"].ToString();
                                string t = tde["editor"].ToString();
                                if (t == "textbox")
                                    t = "String";
                                else if (t == "spinner")
                                {
                                    t = "Decimal";
                                }
                                else if (t=="selectvalue")
                                {
                                    t = "String";
                                }
                                else
                                {
                                    t = "date";
                                }
                                Property.Append(" <P ");
                                Property.Append(" n=" + '"' + n + '"' + " t=" + '"' + t + '"' + ">" + v + "");
                                Property.Append(" </P>  ");
                            }
                            Property.Append(" </PropertySet > ");
                            Sql = "insert into property(boid, ns,md) values('" + BOID + "','" + de.Key + "','" + Property.ToString() + "') ";
                            SqlList.Add(Sql);
                        }
                    }  
                }
                Sql = "insert into aliasname( BOID,  NAME ,  APPDOMAIN) " +
                          " values('" + BOID + "','" + nameBM + "','" + AppDomains + "')";
                SqlList.Add(Sql);
                bool b = manager.InsertBOandPara(SqlList);
                if (b)
                    Result = "OK";
                else
                    Result = "Fails";
            }
            return Result;
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

    public class ParaInfo
    {
        string _ns;
        string _name;
        string _value;

        public string NS
        {
            set { _ns = value; }
            get { return _ns; }
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public string Value
        {
            set { _value = value; }
            get { return _value; }
        }
    }
    public class GridJsonEntity
    {
        public string total { get; set; }
        public object data { get; set; }

    }
}
