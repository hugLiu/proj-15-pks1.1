using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EntityBase;
using Jurassic.CommonModels.ModelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AddinDemo.Models
{
    #region View Models
    [CatalogExt(EntityType = typeof(Supplier))]
    public class SupplierModel : INamedModel
    {
        public SupplierModel()
        {
            Products = new List<ProductModel>();
            IsConfirmed = true;
        }

        /// <summary>
        /// 定义该属性在录入界面不显示
        /// </summary>
        [CatalogExt(DataSourceType = ExtDataSourceType.Hidden)]
        public int Id { get; set; }

        public string Tel { get; set; }

        [CatalogExt(DataType = ExtDataType.MultiLanguage)]

        public string Name { get; set; }


        /// <summary>
        /// </summary>
        [CatalogExt(DataType = ExtDataType.UserId, DisplayProperty = "CreaterName", AuthByUser = true)]
        public int? CreaterId { get; set; }

        /// <summary>
        /// 定义只需要录入时间
        /// </summary>
        [CatalogExt(DataType = ExtDataType.Time)]
        public DateTime CreateTime { get; set; }

        public string Email { get; set; }

        [CatalogExt(DataSourceType = ExtDataSourceType.Hidden, Browsable = false)]
        public bool IsConfirmed { get; set; }

        public SupplierLevel Level { get; set; }

        /// <summary>
        /// 定义本属性用下拉列表输入，用分号隔开的关键词将作为下拉列表的下拉选项
        /// </summary>
        [CatalogExt(DataSource = "北京;上海;武汉;西安")]
        public string Address { get; set; }

        /// <summary>
        /// 该集合对应数据实体中的集合
        /// </summary>
        public IEnumerable<ProductModel> Products { get; set; }

        [CatalogExt(DisplayProperty = "ContacterName", DataSourceType = ExtDataSourceType.UserDefine, Browsable = false)]
        public int? ContacterId { get; set; }

        [CatalogExt(DataSourceType = ExtDataSourceType.Hidden)]
        public string ContacterName { get; set; }
    }

    [CatalogExt(EntityType = typeof(Product))]
    public class ProductModel : INamedModel
    {
        public int Id { get; set; }

        [CatalogExt(DataType = ExtDataType.MultiLanguage)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [CatalogExt(Editable = false)]
        public string Unit { get; set; }

        [Browsable(false)]
        public int MasterId { get; set; }
    }
    #endregion

    #region Data Models
    public class Supplier : CUDEntity, INamedEntity //加上IDataRule接口会筛选全局数据权限
    {
        public Supplier()
        {
            Products = new List<Product>();
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }

        public SupplierLevel Level { get; set; }

        public string Email { get; set; }

        [ForeignKey("MasterId")]
        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// 联系人的ID， 是Person的外键
        /// </summary>
        public int ContacterId { get; set; }

        public virtual Person Contacter { get; set; }
    }

    public class Product : INamedEntity, IDetailEntity<Supplier>
    {
        public int Id { get; set; }

        [ForeignKey("Master")]
        public int MasterId { get; set; }

        public virtual Supplier Master { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Unit { get; set; }
    }
    #endregion

    public enum SupplierLevel
    {
        Small,
        Medium,
        Large
    }

    //class SupplierConverter : IModelEntityConverter<SupplierModel, Supplier>
    //{
    //    public Expression<Func<SupplierModel, Supplier>> ModelToEntity
    //    {
    //        get
    //        {
    //            return s => new Supplier
    //            {
    //                Id = s.Id,
    //                CreaterId = s.CreaterId,
    //                Name = s.Name,
    //                Address = s.Address,
    //                Email = s.Email,
    //                IsDeleted = false,
    //                Tel = s.Tel,
    //                Level = s.Level,
    //                CreateTime = s.CreateTime,
    //            };
    //        }
    //    }
    //}
}