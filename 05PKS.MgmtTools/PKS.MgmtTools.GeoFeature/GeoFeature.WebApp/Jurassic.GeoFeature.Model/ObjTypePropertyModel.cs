using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Jurassic.GeoFeature.Model
{
    /// <summary>
    /// 对象类型参数
    /// </summary>
    public class ObjTypePropertyModel
    {
        private string _botid;
        /// <summary>
        /// 对象类型ID
        /// </summary>
        public string Botid
        {
            get { return _botid; }
            set { _botid = value; }
        }
        private string _botname;
        /// <summary>
        /// 对象类型名称
        /// </summary>
        public string Botname
        {
            get { return _botname; }
            set { _botname = value; }
        }

        private string _ns;
        /// <summary>
        /// 应用主题
        /// </summary>
        public string Ns
        {
            get { return _ns; }
            set { _ns = value; }
        }

        private string _md;
        /// <summary>
        /// 属性数据
        /// </summary>
        public string Md
        {
            get { return _md; }
            set
            {
                _md = value;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(Md);
                XmlNodeList P = xmldoc.SelectNodes("PropertySet/P");
                List<P> pList = new List<P>();
                for (int r = 0; r < P.Count; r++)
                {
                    P p0 = new P()
                    {
                        N = P[r].Attributes["n"] != null ? P[r].Attributes["n"].Value : null,
                        T = P[r].Attributes["t"] != null ? P[r].Attributes["t"].Value : null,
                        D = P[r].Attributes["d"] != null ? P[r].Attributes["d"].Value : null,
                        U = P[r].Attributes["u"] != null ? P[r].Attributes["u"].Value : null,
                        V = P[r].InnerText
                    };
                    pList.Add(p0);
                }
                _pl = pList;
            }
        }

        private string _isUserDefine;
        /// <summary>
        /// 是否内置
        /// </summary>
        public string IsUserDefine
        {
            get { return _isUserDefine; }
            set { _isUserDefine = value; }
        }

        private List<P> _pl;
        /// <summary>
        /// 属性数据list
        /// </summary>
        public List<P> Pl
        {
            get { return _pl; }
            set
            {
                _pl = value;

                XmlDocument myXmlDoc = new XmlDocument();
                XmlElement propertySetElement = myXmlDoc.CreateElement("PropertySet");
                propertySetElement.SetAttribute("name", _ns);
                myXmlDoc.AppendChild(propertySetElement);
                foreach (P p in _pl)
                {
                    if (p.N == "" || p.N == null) { continue; }
                    XmlElement pElement = myXmlDoc.CreateElement("P");
                    pElement.SetAttribute("n", p.N);
                    pElement.SetAttribute("t", p.T);
                    pElement.SetAttribute("d", p.D);
                    pElement.SetAttribute("u", p.U);
                    pElement.InnerText = p.V;
                    propertySetElement.AppendChild(pElement);
                }
                _md = propertySetElement.OuterXml.ToString();
            }
        }
    }
}
