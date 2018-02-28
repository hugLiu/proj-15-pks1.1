using System.Configuration;

namespace PKS.SZXT.Web.Config.PageConfig
{
    public class PageSearchConfigSection:ConfigurationSection
    {
        [ConfigurationProperty("fileBasePath",IsRequired =true)]
        public string ConfigFileBasePath
        {
            get { return (string)this["fileBasePath"]; }
            set { this["fileBasePath"] = value; }
        }

        [ConfigurationProperty("",IsDefaultCollection =true)]
        [ConfigurationCollection(typeof(PageSearchConfigCollection))]
        public PageSearchConfigCollection PageSearchConfigs
        {
            get { return (PageSearchConfigCollection)base[""];}
        }
    }
}
