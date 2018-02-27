#pragma warning disable 1591


namespace PKS.DbModels
{
    using PKS.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>PKS_KGPUBLIC_CATALOG</summary>
    [Serializable]
    public class PKS_KG_PublicCatalog : PKS_AuditedModel, ITreeNode
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
        /// Parent (One-to-One) PksKgpublicCatalog pointed by [PKS_KGPUBLIC_TOPIC].[Id] (FK_PKS_KGSHARETOPIC_PKS_KGPUBLICCATALOG)
        /// </summary>
        public virtual ICollection<PKS_KG_Topic> Topics { get; set; } // PKS_KGPUBLIC_TOPIC.FK_PKS_KGSHARETOPIC_PKS_KGPUBLICCATALOG
        /// <summary>
        /// Child PksKgpublicCatalogs where [PKS_KGPUBLIC_CATALOG].[PARENTID] point to this entity (FK_PKS_KGPUBLICCATALOG_PKS_KGPUBLICCATALOG)
        /// </summary>
        public virtual ICollection<PKS_KG_PublicCatalog> Children { get; set; } // PKS_KGPUBLIC_CATALOG.FK_PKS_KGPUBLICCATALOG_PKS_KGPUBLICCATALOG

        // Foreign keys

        /// <summary>
        /// Parent PksKgpublicCatalog pointed by [PKS_KGPUBLIC_CATALOG].([Parentid]) (FK_PKS_KGPUBLICCATALOG_PKS_KGPUBLICCATALOG)
        /// </summary>
        public virtual PKS_KG_PublicCatalog Parent { get; set; } // FK_PKS_KGPUBLICCATALOG_PKS_KGPUBLICCATALOG

        public PKS_KG_PublicCatalog()
        {
            Topics = new List<PKS_KG_Topic>();
            Children = new List<PKS_KG_PublicCatalog>();
        }
    }

}
