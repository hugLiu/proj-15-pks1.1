using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 定义一个实现了可查询数据集和批量增删改的全功能的数据接口
    /// </summary>
    /// <typeparam name="T">数据实体类</typeparam>
    public interface IModelDataService<T> : IGetQueryData<T>, IDataBatchCUD<T>, IDisposable
        where T : class
    {
    }
}
