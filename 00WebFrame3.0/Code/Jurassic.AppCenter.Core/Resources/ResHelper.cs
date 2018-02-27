using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jurassic.Com.Tools;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Data;
using System.Resources;
using System.Drawing;

namespace Jurassic.AppCenter.Resources
{
    /// <summary>
    /// 资源帮助类,统一管理全局的字符串资源。
    /// </summary>
    public static class ResHelper
    {
        static readonly string mBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
        static Dictionary<string, NameValueCollection> mCultureDict = new Dictionary<string, NameValueCollection>();

        static ResHelper()
        {
            if (!Directory.Exists(mBasePath))
            {
                Directory.CreateDirectory(mBasePath);
            }
            LoadAllRes();
        }

        /// <summary>
        /// 获取资源字符串,此方法将先在CustomStrings.区域名.resx中查找，没找到在静态资源中查找，如再没有找到，则
        /// 直接返回defaultValue或name名并创建一个新的资源
        /// </summary>
        /// <param name="name">资源名</param>
        /// <param name="defaultValue">当资源文件不存在此值时的默认值</param>
        /// <returns>资源字符串</returns>
        public static string GetStr(string name, string defaultValue = null)
        {
            if (name.IsEmpty()) return name;
            if (name.Contains('+'))
            {
                string[] names = name.Split('+');
                if (defaultValue == null)
                {
                    return GetMultiStr(name.Split('+'));
                }
                string[] defaults = defaultValue.Split('+');
                return GetMultiStr(names, defaults);
            }
            return GetStrCore(name, defaultValue);
        }

        /// <summary>
        /// 格式化输出带资源的字符串
        /// </summary>
        /// <param name="format">格式化串，用+号分隔资源关键字，其余同String.Format</param>
        /// <param name="args">参数表</param>
        /// <returns>用本地资源格式化后的串</returns>
        /// <example>
        /// <code>
        /// var str = ResHelper.Format("User+'{0}'+Info+Update+Successed+!", "wangjiaxin");
        /// </code>
        /// </example>
        public static string Format(string format, params object[] args)
        {
            string[] fmts = format.Split('+');
            var coll = GetCollection(CurrentCultureName);

            for (int i = 0; i < fmts.Length; i++)
            {
                if (coll[fmts[i]] != null)
                {
                    fmts[i] = coll[fmts[i]];
                }
            }

            format = String.Join(JStr.WordSpc, fmts);

            return String.Format(format, args);
        }

        private static string GetStrCore(string name, string defaultValue = null)
        {
            if (name.IsEmpty()) throw new ArgumentNullException("name");
            var coll = GetCollection(CurrentCultureName);
            var str = coll[name];
            if (str.IsEmpty())
            {
                str = defaultValue ?? name;
                coll[name] = defaultValue ?? "";
            }
            if (str == "-")
            {
                str = "";
            }
            return str;
        }

        private static string GetMultiStr(string[] names)
        {

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = GetStrCore(names[i]);
            }

            return string.Join(JStr.WordSpc, names);
        }

        private static string GetMultiStr(string[] names, string[] defaults)
        {
            if (defaults.Length < names.Length)
            {
                throw new ArgumentException("default.Length less than names.Length");
            }

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = GetStrCore(names[i], defaults[i]);
            }

            return string.Join(JStr.WordSpc, names);
        }

        public static string CurrentCultureName
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.Name;
            }
            set
            {
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture
                    = new CultureInfo(value);
            }
        }

        static string GetResxFileName(string culture)
        {
            string resxFile = Path.Combine(mBasePath, String.Format("CustomStrings.{0}.resx", culture));
            return resxFile;
        }

        /// <summary>
        /// 保存所有资源，包括没有用过的，可供用户再次编辑
        /// </summary>
        public static void SaveAllRes()
        {
            var allKeys = GetAllResourceKeys();
            foreach (string culture in mCultureDict.Keys)
            {
                ResXResourceWriter rw = new ResXResourceWriter(GetResxFileName(culture));
                foreach (string key in allKeys)
                {
                    rw.AddResource(key, mCultureDict[culture][key] ?? "");
                }
                rw.Generate();
                rw.Close();
            }
        }

        public static void AddKey(string key)
        {
            foreach (string culture in mCultureDict.Keys)
            {
                mCultureDict[culture][key] = null;
            }
        }

        /// <summary>
        /// 获取所有现有资源的名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllResourceKeys()
        {
            List<string> allKeys = new List<string>();
            foreach (string culture in mCultureDict.Keys)
            {
                foreach (string key in mCultureDict[culture].Keys)
                {
                    if (!allKeys.Contains(key)) allKeys.Add(key);
                }
            }
            return allKeys;
        }

        /// <summary>
        /// 获取已使用的语言名称清单
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUsedCultureNames()
        {
            if (mCultureDict.Count == 0)
            {
                mCultureDict.Add("zh-CN", new NameValueCollection());
            }
            return mCultureDict.Keys.ToList();
        }

        /// <summary>
        /// 清除所有资源,所有资源文件变为空文件
        /// </summary>
        public static void ClearAllRes()
        {
            LoadAllRes();
            foreach (string culture in mCultureDict.Keys)
            {
                mCultureDict[culture].Clear();
            }
            SaveAllRes();
        }

        /// <summary>
        /// 一次性加载所有语言的资源
        /// </summary>
        static void LoadAllRes()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                string resxFile = GetResxFileName(culture.Name);
                if (File.Exists(resxFile))
                {
                    ResXResourceReader reader = new ResXResourceReader(resxFile);
                    var coll = new NameValueCollection();
                    foreach (DictionaryEntry d in reader)
                    {
                        coll[CommOp.ToStr(d.Key)] = CommOp.ToStr(d.Value);
                    }
                    mCultureDict[culture.Name] = coll;
                }
            }
        }

        /// <summary>
        /// 初始化某类型的所有静态属性为多语言的Key
        /// </summary>
        /// <param name="type">类型</param>
        public static void InitStartupStr(Type type)
        {
            object obj = Activator.CreateInstance(type);
            var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach(var prop in props)
            {
                prop.GetValue(obj);
            }
        }

        /// <summary>
        /// 从g.resources命名空间中获取流，它通常是生成类型为"Resource"时生成的资源
        /// </summary>
        /// <param name="resNamespace">要保证命名空间和程序集名称相同</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetGResources(Assembly ass, string key)
        {
            ResourceReader breader = GetGResourcesReader(ass);
            if (breader == null) return null;
            object r = null;
            breader.Each(k =>
            {
                DictionaryEntry de = (DictionaryEntry)k;
                if (key.ToLower() == de.Key.ToString().ToLower())
                {
                    r = ((DictionaryEntry)k).Value;
                    return false;
                }
                return true;
            });
            breader.Close();
            return r;
        }

        /// <summary>
        /// 获取一个根据g.resources得到的资源读取器
        /// </summary>
        /// <param name="resNamespace"></param>
        /// <returns></returns>
        public static ResourceReader GetGResourcesReader(Assembly ass)
        {
            string resName = ass.FullName.Split(',')[0] + ".g.resources";
            Stream stream = ass.GetManifestResourceStream(resName);
            if (stream == null) return null;

            ResourceReader breader = new ResourceReader(stream);
            return breader;
        }

        /// <summary>
        /// 合并所有来自其他程序集的资源到主资源，前提是资源文件在对应项目的App_Data文件夹下
        /// </summary>
        /// <param name="resNamespaces">资源所在的命名空间名称列表</param>
        /// <param name="overwriteExists">是否覆盖已经定义的资源</param>
        public static void CombinAssemblyResx(IEnumerable<Assembly> assemblys, bool overwriteExists = false)
        {
            foreach (var ns in assemblys)
            {
                foreach (string cultureName in GetUsedCultureNames())
                {
                    string resName = "app_data/customstrings." + cultureName.ToLower() + ".resx";
                    Stream stream = GetGResources(ns, resName) as Stream;
                    if (stream == null) continue;
                    ResXResourceReader reader = new ResXResourceReader(stream);
                    var coll = GetCollection(cultureName);
                    foreach (DictionaryEntry d in reader)
                    {
                        if ((coll[CommOp.ToStr(d.Key)].IsEmpty() || overwriteExists)
                            && !CommOp.ToStr(d.Value).IsEmpty())
                        {
                            coll[CommOp.ToStr(d.Key)] = CommOp.ToStr(d.Value);
                        }
                    }
                    reader.Close();
                }
            }
        }

        static NameValueCollection GetCollection(string cultureName)
        {
            //如果资源文件里没有带区域的语言文件，则使用不带区域的语言文件
            //如果都找不到，则生成一个带区域的语言文件
            //比如 传过来en-US, 如果找到en-US则用它，否则用en，再找不到，则生成空的en-US
            string key = cultureName.Split('-')[0];
            if (mCultureDict.ContainsKey(cultureName))
            {
                key = cultureName;
            }

            if (!mCultureDict.ContainsKey(key))
            {
                key = cultureName;
                var coll = new NameValueCollection();
                mCultureDict[key] = coll;
            }
            return mCultureDict[key];
        }

        /// <summary>
        /// 将所有语言的资源组成一个DataTable
        /// DataTable第一列为Key,后面每一列代表一个地区的资源
        /// </summary>
        /// <returns>包含资源的DataTable</returns>
        public static DataTable MakeDataTable()
        {
            DataTable dt = new DataTable("ResourceTable");
            var allKeys = GetAllResourceKeys();
            dt.Columns.Add("Key", typeof(String));
            foreach (string culture in mCultureDict.Keys)
            {
                dt.Columns.Add(culture, typeof(String));
                var coll = GetCollection(culture);

                foreach (string key in allKeys)
                {
                    if (mCultureDict[culture][key] == null) mCultureDict[culture][key] = "";
                }
            }
            allKeys.Sort();
            foreach (string key in allKeys)
            {
                var dr = dt.NewRow();
                dr["Key"] = key;
                foreach (string culture in mCultureDict.Keys)
                {
                    dr[culture] = mCultureDict[culture][key];
                }
                dt.Rows.Add(dr);
                
            }
           
            return dt;
        }

        /// <summary>
        /// 保存DataTable中的数据到所有语言的资源文件
        /// </summary>
        /// <param name="dt">存有资源的DataTable</param>
        public static void SaveDataTable(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.Ordinal == 0) continue;
                var coll = GetCollection(dc.ColumnName);
                coll.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    coll[(string)dr[0]] = CommOp.ToStr(dr[dc]);
                }
            }
            SaveAllRes();
        }

        /// <summary>
        /// 将当前的区域信息所对应的资源序列化成js对象供前台js调用
        /// 本方法将自动按照当前线程的区域特性返回对应的资源对象。
        /// </summary>
        /// <param name="varName">生成的Js的对象名</param>
        /// <returns>Js的一个对象语句</returns>
        public static string GetAllResStrJs(string varName)
        {
            List<string> defProps = new List<string>();
            var coll = GetCollection(CurrentCultureName);
            foreach (string key in coll.AllKeys)
            {
                if (key.Contains("-")) continue;
                defProps.Add(String.Format("\"{0}\":\"{1}\"\r\n", key, coll[key]));
            }

            return String.Format("var {0}={{{1}}};", varName, String.Join(",", defProps));
        }


        public static void RemoveCulture(string culture)
        {
            mCultureDict.Remove(culture);
            string resxFile = GetResxFileName(culture);
            if (File.Exists(resxFile))
            {
                File.Delete(resxFile);
            }
        }

        /// <summary>
        /// 获取程序集嵌入的资源流
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static Stream GetAssemblyResourceStream(Assembly assembly, string resName)
        {
            System.IO.Stream stream = assembly.GetManifestResourceStream(resName);//namespace:WindowsApplication1,folder.6.jpg相对路径+文件名+扩展名
            return stream;
        }

        public static string GetContentTypeByExt(string fileName)
        {
            string ext = new FileInfo(fileName).Extension;
            switch (ext)
            {
                case ".css": return "text/css";
                case ".js": return "application/x-javascript";
                case ".json": return "application/x-json";
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".txt": return "plain/text";
                case ".htm":
                case ".html": return "text/html";
                case ".xml": return "text/xml";
                case ".ico": return "image/x-icon";
                default: return "application/octet-stream";
            }
        }
    }
}