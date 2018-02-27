using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// DataManager的数据提供实现接口，
    /// 应用系统通过实现此接口来实现自己的基础数据存取。
    /// </summary>
    public interface IDataProvider<T> : IGetData<T>,IDataCUD<T>
    {
    }
}
