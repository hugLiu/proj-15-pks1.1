using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class BOModel
    {
        private string _boid;
        /// <summary>
        ///对象ID
        /// </summary>
        public string Boid
        {
            get { return _boid; }
            set { _boid = value; }
        }

        private string _bopid;
        /// <summary>
        /// 对象PID
        /// </summary>
        public string Bopid
        {
            get { return _bopid; }
            set { _bopid = value; }
        }
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _botid;
        /// <summary>
        /// 对象类型
        /// </summary>
        public string Botid
        {
            get { return _botid; }
            set { _botid = value; }
        }

        private string _boc;
        /// <summary>
        /// 类别
        /// </summary>
        public string Boc
        {
            get { return _boc; }
            set { _boc = value; }
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

        private string _gatheruser;
        /// <summary>
        /// 采集人
        /// </summary>
        public string Gatheruser
        {
            get { return _gatheruser; }
            set { _gatheruser = value; }
        }
        private DateTime _gatherdate;
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime Gatherdate
        {
            get { return _gatherdate; }
            set { _gatherdate = value; }
        }

        private string _isuse;
        /// <summary>
        /// 是否在用
        /// </summary>
        public string Isuse
        {
            get { return _isuse; }
            set { _isuse = value; }
        }

        private decimal _distance;

        public decimal Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        private string _bot;

        public string Bot
        {
            get { return _bot; }
            set { _bot = value; }
        }

    }
}
