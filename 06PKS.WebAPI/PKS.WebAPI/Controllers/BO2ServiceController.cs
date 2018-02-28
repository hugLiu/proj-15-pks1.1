using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System.Threading.Tasks;

namespace PKS.WebAPI.Controllers
{
    /// <summary>基于MongoDB的对象服务控制器</summary>
    public class BO2ServiceController : PKSApiController
    {
        /// <summary>构造函数</summary>
        public BO2ServiceController(IBO2Service service)
        {
            ServiceImpl = service;
        }

        /// <summary>服务实例</summary>
        private IBO2Service ServiceImpl { get; }

        /// <summary>
        /// 获取服务信息
        /// </summary>
        /// <returns></returns>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "基于MongoDB的业务对象属性与坐标查询与维护服务"
            };
        }

        /// <summary>
        /// 根据bo获取bo对应的属性和坐标信息
        /// </summary>
        /// <param name="bot">业务对象类型</param>
        /// <param name="bo">业务对象名称</param>
        /// <returns><code>BO2</code>对象</returns>
        [HttpGet]
        public async Task<BO2> GetBO(string bot, string bo)
        {
            return await ServiceImpl.GetBOAsync(bot,bo);
        }


        /// <summary>
        /// 获取业务对象类型的属性定义
        /// </summary>
        /// <param name="bot">业务对象类型</param>
        /// <returns><code>BOT</code>对象</returns>
        [HttpGet]
        public async Task<BOT> GetBOT(string bot)
        {
            return await ServiceImpl.GetBOTAsync(bot);
        }

        /// <summary>
        /// 根据对象名称模糊查询BO
        /// </summary>
        /// <param name="bot">业务对象类型</param>
        /// <param name="bo">包括的业务对象名称字符串</param>
        /// <returns><code>BO2</code>列表</returns>
        [HttpGet]
        public async Task<List<BO2>> FindBOs(string bot, string bo)
        {
            return await ServiceImpl.FindBOsAsync(bot,bo);
        }

        /// <summary>
        /// 根据查询条件、返回字段、排序字段、分页设置来获取业务对象
        /// </summary>
        /// <param name="request">
        /// {
        ///    "query":{
        ///        "bot":"井"
        ///    },
        ///    "fields":{"_id":0,"bot":1,"properties":1},
        ///    "sort":{"boid":1},
        ///    "skip":0,
        ///    "limit":2
        /// }
        /// </param>
        /// <returns><code>BO2</code>列表</returns>
        [HttpPost]
        public async Task<List<BO2>> FilterBOs(FilterRequest request)
        {
            return await ServiceImpl.FilterBOsAsync(request);
        }
        /// <summary>根据条件获得BO数量</summary>
        [HttpPost]
        public async Task<long> CountBOs(object query)
        {
            return await ServiceImpl.CountBOsAsync(query);
        }



        /// <summary>根据条件获得BOT数量</summary>
        [HttpPost]
        public async Task<long> CountBOTs(object query)
        {
            return await ServiceImpl.CountBOTsAsync(query);
        }
        /// <summary>
        /// 根据查询条件、返回字段、排序字段、分页设置来获取业务对象类型
        /// </summary>
        /// <param name="request">
        /// {
        ///    "query":{
        ///        "bot":"井"
        ///    },
        ///    "fields":{"_id":0,"bot":1,"properties":1},
        ///    "sort":{"boid":1},
        ///    "skip":0,
        ///    "limit":2
        /// }
        /// </param>
        /// <returns><code>BOT</code>列表</returns>
        [HttpPost]
        public async Task<List<BOT>> FilterBOTs(FilterRequest request)
        {
            return await ServiceImpl.FilterBOTsAsync(request);
        }

        /// <summary>
        /// 插入一组BOT
        /// </summary>
        /// <param name="bots"><code>BOT</code>列表</param>
        /// <returns></returns>
        [HttpPost]
        public async Task InsertBOTs(List<BOT> bots)
        {
            await ServiceImpl.InsertBOTsAsync(bots);
        }

        /// <summary>
        /// 存在更新不存在插入BOT
        /// </summary>
        /// <param name="bots"><code>BOT</code>列表</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> SaveBOTs(List<BOT> bots)
        {
            return await ServiceImpl.SaveBOTsAsync(bots);
        }

        /// <summary>
        /// 根据bot的名称删除bot记录
        /// </summary>
        /// <param name="bots">bot的名称数组</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> DeleteBOTs(List<string> bots)
        {
            return await ServiceImpl.DeleteBOTsAsync(bots);
        }

        /// <summary>
        /// 插入一组BO对象
        /// </summary>
        /// <param name="bos"></param>
        [HttpPost]
        public async Task<IHttpActionResult> InsertBOs(List<BO2> bos)
        {
            await ServiceImpl.InsertBOsAsync(bos);
            return Ok();
        }

        /// <summary>
        /// 存在更新，不存在插入（BO/BOT为key）
        /// </summary>
        /// <param name="bos"></param>
        [HttpPost]
        public async Task<object> SaveBOs(List<BO2> bos)
        {
            return await ServiceImpl.SaveBOsAsync(bos);
        }

        /// <summary>
        /// 删除bot下的一组bo对象
        /// {
        ///    "bot":"井",
        ///    "bos":["WeLL-03"]
        /// }
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public async Task<object> DeleteBOs(BO2DeleteRequest request)
        {
            return await ServiceImpl.DeleteBOsAsync(request);
        }

        /// <summary>
        /// 获取对象的临近对象
        /// {
        ///     "bot":"井", 
        ///     "bo":"well-01",
        ///     "distince":10000.0,
        ///     "top":3
        ///  }
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Near(NearRequest request)
        {
            return await ServiceImpl.NearAsync(request);
        }
    }
}