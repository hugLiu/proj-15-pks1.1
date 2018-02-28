using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class PropertyModel
    {
        private string _boId;
        /// <summary>
        /// 对象ID
        /// </summary>
        public string BOId
        {
            get { return _boId; }
            set { _boId = value; }
        }
        private string _ns;
        /// <summary>
        /// 应用主题
        /// </summary>
        public string NS
        {
            get { return _ns; }
            set { _ns = value; }
        }
        private string _md;
        /// <summary>
        /// 属性信息
        /// </summary>
        public string MD
        {
            get { return _md; }
            set { _md = value; }
        }
        private string mdSource;
        /// <summary>
        /// 来源
        /// </summary>
        public string MdSource
        {
            get { return mdSource; }
            set { mdSource = value; }
        }

    }
}
