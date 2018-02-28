using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    /// <summary>
    /// 对象的扩展，包含对象别名、坐标、参数
    /// </summary>
    public class BoExModel:BOModel
    {

        private string botName;

        public string BotName
        {
            get { return botName; }
            set { botName = value; }
        }

        private List<AliasNameModel> aliasNameList;

        public List<AliasNameModel> AliasNameList
        {
            get { return aliasNameList; }
            set { aliasNameList = value; }
        }

        private List<GeometryModel> geometryList;

        public List<GeometryModel> GeometryList
        {
            get { return geometryList; }
            set { geometryList = value; }
        }

        private List<PropertyModel> propertyList;

        public List<PropertyModel> PropertyList
        {
            get { return propertyList; }
            set { propertyList = value; }
        }
    }
}
