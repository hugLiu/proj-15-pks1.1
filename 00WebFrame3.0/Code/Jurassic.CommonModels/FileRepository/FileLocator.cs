using System.Web;
using Jurassic.CommonModels.Articles;
using Jurassic.Com.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Jurassic.CommonModels.FileRepository
{
    /// <summary>
    /// 文件路径定位器
    /// </summary>
    public class FileLocator : IFileLocator
    {
        private string _rootPath;

        public FileLocator(string rootPath)
        {
            this._rootPath = rootPath;
        }
        // <summary>
        /// 获取文件的全路径名称
        /// </summary>
        /// <param name="fileKey">文件Key，就是文件的名称</param>
        /// <returns></returns>
        public string GetFilePath(string fileKey)
        {
            string returnPath;
            if (fileKey.StartsWith("~"))
            {
                returnPath = InnerGetSysFilePath(fileKey);
            }
            else
            {
                returnPath = InnerGetFilePath(fileKey);
            }

            return returnPath;
        }

        private string InnerGetFilePath(string fileKey)
        {
            //示例，前8位日期字符串，使用“-”分割，后面的是文件名称
            //string fileKey = "20151022-12345.jpg";

            string year = fileKey.Substring(0, 4);
            string month = fileKey.Substring(4, 2);
            string day = fileKey.Substring(6, 2);

            string fileName = fileKey.Substring(9);

            //wang改
            var rootPath = _rootPath.StartsWith("~") ? HttpContext.Current.Server.MapPath(_rootPath) : _rootPath;
            string filePath = Path.Combine(rootPath, year, month, day);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            return Path.Combine(filePath, fileKey);

            //filePath = filePath + @"\" + year;
            //if (!Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}
            //filePath = filePath + @"\" + month;
            //if (!Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}
            //filePath = filePath + @"\" + day;
            //if (!Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}

        }

        private string InnerGetSysFilePath(string filekey)
        {
            //如果是系统的文件，存放在filekey中的数据示例："~/Images/默认头像1.png"
            var rootpath = HttpContext.Current.Server.MapPath("/").ToString();
            var filepath = filekey.Substring(2);
            //var fullpath = rootpath  + filepath;
            var fullpath = Path.Combine(rootpath, filepath);
            return fullpath;
        }
    }
}
