using PKS.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>新图谱主题</summary>
    [Serializable]
    public class KG_NewTopic
    {
        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }
        ///<summary>
        /// 公共分类Id
        ///</summary>
        public int? PublicCatalogId { get; set; }
        ///<summary>
        /// 私有分类Id
        ///</summary>
        public int? PrivateCatalogId { get; set; }
        ///<summary>
        /// 主题
        ///</summary>
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        ///<summary>
        /// 链接URL
        ///</summary>
        [Required]
        [StringLength(255)]
        public string LinkUrl { get; set; }
    }
}
