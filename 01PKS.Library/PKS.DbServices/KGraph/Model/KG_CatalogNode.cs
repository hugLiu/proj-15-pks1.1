using PKS.DbModels;
using PKS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>Í¼Æ×½Úµã</summary>
    [Serializable]
    public class KG_CatalogNode : ITreeNode
    {
        ///<summary>
        /// Id (Primary key)
        ///</summary>
        public int Id { get; set; }
        ///<summary>
        /// CODE (length: 100)
        ///</summary>
        [Required]
        public string Code { get; set; }

        ///<summary>
        /// NAME (length: 100)
        ///</summary>
        [Required]
        public string Name { get; set; }

        ///<summary>
        /// NAME (length: 100)
        ///</summary>
        string ITreeNode.Text => this.Name;
        ///<summary>
        /// DESCRIPTION (length: 255)
        ///</summary>
        public string Description { get; set; }

        ///<summary>
        /// LEVELNUMBER
        ///</summary>
        public int Level { get; set; }

        ///<summary>
        /// ORDERNUMBER
        ///</summary>
        public int Order { get; set; }

        ///<summary>
        /// PARENTID
        ///</summary>
        public int? ParentId { get; set; }
        ///<summary>
        /// IMAGEURL (length: 255)
        ///</summary>
        public string PatternURL { get; set; }
        ///<summary>
        /// IMAGEURL (length: 255)
        ///</summary>
        public string ImageURL { get; set; }
        ///<summary>
        /// CREATEDBY (length: 50)
        ///</summary>
        public string CreatedBy { get; set; }

        ///<summary>
        /// CREATEDDATE
        ///</summary>
        public string CreatedDate { get; set; }

        ///<summary>
        /// LASTUPDATEDBY (length: 50)
        ///</summary>
        public string LastUpdatedBy { get; set; }

        ///<summary>
        /// LASTUPDATEDDATE
        ///</summary>
        public string LastUpdatedDate { get; set; }

    }
}
