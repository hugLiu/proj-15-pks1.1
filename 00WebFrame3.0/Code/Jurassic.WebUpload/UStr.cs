using Jurassic.AppCenter.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebUpload
{
    public class UStr : IStartupStr
    {
        /// <summary>
        /// 系统头像
        /// </summary>
        public static string SystemAvatar
        {
            get { return ResHelper.GetStr("SystemAvatar"); }
        }

        /// <summary>
        /// 请选择一张图片
        /// </summary>
        public static string PlzSelectAPicture
        {
            get { return ResHelper.GetStr("PlzSelectAPicture"); }
        }
        /// <summary>
        /// 请选择图片文件
        /// </summary>
        public static string PlzSelectImgFile
        {
            get { return ResHelper.GetStr("PlzSelectImgFile"); }
        }

        /// <summary>
        /// 加载失败，请重试
        /// </summary>
        public static string LoadFailedTryAgain
        {
            get { return ResHelper.GetStr("LoadFailedTryAgain"); }
        }
        /// <summary>
        /// 内容类型
        /// </summary>
        public static string ContentType
        {
            get { return ResHelper.GetStr("ContentType"); }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        public static string DeleteCatalog
        {
            get { return ResHelper.GetStr("DeleteCatalog"); }
        }
        /// <summary>
        /// 编辑目录
        /// </summary>
        public static string EditCatalog
        {
            get { return ResHelper.GetStr("EditCatalog"); }
        }
        /// <summary>
        /// 请先选择一个需要编辑的目录
        /// </summary>
        public static string PlzSelectACatalogToEdit
        {
            get { return ResHelper.GetStr("PlzSelectACatalogToEdit"); }
        }
        /// <summary>
        /// 请先选择一个需要上传的目录
        /// </summary>
        public static string PlzSelectACatalogToUpload
        {
            get { return ResHelper.GetStr("PlzSelectACatalogToUpload"); }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        public static string MoveFile
        {
            get { return ResHelper.GetStr("MoveFile"); }
        }
        /// <summary>
        /// 是否覆盖同名文件
        /// </summary>
        public static string ConfirmOverwriteDupFiles
        {
            get { return ResHelper.GetStr("ConfirmOverwriteDupFiles"); }
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        public static string CopyFile
        {
            get { return ResHelper.GetStr("CopyFile"); }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public static string UploadFile
        {
            get { return ResHelper.GetStr("UploadFile"); }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        public static string DeleteFile
        {
            get { return ResHelper.GetStr("DeleteFile"); }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        public static string DownloadFile
        {
            get { return ResHelper.GetStr("DownloadFile"); }
        }
        /// <summary>
        /// 请选择要下载的文件
        /// </summary>
        public static string PlzSelectAFileToDownload
        {
            get { return ResHelper.GetStr("PlzSelectAFileToDownload"); }
        }
        /// <summary>
        /// 下载完成
        /// </summary>
        public static string DownloadCompleted
        {
            get { return ResHelper.GetStr("DownloadCompleted"); }
        }
        /// <summary>
        /// 文件属性编辑
        /// </summary>
        public static string FilePropEdit
        {
            get { return ResHelper.GetStr("FilePropEdit"); }
        }
        /// <summary>
        /// 请选择要删除的目录
        /// </summary>
        public static string PlzSelectACatalogToDelete
        {
            get { return ResHelper.GetStr("PlzSelectACatalogToDelete"); }
        }

        /// <summary>
        /// 不能删除子级目录
        /// </summary>
        public static string CantDeleteChildCatalog
        {
            get { return ResHelper.GetStr("CantDeleteChildCatalog"); }
        }
        /// <summary>
        /// 请输入目录名称
        /// </summary>
        public static string PlzEnterCatalogName
        {
            get { return ResHelper.GetStr("PlzEnterCatalogName"); }
        }
        /// <summary>
        /// 新增目录
        /// </summary>
        public static string AddCatalog
        {
            get { return ResHelper.GetStr("AddCatalog"); }
        }
        /// <summary>
        /// 目录名称
        /// </summary>
        public static string CatalogName
        {
            get { return ResHelper.GetStr("CatalogName"); }
        }
        /// <summary>
        /// 选择目录
        /// </summary>
        public static string SelectCatalog
        {
            get { return ResHelper.GetStr("SelectCatalog"); }
        }
         /// <summary>
        /// 更改头像
        /// </summary>
        public static string ChangeAvatar
        {
            get { return ResHelper.GetStr("ChangeAvatar"); }
        }
   }
}