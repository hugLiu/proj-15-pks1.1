using Jurassic.PKS.Service.GF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GF.Server.Factory
{
    /// <summary>
    /// 创建抽象工厂
    /// </summary>
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
        /// 创建BO接口
        /// </summary>
        /// <returns></returns>
        public static IBO CreateIBO(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IBO)obj;
        }
        /// <summary>
        /// 创建GGGX接口
        /// </summary>
        /// <returns></returns>
        public static IGGGX CreateIGGGX(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (IGGGX)obj;
        }
        ///// <summary>
        ///// 创建Submission接口
        ///// </summary>
        ///// <returns></returns>
        public static ISubmission CreateISubmission(string ServerName)
        {
            string className = asseblyDAL + "." + ServerName;
            object obj = CreateObject(asseblyDAL, className);
            return (ISubmission)obj;
        }
    }
}
