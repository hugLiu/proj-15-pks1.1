using Nest;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.ES;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Services
{
    public class UserBehaviorService:IUserBehaviorService, ISingletonAppService
    {

        /// <summary>构造函数</summary>
        public UserBehaviorService(IElasticConfig config)
        {
            this.Client = config.Client.As<ElasticClient>();
            this.IndexType = config.UserBehaviorType.As<TypeName>();
            _esAccess = new ESAccess<UserBehavior>();
        }
        /// <summary>客户端</summary>
        private ElasticClient Client { get; }
        /// <summary>索引类型</summary>
        private TypeName IndexType { get; }
        private ESAccess<UserBehavior> _esAccess = null;
        /// <summary>
        /// 新增一条用户行为记录
        /// </summary>
        /// <param name="requestModel"></param>
        public  void Add(UserBehavior request)
        {
            Task.Run(()=>AddLog(request));
        }  
        public async Task AddLog(UserBehavior request)
        {
            await this.Client.IndexAsync(request, e => e.Type(this.IndexType)).ConfigureAwait(false);
        }

        public virtual string Search(string request)
        {
            return Task.Run(() => SearchAsync(request)).Result;
        }
        
        public virtual async Task<string> SearchAsync(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return string.Empty;
            }
            return await _esAccess.GetDocumentsByRawQueryAsyn(request);
        }
    }
}
