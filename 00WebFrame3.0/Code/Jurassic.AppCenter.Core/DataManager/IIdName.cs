using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 含字符型ID和名称的数据定义接口
    /// </summary>
    public interface IIdName : IIdNameBase<string>
    {
    }

    public interface IIdNameBase<TId> : IId<TId>
    {
        /// <summary>
        /// 对象名称
        /// </summary>
        string Name { get; set; }
    }
}
