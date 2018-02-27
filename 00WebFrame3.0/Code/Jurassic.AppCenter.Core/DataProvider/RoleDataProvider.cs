using Jurassic.AppCenter.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// AppRole的缺省的数据提供类，当外部程序没有提供此接口时，
    /// 由这个类来管理系统角色，它可以将角色数据持久化存到磁盘的Json文件。
    /// </summary>
    class RoleDataProvider : DataProvider<AppRole>
    {
        /// <summary>
        /// 从磁盘json文件加载角色表
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AppRole> GetData()
        {
            return new CachedList<AppRole>();
        }
    }
}
