using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.FileRepository
{
    /// <summary>
    /// 定义一个文件资源存储接口
    /// </summary>
    public interface IFileRepository : IFileRepository<string>
    {
        
    }

    /// <summary>
    /// 泛型文件资源存储接口
    /// </summary>
    /// <typeparam name="TKey">文件的键值</typeparam>
    public interface IFileRepository<TKey>
    {
        Stream this[TKey key] { get; set; }
        void Add(TKey key, Stream value);
        void Remove(TKey key);
        Stream Get(TKey key);
        void Append(TKey key, Stream value,long StartPos);

    }
}
