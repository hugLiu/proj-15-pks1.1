using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class RelTypeModel
    {
        private string _rtid;
        /// <summary>
        /// 对象类型关系ID
        /// </summary>
        public string Rtid
        {
            get { return _rtid; }
            set { _rtid = value; }
        }

        private string _rt;
        /// <summary>
        /// 关系类型ID
        /// </summary>
        public string RT
        {
            get { return _rt; }
            set { _rt = value; }
        }

        private string _botid1;
        /// <summary>
        /// 类型1  
        /// </summary>
        public string Botid1
        {
            get { return _botid1; }
            set { _botid1 = value; }
        }

        private string _bot1;
        /// <summary>
        /// 对象类型1
        /// </summary>
        public string Bot1
        {
            get { return _bot1; }
            set { _bot1 = value; }
        }

        private string _botid2;
        /// <summary>
        /// 类型2
        /// </summary>
        public string Botid2
        {
            get { return _botid2; }
            set { _botid2 = value; }
        }

        private string _bot2;
        /// <summary>
        /// 对象类型2
        /// </summary>
        public string Bot2
        {
            get { return _bot2; }
            set { _bot2 = value; }
        }

        private string _title;
        /// <summary>
        /// 关系类型名称
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public override string ToString()
        {
            return _title;
        }

        private string _rtRule;

        public string RtRule
        {
            get { return _rtRule; }
            set { _rtRule = value; }
        }       
    }
}
