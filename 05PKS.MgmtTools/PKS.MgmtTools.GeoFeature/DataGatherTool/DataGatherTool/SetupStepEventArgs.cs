using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGatherTool
{
    public class SetupStepEventArgs : EventArgs
    {
        #region 私有成员
        private int _stepLength;
        private int _stepCount;
        #endregion

        #region 构造函数
        public SetupStepEventArgs(int length, int count)
        {
            _stepLength = length;
            _stepCount = count;
        }
        #endregion

        #region 属性
        public int StepLength
        {
            get { return _stepLength; }
            set { _stepLength = value; }
        }

        public int StepCount
        {
            get { return _stepCount; }
            set { _stepCount = value; }
        }
        #endregion
    }
}
