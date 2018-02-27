namespace Jurassic.CommonModels.Articles
{
    using Jurassic.AppCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ��Ŀʵ����
    /// </summary>
    [Table("BASE_CATALOG")]
    public partial class Base_Catalog : StateEntity, IIdNameBase<int>
    {
        /// <summary>
        /// ����һ����Ŀʵ����
        /// </summary>
        public Base_Catalog()
        {
            Exts = new HashSet<Base_CatalogExt>();
        }

        /// <summary>
        /// ʵ���ุID
        /// </summary>
        [Column("PARENTID")]
        public int? ParentId { get; set; }

        /// <summary>
        /// λ�ã�һ��ָURL
        /// </summary>
        [Column("LOCATION")]
        public string Location { get; set; }

        /// <summary>
        /// ͼ���ַ
        /// </summary>
        [Column("ICONLOCATION")]
        public string IconLocation { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        [Column("NAME")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// ��Ŀ���
        /// </summary>
        [Column("ABSTRACT")]
        [StringLength(200)]
        public string Abstract { get; set; }

        /// <summary>
        /// ��Ŀ��������(��zh-CN)
        /// </summary>
        [Column("LANGUAGE")]
        [StringLength(10)]
        public string Language { get; set; }

        /// <summary>
        /// Ŀ¼������ID
        /// </summary>
        [Column("OWNERID")]
        public int OwnerId { get; set; }

        /// <summary>
        /// Ŀ¼����������
        /// </summary>
        [Column("OWNERTYPE")]
        public CatalogOwnerType OwnerType { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        [Column("EDITTIME")]
        public DateTime EditTime { get; set; }

        /// <summary>
        /// �޸���ID
        /// </summary>
        [Column("EDITORID")]
        public int EditorId { get; set; }
        
      
        /// <summary>
        /// ��Ŀ����չ���Լ�
        /// </summary>
        [ForeignKey("CatalogId")]
        public virtual ICollection<Base_CatalogExt> Exts { get; set; }

        /// <summary>
        /// ����λ
        /// </summary>
        [Column("ORD")]
        public int Ord { get; set; }
    }
}
