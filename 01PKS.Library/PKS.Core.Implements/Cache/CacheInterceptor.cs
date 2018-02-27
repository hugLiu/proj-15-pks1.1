//using System;
//using CacheManager.Core;
//using PKS.Utils;
////using EventBus;
//using Ninject.Extensions.Interception;

//namespace PKS.Core
//{
//    /// <summary>缓存拦截器</summary>
//    public class CacheInterceptor : ICacheInterceptor
//    {
//        /// <summary>构造函数</summary>
//        public CacheInterceptor(ICacheManager<object> cacheManager)//, IEventBus eventBus)
//        {
//            this.CacheManager = cacheManager;
//            //this.EventBus = eventBus;
//        }
//        /// <summary>缓存管理器</summary>
//        private ICacheManager<object> CacheManager { get; set; }
//        ///// <summary>事件总线服务</summary>
//        //private IEventBus EventBus { get; set; }
//        /// <summary>拦截</summary>
//        public void Intercept(IInvocation invocation)
//        {
//            var cacheAttributes = invocation.Request.Method.GetAttributes<CacheBaseAttribute>();
//            var interceptedCount = 0;
//            foreach (var cacheAttribute in cacheAttributes)
//            {
//                if (cacheAttribute is CacheItemAttribute)
//                {
//                    Add(invocation, cacheAttribute.As<CacheItemAttribute>());
//                    return;
//                }
//                if (cacheAttribute is CacheRemoveAttribute)
//                {
//                    if (interceptedCount == 0) invocation.Proceed();
//                    Remove(invocation, cacheAttribute.As<CacheRemoveAttribute>());
//                    interceptedCount++;
//                    continue;
//                }
//            }
//            if (interceptedCount > 0) return;
//            invocation.Proceed();
//        }
//        /// <summary>加入缓存</summary>
//        private void Add(IInvocation invocation, CacheItemAttribute cacheAttribute)
//        {
//            var returnValue = this.CacheManager.Get(cacheAttribute.Key, cacheAttribute.Region);
//            if (returnValue != null)
//            {
//                invocation.ReturnValue = returnValue;
//                return;
//            }
//            invocation.Proceed();
//            if (invocation.ReturnValue == null) return;
//            var cacheItem = new CacheItem<object>(cacheAttribute.Key, cacheAttribute.Region, invocation.ReturnValue, cacheAttribute.Mode, cacheAttribute.Timeout);
//            this.CacheManager.Add(cacheItem);
//        }
//        /// <summary>删除缓存</summary>
//        private void Remove(IInvocation invocation, CacheRemoveAttribute cacheAttribute)
//        {
//            this.CacheManager.Remove(cacheAttribute.Key, cacheAttribute.Region);
//            //this.EventBus.Post(new CacheItemRemoveEventArgs() { Region = cacheAttribute.Region, Key = cacheAttribute.Key }, TimeSpan.Zero);
//        }
//    }
//}
