using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Xml;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PKS.Utils
{
    /// <summary>通用工具</summary>
    public static class Utility
    {
        #region 常数成员
        /// <summary>GUID字符串长度</summary>
        public static readonly int GuidLength = 36;
        /// <summary>已完成任务</summary>
        public static readonly Task CompletedTask = Task.WhenAll(new Task[0]);
        #endregion

        #region 转换方法
        /// <summary>转换类型</summary>
        public static T As<T>(this object value)
            where T : class
        {
            return value as T;
        }
        /// <summary>强制转换类型</summary>
        public static T CastTo<T>(this object value)
        {
            return (T)value;
        }
        /// <summary>自动转换类型</summary>
        public static T ConvertTo<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        #endregion

        #region 逻辑方法
        /// <summary>值是否在数组中</summary>
        public static bool In<T>(this T value, params T[] values)
        {
            return values.Any(e => Equals(e, value));
        }
        /// <summary>值是否在数组中</summary>
        public static bool FastIn<T>(this T value, params T[] values)
            where T : IEquatable<T>
        {
            return values.Any(e => e.Equals(value));
        }
        #endregion

        #region 克隆方法
        /// <summary>深克隆</summary>
        public static T DeepClone<T>(this T value)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
        #endregion

        #region 分页方法
        ///// <summary>获得分页参数或其默认值</summary>
        //public static Pager GetPagerOrDefault(this Pager pager)
        //{
        //    if (pager == null) pager = new Pager();
        //    return pager;
        //}
        ///// <summary>获得分页参数或其默认值</summary>
        //public static Pager GetPagerOrDefault(this IPagerRequest request)
        //{
        //    var from = request.From.GetValueOrDefault();
        //    if (from < 0) from = 0;
        //    var size = request.Size.GetValueOrDefault();
        //    if (size < 0) size = 0;
        //    return new Pager(from, size);
        //}
        ///// <summary>
        ///// 获得合法的pager
        ///// </summary>
        ///// <param name="pager">给定pager</param>
        ///// <param name="validPager">转化之后的pager</param>
        //public static void GetValidPager(this Pager pager, out Pager validPager)
        //{
        //    var defalurPager = GetPagerOrDefault(pager);
        //    validPager = defalurPager;
        //    if (defalurPager.From < 0 || defalurPager.Size < 0)
        //        ExceptionCodes.InvalidPagerParameter
        //            .ThrowUserFriendly("分页参数异常", "from 和 size值必须大于等于0");

        //    //如果size=0，就返回所有匹配结果

        //    //else if (defalurPager.Size == 0)
        //    //{
        //    //    validPager = new Pager(pager.From, 10);
        //    //}
        //}
        #endregion

        #region 流方法
        /// <summary>生成字节数组</summary>
        public static byte[] ToByteArray(this Stream stream)
        {
            var memoryStream = stream.As<MemoryStream>();
            if (memoryStream == null)
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            return memoryStream.ToArray();
        }
        /// <summary>生成流</summary>
        public static Stream ToStream(this XmlDocument xdoc)
        {
            var memoryStream = new MemoryStream();
            xdoc.Save(memoryStream);
            return memoryStream;
        }
        #endregion

        #region 程序集方法
        /// <summary>获得程序集配置文件</summary>
        public static string GetAssemblyConfigFile(this Assembly assembly)
        {
            var dllFile = assembly.CodeBase;
            var dllUri = new Uri(dllFile);
            return dllUri.LocalPath + ".Config";
        }
        #endregion

        #region 特性方法
        /// <summary>解析描述特性值</summary>
        public static string[] ParseDescriptions<T>()
        {
            var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields.Select(e => e.GetCustomAttribute(typeof(DescriptionAttribute), false).As<DescriptionAttribute>()?.Description).ToArray();
        }
        #endregion

        #region 编码方法
        /// <summary>获得支持的编码</summary>
        public static string[] GetEncodings(bool addEmpty = false)
        {
            var names = new List<string>();
            if (addEmpty) names.Add(string.Empty);
            names.Add("gb2312");
            names.Add("GB18030");
            names.Add("utf-8");
            names.Add("utf-16");
            return names.ToArray();
        }
        #endregion
    }
}
