using System;
using System.Collections.Generic;
using System.Net;

namespace PKS.Core
{
    /// <summary>WEB异常处理器</summary>
    public interface IWebExceptionHandler
    {
        /// <summary>处理异常类型映射</summary>
        /// <param name="ex">异常实例</param>
        /// <param name="service">服务名称，MVC控制器或API控制器默认为控制器名称(不包括后缀Controller)</param>
        /// <param name="exceptionModel">异常数据</param>
        WebExceptionModel Handle(Exception ex, string service, ExceptionModel exceptionModel);
    }
}
