using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.FileRepository;
using Jurassic.CommonModels.Properties;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.DiskFile
{
    /// <summary>
    /// 在Windows文件目录中存取文件。此服务类没有使用数据库
    /// </summary>
    public class DiskFileInfoService
    {
        
        public DiskFileInfoService()
        {
            
        }


        /// <summary>
        /// 保存文件信息
        /// </summary>
        /// <param name="info">文件信息</param>
        public DiskFileInfo Save(DirectoryInfo directoryInfo, DiskFileInfo info)
        {
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            InnerSaveFile(directoryInfo.FullName, info.FileName, info.FileStream, info.FileChunkInfo);

            //文件流清空
            info.FileStream = null;

            return info;
        }

        public bool IsFileExists(DirectoryInfo directoryInfo, string key)
        {
            bool isExists = false;
            if(directoryInfo.Exists)
            {
                //查找文件
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo havedFilename in files)
                {
                    if (havedFilename.Name.Equals(key))
                    {
                        isExists = true;
                        break;
                    }
                }
            }
            return isExists;
        }

        private void InnerSaveFile(string path, string key, Stream inputStream, FileChunkInfo fileChunkInfo)
        {
            string targetFilePath = path + @"\" + key;
            //查找文件流
            long startPos = 0;
            Stream fs = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if(fileChunkInfo.End > 0)
            {
                //分块文件
                startPos = fileChunkInfo.Start;
            }
            
            //保存流到文件
            using (fs)
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                //定位开始写入位置
                fs.Seek(startPos, SeekOrigin.Begin);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
            }

        }
        
        /// <summary>
        /// 获取所有文件信息列表（不包括文件流）
        /// </summary>
        /// <returns></returns>
        public List<DiskFileInfo> GetAll(DirectoryInfo directoryInfo)
        {
            List<DiskFileInfo> rl = new List<DiskFileInfo>();

            if (directoryInfo.Exists)
            {
                //查找文件
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo havedFilename in files)
                {
                    DiskFileInfo a = new DiskFileInfo();
                    a.FileName = havedFilename.Name;
                    a.FileSize = havedFilename.Length;
                    a.ContentType = havedFilename.Extension;

                    rl.Add(a);
                }
            }

            return rl;
        }
        private FileInfo InnerGetFileInfo(DirectoryInfo directoryInfo, string key)
        {
            if (directoryInfo.Exists)
            {
                //查找文件
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo havedFilename in files)
                {
                    if (havedFilename.Name.Equals(key))
                    {
                        return havedFilename;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取文件信息（包括文件流），不存在返回null
        /// </summary>
        /// <param name="id">文件信息ID</param>
        /// <returns></returns>
        public DiskFileInfo Get(DirectoryInfo directoryInfo, string key)
        {
            FileInfo havedFile = InnerGetFileInfo(directoryInfo, key);
            if (havedFile != null)
            {
                DiskFileInfo info = new DiskFileInfo();

                info.FileStream = new FileStream(havedFile.FullName, FileMode.Open, FileAccess.Read);
                info.FileName = havedFile.Name;
                info.FileSize = havedFile.Length;
                info.ContentType = havedFile.Extension;

                return info;
            }
            return null;
        }
        
        /// <summary>
        /// 删除文件信息
        /// </summary>
        /// <param name="id">文件信息ID</param>
        /// <returns>被删除的文件信息，如果没有找到文件返回null</returns>
        public void DeleteFileInfo(DirectoryInfo directoryInfo, string key)
        {
            FileInfo havedFile = InnerGetFileInfo(directoryInfo, key);
            if (havedFile != null)
            {
                havedFile.Delete();
            }
        }
        /// <summary>
        /// 获得文件缩略图流
        /// </summary>
        /// <param name="id">文件信息ID</param>
        /// <returns>缩略图流</returns>
        public Stream GetThumbnail(DirectoryInfo directoryInfo, string key)
        {
            Size thumbnailSize = ThumbnailSize.Large;
            return GetThumbnail(directoryInfo, key, thumbnailSize);
        }
        /// <summary>
        /// 获得文件缩略图流
        /// </summary>
        /// <param name="id">文件信息ID</param>
        /// <param name="thumbnailSize">缩略图大小</param>
        /// <returns>缩略图流</returns>
        public Stream GetThumbnail(DirectoryInfo directoryInfo, string key, Size thumbnailSize)
        {
            Stream targetStream = null;
            DiskFileInfo info = Get(directoryInfo, key);
            if(info == null)
            {
                return null;
            }
            Stream sourceStream = info.FileStream;
            if (sourceStream == null)
            {
                return null;
            }

            targetStream = FileExtensionTypeHelper.GetThumbnail(sourceStream, thumbnailSize);

            //所以要手动释放
            sourceStream.Flush();
            sourceStream.Close();

            return targetStream;
        }

    }
}
