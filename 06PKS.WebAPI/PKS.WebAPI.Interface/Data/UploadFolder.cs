using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>文件下载结果</summary>
    public class UploadFolder
    {
        /// <summary>ID</summary>
        public int Id { get; set; }
        /// <summary>父文件夹ID</summary>
        public int ParentId { get; set; }
        /// <summary>级别</summary>
        public int Level { get; set; }
        /// <summary>文件夹名称</summary>
        public string Name { get; set; }
        /// <summary>文件夹全名称</summary>
        public string FullName { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}