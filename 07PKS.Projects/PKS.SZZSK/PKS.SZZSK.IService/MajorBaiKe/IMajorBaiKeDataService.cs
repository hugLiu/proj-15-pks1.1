using PKS.SZZSK.IService.Common;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.MajorBaiKe
{
    public interface IMajorBaiKeDataService : IViewService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">目标名称</param>
        /// <param name="type">专题研究类别</param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IList<string> GetMajorListByName(string name, string type, int? from = 1, int? size = 100);

        List<string> GetBoListByFeature(string bot, Dictionary<string, List<string>> feature, int? from = 1, int? size = 100);

        object GetSearchCondition(string grid);

        List<BOT> GetBots();

        List<string> GetBoListByName(string bot, string bo, int? from = 1, int? size = 100);

        BOT GetBot(string bot);
    }
}
