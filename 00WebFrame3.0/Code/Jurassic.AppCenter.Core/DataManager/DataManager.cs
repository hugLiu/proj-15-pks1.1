using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 统一的数据管理类，提供对类型T数据列表的增删改。
    /// </summary>
    public class DataManager<T> : DataManagerBase<T, string> where T : class,IIdNameBase<string>
    {
    }
}