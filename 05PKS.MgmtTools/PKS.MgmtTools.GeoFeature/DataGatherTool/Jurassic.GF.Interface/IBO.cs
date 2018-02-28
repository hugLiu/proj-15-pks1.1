using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IBO
    {
        /// <summary>
        /// 根据对象名称、类型获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        BoMode GetBoListByName(string name, string bot);
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        bool ExistBO(string name, string bot);

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool InsertBO(BoMode model);

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateBO(BoMode model);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        bool DelBO(string boid);
    }
}
