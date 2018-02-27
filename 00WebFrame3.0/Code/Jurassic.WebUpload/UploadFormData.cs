using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebUpload
{
    /// <summary>
    /// 数据组件的表单数据对象
    /// </summary>
    public class UploadFormData
    {
        /// <summary>
        /// 上传控件的HTML元素id,如果不指定，则由系统随机生成一个
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// 上传到的目录ID，默认为0，则传到当前用户的根目录
        /// </summary>
        public int CatalogId { get; set; }
        /// <summary>
        /// 表单数据元素名称，在上传成功后，将该元素内值填写为所有上传资源的逗号分隔的ID
        /// 如果页面表单没有这个名称的元素，则自动加上
        /// </summary>
        public string FormDataName { get; set; }

        /// <summary>
        /// 上传完毕后服务端要执行的方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 上传完毕后服务端要执行的方法所在控制器名称,不带'Controller'后缀
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 服务端处理完后回调的js函数名称
        /// </summary>
        public string JsDoneFunction { get; set; }

        /// <summary>
        /// 文件添加到上传列表后没上传前的js函数名称
        /// </summary>
        public string JsAddFunction { get; set; }

        /// <summary>
        /// 文件添加到上传列表后没上传前的js函数名称,作用为预览图片
        /// </summary>
        public string JsPreviewFunction { get; set; }

        public string JsCommitUpload { get; set; }

        /// <summary>
        /// 限制同时上传数，为0表示不限制
        /// </summary>
        public int MaxFileCount { get; set; }

        /// <summary>
        /// 允许上传的文件扩展名称，每一项用.扩展名表示。如".jpg"
        /// </summary>
        public string[] AllowedTypes { get; set; }

        /// <summary>
        /// 限制最大文件大小，为0表示不限制
        /// </summary>
        public int MaxFileSize { get; set; }

        /// <summary>
        /// 是否选择文件后自动上传，默认值是True
        /// </summary>
        public bool AutoUpload { get; set; }

        /// <summary>
        /// 是否显示文件管理按钮
        /// </summary>
        public bool ShowManager { get; set; }

        public UploadFormData()
        {
            AutoUpload = true;
            ShowManager = true;
        }
    }
}