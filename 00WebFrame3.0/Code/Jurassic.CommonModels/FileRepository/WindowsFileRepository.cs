using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using Ninject;

namespace Jurassic.CommonModels.FileRepository
{

    /// <summary>
    /// Windows本地文件存取类
    /// </summary>
    public class WindowsFileRepository : IFileRepository
    {
        public IFileLocator FileLocator
        {
            get;
            private set;
        }

        public WindowsFileRepository(IFileLocator iFileLocator)
        {
            this.FileLocator = iFileLocator;
        }
        public Stream this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                InnerSaveFile(key, value);
            }
        }

        public Stream Get(string key)
        {
            if (key.IsEmpty()) return null;
            string filePath = InnerGetFilePath(key);

            return InnerGetFileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public void Add(string key, Stream value)
        {
            InnerSaveFile(key, value);
        }
        public void Remove(string key)
        {
            InnerRemoveFile(key);
            //return null;
        }
        private void InnerSaveFile(string key, Stream inputStream)
        {
            string filePath = InnerGetFilePath(key);

            //查找文件流
           
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            long startPos = 0;
            Stream fs = null;
            fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);

            //保存流到文件
            InnerSaveStream(inputStream, fs, startPos);
        }
        /// <summary>
        /// 取得文件路径
        /// </summary>
        /// <param name="key">key是文件名称</param>
        /// <returns></returns>
        private string InnerGetFilePath(string key)
        {

            string filePath = FileLocator.GetFilePath(key);

            //string targetFilePath = filePath + @"\" + key;

            return filePath;
        }

        private Stream InnerGetFileStream(string filePath, FileMode fileMode, FileAccess fileAccess)
        {
            //如果找到，打开文件，取得文件流返回
            if (File.Exists(filePath))
            {
                var fs = new FileStream(filePath, fileMode, fileAccess);

                return fs;
            }

            return null;
        }
        private void InnerRemoveFile(string key)
        {
            //取得文件路径
            string filePath = InnerGetFilePath(key);

            //如果找到，打开文件，取得文件流返回
            if (filePath != null && filePath != "")
            {
                File.Delete(filePath);
            }
        }


        private void InnerSaveStream(Stream inputStream, Stream fileStream, long startPos)
        {
            
            //保存流到文件
            var buffer = new byte[1024];

            var l = inputStream.Read(buffer, 0, 1024);
            //定位开始写入位置
            fileStream.Seek(startPos, SeekOrigin.Begin);
            while (l > 0)
            {
                fileStream.Write(buffer, 0, l);
                l = inputStream.Read(buffer, 0, 1024);
            }
            fileStream.Flush();
            fileStream.Close();
        }

        public void Append(string key, Stream value, long StartPos)
        {
            string filePath = InnerGetFilePath(key);

            //查找文件流
            long startPos = 0;
            Stream fs = null;
            if (File.Exists(filePath))
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                startPos = StartPos;
            }
            else
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                startPos = 0;
            }

            //保存流到文件
            InnerSaveStream(value, fs, startPos);

        }
    }


}
