using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace PKS.Core
{
    /// <summary>缓存拦截器</summary>
    public interface ICacheInterceptor : IInterceptor
    {
    }

    /// <summary>缓存拦截特性</summary>
    public sealed class CacheInterceptAttribute : InterceptAttribute
    {
        /// <summary>创建拦截器</summary>
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Kernel.Get<ICacheInterceptor>();
        }
    }
}