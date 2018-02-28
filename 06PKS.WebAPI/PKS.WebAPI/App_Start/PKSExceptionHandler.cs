using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Common.Logging;
using PKS.Core;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Filter
{
    /// <summary>异常处理拦截器</summary>
    public class PKSExceptionHandler : ExceptionHandler
    {
        /// <summary>When overridden in a derived class, handles the exception synchronously.</summary>
        /// <param name="context">The exception handler context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
        }
        /// <summary>Determines whether the exception should be handled.</summary>
        /// <returns>true if the exception should be handled; otherwise, false.</returns>
        /// <param name="context">The exception handler context.</param>
        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return base.ShouldHandle(context);
        }
    }
}