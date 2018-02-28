using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Spatial;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.IO;
using Terradue.ServiceModel.Ogc.Gml311;

namespace GGGXParse
{
    public class ConvertFT
    {
        private static XMLHelper xmlHelp;

        /// <summary>
        /// 将Document转换成Feature集合
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static List<GeoFeature> ConvertToFTListByXML(string xmlStr)
        {
            List<GeoFeature> ftList = new List<GeoFeature>();
            //try
            //{

            xmlHelp = new XMLHelper(xmlStr);
            XmlNodeList nodeList = xmlHelp.GetXmlNodeListByXpath("x:FeatureCollection/x:GF");
            foreach (XmlNode node in nodeList)
            {
                ConvertToFeatureByNode(node).ForEach(e => ftList.Add(e));
            }
            //}
            //catch (Exception e)
            //{
            //    return null;
            //}
            return ftList;
        }

        /// <summary>
        /// 将XmlNode转换成Feature集合
        /// </summary>
        /// <param name="nodeGF"></param>
        /// <returns></returns>
        private static List<GeoFeature> ConvertToFeatureByNode(XmlNode nodeGF)
        {
            List<GeoFeature> ftList = new List<GeoFeature>();
            //对象
            GeoFeature ft = new GeoFeature();
            ft.BOID = null;
            ft.BOT = null;
            ft.NAME = xmlHelp.GetXmlNodeByXpath(nodeGF, "x:Title").InnerText;
            ft.FT = nodeGF.Attributes["type"].Value;

            //对象别名
            List<AliasName> aliasNameList = new List<AliasName>();
            foreach (XmlNode nodeAlisaName in xmlHelp.GetXmlNodeListByXpath(nodeGF, "x:Name"))
            {
                if (nodeAlisaName.Attributes["codeSpace"] == null) continue;
                AliasName aliasNameM = new AliasName();
                aliasNameM.BOID = ft.BOID;
                aliasNameM.NAME = nodeAlisaName.InnerText;
                aliasNameM.APPDOMAIN = nodeAlisaName.Attributes["codeSpace"].Value;
                aliasNameList.Add(aliasNameM);
            }
            ft.AliasNameList = aliasNameList;

            //对象参数
            List<Property> propertyList = new List<Property>();
            foreach (XmlNode nodePropertySet in xmlHelp.GetXmlNodeListByXpath(nodeGF, "x:PropertySets/x:PropertySet"))
            {
                if (nodePropertySet.ChildNodes.Count > 0)
                {
                    if (nodePropertySet.Attributes["name"] == null) continue;
                    Property propertyM = new Property();
                    propertyM.BOID = ft.BOID;
                    propertyM.NS = nodePropertySet.Attributes["name"].Value;
                    propertyM.MD = nodePropertySet.OuterXml;
                    propertyList.Add(propertyM);
                }
                else if (!string.IsNullOrEmpty(nodeGF.Attributes["class"].Value) && ft.FT == "井位")
                {
                    Property propertyM = new Property();
                    propertyM.BOID = ft.BOID;
                    propertyM.NS = "基础参数";
                    propertyM.MD = " <PropertySet name=\"基础数据\" codeSpace=\"http://www.Petrochina.com/IS/PCEDM\" xmlns=\"http://www.jurassic.com.cn/3gx\"><P n=\"井别\" t=\"String\" r=\"eq\">" + nodeGF.Attributes["class"].Value + "</P></PropertySet>";
                    propertyM.MdSource = "3GX数据";
                    if (!propertyList.Exists(p => p.MD == propertyM.MD))
                    {
                        propertyList.Add(propertyM);
                    }
                }
            }
            ft.PropertyList = propertyList;

            //对象坐标
            List<Geometry> geometryList = new List<Geometry>();
            foreach (XmlNode nodeShape in xmlHelp.GetXmlNodeListByXpath(nodeGF, "x:Shapes/x:Shape"))
            {
                Geometry geometryM = new Geometry();
                geometryM.BOID = ft.BOID;
                geometryM.NAME = nodeShape.Attributes["name"] != null ? nodeShape.Attributes["name"].Value : null;
                geometryM.GEOMETRY = DbGeography.FromGml(nodeShape.InnerXml).AsText();
                geometryList.Add(geometryM);
            }
            ft.GeometryList = geometryList;
            ftList.Add(ft);

            //子对象
            foreach (XmlNode node in xmlHelp.GetXmlNodeListByXpath(nodeGF, "x:SubFeatures/x:GF"))
            {
                ConvertToFeatureByNode(node).ForEach(e => ftList.Add(e));
            }
            return ftList;
        }

        /// <summary>
        /// Feature转换成GGGX数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static XmlDocument FeatureToGGGX(List<GeoFeature> list)
        {
            List<GeoFeature> ftList = list;
            //初始化一个xml实例
            XmlDocument myXmlDoc = new XmlDocument();
            XmlDeclaration declaration = myXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            myXmlDoc.AppendChild(declaration);

            XmlNamespaceManager xnm = new XmlNamespaceManager(myXmlDoc.NameTable);
            xnm.AddNamespace("gml", "http://www.opengis.net/gml");
            xnm.AddNamespace(string.Empty, "http://www.jurassic.com.cn/3gx");

            //创建xml的根节点
            XmlElement rootElement = myXmlDoc.CreateElement("", "FeatureCollection", "http://www.jurassic.com.cn/3gx");

            rootElement.SetAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");
            rootElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            rootElement.SetAttribute("name", "GGGX数据 ");//设置该节点genre属性 
            rootElement.SetAttribute("xsi:schemaLocation", "http://www.jurassic.com.cn/3gx 3GX.Data.Feature.GeoMap.xsd");
            myXmlDoc.AppendChild(rootElement);

            XmlElement crsElement = myXmlDoc.CreateElement("CRS", "http://www.jurassic.com.cn/3gx");
            crsElement.SetAttribute("codeSpace", "china");
            crsElement.InnerText = "地理坐标(经纬度)";
            rootElement.AppendChild(crsElement);

            foreach (GeoFeature ft in ftList)
            {
                XmlElement gfElement = myXmlDoc.CreateElement("GF", "http://www.jurassic.com.cn/3gx");
                gfElement.SetAttribute("class", ft.CLASS);
                gfElement.SetAttribute("id", ft.BOID);
                gfElement.SetAttribute("bot", ft.BOT);
                gfElement.SetAttribute("type", ft.FT);

                XmlElement titleElement = myXmlDoc.CreateElement("Title", "http://www.jurassic.com.cn/3gx");
                titleElement.InnerText = ft.NAME;
                gfElement.AppendChild(titleElement);
                if (ft.AliasNameList != null)
                {
                    foreach (AliasName aliasName in ft.AliasNameList)
                    {
                        XmlElement nameElement = myXmlDoc.CreateElement("Name");
                        nameElement.SetAttribute("codeSpace", aliasName.APPDOMAIN);
                        nameElement.InnerText = aliasName.NAME;
                        gfElement.AppendChild(nameElement);
                    }
                }
                XmlElement PropertySetsElement = myXmlDoc.CreateElement("PropertySets", "http://www.jurassic.com.cn/3gx");
                if (ft.PropertyList != null)
                {
                    foreach (Property property in ft.PropertyList)
                    {
                        XmlDocumentFragment propertyElement = myXmlDoc.CreateDocumentFragment();
                        propertyElement.InnerXml = property.MD;
                        PropertySetsElement.AppendChild(propertyElement);
                    }
                }
                gfElement.AppendChild(PropertySetsElement);

                XmlElement shapesElement = myXmlDoc.CreateElement("Shapes", "http://www.jurassic.com.cn/3gx");
                if (ft.GeometryList != null)
                {
                    foreach (Geometry geometry in ft.GeometryList)
                    {
                        XmlElement shapeElement = myXmlDoc.CreateElement("Shape", "http://www.jurassic.com.cn/3gx");
                        shapeElement.SetAttribute("name", geometry.NAME);

                        XmlReader reader = XmlReader.Create(new StringReader(DbGeography.FromText(geometry.GEOMETRY).AsGml()));
                        AbstractGeometryType gml = GmlHelper.Deserialize(reader);
                        StringWriter sw = new StringWriter();
                        XmlWriter xw = XmlWriter.Create(sw);
                        GmlHelper.Serialize(xw, gml);

                        XmlDocument shapeDoc = new XmlDocument();
                        shapeDoc.LoadXml(sw.ToString());

                        XmlDocumentFragment shape = myXmlDoc.CreateDocumentFragment();
                        shape.InnerXml = shapeDoc.DocumentElement.OuterXml;

                        shapeElement.AppendChild(shape);
                        shapesElement.AppendChild(shapeElement);
                    }
                }
                gfElement.AppendChild(shapesElement);

                rootElement.AppendChild(gfElement);
            }
            return myXmlDoc;
        }
    }
}
