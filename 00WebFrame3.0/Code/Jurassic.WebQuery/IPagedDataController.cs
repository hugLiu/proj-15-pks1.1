using Jurassic.AppCenter;
using System;
namespace Jurassic.WebQuery
{
    /// <summary>
    /// 定义用于使用AdvQuery查询组件的控制器，实现分页后的数据处理的接口
    /// 避免整个数据集的ToList
    /// </summary>
    /// <typeparam name="TModel">分页的数据实体类型</typeparam>
    public interface IPagedDataController<TModel>
     where TModel : class
    {
        /// <summary>
        /// 分页后的数据处理方法
        /// </summary>
        /// <param name="pagedData">分页后的分页数据对象</param>
        void BeforeShowingPage(Pager<TModel> pagedData);
    }
}
