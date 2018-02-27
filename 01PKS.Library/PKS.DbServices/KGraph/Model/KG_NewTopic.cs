using PKS.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>��ͼ������</summary>
    [Serializable]
    public class KG_NewTopic
    {
        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }
        ///<summary>
        /// ��������Id
        ///</summary>
        public int? PublicCatalogId { get; set; }
        ///<summary>
        /// ˽�з���Id
        ///</summary>
        public int? PrivateCatalogId { get; set; }
        ///<summary>
        /// ����
        ///</summary>
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        ///<summary>
        /// ����URL
        ///</summary>
        [Required]
        [StringLength(255)]
        public string LinkUrl { get; set; }
    }
}
