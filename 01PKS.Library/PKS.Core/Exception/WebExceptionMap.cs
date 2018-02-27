using System;
using System.Collections.Generic;
using System.Net;

namespace PKS.Core
{
    /// <summary>WEB异常映射</summary>
    public abstract class WebExceptionMap
    {
        /// <summary>服务名称</summary>
        public abstract string ServiceName { get; }

        /// <summary>映射集合</summary>
        private IDictionary<string, WebExceptionModel> Mappers { get; set; }
        /// <summary>生成配置</summary>
        public IDictionary<string, WebExceptionModel> Build()
        {
            this.Mappers = new Dictionary<string, WebExceptionModel>();
            Add();
            return this.Mappers;
        }
        /// <summary>加入配置</summary>
        protected abstract void Add();
        /// <summary>加入一个配置</summary>
        protected void Add(string code, WebExceptionModel model)
        {
            this.Mappers.Add(code, model);
        }
    }
}
