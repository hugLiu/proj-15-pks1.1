using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using Jurassic.Com.Tools;

namespace Jurassic.AppCenter.Config
{
    /// <summary>
    /// 保存本地UI的上下文配置
    /// </summary>
    public class UIContext : IDisposable
    {
        private object mContextObject;
        /// <summary>
        /// 上下文相关的对象(一般指窗体)
        /// </summary>
        public object ContextObject
        {
            get
            {
                return mContextObject;
            }
            set
            {
                mContextObject = value;
                CreateKey();
            }
        }


        const char SPILTER = '|'; //在保存字符中数组时用的分隔符


        //保存所有窗体上下文的配置字典
        static Dictionary<string, Dictionary<string, TypeAndValue>> ContextDict = new Dictionary<string, Dictionary<string, TypeAndValue>>();

        class TypeAndValue
        {
            public string TypeName;
            public string Value;
        }

        //用于反射属性或字段的选项
        static readonly BindingFlags bindAttr = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        string ContextKey;

        IDataSetProvider mDataSetLoader;

        /// <summary>
        /// 获取当前窗体的上下文配置字典
        /// </summary>
        Dictionary<string, TypeAndValue> CurrentContextDict
        {
            get
            {
                if (ContextKey.IsEmpty()) return null;
                if (!ContextDict.ContainsKey(ContextKey))
                {
                    ContextDict.Add(ContextKey, new Dictionary<string, TypeAndValue>());
                }
                return ContextDict[ContextKey];
            }
        }

        /// <summary>
        /// 从内存中获取指定对象名称的配置值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        TypeAndValue GetContextValue(string propertyName)
        {
            if (!CurrentContextDict.ContainsKey(propertyName))
            {
                CurrentContextDict[propertyName] = new TypeAndValue();
            }
            return CurrentContextDict[propertyName];
        }

        /// <summary>
        /// 反射获取用'.'号隔开的对象路径中的对象
        /// </summary>
        /// <param name="propertyNames">将'.'号分隔的对象路径split后的名称数组</param>
        /// <returns>反射获取的对象</returns>
        object FindObject(string[] propertyNames)
        {
            object obj = ContextObject;
            foreach (string p in propertyNames.Take(propertyNames.Length - 1))
            {
                FieldInfo fi = obj.GetType().GetField(p, bindAttr);
                if (fi != null)
                {
                    obj = fi.GetValue(obj);
                }
                else
                {
                    PropertyInfo pi = RefHelper.GetPropertyInfo(obj, p);
                    if (pi == null) return null;
                    obj = pi.GetValue(obj, null);
                }
            }
            return obj;
        }

        /// <summary>
        /// 从Profile中获取并设置本窗体的指定控件的属性值
        /// </summary>
        /// <param name="propertyName">用'.'号分隔的控件及属性名称 </param>
        public void Load(string propertyName)
        {
            Load(propertyName, null);
        }

        /// <summary>
        /// 从Profile中获取并设置本窗体的指定控件的属性值,如果为空则设置默认值
        /// </summary>
        /// <param name="propertyName">用'.'号分隔的控件及属性名称 </param>
        /// <param name="defaultValue">默认值</param>
        public void Load(string propertyName, string defaultValue)
        {
            TypeAndValue value = GetContextValue(propertyName);
            if (String.IsNullOrEmpty(value.Value)) value.Value = defaultValue;
            //  if (String.IsNullOrEmpty(value.Value)) return;
            string[] propertyNames = propertyName.Split('.');
            object obj = FindObject(propertyNames);
            if (obj == null) return;
            RefHelper.SetValue(obj, propertyNames[propertyNames.Length - 1], value.Value, RefHelper.LoadType(value.TypeName));
        }

        /// <summary>
        /// 将本对象的用户配置值保存到内存中或持久化的配置文件中
        /// </summary>
        public void Save(bool persistent = false)
        {
            if (CurrentContextDict != null)
            {
                foreach (string propertyName in CurrentContextDict.Keys.ToList())
                {
                    string[] propertyNames = propertyName.Split('.');
                    object obj = propertyNames.Length == 1 ? ContextObject : FindObject(propertyNames);
                    obj = RefHelper.GetValue(obj, propertyNames[propertyNames.Length - 1]);
                    if (obj == null)
                    {
                        CurrentContextDict.Remove(propertyName);
                    }
                    else if (obj.GetType() == typeof(String) || !obj.GetType().IsClass)
                    {
                        CurrentContextDict[propertyName] = new TypeAndValue { TypeName = obj.GetType().AssemblyQualifiedName, Value = CommOp.ToFullStr(obj) };
                    }
                    else
                    {
                        CurrentContextDict[propertyName] = new TypeAndValue { TypeName = obj.GetType().AssemblyQualifiedName, Value = JsonHelper.ToJson(obj) };
                    }
                }
            }
            if (persistent)
            {
                SaveAllToPersistent();
            }
        }

        /// <summary>
        /// 创建一个空上下文对象
        /// </summary>
        public UIContext()
            : this(null)
        {

        }

        /// <summary>
        /// 根据需要保存属性或字段信息的对象,新建一个上下文操作对象
        /// </summary>
        /// <param name="contextObject">需要保存属性或字段信息的对象，一般指窗体或控件</param>
        public UIContext(object contextObject)
            : this(contextObject, new DefaultDataSetProvider(), null)
        {

        }

        /// <summary>
        /// 根据需要保存属性或字段信息的对象,新建一个上下文操作对象
        /// </summary>
        /// <param name="contextObject">需要保存属性或字段信息的对象，一般指窗体或控件</param>
        /// <param name="key">代表对象的键值</param>
        public UIContext(object contextObject, string key)
            : this(contextObject, new DefaultDataSetProvider(), key)
        {

        }

        /// <summary>
        /// 根据需要保存属性或字段信息的对象,新建一个上下文操作对象
        /// </summary>
        /// <param name="contextObject">需要保存属性或字段信息的对象，一般指窗体或控件</param>
        /// <param name="loader">DataSet存取类</param>
        public UIContext(object contextObject, IDataSetProvider loader)
            : this(contextObject, loader, null)
        {

        }

        /// <summary>
        /// 根据需要保存属性或字段信息的对象,新建一个上下文操作对象
        /// </summary>
        /// <param name="contextObject">需要保存属性或字段信息的对象，一般指窗体或控件</param>
        /// <param name="loader">DataSet存取类</param>
        /// <param name="key">代表对象的键值</param>
        public UIContext(object contextObject, IDataSetProvider loader, string key)
        {
            ContextKey = key;
            ContextObject = contextObject;
            mDataSetLoader = loader;
            LoadAllFromPersistent();
        }

        private void CreateKey()
        {
            if (ContextObject == null) return;
            Form form = ContextObject as Form;
            UserControl ctrl = ContextObject as UserControl;

            if (form != null)
            {
                ContextKey = form.Name;
                form.FormClosed -= ProfileForm_FormClosed;
                form.FormClosed += ProfileForm_FormClosed;
            }
            else if (ctrl != null)
            {
                EventHandler eh = (s, e) =>
                {
                    Form frm = ctrl.FindForm();
                    ContextKey = frm.Name + "." + ctrl.Name;
                    frm.FormClosed -= ProfileForm_FormClosed;
                    frm.FormClosed += ProfileForm_FormClosed;
                };

                if (ctrl.IsHandleCreated)
                {
                    eh(ctrl, EventArgs.Empty);
                }
                else
                {
                    ctrl.Load += eh;
                }
            }
            else
            {
                //ContextKey = CommOp.ToStr(RefHelper.GetValue(contextObject, "Name"));
                if (ContextKey.IsEmpty())
                {
                    ContextKey = (ContextObject is String) ?
                        (string)ContextObject : ContextObject.GetType().Name;
                }
            }
        }

        void ProfileForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save(true);
        }

        #region 持久化保存


        static object synObj;
        /// <summary>
        /// 将应用程序所有上下文配置加载到内存
        /// </summary>
        void LoadAllFromPersistent()
        {
            lock (synObj = new object())
            {
                if (ContextDict.Count > 0) return;
                DataSet ds = mDataSetLoader.LoadData();
                foreach (DataTable dt in ds.Tables)
                {
                    Dictionary<string, TypeAndValue> profileDict = new Dictionary<string, TypeAndValue>();
                    if (dt.Columns.Count < 3) continue;
                    foreach (DataRow dr in dt.Rows)
                    {
                        profileDict[dr["Key"].ToString()] = new TypeAndValue { TypeName = dr["TypeName"].ToString(), Value = dr["Value"].ToString() };
                    }
                    ContextDict[dt.TableName] = profileDict;
                }
            }
        }

        /// <summary>
        /// 将配置持久化保存
        /// </summary>
        void SaveAllToPersistent()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Key");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("Value");

            foreach (var profileDict in ContextDict)
            {
                DataTable dt1 = dt.Clone();
                dt1.TableName = profileDict.Key;

                foreach (var profile in profileDict.Value)
                {
                    dt1.Rows.Add(profile.Key, profile.Value.TypeName, profile.Value.Value);
                }
                ds.Tables.Add(dt1);
            }
            mDataSetLoader.SaveData(ds);
        }
        #endregion

        /// <summary>
        /// 清除用户的所有个性化选项
        /// </summary>
        public void Clear()
        {
            ContextDict.Clear();
        }

        /// <summary>
        /// 通过上下文字符串设置指定上下文对象的属性值或字段值>
        /// 其中contextSettings参数的格式为：
        ///对象属性名（可以用.号表示层次关系）[=默认值]
        ///如果有多个，用回车或;号分隔
        /// </summary>
        public void LoadContext(string contextSettings)
        {
            if (String.IsNullOrEmpty(contextSettings)) return;
            string names = contextSettings.Replace("\r\n", ";");
            StringSpliter dp = new StringSpliter(names, ";", "=");

            foreach (var key in dp.Keys)
            {
                Load(key, dp[key]);
            }
        }

        /// <summary>
        /// 从其他对象(窗口)的获取指定属性(或字段)的值
        /// </summary>
        /// <param name="objName">其他对象(窗口)</param>
        /// <param name="propertyName">属性或字段名称</param>
        /// <returns>字符串表示的值</returns>
        public string GetOtherContextValue(string objName, string propertyName)
        {
            if (ContextDict.ContainsKey(objName) && ContextDict[objName].ContainsKey(propertyName))
            {
                return ContextDict[objName][propertyName].Value;
            }
            return "";
        }

        /// <summary>
        /// 持久化保存配置信息并清空内部数据
        /// </summary>
        public void Dispose()
        {
            if (ContextDict == null) return;
            Save(true);
            ContextDict.Clear();
            ContextDict = null;
        }
    }

    /// <summary>
    /// 用于UIContext的XML数据持久化接口
    /// </summary>
    public interface IDataSetProvider
    {
        DataSet LoadData();

        void SaveData(DataSet ds);
    }

    /// <summary>
    /// 用于UIContext的XML数据持久化接口的默认实现
    /// </summary>
    class DefaultDataSetProvider : IDataSetProvider
    {
        string userDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData");
        string dataSetXmlName;

        public DefaultDataSetProvider()
        {
            dataSetXmlName = Path.Combine(userDataDir, "UIContext.xml");
        }

        /// <summary>
        /// 在派生类中重写，以定义读取XML配置的方法
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet LoadData()
        {
            DataSet ds = new DataSet();
            if (!Directory.Exists(userDataDir))
            {
                return ds;
            }
            if (!File.Exists(dataSetXmlName))
            {
                return ds;
            }
            ds.ReadXml(dataSetXmlName);
            return ds;
        }

        /// <summary>
        /// 在派生类中重写，以定义保存XML配置的方法
        /// </summary>
        /// <param name="ds">DataSet</param>
        public void SaveData(DataSet ds)
        {
            if (!Directory.Exists(userDataDir))
            {
                Directory.CreateDirectory(userDataDir);
            }
            ds.WriteXml(dataSetXmlName);
        }
    }
}
