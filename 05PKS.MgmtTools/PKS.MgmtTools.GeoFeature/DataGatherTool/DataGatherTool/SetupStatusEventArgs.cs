using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGatherTool
{
    public class SetupStatusEventArgs : EventArgs
    {
        #region 私有成员
        private string _message;
        #endregion

        #region 构造函数
        public SetupStatusEventArgs(string message)
        {
            _message = message;
        }
        #endregion

        #region 属性
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        #endregion
    }
}
