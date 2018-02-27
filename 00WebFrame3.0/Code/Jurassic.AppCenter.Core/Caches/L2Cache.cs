using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.Caches
{
    /// <summary>
    /// 分布式二级缓存
    /// </summary>
    public class L2Cache<T> : ICacheProvider<T> where T : class
    {
        private ICacheProvider<T> mL1Cache;

        private ICacheProvider<T> mL2Cache;

        private int AccessCount = 1, mL1Hit, mL2Hit;

        public bool L1Disabled { get; set; }

        /// <summary>
        /// L1命中率
        /// </summary>
        public double L1Hit { get { return mL1Hit / AccessCount; } }

        /// <summary>
        /// L2命中率
        /// </summary>
        public double L2Hit { get { return mL2Hit / AccessCount; } }

        /// <summary>
        /// 总命中率
        /// </summary>
        public double Hit { get { return (mL1Hit + mL1Hit) / AccessCount; } }

        public L2Cache(ICacheProvider<T> l1Cache, ICacheProvider<T> l2Cache)
        {
            mL1Cache = l1Cache;
            mL2Cache = l2Cache;
        }

        public T this[string key]
        {
            get
            {
                T val = null;

                if (!L1Disabled)
                {
                    val = mL1Cache[key];
                    if (val != null)
                    {
                        mL1Hit++;
                    }
                }

                if (val == null)
                {
                    val = mL2Cache[key];
                    if (val != null)
                    {
                        mL2Hit++;
                        mL1Cache[key] = val;
                    }
                }

                AccessCount++;
                return val;
            }
            set
            {
                if (L1Disabled)
                {
                    mL2Cache[key] = value;
                }
                else
                {
                    mL1Cache[key] = value;
                    mL2Cache[key] = mL1Cache[key];
                }
            }
        }


        public T Add(string key, T value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
        }

        public T Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
