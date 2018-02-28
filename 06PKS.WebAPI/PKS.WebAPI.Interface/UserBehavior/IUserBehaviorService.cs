using PKS.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Models
{
    public interface IUserBehaviorServiceWrapper : IUserBehaviorService, IApiServiceWrapper
    {

    }
    public interface IUserBehaviorService
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="requestModel"></param>
        void Add(UserBehavior requestModel);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string Search(string query);

        Task<string> SearchAsync(string request);
    }
}
