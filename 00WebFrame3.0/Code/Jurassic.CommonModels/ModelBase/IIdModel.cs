using Jurassic.AppCenter;
using System;
namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 所有具有单一主键Id的实体接口
    /// </summary>
    public interface IIdModel : IId<int>
    {
    }
}
