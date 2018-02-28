using Jurassic.GF.Interface;
using System.Reflection;

namespace Jurassic.GF.Server.Factory
{

    public sealed class ObjectCreate<T> where T : class
    {
        /*所使用程序集*/
        public static readonly string asseblyDAL = System.Configuration.ConfigurationManager.AppSettings["GFDAL"];

        /// <summary>
        /// 创建对象（不使用缓存：B/S使用）
        /// </summary>
        /// <param name="AssemblyPath"></param>
        /// <param name="classNamespace"></param>
        /// <returns></returns>
        private static object CreateObject(string AssemblyPath, string classNamespace)
        {
            try
            {
                object objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                return objType;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建IObjectType接口
        /// </summary>
        /// <returns></returns>
        public static IObjectType CreateIObjectType(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IObjectType)obj;
        }

        /// <summary>
        /// 创建IObjTypeProperty接口
        /// </summary>
        /// <returns></returns>
        public static IObjTypeProperty CreateIObjTypeProperty(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IObjTypeProperty)obj;
        }


        /// <summary>
        /// 创建IBO接口
        /// </summary>
        /// <returns></returns>
        public static IBO CreateIBO(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IBO)obj;
        }


        /// <summary>
        /// 创建IBOProperty接口
        /// </summary>
        /// <returns></returns>
        public static IBOProperty CreatIBOProperty(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IBOProperty)obj;
        }

        /// <summary>
        /// 创建IDataGather接口
        /// </summary>
        /// <returns></returns>
        public static IDataGather CreateIDataGather(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IDataGather)obj;
        }

        /// <summary>
        /// 创建IGeometry接口
        /// </summary>
        /// <returns></returns>
        public static IGeometry CreateIGeometry(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IGeometry)obj;
        }


        /// <summary>
        /// 创建IBO接口
        /// </summary>
        /// <returns></returns>
        public static IHisBOProperty CreateIHisBOProperty(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IHisBOProperty)obj;
        }

        /// <summary>
        /// 创建IBO接口
        /// </summary>
        /// <returns></returns>
        public static IHisGeometry CreateIHisGeometry(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IHisGeometry)obj;
        }
    }

}
