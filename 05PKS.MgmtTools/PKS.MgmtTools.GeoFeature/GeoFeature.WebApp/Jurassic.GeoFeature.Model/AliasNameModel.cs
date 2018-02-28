using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class AliasNameModel
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
        private string _name;
        /// <summary>
        /// 别名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _appDomain;
        /// <summary>
        /// 应用域
        /// </summary>
        public string AppDomain
        {
            get { return _appDomain; }
            set { _appDomain = value; }
        }
        private string _creatUser;
        /// <summary>
        /// 添加人
        /// </summary>
        public string CreatUser
        {
            get { return _creatUser; }
            set { _creatUser = value; }
        }

        private DateTime _uploadDate;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime UploadDate
        {
            get { return _uploadDate; }
            set { _uploadDate = value; }
        }
    }
}
