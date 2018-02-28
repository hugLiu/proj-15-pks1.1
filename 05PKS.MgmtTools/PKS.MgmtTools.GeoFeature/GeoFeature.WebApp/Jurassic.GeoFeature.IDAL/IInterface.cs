using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.IDAL
{
    public interface IInterface<T> where T : class
    {
        /// <summary>
        /// 根据ID、名称判断是否存在
        /// 1、判断对象是否存在应用对象名称、对象类型判断
        /// 2、判断别名是否存在应用对象ID、别名、应用域判断
        /// 3、判断参数是否存在应用对象ID、应用域判断
        /// 4、判断坐标是否存在应用对象ID判断
        /// 5、判断对象类型是否存在应用对象类型判断
        /// 6、判断类型关系是否存在应用类型1、关系名称、类型2判断
        /// 7、判断对象关系是否存在应用对象1、关系ID、对象2判断
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Exist(T model);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert(T model);
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(T model);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(T model);
        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        IList<T> GetListByID(string ID);

    
        /// <summary>
        /// 根据名称获取对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        IList<T> GetListByName(string Name);
        /// <summary>
        /// 获取全部对象
        /// </summary>
        /// <returns></returns>
        IList<T> GetList();
    }
}
