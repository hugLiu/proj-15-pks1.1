using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.WebAPI.Controllers
{
    /// <summary>索引数据服务控制器</summary>
    public class IndexerServiceController : PKSApiController
    {
        /// <summary>构造函数</summary>
        public IndexerServiceController(IIndexerService service)
        {
            ServiceImpl = service;
        }

        /// <summary>服务实例</summary>
        private IIndexerService ServiceImpl { get; }

        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "索引数据服务用于索引数据的增删改"
            };
        }

        /// <summary>插入</summary>
        [HttpPost]
        public async Task<string[]> Insert(IndexInsertRequest request)
        {
            return await ServiceImpl.InsertAsync(request);
        }
        /// <summary>保存</summary>
        [HttpPost]
        public async Task<string[]> Save(IndexSaveRequest request)
        {
            return await ServiceImpl.SaveAsync(request);
        }
        /// <summary>删除</summary>
        [HttpPost]
        public async Task<string[]> Delete(List<string> iiids)
        {
            return await ServiceImpl.DeleteAsync(iiids);
        }
    }
}
