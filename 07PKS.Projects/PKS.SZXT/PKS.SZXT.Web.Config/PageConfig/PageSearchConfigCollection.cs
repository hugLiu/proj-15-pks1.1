using System.Configuration;

namespace PKS.SZXT.Web.Config.PageConfig
{
    public class PageSearchConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PageSearchConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PageSearchConfigElement)element).FileName;
        }


        public new PageSearchConfigElement this[string key]
        {
            get { return (PageSearchConfigElement)BaseGet(key);}
        }
    }
}
