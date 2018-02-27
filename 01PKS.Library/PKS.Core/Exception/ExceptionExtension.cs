using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net;
using PKS.Utils;

namespace PKS.Core
{
    /// <summary>异常扩展</summary>
    public static class ExceptionExtension
    {
        /// <summary>异常代码</summary>
        private static readonly string Code = "PKS.Exception:Code";
        /// <summary>异常明细</summary>
        private static readonly string Details = "PKS.Exception:Details";
        /// <summary>异常代码</summary>
        private static readonly string HttpResponseStatusCode = "PKS.Exception:HttpResponseStatusCode";
        /// <summary>异常明细</summary>
        private static readonly string HttpResponseReasonPhrase = "PKS.Exception:HttpResponseReasonPhrase";
        /// <summary>抛出符合规范的异常</summary>
        public static void Throw<TCode>(this Exception ex, TCode code, string details)
            where TCode : struct
        {
            Throw(ex, code.ToString(), details);
        }
        /// <summary>抛出符合规范的异常</summary>
        public static void Throw(this Exception ex, string code, string details)
        {
            if (!(ex is UserFriendlyException) && !ex.Data.Contains(Code))
            {
                ex.Data[Code] = code;
                ex.Data[Details] = details;
            }
            throw ex;
        }
        /// <summary>抛出符合规范的异常</summary>
        public static void Throw(this HttpRequestException ex, HttpResponseMessage response)
        {
            ex.SetResponseReason(response);
            throw ex;
        }
        /// <summary>设置应答原因</summary>
        public static void SetResponseReason(this Exception ex, HttpResponseMessage response)
        {
            ex.Data[HttpResponseStatusCode] = response.StatusCode;
            ex.Data[HttpResponseReasonPhrase] = response.ReasonPhrase;
        }
        /// <summary>抛出用户友好异常</summary>
        public static void ThrowUserFriendly<TCode>(this TCode code, string friendlyMessage, string message)
            where TCode : struct
        {
            throw new UserFriendlyException(code.ToString(), message, friendlyMessage);
        }
        /// <summary>转换为异常数据</summary>
        public static ExceptionModel ToModel(this Exception ex)
        {
            var model = new ExceptionModel();
            var ex2 = ex.As<UserFriendlyException>();
            if (ex2 == null)
            {
                model.Code = ex.Data[Code].As<string>();
                model.Message = ex.Data[Details].As<string>();
            }
            else
            {
                model.Code = ex2.Code;
                model.Message = ex2.FriendlyMessage;
            }
            model.Details = ex.GetFullMessage();
            return model;
        }
        /// <summary>转换为WEB异常数据</summary>
        public static WebExceptionModel ToWebModel(this Exception ex)
        {
            var reasonPhrase = ex.Data[HttpResponseReasonPhrase].As<string>();
            if (reasonPhrase.IsNullOrEmpty()) return null;
            var model = new WebExceptionModel();
            model.ReasonPhrase = reasonPhrase;
            model.StatusCode = (HttpStatusCode)ex.Data[HttpResponseStatusCode];
            return model;
        }
        /// <summary>获得完整异常信息</summary>
        public static string GetFullMessage(this Exception ex, int level = 0)
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append(ex.Message);
            var ufex = ex.As<UserFriendlyException>();
            if (ufex != null) sBuilder.Append(ufex.FriendlyMessage);
            var indent = (level + 1) * 2;
            var aex = ex.As<AggregateException>();
            if (aex != null)
            {
                sBuilder.AppendLine();
                for (int i = 0; i < indent; i++) sBuilder.Append(' ');
                foreach (var innerException in aex.InnerExceptions)
                {
                    sBuilder.Append(GetFullMessage(innerException, level + 1));
                }
            }
            else if (ex.InnerException != null)
            {
                sBuilder.AppendLine();
                for (int i = 0; i < indent; i++) sBuilder.Append(' ');
                sBuilder.Append(GetFullMessage(ex.InnerException, level + 1));
            }
            return sBuilder.ToString();
        }
    }
}
