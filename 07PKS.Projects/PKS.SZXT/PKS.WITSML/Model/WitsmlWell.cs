using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PKS.WITSML.Model
{
    public class WitsmlWell : WitsmlObject
    {
        private static String WITSML_TYPE = "well";
        public static WitsmlWell Instance(XElement node)
        {
            var id = node.Attribute("uid");
            if (id == null) { throw new ArgumentNullException(); }
            var nameSpace = node.Name.Namespace;
            var name = node.Element(nameSpace + "name");
            return new WitsmlWell
            {
                Uid = id.Value.ToString(),
                Name = name == null ? "" : name.Value.ToString()
            };
        }

        public static string QueryString(string uidWell = "")
        {
            System.Text.StringBuilder strB = new System.Text.StringBuilder();
            strB.AppendLine("<wells xmlns=\"http://www.witsml.org/schemas/140ex\">");
            strB.AppendLine("<well uid=\"{0}\">");
            strB.AppendLine("<name/>");
            strB.AppendLine("</well>");
            strB.AppendLine("</wells>");
            var str = string.Format(strB.ToString(), uidWell);
            return str;
        }
    }
}
