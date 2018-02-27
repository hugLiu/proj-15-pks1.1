using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.FileRepository
{
    /// <summary>
    /// 文件路径定位器
    /// </summary>
    public interface IFileLocator
    {
        /// <summary>
        /// 获取文件的全路径名称
        /// </summary>
        /// <param name="fileKey">文件Key，就是文件的名称</param>
        /// <returns></returns>
        string GetFilePath(string fileKey);
        /// <summary>
        /// 获取目录的全路径名称
        /// </summary>
        /// <param name="fileKey">文件Key值</param>
        /// <returns></returns>
        //string GetCatalogPath(int catalogId);
    }
}
