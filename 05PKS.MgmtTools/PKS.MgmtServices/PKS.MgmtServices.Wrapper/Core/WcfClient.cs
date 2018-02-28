using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using PKS.Utils;

namespace PKS.MgmtServices.Core
{
    /// <summary>WCF客户端扩展</summary>
    public static class WcfClientExtension
    {
        /// <summary>操作超时</summary>
        private static TimeSpan s_OperationTimeout = TimeSpan.FromMinutes(30);
        /// <summary>调用</summary>
        public static async Task InvokeAsync<IWcfService>(this ClientBase<IWcfService> client, Func<IWcfService, Task> command)
            where IWcfService : class
        {
            var proxy = client.As<IWcfService>();
            client.InnerChannel.OperationTimeout = s_OperationTimeout;
            try
            {
                await command(proxy);
                client.Close();
            }
            catch
            {
                client.Abort();
                throw;
            }
        }
        /// <summary>调用</summary>
        public static async Task<TResult> InvokeAsync<IWcfService, TResult>(this ClientBase<IWcfService> client, Func<IWcfService, Task<TResult>> command)
            where IWcfService : class
        {
            var proxy = client.As<IWcfService>();
            client.InnerChannel.OperationTimeout = s_OperationTimeout;
            try
            {
                var result = await command(proxy);
                client.Close();
                return result;
            }
            catch
            {
                client.Abort();
                throw;
            }
        }
    }
}