using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PKS.WITSML.Model
{
    public class WitsmlWellbore : WitsmlObject
    {
        private static String WITSML_TYPE = "wellbore";
        public string UidWell { get; set; }

        public static WitsmlWellbore Instance(XElement node)
        {
            var uidWell = node.Attribute("uidWell");
            var uid = node.Attribute("uid");
            if (uid == null || uidWell == null) { throw new ArgumentNullException(); }
            var nameSpace = node.Name.Namespace;
            var name = node.Element(nameSpace + "name");
            return new WitsmlWellbore
            {
                Uid = uid.Value.ToString(),
                UidWell = uidWell.Value.ToString(),
                Name = name == null ? "" : name.Value.ToString()
            };
        }

        public static string QueryString(string uidWell = "", string uidWellbore = "")
        {
            System.Text.StringBuilder strB = new System.Text.StringBuilder();
            strB.AppendLine("<wellbores xmlns=\"http://www.witsml.org/schemas/131\">");
            strB.AppendLine("<wellbore uidWell=\"{0}\" uid=\"{1}\">");
            strB.AppendLine("<name />");
            strB.AppendLine("</wellbore>");
            strB.AppendLine("</wellbores>");
            var str = string.Format(strB.ToString(), uidWell, uidWellbore);
            return str;
        }
    }
}
