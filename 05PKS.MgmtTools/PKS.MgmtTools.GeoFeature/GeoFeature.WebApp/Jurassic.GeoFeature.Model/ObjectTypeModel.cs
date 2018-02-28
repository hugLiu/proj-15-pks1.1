using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Jurassic.GeoFeature.Model
{
    public class ObjectTypeModel
    {
        private string _botid;
        /// <summary>
        /// 对象类型ID
        /// </summary>
        //[Browsable(true)]
        public string Botid
        {
            get { return _botid; }
            set { _botid = value; }
        }

        private string _pbotid;
        /// <summary>
        /// 所属对象类型ID
        /// </summary>
        public string Pbotid
        {
            get { return _pbotid; }
            set { _pbotid = value; }
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
        /// 对象类型名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _featureType;
        /// <summary>
        /// 特征类型
        /// </summary>
        public string FeatureType
        {
            get { return _featureType; }
            set { _featureType = value; }
        }
        private string _shape;
        /// <summary>
        /// 几何类型
        /// </summary>
        public string Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        private string _classId;

        public string ClassId
        {
            get { return _classId; }
            set { _classId = value; }
        }

        private string _hasGeometry;
        /// <summary>
        /// 该类型是否有坐标特征
        /// </summary>
        public string HasGeometry
        {
            get { return _hasGeometry; }
            set
            {
                switch (value)
                {
                    case "1":
                        _hasGeometry = "是";
                        break;
                    default:
                        _hasGeometry = "否";
                        break;
                }
            }
        }
        private string _isUserDefin;
        /// <summary>
        /// 是否是用户自定义类型
        /// </summary>
        public string IsUserDefin
        {
            get { return _isUserDefin; }
            set
            {
                switch (value)
                {
                    case "1":
                        _isUserDefin = "否";
                        break;
                    default:
                        _isUserDefin = "是";
                        break;
                }
            }
        }
    }
}
