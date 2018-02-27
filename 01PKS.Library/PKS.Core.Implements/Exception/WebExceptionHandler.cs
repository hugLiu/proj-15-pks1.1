using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using PKS.Utils;

namespace PKS.Core
{
    /// <summary>WEB异常处理器</summary>
    public class WebExceptionHandler : IWebExceptionHandler
    {
        /// <summary>异常类型映射</summary>
        private Dictionary<Type, ExceptionModel> TypeMappers { get; set; }

        /// <summary>WEB异常类型映射</summary>
        private Dictionary<Type, WebExceptionModel> WebTypeMappers { get; set; }

        /// <summary>WEB异常代码映射</summary>
        private Dictionary<string, Dictionary<string, WebExceptionModel>> WebCodeMappers { get; set; }

        /// <summary>处理异常类型映射</summary>
        /// <param name="ex">异常实例</param>
        /// <param name="service">服务名称，MVC控制器或API控制器默认为控制器名称(不包括后缀Controller)</param>
        /// <param name="exceptionModel">异常数据</param>
        public WebExceptionModel Handle(Exception ex, string service, ExceptionModel exceptionModel)
        {
            var webExceptionModel = ex.ToWebModel();
            if (webExceptionModel != null) return webExceptionModel;
            if (exceptionModel.Code.IsNullOrEmpty())
            {
                ExceptionModel exceptionModel2 = null;
                var exType = ex.GetType();
                do
                {
                    exceptionModel2 = TypeMappers.GetValueBy(exType);
                    exType = exType.BaseType;
                } while (exceptionModel2 == null);
                exceptionModel.Code = exceptionModel2.Code;
                if (exceptionModel.Message.IsNullOrEmpty())
                    exceptionModel.Message = exceptionModel2.Message;
                if (exceptionModel.Details.IsNullOrEmpty())
                    exceptionModel.Details = exceptionModel2.Details;
            }
            else
            {
                if (service == null) service = "";
                var codeMappers = WebCodeMappers.GetValueBy(service);
                if (codeMappers == null && service.Length > 0)
                {
                    service = "";
                    codeMappers = WebCodeMappers.GetValueBy(service);
                }
                webExceptionModel = codeMappers.GetValueBy(exceptionModel.Code);
            }
            if (webExceptionModel == null)
            {
                var exType = ex.GetType();
                do
                {
                    webExceptionModel = WebTypeMappers.GetValueBy(exType);
                    exType = exType.BaseType;
                } while (webExceptionModel == null);
            }
            return webExceptionModel;
        }

        /// <summary>初始化</summary>
        public void Initialize()
        {
            InitTypeMappers();
            InitCodeMappers();
        }

        /// <summary>配置异常类型映射</summary>
        private void InitTypeMappers()
        {
            TypeMappers = new Dictionary<Type, ExceptionModel>
            {
                {
                    typeof(Exception),
                    new ExceptionModel
                    {
                        Code = ExceptionCodes.OperationProcessingFailed.ToString(),
                        Message = "操作处理失败！",
                        Details = "操作处理失败！"
                    }
                },
                {
                    typeof(ArgumentException),
                    new ExceptionModel
                    {
                        Code = ExceptionCodes.MissingParameterValue.ToString(),
                        Message = "请求参数无效！",
                        Details = "缺少请求参数！"
                    }
                },
                {
                    typeof(ArgumentNullException),
                    new ExceptionModel
                    {
                        Code = ExceptionCodes.MissingParameterValue.ToString(),
                        Message = "请求参数无效！",
                        Details = "请求参数不能为null！"
                    }
                },
                {
                    typeof(InvalidEnumArgumentException),
                    new ExceptionModel
                    {
                        Code = ExceptionCodes.InvalidEnumValue.ToString(),
                        Message = "请求参数无效！",
                        Details = "请求参数为枚举类型，参数值不合法！"
                    }
                },
                {
                    typeof(AuthenticationException),
                    new ExceptionModel
                    {
                        Code = ExceptionCodes.OperationUnauthorized.ToString(),
                        Message = "操作未授权！",
                        Details = "操作未授权！"
                    }
                }
            };
            WebTypeMappers = new Dictionary<Type, WebExceptionModel>
            {
                {typeof(Exception), WebExceptionModel.ServerProcessingFailed},
                {typeof(ArgumentException), WebExceptionModel.BadRequest},
                {typeof(ArgumentNullException), WebExceptionModel.BadRequest},
                {typeof(InvalidEnumArgumentException), WebExceptionModel.BadRequest},
                {typeof(InvalidOperationException), WebExceptionModel.ServerProcessingFailed},
                {typeof(AuthenticationException), WebExceptionModel.Unauthorized}
            };
        }

        /// <summary>配置异常代码映射</summary>
        private void InitCodeMappers()
        {
            WebCodeMappers =
                new Dictionary<string, Dictionary<string, WebExceptionModel>>(StringComparer.OrdinalIgnoreCase);
            var codeMappers = new Dictionary<string, WebExceptionModel>(StringComparer.OrdinalIgnoreCase)
            {
                {ExceptionCodes.MissingParameterValue.ToString(), WebExceptionModel.BadRequest},
                {ExceptionCodes.ParameterParsingFailed.ToString(), WebExceptionModel.BadRequest},
                {ExceptionCodes.InvalidEnumValue.ToString(), WebExceptionModel.BadRequest},
                {
                    ExceptionCodes.InvalidPagerParameter.ToString(), new WebExceptionModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = "Invalid pager value"
                    }
                },
                {ExceptionCodes.OperationProcessingFailed.ToString(), WebExceptionModel.ServerProcessingFailed}
            };
            WebCodeMappers.Add("", codeMappers);
        }

        /// <summary>从配置文件加载</summary>
        public void LoadConfig(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(e => e.IsClass && !e.IsAbstract && typeof(WebExceptionMap).IsAssignableFrom(e)).ToArray();
                foreach (var type in types)
                {
                    var map = Activator.CreateInstance(type).As<WebExceptionMap>();
                    var codeMappers = WebCodeMappers.GetValueBy(map.ServiceName);
                    if (codeMappers == null)
                    {
                        codeMappers = new Dictionary<string, WebExceptionModel>();
                        WebCodeMappers.Add(map.ServiceName, codeMappers);
                    }
                    codeMappers.AddRange(map.Build());
                }
            }
        }
    }
}