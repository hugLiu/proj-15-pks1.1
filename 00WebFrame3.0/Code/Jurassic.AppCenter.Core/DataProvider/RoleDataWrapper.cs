using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Caches;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 在AppRole数据提供类基础上的一层包装，用于与本地json文件交换FuncionIds的数据
    /// </summary>
    class RoleDataWrapper : IDataProvider<AppRole>
    {
        IDataProvider<AppRole> mProvider;
        CachedList<AppRole> mCachedData;

        /// <summary>
        /// 在基本的角色提供程序基础上，创建一个角色包装类
        /// </summary>
        /// <param name="provider">基本的角色提供程序</param>
        internal RoleDataWrapper(IDataProvider<AppRole> provider, CachedList<AppRole> cachedData)
        {
            mProvider = provider;
            mCachedData = cachedData;
        }

        /// <summary>
        /// 当需要持久化数据Provider中没有的信息时，用此类来合并Provider的数据
        /// 和本地json数据文件。
        /// </summary>
        /// <returns>合并后的数据列表</returns>
        public IEnumerable<AppRole> GetData()
        {
            var dbData = mProvider.GetData();
            lock (mCachedData)
            {
                foreach (AppRole dbItem in dbData)
                {
                    AppRole chItem = mCachedData.FirstOrDefault(ch => ch.Id == dbItem.Id);
                    if (chItem != null)
                    {
                        dbItem.FunctionIds = chItem.FunctionIds;
                        mCachedData[mCachedData.IndexOf(chItem)] = dbItem;
                    }
                    else
                    {
                        mCachedData.Add(dbItem);
                    }
                }
                //移除Provider中没有的值
                for (int i = mCachedData.Count - 1; i >= 0; i--)
                {
                    if (!dbData.Any(d => d.Id == mCachedData[i].Id))
                    {
                        mCachedData.RemoveAt(i);
                    }
                }
                return mCachedData;
            }
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="t">要新增的角色</param>
        /// <returns>成功返回非0值，不成功返回0</returns>
        public int Add(AppRole t)
        {
            return mProvider.Add(t);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="t">要修改的角色</param>
        /// <returns>成功返回非0值，不成功返回0</returns>
        public int Change(AppRole t)
        {
            return mProvider.Change(t);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="t">要删除的角色</param>
        /// <returns>成功返回非0值，不成功返回0</returns>
        public int Delete(AppRole t)
        {
            return mProvider.Delete(t);
        }
    }
}
