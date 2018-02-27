#pragma warning disable 1591


namespace PKS.DbModels
{
    using PKS.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>PKS_KG_PublicTopic</summary>
    [Serializable]
    public class PKS_KG_Topic : PKS_AuditedModel
    {
        ///<summary>
        /// ˽��ͼ�׷���ID
        ///</summary>
        public int? PrivateCatalogId { get; set; }
        ///<summary>
        /// ����ͼ�׷���ID
        ///</summary>
        public int? PublicCatalogId { get; set; }

        ///<summary>
        /// Title (length: 255)
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// LinkUrl (length: 255)
        ///</summary>
        public string LinkUrl { get; set; }

        ///<summary>
        /// CONTENTS (length: 4000)
        ///</summary>
        public string Contents { get; set; }

        // Reverse navigation
        /// <summary>
        /// ����ͼ�׷���
        /// </summary>
        public virtual PKS_KG_PublicCatalog PublicCatalog { get; set; }
        /// <summary>
        /// ���˽��ͼ�׷�����
        /// </summary>
        public List<TreeNode> GetPublicCatalogs()
        {
            if (this.PublicCatalog == null) return null;
            var nodes = new List<TreeNode>();
            var catalog = this.PublicCatalog;
            do
            {
                nodes.Add(catalog.ToTreeNode());
                catalog = catalog.Parent;
            }
            while (catalog != null);
            nodes.Reverse();
            return nodes;
        }

        /// <summary>
        /// ˽��ͼ�׷���
        /// </summary>
        public virtual PKS_KG_PrivateCatalog PrivateCatalog { get; set; }
        /// <summary>
        /// ���˽��ͼ�׷�����
        /// </summary>
        public List<TreeNode> GetPrivateCatalogs()
        {
            if (this.PrivateCatalogId == null) return null;
            var nodes = new List<TreeNode>();
            var catalog = this.PrivateCatalog;
            do
            {
                nodes.Add(catalog.ToTreeNode());
                catalog = catalog.Parent;
            }
            while (catalog != null);
            nodes.Reverse();
            return nodes;
        }
    }

}
