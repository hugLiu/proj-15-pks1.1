using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace GGGXParse
{
    public class XMLHelper
    {
        XmlDocument xmldoc;
        XmlNamespaceManager nsmgr;

        public XMLHelper(string xmlStr)
        {
            xmldoc = new XmlDocument();
            using (StringReader rdr = new StringReader(xmlStr))
            {
                xmldoc.Load(rdr);
                nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
                nsmgr.AddNamespace("gml", "http://www.opengis.net/gml");
                nsmgr.AddNamespace("x", "http://www.jurassic.com.cn/3gx");
            }
        }

        /// <summary>
        /// XML序列化某一类型到指定的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public static void SerializeToXml<T>(string filePath, T obj)
        {
            //try
            //{
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));

                // 强制指定命名空间，覆盖默认的命名空间。
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("gml", "http://www.opengis.net/gml");

                xs.Serialize(writer, obj, namespaces);
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// 从某一XML文件反序列化到某一类型
        /// </summary>
        /// <param name="filePath">待反序列化的XML文件名称</param>
        /// <param name="type">反序列化出的</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string filePath)
        {
            //try
            //{
            if (!System.IO.File.Exists(filePath))
                throw new ArgumentNullException(filePath + " not Exists");

            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T ret = (T)xs.Deserialize(reader);
                return ret;
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }




        /// <summary>  
        /// 序列化 对象到字符串  
        /// </summary>  
        /// <param name="obj">泛型对象</param>  
        /// <returns>序列化后的字符串</returns>  
        public static string Serialize<T>(T obj)
        {
            //try
            //{
            MemoryStream stream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));

            // 强制指定命名空间，覆盖默认的命名空间。
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "http://www.opengis.net/gml");

            xs.Serialize(stream, obj, namespaces);

            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();

            return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("序列化失败,原因:" + ex.Message);
            //}
        }


        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="xml">XML字符串</param>  
        /// <returns></returns>  
        public static object Deserialize(Type type, string xml)
        {
            //try
            //{
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
            //}
            //catch (Exception e)
            //{

            //    throw e;
            //}
        }


        #region XML文档节点查询和读取
        /**/
        /// <summary>
        /// 选择匹配XPath表达式的第一个节点XmlNode.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNode</returns>
        public XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
        {
            xmldoc = new XmlDocument();
            //try
            //{
            xmldoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmldoc.SelectSingleNode(xpath, nsmgr);
            return xmlNode;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    //throw ex; //这里可以定义你自己的异常处理
            //}
        }

        public XmlNode GetXmlNodeByXpath(string xpath)
        {
            //try
            //{
            XmlNode xmlNode = xmldoc.SelectSingleNode(xpath, nsmgr);
            return xmlNode;
            //}
            //catch (Exception ex)
            //{
            //    throw null;
            //    //throw ex; //这里可以定义你自己的异常处理
            //}
        }

        public XmlNodeList GetXmlNodeListByXpath(XmlNode node, string xpath)
        {
            //try
            //{
            XmlNodeList xmlNodeList = node.SelectNodes(xpath, nsmgr);
            return xmlNodeList;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    //throw ex; //这里可以定义你自己的异常处理
            //}
        }

        public XmlNode GetXmlNodeByXpath(XmlNode node, string xpath)
        {
            //try
            //{
            XmlNode xmlNode = node.SelectSingleNode(xpath, nsmgr);
            return xmlNode;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    //throw ex; //这里可以定义你自己的异常处理
            //}
        }

        /**/
        /// <summary>
        /// 选择匹配XPath表达式的节点列表XmlNodeList.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNodeList</returns>
        public XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
        {
            xmldoc = new XmlDocument();
            //try
            //{
            xmldoc.Load(xmlFileName); //加载XML文档
            XmlNodeList xmlNodeList = xmldoc.SelectNodes(xpath);
            return xmlNodeList;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    //throw ex; //这里可以定义你自己的异常处理
            //}
        }

        public XmlNodeList GetXmlNodeListByXpath(string xpath)
        {
            //try
            //{
            XmlNodeList nodelist = xmldoc.SelectNodes(xpath, nsmgr);
            return nodelist;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    //throw ex; //这里可以定义你自己的异常处理
            //}
        }

        /**/
        /// <summary>
        /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute. 柯乐义
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <returns>返回xmlAttributeName</returns>
        public XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)
        {
            string content = string.Empty;
            xmldoc = new XmlDocument();
            XmlAttribute xmlAttribute = null;
            //try
            //{
            xmldoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                if (xmlNode.Attributes.Count > 0)
                {
                    xmlAttribute = xmlNode.Attributes[xmlAttributeName];
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex; //这里可以定义你自己的异常处理
            //}
            return xmlAttribute;
        }
        #endregion
        #region 移除xml的命名空间
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            XElement xe = new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
            foreach (XAttribute attribute in xmlDocument.Attributes())
            {
                if (attribute.Name == "xmlns") continue;
                xe.Add(attribute);
            }
            return xe;
        }
        #endregion
    }
}
