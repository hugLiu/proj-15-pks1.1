#pragma warning disable 1591


namespace PKS.DbModels
{
    using PKS.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>PKS_KG_PrivateCatalog</summary>
    [Serializable]
    public class PKS_KG_PrivateCatalog : PKS_AuditedModel , ITreeNode
    {
        ///<summary>
        /// CODE (length: 100)
        ///</summary>
        public string Code { get; set; }

        ///<summary>
        /// NAME (length: 100)
        ///</summary>
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
        public int LevelNumber { get; set; }

        ///<summary>
        /// ORDERNUMBER
        ///</summary>
        public int OrderNumber { get; set; }

        ///<summary>
        /// PARENTID
        ///</summary>
        public int? ParentId { get; set; }


        ///<summary>
        /// IMAGEURL (length: 255)
        ///</summary>
        public string ImageURL { get; set; }

        // Reverse navigation

        /// <summary>
        /// Child PksKgprivateCatalogs where [PKS_KGPRIVATE_CATALOG].[PARENTID] point to this entity (FK_PKS_KGPRIVATE_CATALOG_PKS_KGPRIVATE_CATALOG)
        /// </summary>
        public virtual ICollection<PKS_KG_PrivateCatalog> Children { get; set; } // PKS_KGPRIVATE_CATALOG.FK_PKS_KGPRIVATE_CATALOG_PKS_KGPRIVATE_CATALOG
        /// <summary>
        /// Child PksKgprivateTopics where [PKS_KGPRIVATE_TOPIC].[CATALOGID] point to this entity (FK_PKS_KGPRIVATE_TOPIC_PKS_KGPRIVATE_CATALOG)
        /// </summary>
        public virtual ICollection<PKS_KG_Topic> Topics { get; set; } // PKS_KGPRIVATE_TOPIC.FK_PKS_KGPRIVATE_TOPIC_PKS_KGPRIVATE_CATALOG

        // Foreign keys

        /// <summary>
        /// Parent PksKgprivateCatalog pointed by [PKS_KGPRIVATE_CATALOG].([Parentid]) (FK_PKS_KGPRIVATE_CATALOG_PKS_KGPRIVATE_CATALOG)
        /// </summary>
        public virtual PKS_KG_PrivateCatalog Parent { get; set; } // FK_PKS_KGPRIVATE_CATALOG_PKS_KGPRIVATE_CATALOG

        public PKS_KG_PrivateCatalog()
        {
            Children = new List<PKS_KG_PrivateCatalog>();
            Topics = new List<PKS_KG_Topic>();
        }
    }

}
