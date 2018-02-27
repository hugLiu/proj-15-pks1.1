using System.IO;
using System.Xml.Serialization;

namespace PKS.Utils
{
    /// <summary>XML工具</summary>
    public static class XmlUtil
    {
        #region 序列化方法
        /// <summary>把对象序列化为XML文件</summary>
        public static void XmlSerialize(this string xmlFile, object instance)
        {
            using (var stream = new FileStream(xmlFile, FileMode.Create, FileAccess.Write))
            {
                var serializer = new XmlSerializer(instance.GetType());
                serializer.Serialize(stream, instance);
            }
        }
        /// <summary>把XML文件反序列化为对象</summary>
        public static T XmlDeserialize<T>(this string xmlFile)
        {
            using (var stream = new FileStream(xmlFile, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }
        #endregion
    }
}
