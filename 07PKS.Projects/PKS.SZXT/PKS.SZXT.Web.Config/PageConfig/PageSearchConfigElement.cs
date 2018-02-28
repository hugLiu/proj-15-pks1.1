using System.Configuration;

namespace PKS.SZXT.Web.Config.PageConfig
{
    public class PageSearchConfigElement:ConfigurationElement
    {
        [ConfigurationProperty("fileName",IsRequired =true,IsKey =true)]
        public string FileName
        {
            get { return (string)this["fileName"]; }
            set { this["fileName"] = value; }
        }
        [ConfigurationProperty("controllerName", IsRequired = true)]
        public string ControllerName
        {
            get { return (string)this["controllerName"]; }
            set { this["controllerName"] = value; }
        }
        [ConfigurationProperty("actionName", IsRequired = true)]
        public string ActionName
        {
            get { return (string)this["actionName"]; }
            set { this["actionName"] = value; }
        }

    }
}
