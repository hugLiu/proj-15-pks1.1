using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.Model
{
    public class PendingReviewModel
    {
        private string _id;
        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _title;
        /// <summary>
        /// 名字
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _content;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        private string _ischecked;
        /// <summary>
        /// 是否已经处置
        /// </summary>
        public string Ischecked
        {
            get { return _ischecked; }
            set { _ischecked = value; }
        }
        private string _checkedUser;
        /// <summary>
        /// 处置人
        /// </summary>
        public string CheckedUser
        {
            get { return _checkedUser; }
            set { _checkedUser = value; }
        }
        private DateTime _uploadDate;
        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate
        {
            get { return _uploadDate; }
            set { _uploadDate = value; }
        }
        private DateTime _checkedDate;
        /// <summary>
        /// 处置日期
        /// </summary>
        public DateTime CheckedDate
        {
            get { return _checkedDate; }
            set { _checkedDate = value; }
        }
        private string _uploadUser;
        /// <summary>
        /// 上传人
        /// </summary>
        public string UploadUser
        {
            get { return _uploadUser; }
            set { _uploadUser = value; }
        }

    }
}
