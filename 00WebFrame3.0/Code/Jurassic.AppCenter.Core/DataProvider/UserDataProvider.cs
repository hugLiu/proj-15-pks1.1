using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// AppUser的缺省的数据提供类，这个类只保证外部程序
    /// 没有提供相应的接口类时，应用程序不至于因空引用而崩溃
    /// </summary>
    class UserDataProvider : DataProvider<AppUser>
    {
    }
}
