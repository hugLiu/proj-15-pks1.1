using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class DBInfoModel
    {
        private string _crs;
        /// <summary>
        /// 坐标参照系统
        /// </summary>
        public string Crs
        {
            get { return _crs; }
            set { _crs = value; }
        }
        private string _csParam;
        /// <summary>
        /// 坐标系统参数
        /// </summary>
        public string CsParam
        {
            get { return _csParam; }
            set { _csParam = value; }
        }
        private string _dbserName;
        /// <summary>
        /// 数据库部署名称
        /// </summary>
        public string DbserName
        {
            get { return _dbserName; }
            set { _dbserName = value; }
        }
    }
}
