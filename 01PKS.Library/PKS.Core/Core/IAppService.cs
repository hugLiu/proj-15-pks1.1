using System;
using System.Collections;
using System.Collections.Generic;

namespace PKS.Core
{
    /// <summary>应用服务接口(按约定自动注入)</summary>
    /// <remarks>每次都实例化的服务继承本接口</remarks>
    public interface IAppService
    {
    }

    /// <summary>请求相关应用服务接口(按约定自动注入)</summary>
    /// <remarks>按请求实例化的服务继承本接口</remarks>
    public interface IPerRequestAppService
    {
    }


    /// <summary>线程相关应用服务接口(按约定自动注入)</summary>
    /// <remarks>按线程实例化的服务继承本接口</remarks>
    public interface IPerThreadAppService
    {
    }

    /// <summary>单例应用服务接口(按约定自动注入)</summary>
    /// <remarks>单例化的服务继承本接口</remarks>
    public interface ISingletonAppService
    {
    }
}
