using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class RelationModel
    {
        private string relationID;
        /// <summary>
        /// 对象关系ID
        /// </summary>
        public string RelationID
        {
            get { return relationID; }
            set { relationID = value; }
        }

        private string _boId1;
        /// <summary>
        /// 对象ID1
        /// </summary>
        public string BOId1
        {
            get { return _boId1; }
            set { _boId1 = value; }
        }

        private string name1;
        /// <summary>
        ///对象1名称
        /// </summary>
        public string Name1
        {
            get { return name1; }
            set { name1 = value; }
        }


        private string _boId2;
        /// <summary>
        /// 对象ID2
        /// </summary>
        public string BOId2
        {
            get { return _boId2; }
            set { _boId2 = value; }
        }
        private string name2;
        /// <summary>
        /// 对象2名称
        /// </summary>
        public string Name2
        {
            get { return name2; }
            set { name2 = value; }
        }


        private string _rtid;
        /// <summary>
        /// 关系类型
        /// </summary>
        public string RTID
        {
            get { return _rtid; }
            set { _rtid = value; }
        }

        private string _rt;
        /// <summary>
        /// 关系类型
        /// </summary>
        public string Rt
        {
            get { return _rt; }
            set { _rt = value; }
        }
    }
}
