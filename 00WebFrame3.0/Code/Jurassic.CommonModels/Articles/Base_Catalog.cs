namespace Jurassic.CommonModels.Articles
{
    using Jurassic.AppCenter;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 栏目实体类
    /// </summary>
    [Table("BASE_CATALOG")]
    public partial class Base_Catalog : StateEntity, IIdNameBase<int>
    {
        /// <summary>
        /// 创建一个栏目实体类
        /// </summary>
        public Base_Catalog()
        {
            Exts = new HashSet<Base_CatalogExt>();
        }

        /// <summary>
        /// 实体类父ID
        /// </summary>
        [Column("PARENTID")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 位置，一般指URL
        /// </summary>
        [Column("LOCATION")]
        public string Location { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        [Column("ICONLOCATION")]
        public string IconLocation { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Column("NAME")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 栏目简介
        /// </summary>
        [Column("ABSTRACT")]
        [StringLength(200)]
        public string Abstract { get; set; }

        /// <summary>
        /// 栏目语言名称(如zh-CN)
        /// </summary>
        [Column("LANGUAGE")]
        [StringLength(10)]
        public string Language { get; set; }

        /// <summary>
        /// 目录所有者ID
        /// </summary>
        [Column("OWNERID")]
        public int OwnerId { get; set; }

        /// <summary>
        /// 目录所有者类型
        /// </summary>
        [Column("OWNERTYPE")]
        public CatalogOwnerType OwnerType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("EDITTIME")]
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 修改者ID
        /// </summary>
        [Column("EDITORID")]
        public int EditorId { get; set; }
        
      
        /// <summary>
        /// 栏目的扩展属性集
        /// </summary>
        [ForeignKey("CatalogId")]
        public virtual ICollection<Base_CatalogExt> Exts { get; set; }

        /// <summary>
        /// 排序位
        /// </summary>
        [Column("ORD")]
        public int Ord { get; set; }
    }
}
