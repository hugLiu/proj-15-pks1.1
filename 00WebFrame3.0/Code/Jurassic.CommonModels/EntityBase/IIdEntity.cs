using Jurassic.AppCenter;
using System;
namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 所有具有单一主键Id的实体接口
    /// </summary>
    public interface IIdEntity : IId<int>
    {
    }
}
