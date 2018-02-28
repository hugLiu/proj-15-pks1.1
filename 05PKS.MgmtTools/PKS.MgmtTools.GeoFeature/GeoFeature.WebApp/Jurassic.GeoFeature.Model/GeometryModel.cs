using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Jurassic.GeoFeature.Model
{
    public class GeometryModel
    {
        private string _boid;
        /// <summary>
        /// 对象ID
        /// </summary>
        public string Boid
        {
            get { return _boid; }
            set { _boid = value; }
        }

        private string _boname;
        //对象名称
        public string Boname
        {
            get { return _boname; }
            set { _boname = value; }
        }

        private string _bot;
        /// <summary>
        /// 对象类型
        /// </summary>
        public string Bot
        {
            get { return _bot; }
            set { _bot = value; }
        }

        private string _name;
        /// <summary>
        /// 形状名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _geometry;
        /// <summary>
        /// 几何对象
        /// </summary>
        public string Geometry
        {
            get { return _geometry; }
            set { _geometry = value; }
        }
        private string _sourcedb;
        /// <summary>
        /// 数据来源
        /// </summary>
        public string Sourcedb
        {
            get { return _sourcedb; }
            set { _sourcedb = value; }
        }
    }
}
