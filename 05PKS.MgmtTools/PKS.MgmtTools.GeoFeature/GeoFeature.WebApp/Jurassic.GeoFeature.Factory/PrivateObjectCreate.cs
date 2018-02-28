using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Jurassic.GeoFeature.IDAL;

namespace Jurassic.GeoFeature.Factory
{

    /// <summary>
    /// 创建抽象工厂
    /// </summary>
    public sealed class PrivateObjectCreate<T> where T : class
    {
        /*所使用程序集*/
        public static readonly string asseblyDAL = System.Configuration.ConfigurationManager.AppSettings["DAL"];

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
        /// 创建接口
        /// </summary>
        /// <returns></returns>
        public static IInterface<T> CreateIInterface(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IInterface<T>)obj;
        }

        /// <summary>
        /// 创建对象类型接口
        /// </summary>
        /// <returns></returns>
        public static IObjectType CreateIObjectType(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IObjectType)obj;
        }

        /// <summary>
        /// 创建对象接口
        /// </summary>
        /// <returns></returns>
        public static IBO CreateIBO(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IBO)obj;
        }

        /// <summary>
        /// 创建别名接口
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static IAlisaName CreatIAlisaName(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IAlisaName)obj;
        }

        /// <summary>
        /// 创建提交审核接口
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static IPendingReview CreatIPendingReview(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IPendingReview)obj;
        }

        /// <summary>
        /// 创建类型关系接口
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static IRelType CreatIRelType(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IRelType)obj;
        }

        /// <summary>
        /// 创建对象关系接口
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static IRelation CreatIRelation(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IRelation)obj;
        }
        /// <summary>
        /// 创建对象类型属性接口
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public static IObjectTypeProperty CreatIObjectTypeProperty(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (IObjectTypeProperty)obj;
        }

        public static ITypeClass CreatITypeClass(string serverName)
        {
            string className = asseblyDAL + "." + serverName;
            object obj = CreateObject(asseblyDAL, className);
            return (ITypeClass)obj;
        }

    }
}
